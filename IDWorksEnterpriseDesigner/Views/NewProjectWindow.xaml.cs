using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName
        {
            get { return TxtProjectName.Text; }
        }

        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ChkCardDesign_Checked(object sender, RoutedEventArgs e)
        {
            SetCardOptionsEnabled(true);
        }

        private void ChkCardDesign_Unchecked(object sender, RoutedEventArgs e)
        {
            SetCardOptionsEnabled(false);
        }

        private void SetCardOptionsEnabled(bool isEnabled)
        {
            if (RbLandscape != null) RbLandscape.IsEnabled = isEnabled;
            if (RbPortrait != null) RbPortrait.IsEnabled = isEnabled;
            if (ChkAllowMultiple != null) ChkAllowMultiple.IsEnabled = isEnabled;
            if (TxtAllowMultiple != null) TxtAllowMultiple.IsEnabled = isEnabled;
        }
    }
}
