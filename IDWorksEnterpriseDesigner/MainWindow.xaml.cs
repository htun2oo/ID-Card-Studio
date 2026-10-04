using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow newProjectDlg = new NewProjectWindow();
            newProjectDlg.Owner = this;
            if (newProjectDlg.ShowDialog() == true)
            {
                // Create Workspaces based on selection
                StatusText.Content = $"Project '{newProjectDlg.ProjectName}' Created.";
                LoadProjectWorkspace(newProjectDlg.ProjectName, newProjectDlg.IsLandscape);
            }
        }

        private void LoadProjectWorkspace(string projectName, bool isLandscape)
        {
            WorkspaceGrid.Children.Clear();
            this.Title = $"Datacard ID Works Enterprise Designer - {projectName} - [Card]";
            
            // Add Sample Designer Panels representing UI from Window 4.png
            MessageBox.Show($"New ID Design Project '{projectName}' initialized successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
