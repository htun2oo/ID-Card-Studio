using System.Windows;
using System.Windows.Controls;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Software စတင်ဖွင့်လိုက်ချိန်တွင် Workspace ကို ပုံမှန်ပိတ်ထားမည် (Main Window သက်သက်သာ ပေါ်မည်)
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Collapsed;
            }
        }

private void NewProject_Click(object sender, RoutedEventArgs e)
{
    // New Project Dialog Window ကို ခေါ်ယူပြသခြင်း
    NewProjectWindow newProjectDlg = new NewProjectWindow();
    if (newProjectDlg.ShowDialog() == true)
    {
        // Dialog တွင် OK နှိပ်မှသာ Workspace နှင့် Menu များ ပေါ်လာမည်
        if (ProjectWorkspace != null)
        {
            ProjectWorkspace.Visibility = Visibility.Visible;
        }

        if (MenuCloseProject != null) MenuCloseProject.Visibility = Visibility.Visible;
        if (MenuSaveProject != null) MenuSaveProject.Visibility = Visibility.Visible;
        if (MenuSaveProjectAs != null) MenuSaveProjectAs.Visibility = Visibility.Visible;
        if (MenuProjectProperties != null) MenuProjectProperties.Visibility = Visibility.Visible;
        if (MenuPrintSampleCard != null) MenuPrintSampleCard.Visibility = Visibility.Visible;
        if (MenuPreviewSampleReport != null) MenuPreviewSampleReport.Visibility = Visibility.Visible;
        if (MenuDeleteProject != null) MenuDeleteProject.Visibility = Visibility.Visible;
        
        if (Sep1 != null) Sep1.Visibility = Visibility.Visible;
        if (Sep3 != null) Sep3.Visibility = Visibility.Visible;
        if (Sep4 != null) Sep4.Visibility = Visibility.Visible;

        if (MenuEdit != null) MenuEdit.Visibility = Visibility.Visible;
        if (MenuFormat != null) MenuFormat.Visibility = Visibility.Visible;
        if (MenuTools != null) MenuTools.Visibility = Visibility.Visible;
        if (MenuWindow != null) MenuWindow.Visibility = Visibility.Visible;

        if (ViewSep1 != null) ViewSep1.Visibility = Visibility.Visible;
        if (MenuViewFieldNames != null) MenuViewFieldNames.Visibility = Visibility.Visible;
        if (MenuViewSampleData != null) MenuViewSampleData.Visibility = Visibility.Visible;
        if (ViewSep2 != null) ViewSep2.Visibility = Visibility.Visible;
        if (MenuViewCard != null) MenuViewCard.Visibility = Visibility.Visible;
        if (MenuViewReport != null) MenuViewReport.Visibility = Visibility.Visible;
        if (MenuViewProductionForm != null) MenuViewProductionForm.Visibility = Visibility.Visible;
        if (MenuViewFieldConnector != null) MenuViewFieldConnector.Visibility = Visibility.Visible;
        if (ViewSep3 != null) ViewSep3.Visibility = Visibility.Visible;
        if (MenuViewFrontCard != null) MenuViewFrontCard.Visibility = Visibility.Visible;
        if (MenuViewBackCard != null) MenuViewBackCard.Visibility = Visibility.Visible;
        if (ViewSep4 != null) ViewSep4.Visibility = Visibility.Visible;
        if (MenuViewRuler != null) MenuViewRuler.Visibility = Visibility.Visible;
    }
}

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report Page Setup dialog will open here.", "Report Page Setup", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportDocumentBorder != null && ReportSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString();
                switch (tag)
                {
                    case "A4":
                        ReportDocumentBorder.Width = 314;
                        ReportDocumentBorder.Height = 442;
                        break;
                    case "A5":
                        ReportDocumentBorder.Width = 219;
                        ReportDocumentBorder.Height = 314;
                        break;
                    case "B5":
                        ReportDocumentBorder.Width = 261;
                        ReportDocumentBorder.Height = 370;
                        break;
                    case "Letter":
                        ReportDocumentBorder.Width = 321;
                        ReportDocumentBorder.Height = 416;
                        break;
                    case "Legal":
                        ReportDocumentBorder.Width = 321;
                        ReportDocumentBorder.Height = 529;
                        break;
                }
            }
        }
    }
}
