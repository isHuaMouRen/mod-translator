using MinecraftModTranslator.Classes;
using MinecraftModTranslator.Utils;
using System.IO;
using System.Windows.Media.Imaging;

namespace MinecraftModTranslator.Pages
{
    /// <summary>
    /// PageModInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PageModInfo : ModernWpf.Controls.Page
    {
        public PageModInfo()
        {
            InitializeComponent();

            Loaded += ((s, e) =>
            {
                try
                {
                    //加载Mod图标
                    if (
                        Globals.ModFabricInfo != null &&
                        Globals.ModRoot != null &&
                        Globals.ModFabricInfo.Icon != null &&
                        File.Exists(Path.Combine(Globals.ModRoot!, Globals.ModFabricInfo!.Icon))
                        )
                    {
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(Path.Combine(Globals.ModRoot!, Globals.ModFabricInfo!.Icon), UriKind.Absolute);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();
                        bitmap.Freeze();

                        image_ModIcon.Source = bitmap;
                    }

                    //加载Mod信息
                    if (Globals.ModFabricInfo != null)
                    {
                        //标题
                        if (Globals.ModFabricInfo!.Name != null)
                            textBlock_ModName.Text = Globals.ModFabricInfo.Name;
                        else
                            textBlock_ModName.Text = Path.GetFileNameWithoutExtension(Globals.ModJarPath!);

                        //描述
                        if (Globals.ModFabricInfo!.Description != null)
                            textBlock_ModDescription.Text = Globals.ModFabricInfo.Description;
                        else
                            textBlock_ModDescription.Text = "该Mod似乎没有提供描述";
                    }
                    else
                    {
                        textBlock_ModName.Text = Path.GetFileNameWithoutExtension(Globals.ModJarPath!);
                        textBlock_ModDescription.Text = "该Mod似乎没有提供描述";
                    }

                    Globals.ModName = textBlock_ModName.Text;

                    //检测语言文件夹
                    if (Globals.ModLangDir.Count == 0)
                    {
                        button_Next.Content = "该Mod无可用语言文件";
                        button_Next.IsEnabled = false;
                    }


                }
                catch (Exception ex)
                {
                    ErrorReportDialog.Show(ex);
                }
            });
        }

        private void button_Back_Click(object sender, System.Windows.RoutedEventArgs e) => Frame.Navigate(typeof(PageMod), null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());

        private void button_Next_Click(object sender, System.Windows.RoutedEventArgs e) => Frame.Navigate(typeof(PageTranslate), null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());
    }
}
