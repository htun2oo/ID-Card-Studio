using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class OpenProjectWindow : Window
    {
        public string SelectedProject { get; private set; } = string.Empty;

        public OpenProjectWindow(List<string>? savedProjects = null)
        {
            InitializeComponent();

            // အကယ်၍ save လုပ်ထားသော project များ ရှိမနေပါက (Null သို့မဟုတ် Empty) blank ထားမည်
            if (savedProjects == null || savedProjects.Count == 0)
            {
                ProjectListBox.ItemsSource = null;
                OpenButton.IsEnabled = false;
            }
            else
            {
                ProjectListBox.ItemsSource = savedProjects;
                ProjectListBox.SelectionChanged += ProjectListBox_SelectionChanged;
            }
        }

        private void ProjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OpenButton.IsEnabled = ProjectListBox.SelectedItem != null;
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectListBox.SelectedItem != null)
            {
                SelectedProject = ProjectListBox.SelectedItem.ToString() ?? string.Empty;
                this.DialogResult = true;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
