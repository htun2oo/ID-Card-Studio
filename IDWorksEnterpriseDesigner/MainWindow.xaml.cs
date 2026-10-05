using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IDWorksEnterpriseDesigner
{
    public enum ActiveViewMode
    {
        None,           // Open Project မဖွင့်မီ Initial State (ပုံ ၁၊ ၂၊ ၃ အခြေအနေ)
        CardFront,      // Card Front Selected
        CardBack,       // Card Back Selected
        ProductionForm, // Production Form Active
        Report          // Report Active
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // မူလ စတင်ချိန်တွင် ပထမပုံအတိုင်း Initial Mode သတ်မှတ်ထားပါမည်
            SetViewMenuMode(ActiveViewMode.None);
        }

        public void SetViewMenuMode(ActiveViewMode mode)
        {
            if (mode == ActiveViewMode.None)
            {
                // Project မဖွင့်မီ View Menu Items အဓိက အစိတ်အပိုင်းများကို ခေတ္တ ဖျောက်ထားမည်
                SetGroupVisibility(Visibility.Collapsed);
                return;
            }

            // Project/Window ပွင့်လာပါက View Menu ၏ Dynamic Items များကို ဖော်ပြမည်
            SetGroupVisibility(Visibility.Visible);

            switch (mode)
            {
                case ActiveViewMode.CardFront:
                    MenuViewFrontCard.IsEnabled = true;
                    MenuViewFrontCard.IsChecked = true;

                    MenuViewBackCard.IsEnabled = true;
                    MenuViewBackCard.IsChecked = false;

                    MenuViewRuler.Header = "Ruler for Selected Card Side";
                    MenuViewRuler.IsEnabled = true;

                    MenuViewZoomCard.Visibility = Visibility.Visible;
                    MenuViewZoomReport.Visibility = Visibility.Collapsed;
                    break;

                case ActiveViewMode.CardBack:
                    MenuViewFrontCard.IsEnabled = true;
                    MenuViewFrontCard.IsChecked = false;

                    MenuViewBackCard.IsEnabled = true;
                    MenuViewBackCard.IsChecked = true;

                    MenuViewRuler.Header = "Ruler for Selected Card Side";
                    MenuViewRuler.IsEnabled = true;

                    MenuViewZoomCard.Visibility = Visibility.Visible;
                    MenuViewZoomReport.Visibility = Visibility.Collapsed;
                    break;

                case ActiveViewMode.ProductionForm:
                    MenuViewFrontCard.IsEnabled = false;
                    MenuViewFrontCard.IsChecked = false;

                    MenuViewBackCard.IsEnabled = false;
                    MenuViewBackCard.IsChecked = false;

                    MenuViewRuler.Header = "Ruler for Selected Card Side";
                    MenuViewRuler.IsEnabled = false;

                    MenuViewZoomCard.Visibility = Visibility.Collapsed;
                    MenuViewZoomReport.Visibility = Visibility.Collapsed;
                    break;

                case ActiveViewMode.Report:
                    MenuViewFrontCard.IsEnabled = false;
                    MenuViewFrontCard.IsChecked = false;

                    MenuViewBackCard.IsEnabled = false;
                    MenuViewBackCard.IsChecked = false;

                    MenuViewRuler.Header = "Ruler for Report";
                    MenuViewRuler.IsEnabled = true;

                    MenuViewZoomCard.Visibility = Visibility.Collapsed;
                    MenuViewZoomReport.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SetGroupVisibility(Visibility vis)
        {
            ViewSep1.Visibility = vis;
            MenuViewFieldNames.Visibility = vis;
            MenuViewSampleData.Visibility = vis;
            
            ViewSep2.Visibility = vis;
            MenuViewCard.Visibility = vis;
            MenuViewReport.Visibility = vis;
            MenuViewProductionForm.Visibility = vis;
            MenuViewFieldConnector.Visibility = vis;

            ViewSep3.Visibility = vis;
            MenuViewFrontCard.Visibility = vis;
            MenuViewBackCard.Visibility = vis;

            ViewSep4.Visibility = vis;
            MenuViewRuler.Visibility = vis;

            ViewSep5.Visibility = vis;
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // ၁။ Title Bar စာသားအား Project Opened အခြေအနေသို့ ပြောင်းလဲခြင်း
            this.Title = "Datacard ID Works Enterprise Designer - Untitled - [Card]";

            // ၂။ Menu Bar ရှိ ကျန် Top Menus များကို ဖော်ပြပေးခြင်း (ပုံ ၄ ပါ အခြေအနေ)
            if (MenuEdit != null) MenuEdit.Visibility = Visibility.Visible;
            if (MenuInsert != null) MenuInsert.Visibility = Visibility.Visible;
            if (MenuFormat != null) MenuFormat.Visibility = Visibility.Visible;
            if (MenuTools != null) MenuTools.Visibility = Visibility.Visible;
            if (MenuWindow != null) MenuWindow.Visibility = Visibility.Visible;

            // File Submenus အပိုများကို ဖော်ပြပေးခြင်း
            if (Sep1 != null) Sep1.Visibility = Visibility.Visible;
            if (MenuCloseProject != null) MenuCloseProject.Visibility = Visibility.Visible;
            if (MenuSaveProject != null) MenuSaveProject.Visibility = Visibility.Visible;
            if (MenuSaveProjectAs != null) MenuSaveProjectAs.Visibility = Visibility.Visible;
            if (Sep2 != null) Sep2.Visibility = Visibility.Visible;
            if (MenuProjectProperties != null) MenuProjectProperties.Visibility = Visibility.Visible;
            if (Sep3 != null) Sep3.Visibility = Visibility.Visible;
            if (MenuPrintSampleCard != null) MenuPrintSampleCard.Visibility = Visibility.Visible;
            if (MenuPreviewSampleReport != null) MenuPreviewSampleReport.Visibility = Visibility.Visible;
            if (MenuReportPageSetup != null) MenuReportPageSetup.Visibility = Visibility.Visible;
            if (Sep4 != null) Sep4.Visibility = Visibility.Visible;
            if (MenuDeleteProject != null) MenuDeleteProject.Visibility = Visibility.Visible;

            // ၃။ View Menu Mode ကို Active (CardFront) သို့ ပြောင်းလဲပေးခြင်း
            SetViewMenuMode(ActiveViewMode.CardFront);

            // ၄။ Card Design Workspace Window အသစ်တစ်ခု တည်ဆောက်၍ Grid ထဲသို့ ထည့်သွင်းခြင်း
            CreateCardWorkspaceWindow();
        }

        private void CreateCardWorkspaceWindow()
        {
            MdiHostGrid.Children.Clear();

            // Designer Canvas Window Container တည်ဆောက်ခြင်း
            Border cardWindow = new Border
            {
                Width = 650,
                Height = 420,
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(20)
            };

            MdiHostGrid.Children.Add(cardWindow);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
