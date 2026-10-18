using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        // Portrait ကို ရွေးချယ်ထားခြင်း ရှိမရှိ စစ်ဆေးရန် Property
        public bool IsPortrait => PortraitRadio?.IsChecked == true;
        
        public NewProjectWindow()
        {
            InitializeComponent();

            // ဒိုင်ယာလော့ခ် ပွင့်လာသည်နှင့် Project Name TextBox သို့ Focus အလိုအလျောက် ရောက်စေရန်
            Loaded += (s, e) => ProjectNameTextBox.Focus();
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
