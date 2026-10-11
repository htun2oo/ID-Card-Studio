using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; private set; }
        public bool IsProductionFormSelected { get; private set; }
        public bool IsCardDesignSelected { get; private set; }
        public bool IsLandscape { get; private set; }
        public bool IsReportDesignSelected { get; private set; }

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CardDesign_Checked(object sender, RoutedEventArgs e)
        {
            if (CardOptionsPanel != null)
                CardOptionsPanel.IsEnabled = true;
        }

        private void CardDesign_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CardOptionsPanel != null)
                CardOptionsPanel.IsEnabled = false;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ProjectName = ProjectNameTextBox.Text;
            IsProductionFormSelected = ProdFormCheckBox.IsChecked ?? false;
            IsCardDesignSelected = CardDesignCheckBox.IsChecked ?? false;
            IsLandscape = LandscapeRadio.IsChecked ?? true;
            IsReportDesignSelected = ReportDesignCheckBox.IsChecked ?? true;

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
