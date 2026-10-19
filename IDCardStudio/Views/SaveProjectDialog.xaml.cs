using System.Windows;

namespace IDCardStudio.Views
{
    public partial class SaveProjectDialog : Window
    {
        public string ProjectName { get; private set; } = "Untitled-1";

        public SaveProjectDialog()
        {
            InitializeComponent();
            ProjectNameTextBox.Focus();
            ProjectNameTextBox.SelectAll();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProjectNameTextBox.Text))
            {
                MessageBox.Show("Please enter a valid project name.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProjectName = ProjectNameTextBox.Text.Trim();
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
