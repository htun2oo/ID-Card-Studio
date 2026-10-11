using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; private set; }

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = ProjectNameTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
