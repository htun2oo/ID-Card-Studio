using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public enum ActiveViewMode
    {
        None,           // Open Project မဖွင့်မီ Initial State (Figure 1)
        CardFront,      // Card Front Select ထားချိန် (Figure 2)
        CardBack,       // Card Back Side Select ထားချိန် (Figure 3)
        ProductionForm, // Production Form Select ထားချိန် (Figure 4)
        Report          // Report Window Select ထားချိန် (Figure 5)
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // မူလ စတင်ချိန်တွင် ပထမပုံအတိုင်း ထားရှိပါမည်
            SetViewMenuMode(ActiveViewMode.None);
        }

        public void SetViewMenuMode(ActiveViewMode mode)
        {
            if (mode == ActiveViewMode.None)
            {
                // ပထမပုံ: Toolbars နှင့် Show Welcome သာ ဖော်ပြမည်
                SetGroupVisibility(Visibility.Collapsed);
                return;
            }

            // Project/Window ပွင့်နေပါက View Menu ၏ Group များကို ဖော်ပြမည်
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
            // New Project နှိပ်လိုက်သောအခါ Edit Menu နှင့် View Menu ၏ Items များကို ဖွင့်ပေးမည်ဖြစ်သည်
            MenuEdit.Visibility = Visibility.Visible;
            MenuInsert.Visibility = Visibility.Visible;
            MenuFormat.Visibility = Visibility.Visible;
            MenuTools.Visibility = Visibility.Visible;
            MenuWindow.Visibility = Visibility.Visible;

            // Project ဖွင့်လိုက်သည်နှင့် Card Front Selected Mode သို့ ပြောင်းလဲပေးမည်
            SetViewMenuMode(ActiveViewMode.CardFront);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
