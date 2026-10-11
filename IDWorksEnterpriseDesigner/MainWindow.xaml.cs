using System.Windows;
using System.Windows.Controls;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // အပလီကေးရှင်းစတင်ချိန်တွင် Workspace ကို ပိတ်ထားမည် (ဗလာဖြစ်နေမည်)
            HideWorkspace();
        }

        private void HideWorkspace()
        {
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Collapsed;
                MenuCloseProject.Visibility = Visibility.Collapsed;
                MenuSaveProject.Visibility = Visibility.Collapsed;
                MenuSaveProjectAs.Visibility = Visibility.Collapsed;
                MenuProjectProperties.Visibility = Visibility.Collapsed;
                MenuPrintSampleCard.Visibility = Visibility.Collapsed;
                MenuPreviewSampleReport.Visibility = Visibility.Collapsed;
                MenuDeleteProject.Visibility = Visibility.Collapsed;
                MenuEdit.Visibility = Visibility.Collapsed;
                MenuFormat.Visibility = Visibility.Collapsed;
                MenuTools.Visibility = Visibility.Collapsed;
                MenuWindow.Visibility = Visibility.Collapsed;
                Sep1.Visibility = Visibility.Collapsed;
                ViewSep1.Visibility = Visibility.Collapsed;
                ViewSep2.Visibility = Visibility.Collapsed;
                ViewSep3.Visibility = Visibility.Collapsed;
                ViewSep4.Visibility = Visibility.Collapsed;
                MenuViewFieldNames.Visibility = Visibility.Collapsed;
                MenuViewSampleData.Visibility = Visibility.Collapsed;
                MenuViewCard.Visibility = Visibility.Collapsed;
                MenuViewReport.Visibility = Visibility.Collapsed;
                MenuViewProductionForm.Visibility = Visibility.Collapsed;
                MenuViewFieldConnector.Visibility = Visibility.Collapsed;
                MenuViewFrontCard.Visibility = Visibility.Collapsed;
                MenuViewBackCard.Visibility = Visibility.Collapsed;
                MenuViewRuler.Visibility = Visibility.Collapsed;
                MenuViewZoomCard.Visibility = Visibility.Collapsed;
                MenuViewZoomReport.Visibility = Visibility.Collapsed;
            }
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // မူရင်းကဲ့သို့ New Project Window ကို ပေါ်လာစေရန်
            NewProjectWindow newProjWin = new NewProjectWindow();
            newProjWin.Owner = this;
            
            if (newProjWin.ShowDialog() == true)
            {
                // OK နှိပ်ပြီးမှ Workspace ကို ဖွင့်ပေးမည်
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
