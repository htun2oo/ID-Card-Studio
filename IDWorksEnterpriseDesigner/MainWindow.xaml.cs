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
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Visible;
                MenuCloseProject.Visibility = Visibility.Visible;
                MenuSaveProject.Visibility = Visibility.Visible;
                MenuSaveProjectAs.Visibility = Visibility.Visible;
                MenuProjectProperties.Visibility = Visibility.Visible;
                MenuPrintSampleCard.Visibility = Visibility.Visible;
                MenuPreviewSampleReport.Visibility = Visibility.Visible;
                MenuDeleteProject.Visibility = Visibility.Visible;
                MenuEdit.Visibility = Visibility.Visible;
                MenuFormat.Visibility = Visibility.Visible;
                MenuTools.Visibility = Visibility.Visible;
                MenuWindow.Visibility = Visibility.Visible;
                Sep1.Visibility = Visibility.Visible;
                ViewSep1.Visibility = Visibility.Visible;
                ViewSep2.Visibility = Visibility.Visible;
                ViewSep3.Visibility = Visibility.Visible;
                ViewSep4.Visibility = Visibility.Visible;
                ViewSep5.Visibility = Visibility.Visible;
                MenuViewFieldNames.Visibility = Visibility.Visible;
                MenuViewSampleData.Visibility = Visibility.Visible;
                MenuViewCard.Visibility = Visibility.Visible;
                MenuViewReport.Visibility = Visibility.Visible;
                MenuViewProductionForm.Visibility = Visibility.Visible;
                MenuViewFieldConnector.Visibility = Visibility.Visible;
                MenuViewFrontCard.Visibility = Visibility.Visible;
                MenuViewBackCard.Visibility = Visibility.Visible;
                MenuViewRuler.Visibility = Visibility.Visible;
                MenuViewZoomCard.Visibility = Visibility.Visible;
                MenuViewZoomReport.Visibility = Visibility.Visible;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportSizeComboBox != null && ReportDocumentBorder != null)
            {
                ComboBoxItem selectedItem = ReportSizeComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    string tag = selectedItem.Tag.ToString();
                    switch (tag)
                    {
                        case "A4":
                            ReportDocumentBorder.Width = 450;
                            ReportDocumentBorder.Height = 635;
                            break;
                        case "A5":
                            ReportDocumentBorder.Width = 320;
                            ReportDocumentBorder.Height = 450;
                            break;
                        case "B5":
                            ReportDocumentBorder.Width = 380;
                            ReportDocumentBorder.Height = 540;
                            break;
                        case "Letter":
                            ReportDocumentBorder.Width = 460;
                            ReportDocumentBorder.Height = 595;
                            break;
                        case "Legal":
                            ReportDocumentBorder.Width = 460;
                            ReportDocumentBorder.Height = 756;
                            break;
                    }
                }
            }
        }

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report page setup can be selected directly from the dropdown menu on the top-right of the Report window.", "Report Page Setup", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
