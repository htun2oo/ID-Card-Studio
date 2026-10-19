using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace IDCardStudio
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Software စတင်ဖွင့်လိုက်သည်နှင့် Public Documents ထဲတွင် ဖိုင်တွဲအလိုအလျောက် ဆောက်ပေးရန်
            CreateDefaultProjectFolder();

            // Global Exception Catching
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        private void CreateDefaultProjectFolder()
        {
            try
            {
                string publicDocs = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                string projectFolderPath = Path.Combine(publicDocs, "ID Card Studio", "Projects");

                if (!Directory.Exists(projectFolderPath))
                {
                    Directory.CreateDirectory(projectFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating project folder: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"Unhandled Exception: {e.Exception.Message}\n\n{e.Exception.StackTrace}", 
                            "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
            Current.Shutdown();
        }
    }
}
