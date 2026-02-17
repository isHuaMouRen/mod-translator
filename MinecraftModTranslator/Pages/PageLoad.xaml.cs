using System.Windows;
using MinecraftModTranslator.Classes;
using MinecraftModTranslator.Utils;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using MinecraftModTranslator.Classes.JsonConfigs;

namespace MinecraftModTranslator.Pages
{
    /// <summary>
    /// PageLoad.xaml 的交互逻辑
    /// </summary>
    public partial class PageLoad : ModernWpf.Controls.Page
    {
        public PageLoad()
        {
            InitializeComponent();

            Loaded += (async (s, e) =>
            {
                try
                {
                    await Task.Delay(500);

                    string outputDir = Path.Combine(Path.GetTempPath(), $"MMT.{DateTimeOffset.Now.ToUnixTimeMilliseconds()}");

                    if (!Directory.Exists(outputDir))
                        Directory.CreateDirectory(outputDir);

                    await CompressHelper.Extract(Globals.ModJarPath!, outputDir);
                    Globals.ModRoot = outputDir;

                    //如果是FabricMod就加载信息，Forge的toml信息暂时不考虑
                    if (File.Exists(Path.Combine(Globals.ModRoot, "fabric.mod.json")))
                        Globals.ModFabricInfo = JsonConvert.DeserializeObject<JsonFabricInfo.Index>(File.ReadAllText(Path.Combine(Globals.ModRoot, "fabric.mod.json")));
                    else
                        Globals.ModFabricInfo = null;

                    //检测语言文件
                    string assetsDir = Path.Combine(Globals.ModRoot!, "assets");
                    Globals.ModLangDir.Clear();
                    foreach (var asset in Directory.GetDirectories(assetsDir))
                        if (Directory.Exists(Path.Combine(asset, "lang")))
                            Globals.ModLangDir.Add(Path.Combine(asset, "lang"));


                    Frame.Navigate(typeof(PageModInfo), null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());
                }
                catch (Exception ex)
                {
                    grid_Error.Visibility = Visibility.Visible;
                    grid_Loading.Visibility = Visibility.Hidden;

                    //创建错误信息文件
                    string errorFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, $"MMT.LOAD.ERROR.{DateTimeOffset.Now.ToUnixTimeMilliseconds()}");
                    File.WriteAllText(errorFile, ex.ToString());
                }
            });
        }

        private void button_Back_Click(object sender, RoutedEventArgs e) => Frame.Navigate(typeof(PageMod), null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());
    }
}
