using MinecraftModTranslator.Classes;
using MinecraftModTranslator.Controls;
using MinecraftModTranslator.Utils;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MinecraftModTranslator.Pages
{
    /// <summary>
    /// PageTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class PageTranslate : ModernWpf.Controls.Page
    {

        private List<object> displayItems = new List<object>();

        public PageTranslate()
        {
            InitializeComponent();

            Loaded += (async (s, e) =>
            {
                try
                {
                    


                    //遍历每个Lang文件夹
                    foreach (var langDir in Globals.ModLangDir)
                    {
                        var textBlock = new TextBlock
                        {
                            Text = $"assets.{Path.GetFileName(Path.GetDirectoryName(langDir))}"
                        };
                        displayItems.Add(textBlock);

                        string enUSPath = Path.Combine(langDir, "en_us.json");
                        string zhCNPath = Path.Combine(langDir, "zh_cn.json");
                        var enUSDict = new Dictionary<string, string>();
                        var zhCNDict = new Dictionary<string, string>();

                        if (File.Exists(enUSPath))
                            enUSDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(enUSPath));
                        if(File.Exists(zhCNPath))
                            zhCNDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(zhCNPath));

                        //遍历键
                        foreach (var key in enUSDict!)
                        {
                            var ctrl = new UserLangKey
                            {
                                Key = key.Key,
                                ValueOriginal = key.Value,
                                ValueTranslate = zhCNDict!.Count > 0 ? zhCNDict!.ContainsKey(key.Key) ? zhCNDict![key.Key] : "" : "",
                                Tag = langDir
                            };
                            displayItems.Add(ctrl);
                        }


                    }


                    itemsControl.ItemsSource = displayItems;
                }
                catch (Exception ex)
                {
                    ErrorReportDialog.Show(ex);
                }
            });
        }

        private async void button_Done_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                foreach (var langDir in Globals.ModLangDir)
                {
                    string enUSPath = Path.Combine(langDir, "en_us.json");
                    string zhCNPath = Path.Combine(langDir, "zh_cn.json");

                    var zhCNDict = new Dictionary<string, string>();

                    foreach (var child in displayItems)
                    {
                        if (child is UserLangKey ulk && (string)ulk.Tag == langDir) 
                        {
                            zhCNDict.Add(ulk.Key!, string.IsNullOrEmpty(ulk.ValueTranslate) ? string.IsNullOrEmpty(ulk.ValueOriginal) ? "" : ulk.ValueOriginal : ulk.ValueTranslate);
                        }
                    }

                    File.WriteAllText(zhCNPath, JsonConvert.SerializeObject(zhCNDict));
                }

                Frame.Navigate(typeof(PageSave), null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());
            }
            catch (Exception ex)
            {
                ErrorReportDialog.Show(ex);
            }
        }
    }
}
