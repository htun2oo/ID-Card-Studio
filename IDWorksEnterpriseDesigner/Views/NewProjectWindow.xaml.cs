using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; set; } = string.Empty;

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CardDesign_Checked(object sender, RoutedEventArgs e) { }
        private void CardDesign_Unchecked(object sender, RoutedEventArgs e) { }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
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
