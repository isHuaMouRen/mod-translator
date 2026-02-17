using System.Diagnostics;
using System.Reflection;

namespace ExecuteShell
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                string executePath = AppContext.BaseDirectory;
                string binPath = $"{Path.Combine(executePath, "bin")}";
                string exePath = $"{Path.Combine(binPath, "MinecraftModTranslator.exe")}";

                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    UseShellExecute = true,
                    Arguments = $"-shell"
                });

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", $"The main program cannot be launched.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}