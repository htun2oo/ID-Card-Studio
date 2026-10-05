using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public partial class NewProjectWindow : Window
    {
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

        private void ChkCardDesign_Unchecked(object sender, RoutedEventArgs e)
        {
            if (RbLandscape != null) RbLandscape.IsEnabled = false;
            if (RbPortrait != null) RbPortrait.IsEnabled = false;
            if (ChkAllowMultiple != null) ChkAllowMultiple.IsEnabled = false;
        }

        private void ChkCardDesign_Checked(object sender, RoutedEventArgs e)
        {
            if (RbLandscape != null) RbLandscape.IsEnabled = true;
            if (RbPortrait != null) RbPortrait.IsEnabled = true;
            if (ChkAllowMultiple != null) ChkAllowMultiple.IsEnabled = true;
        }
    }
}
