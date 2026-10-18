using System.Collections.Generic;
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

                // 3. Photoshop ကဲ့သို့ ရိုက်ထည့်ရလွယ်ကူစေရန် စာသားအားလုံးကို Select လုပ်ထားပေးခြင်း
                ProjectNameTextBox.Focus();
                ProjectNameTextBox.SelectAll(); // စာအပြည့် Select လုပ်ထားလိုပါက
            };
        }

        // --- "Untitled-1", "Untitled-2" စသည်ဖြင့် နံပါတ် အစဉ်လိုက် တိုးပေးသည့် Method ---
        private string GetNextUntitledProjectName()
        {
            int counter = 1;
            string baseName = "Untitled-";

            // လက်ရှိ ရှိပြီးသား Project နာမည်များ စာရင်းကို ရယူသည့် Method
            List<string> existingProjectNames = GetExistingProjectNames(); 

            while (existingProjectNames.Contains($"{baseName}{counter}"))
            {
                counter++;
            }

            return $"{baseName}{counter}";
        }

        // လက်ရှိ Project စာရင်းများ ရယူရန် Helper Method (မိမိတို့ project ၏ directory သို့မဟုတ် data structure အလိုက် ချိတ်ဆက်နိုင်ပါသည်)
        private List<string> GetExistingProjectNames()
        {
            // ဥပမာအနေဖြင့် လက်ရှိ ရှိနှင့်ပြီးသား project name များကို List ဖြင့် ပြန်ပေးရန်
            return new List<string>(); 
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
