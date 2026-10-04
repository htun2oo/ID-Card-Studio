using System.Windows;
using IDWorksEnterpriseDesigner.Views;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenNewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow dialog = new NewProjectWindow();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                // Window Title ကို ဒုတိယပုံအတိုင်း ပြောင်းလဲခြင်း
                this.Title = $"Datacard ID Works Enterprise Designer - {dialog.ProjectName} - [Card]";

                // MDI Container / Canvas များကို ရှင်းလင်းခြင်း
                // (သင့် Project ရှိ MDI Container ContainerName အတိုင်း အသုံးပြုပါ)
                ClearMdiContainer();

                // 1. Production Form Window ကို ညာဘက်အပေါ်တွင် ပြသခြင်း
                if (dialog.HasProductionForm)
                {
                    CreateProductionFormWindow(dialog.ProjectName);
                }

                // 2. Card Design Windows (Card & Card (back side)) များကို ဘယ်ဘက်တွင် ပြသခြင်း
                if (dialog.HasCardDesign)
                {
                    CreateCardDesignWindows(dialog.IsLandscape);
                }

                // 3. Report Design Window ကို ညာဘက်အောက်တွင် ပြသခြင်း
                if (dialog.HasReportDesign)
                {
                    CreateReportDesignWindow();
                }
            }
        }

        private void ClearMdiContainer()
        {
            // MDI Container ထဲမှ Existing Window များကို ဖျက်ထုတ်ရန် Code
        }

        private void CreateProductionFormWindow(string projectName)
        {
            // Production Form Child Window ကို ဒုတိယပုံအတိုင်း
            // Position: Top-Right ( Width: ~65%, Height: ~60% ) ဖြင့် ထည့်သွင်းပါ
        }

        private void CreateCardDesignWindows(bool isLandscape)
        {
            // Card (Front Side) Child Window - Position: Top-Left
            // Card (Back Side) Child Window - Position: Bottom-Left
        }

        private void CreateReportDesignWindow()
        {
            // Report Child Window - Position: Bottom-Right
        }
    }
}
