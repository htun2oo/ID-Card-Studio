using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; private set; } = string.Empty;
        public bool HasProductionForm { get; private set; }
        public bool HasCardDesign { get; private set; }
        public bool IsLandscape { get; private set; }
        public bool HasReportDesign { get; private set; }

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtProjectName.Text))
            {
                MessageBox.Show("Please enter a Project Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProjectName = TxtProjectName.Text.Trim();
            HasProductionForm = ChkProductionFormDesign.IsChecked == true;
            HasCardDesign = ChkCardDesign.IsChecked == true;
            IsLandscape = RadLandscape.IsChecked == true;
            HasReportDesign = ChkReportDesign.IsChecked == true;

            this.DialogResult = true;
            this.Close();
        }
    }
}
