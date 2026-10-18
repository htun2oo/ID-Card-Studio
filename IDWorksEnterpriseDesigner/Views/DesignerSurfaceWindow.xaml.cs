using System;
using System.IO;
using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class DesignerSurfaceWindow : Window
    {
        // Warning ရှင်းရန် string? သုံးပေးခြင်း
        private string? currentFilePath = null;
        
        public DesignerSurfaceWindow()
        {
            InitializeComponent();

            // Window အပြည့်အစုံ Load ဖြစ်လာချိန်တွင် အလုပ်လုပ်ရန် ချိတ်ဆက်ခြင်း
            this.Loaded += DesignerSurfaceWindow_Loaded;
        }

        private void DesignerSurfaceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Window load ဖြစ်ချိန် လုပ်ဆောင်ရန်များ
        }

        // --- Save Project As... Implementation ---
        private void SaveProjectAs_Click(object sender, RoutedEventArgs e)
        {
            SaveProjectDialog dialog = new SaveProjectDialog();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                string projectName = dialog.ProjectName;
                string publicDocs = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                string projectsFolder = Path.Combine(publicDocs, "ID Card Studio", "Projects");

                // ဖိုင်တွဲ မရှိသေးပါက အလိုအလျောက်ဖန်တီးရန်
                if (!Directory.Exists(projectsFolder))
                {
                    Directory.CreateDirectory(projectsFolder);
                }

                // .idp extension ဖြင့် ဖိုင်လမ်းကြောင်းသတ်မှတ်ခြင်း
                string filePath = Path.Combine(projectsFolder, projectName + ".idp");

                try
                {
                    // Project ဖိုင်သိမ်းဆည်းသည့် လုပ်ငန်းစဉ်
                    File.WriteAllText(filePath, "// ID Card Studio Project File");
                    currentFilePath = filePath;
                    
                    MessageBox.Show($"Project saved successfully to:\n{filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving project: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
