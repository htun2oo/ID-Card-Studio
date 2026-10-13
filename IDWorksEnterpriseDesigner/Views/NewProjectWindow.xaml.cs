using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private void CardCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            bool isChecked = CardCheckBox.IsChecked ?? false;
            if (LandscapeRadio != null && PortraitRadio != null && MultiCardCheckBox != null)
            {
                LandscapeRadio.IsEnabled = isChecked;
                PortraitRadio.IsEnabled = isChecked;
                MultiCardCheckBox.IsEnabled = isChecked;
            }
        }

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
