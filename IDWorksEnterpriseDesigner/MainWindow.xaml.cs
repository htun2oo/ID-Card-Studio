using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    public enum ActiveViewMode
    {
        None,           // Open Project မဖွင့်မီ Initial State
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
            SetViewMenuMode(ActiveViewMode.None);
        }

        public void SetViewMenuMode(ActiveViewMode mode)
        {
            if (mode == ActiveViewMode.None)
            {
                SetGroupVisibility(Visibility.Collapsed);
                return;
            }

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
            // New Project Click Handler
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
