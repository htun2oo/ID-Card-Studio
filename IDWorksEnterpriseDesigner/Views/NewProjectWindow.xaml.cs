using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; private set; } = string.Empty;
        public bool IncludeProductionForm { get; private set; } = true;
        public bool IncludeCardDesign { get; private set; } = true;
        public bool IsLandscape { get; private set; } = true;
        public bool IncludeReportDesign { get; private set; } = true;

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = TxtProjectName.Text;
            IncludeProductionForm = ChkProductionForm.IsChecked ?? false;
            IncludeCardDesign = ChkCardDesign.IsChecked ?? false;
            IsLandscape = RbLandscape.IsChecked ?? true;
            IncludeReportDesign = ChkReportDesign.IsChecked ?? false;

            this.DialogResult = true;
            this.Close();
        }
    }
}
