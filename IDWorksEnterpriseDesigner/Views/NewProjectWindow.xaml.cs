using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        // CheckBox ရွေးချယ်ထားမှု အခြေအနေများ
        public bool HasProductionForm => ProductionFormCheckBox?.IsChecked == true; // သို့မဟုတ် XAML ရှိ CheckBox အမည်
        public bool HasCardDesign => CardCheckBox?.IsChecked == true;
        public bool HasReportDesign => ReportCheckBox?.IsChecked == true; // သို့မဟုတ် XAML ရှိ Report CheckBox အမည်
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
