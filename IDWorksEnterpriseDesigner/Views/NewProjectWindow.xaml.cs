using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        // Initial Components CheckBox များ၏ အခြေအနေနှင့် Orientation ကို MainWindow သို့ ပို့ပေးရန် Properties များ
        public bool HasProductionForm => ProductionFormCheckBox?.IsChecked == true;
        public bool HasCardDesign => CardCheckBox?.IsChecked == true;
        public bool HasReportDesign => ReportCheckBox?.IsChecked == true;
        public bool IsPortrait => PortraitRadio?.IsChecked == true;

        public NewProjectWindow()
        {
            InitializeComponent();

            // Window Load ဖြစ်ချိန်တွင် Project Name သတ်မှတ်ပေးခြင်း
            this.Loaded += (s, e) =>
            {
                // 1. "Untitled-1", "Untitled-2" စသည်ဖြင့် နံပါတ်တိုးထားသော နာမည်ယူခြင်း
                string defaultName = GetNextUntitledProjectName();

                // 2. TextBox သို့ စာသား ထည့်သွင်းခြင်း
                ProjectNameTextBox.Text = defaultName;

                // 3. Photoshop ကဲ့သို့ ရိုက်ထည့်ရလွယ်ကူစေရန် စာသားအားလုံးကို Select လုပ်ထားပေးခြင်း (သို့မဟုတ် Cursor အဆုံးပို့ခြင်း)
                ProjectNameTextBox.Focus();
                ProjectNameTextBox.SelectAll(); // စာအပြည့် Select လုပ်ထားလိုပါက
                // ProjectNameTextBox.CaretIndex = ProjectNameTextBox.Text.Length; // Cursor ကို စာသား အဆုံးတွင် ထားလိုပါက
            };
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
