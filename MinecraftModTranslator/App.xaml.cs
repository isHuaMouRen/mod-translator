using MinecraftModTranslator.Windows;
using System.Configuration;
using System.Data;
using System.Windows;

namespace MinecraftModTranslator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var window = new WindowMain();
            window.Show();
            Application.Current.MainWindow = window;

        }
    }

}
