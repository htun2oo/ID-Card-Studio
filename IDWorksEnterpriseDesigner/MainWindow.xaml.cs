using System.Windows;
using System.Windows.Controls;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // Show project workspace when New Project is clicked
            ProjectWorkspace.Visibility = Visibility.Visible;
            MenuCloseProject.Visibility = Visibility.Visible;
            MenuSaveProject.Visibility = Visibility.Visible;
            MenuSaveProjectAs.Visibility = Visibility.Visible;
            MenuProjectProperties.Visibility = Visibility.Visible;
            MenuPrintSampleCard.Visibility = Visibility.Visible;
            MenuPreviewSampleReport.Visibility = Visibility.Visible;
            MenuDeleteProject.Visibility = Visibility.Visible;
            Sep1.Visibility = Visibility.Visible;
            Sep3.Visibility = Visibility.Visible;
            Sep4.Visibility = Visibility.Visible;

            MenuEdit.Visibility = Visibility.Visible;
            MenuFormat.Visibility = Visibility.Visible;
            MenuTools.Visibility = Visibility.Visible;
            MenuWindow.Visibility = Visibility.Visible;

            ViewSep1.Visibility = Visibility.Visible;
            MenuViewFieldNames.Visibility = Visibility.Visible;
            MenuViewSampleData.Visibility = Visibility.Visible;
            ViewSep2.Visibility = Visibility.Visible;
            MenuViewCard.Visibility = Visibility.Visible;
            MenuViewReport.Visibility = Visibility.Visible;
            MenuViewProductionForm.Visibility = Visibility.Visible;
            MenuViewFieldConnector.Visibility = Visibility.Visible;
            ViewSep3.Visibility = Visibility.Visible;
            MenuViewFrontCard.Visibility = Visibility.Visible;
            MenuViewBackCard.Visibility = Visibility.Visible;
            ViewSep4.Visibility = Visibility.Visible;
            MenuViewRuler.Visibility = Visibility.Visible;
        }

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report Page Setup dialog will open here.", "Report Page Setup", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Report Document Size Changer based on Dropdown Selection (1 inch = 37.8 pixels)
        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportDocumentBorder != null && ReportSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString();
                switch (tag)
                {
                    case "A4":
                        // A4: 8.3 x 11.7 inches -> ~314 x 442 pixels
                        ReportDocumentBorder.Width = 314;
                        ReportDocumentBorder.Height = 442;
                        break;
                    case "A5":
                        // A5: 5.8 x 8.3 inches -> ~219 x 314 pixels
                        ReportDocumentBorder.Width = 219;
                        ReportDocumentBorder.Height = 314;
                        break;
                    case "B5":
                        // B5: 6.9 x 9.8 inches -> ~261 x 370 pixels
                        ReportDocumentBorder.Width = 261;
                        ReportDocumentBorder.Height = 370;
                        break;
                    case "Letter":
                        // Letter: 8.5 x 11 inches -> ~321 x 416 pixels
                        ReportDocumentBorder.Width = 321;
                        ReportDocumentBorder.Height = 416;
                        break;
                    case "Legal":
                        // Legal: 8.5 x 14 inches -> ~321 x 529 pixels
                        ReportDocumentBorder.Width = 321;
                        ReportDocumentBorder.Height = 529;
                        break;
                }
            }
        }
    }
}
