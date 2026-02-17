using Microsoft.Win32;
using MinecraftModTranslator.Classes;
using MinecraftModTranslator.Classes.JsonConfigs;
using MinecraftModTranslator.Utils;
using ModernWpf.Controls;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace MinecraftModTranslator.Pages
{
    /// <summary>
    /// PageSave.xaml 的交互逻辑
    /// </summary>
    public partial class PageSave : ModernWpf.Controls.Page
    {
        private string? selectedOption;



        public PageSave()
        {
            InitializeComponent();
        }

        private void StartLoad() => SetLoad(true);
        private void EndLoad() => SetLoad(false);
        private void SetLoad(bool isLoad)
        {
            grid_Loading.Visibility = isLoad ? Visibility.Visible : Visibility.Hidden;
            grid_Main.IsEnabled = !isLoad;
            grid_Main.Effect = isLoad ? new BlurEffect { Radius = 10 } : null;
        }





        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Border border)
                return;

            var animation = new DoubleAnimation
            {
                From = 1,
                To = 1.05,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new PowerEase { Power = 5, EasingMode = EasingMode.EaseOut }
            };
            if (border.RenderTransform is ScaleTransform transform)
            {
                transform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                transform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                transform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Border border)
                return;

            var animation = new DoubleAnimation
            {
                From = 1.05,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new PowerEase { Power = 5, EasingMode = EasingMode.EaseOut }
            };
            if (border.RenderTransform is ScaleTransform transform)
            {
                transform.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                transform.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                transform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                transform.BeginAnimation(ScaleTransform.ScaleYProperty, animation);
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Border border)
                return;

            border_Mod.BorderBrush = new SolidColorBrush(Colors.Gray);
            border_Package.BorderBrush = new SolidColorBrush(Colors.Gray);

            border.BorderBrush = new SolidColorBrush(Color.FromRgb(50, 50, 255));


            if (sender == border_Mod)
                selectedOption = "mod";
            else if (sender == border_Package)
                selectedOption = "pack";

            button_Save.IsEnabled = true;
        }

        private async void button_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StartLoad();

                string? savePath;

                if (selectedOption == "mod")
                {
                    var dialog = new SaveFileDialog
                    {
                        Filter = "Minecraft mod file|*.jar"
                    };
                    if (dialog.ShowDialog() != true)
                    {
                        EndLoad();
                        return;
                    }
                    savePath = dialog.FileName;

                    await CompressHelper.Compress(Globals.ModRoot!, savePath);

                    await DialogManager.ShowDialogAsync(new ContentDialog
                    {
                        Title = "导出成功",
                        Content = $"成功导出翻译后的Mod文件到 \"{savePath}\"",
                        PrimaryButtonText = "定位",
                        CloseButtonText = "退出",
                        DefaultButton = ContentDialogButton.Primary
                    }, (() =>
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = $"/select \"{savePath}\"",
                            UseShellExecute = true
                        });
                    }));

                    Application.Current.Shutdown(0);
                }
                else if (selectedOption == "pack")
                {
                    var dialog = new SaveFileDialog
                    {
                        Filter = "Minecraft resourcepack|*.zip"
                    };
                    if (dialog.ShowDialog() != true)
                    {
                        EndLoad();
                        return;
                    }
                    savePath = dialog.FileName;


                    string tempDir = Path.Combine(Path.GetTempPath(), $"MMT.RESOURCEPACK.TEMP.{DateTimeOffset.Now.ToUnixTimeMilliseconds()}");
                    Directory.CreateDirectory(tempDir);
                    Directory.CreateDirectory(Path.Combine(tempDir, "assets"));

                    //语言
                    foreach (var langDir in Globals.ModLangDir)
                    {
                        string lang = Path.Combine(tempDir, "assets", Path.GetFileName(Path.GetDirectoryName(langDir))!, "lang");
                        Directory.CreateDirectory(lang);

                        File.Copy(Path.Combine(langDir, "zh_cn.json"), Path.Combine(lang, "zh_cn.json"));
                    }

                    //pack.mcmeta
                    var radio1 = new RadioButton { Content = "1.21.9-", IsChecked = false };
                    var radio2 = new RadioButton { Content = "1.21.9+ (包含1.21.9)", IsChecked = true };
                    await DialogManager.ShowDialogAsync(new ContentDialog
                    {
                        Title = "选择资源包版本",
                        Content = new StackPanel
                        {
                            Children = { radio1, radio2 }
                        },
                        PrimaryButtonText = "确定",
                        DefaultButton = ContentDialogButton.Primary
                    });

                    if (radio1.IsChecked == true)
                    {
                        File.WriteAllText(Path.Combine(tempDir, "pack.mcmeta"), JsonConvert.SerializeObject(new JsonMcmetaOld.Index
                        {
                            Pack = new JsonMcmetaOld.Pack
                            {
                                Description = $"§b{Globals.ModName}§r §a§l翻译包§r§r\n{JsonMcmetaNew.MadeInfo}"
                            }
                        }));
                    }
                    else
                    {
                        File.WriteAllText(Path.Combine(tempDir, "pack.mcmeta"), JsonConvert.SerializeObject(new JsonMcmetaNew.Index
                        {
                            Pack = new JsonMcmetaNew.Pack
                            {
                                Description = $"§b{Globals.ModName}§r §a§l翻译包§r§r\n{JsonMcmetaNew.MadeInfo}"
                            }
                        }));
                    }


                    //图标
                    if (
                        Globals.ModFabricInfo != null && Globals.ModFabricInfo.Icon != null &&
                        File.Exists(Path.Combine(Globals.ModRoot!, Globals.ModFabricInfo.Icon))
                        )
                    {
                        File.Copy(Path.Combine(Globals.ModRoot!, Globals.ModFabricInfo.Icon), Path.Combine(tempDir, "pack.png"));
                    }
                    else
                    {
                        File.Copy(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "Assets", "Image", "Mod.Icon.Placeholder.png"), Path.Combine(tempDir, "pack.png"));
                    }

                    await CompressHelper.Compress(tempDir, savePath);


                    await DialogManager.ShowDialogAsync(new ContentDialog
                    {
                        Title = "导出成功",
                        Content = $"成功导出汉化包到 \"{savePath}\"",
                        PrimaryButtonText = "定位",
                        CloseButtonText = "退出",
                        DefaultButton = ContentDialogButton.Primary
                    }, (() =>
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "explorer.exe",
                            Arguments = $"/select \"{savePath}\"",
                            UseShellExecute = true
                        });
                    }));


                    Application.Current.Shutdown(0);
                }
            }
            catch (Exception ex)
            {
                ErrorReportDialog.Show(ex);
            }
            finally
            {
                EndLoad();
            }
        }
    }
}
