using Microsoft.Win32;
using MinecraftModTranslator.Classes;
using MinecraftModTranslator.Utils;
using ModernWpf.Controls;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftModTranslator.Pages
{
    /// <summary>
    /// PageMod.xaml 的交互逻辑
    /// </summary>
    public partial class PageMod : ModernWpf.Controls.Page
    {
        private string? JarPath;


        public PageMod()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;

                string[]? paths = e.Data.GetData(DataFormats.FileDrop) as string[];
                if (paths == null || paths.Length != 1)
                    return;
                string path = paths[0];
                if (!File.Exists(path))
                    return;
                string extension = Path.GetExtension(path);
                if (!string.Equals(extension, ".jar", StringComparison.OrdinalIgnoreCase))
                    return;

                textBlock_Title.Text = $"已选择 \"{Path.GetFileName(path)}\"";
                textBlock_Subtitle.Text = $"路径: \"{path}\" 这样对吗？";

                button_Next.IsEnabled = true;

                JarPath = path;
            }
            catch (Exception ex)
            {
                ErrorReportDialog.Show(ex);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                    return;

                string[]? paths = e.Data.GetData(DataFormats.FileDrop) as string[];

                if (paths == null || paths.Length != 1)
                    return;

                string path = paths[0];

                if (!File.Exists(path))
                    return;

                string extension = Path.GetExtension(path);
                if (!string.Equals(extension, ".jar", StringComparison.OrdinalIgnoreCase))
                    return;

                e.Effects = DragDropEffects.Copy;
            }
            catch (Exception ex)
            {
                ErrorReportDialog.Show(ex);
            }
        }

        private void button_Select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "Minecraft mod 文件|*.jar",
                    Title = "选择一个有效的Minecraft mod文件"
                };
                if (dialog.ShowDialog() != true)
                    return;

                textBlock_Title.Text = $"已选择 \"{Path.GetFileName(dialog.FileName)}\"";
                textBlock_Subtitle.Text = $"路径: \"{dialog.FileName}\" 这样对吗？";

                button_Next.IsEnabled = true;

                JarPath = dialog.FileName;
            }
            catch (Exception ex)
            {
                ErrorReportDialog.Show(ex);
            }
        }

        private async void button_Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(JarPath) || !File.Exists(JarPath))
                {
                    await DialogManager.ShowDialogAsync(new ContentDialog
                    {
                        Title = "提示",
                        Content = $"文件 \"{JarPath}\" 不存在，请重新选择",
                        PrimaryButtonText = "确定",
                        DefaultButton = ContentDialogButton.Primary
                    });
                    return;
                }

                Globals.ModJarPath = JarPath;

                Frame.Navigate(typeof(PageLoad), null, new ModernWpf.Media.Animation.DrillInNavigationTransitionInfo());
            }
            catch (Exception ex)
            {
                ErrorReportDialog.Show(ex);
            }
        }
    }
}
