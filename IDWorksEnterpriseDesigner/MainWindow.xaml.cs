using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetStatusMessage("For Help, press F1");
        }

        public void SetStatusMessage(string message)
        {
            if (StatusText != null)
            {
                StatusText.Text = message;
            }
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            IDWorksEnterpriseDesigner.Views.NewProjectWindow newProjectWin = new IDWorksEnterpriseDesigner.Views.NewProjectWindow();
            newProjectWin.Owner = this;
            newProjectWin.ShowDialog();
        }
    }
}
