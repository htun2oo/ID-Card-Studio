using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; private set; } = string.Empty;

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = TxtProjectName.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
