private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (ReportSizeComboBox != null && ReportDocumentBorder != null)
    {
        ComboBoxItem selectedItem = ReportSizeComboBox.SelectedItem as ComboBoxItem;
        if (selectedItem != null)
        {
            string tag = selectedItem.Tag.ToString();
            // WPF pixels per inch conversion (Standard 96 DPI)
            double dpi = 96.0;

            switch (tag)
            {
                case "A4": // 8.27 x 11.69 inches -> Scaled down nicely for UI workspace
                    ReportDocumentBorder.Width = 450;
                    ReportDocumentBorder.Height = 635;
                    break;
                case "A5": // 5.83 x 8.27 inches
                    ReportDocumentBorder.Width = 320;
                    ReportDocumentBorder.Height = 450;
                    break;
                case "B5": // 6.93 x 9.84 inches
                    ReportDocumentBorder.Width = 380;
                    ReportDocumentBorder.Height = 540;
                    break;
                case "Letter": // 8.5 x 11 inches
                    ReportDocumentBorder.Width = 460;
                    ReportDocumentBorder.Height = 595;
                    break;
                case "Legal": // 8.5 x 14 inches
                    ReportDocumentBorder.Width = 460;
                    ReportDocumentBorder.Height = 756;
                    break;
            }
        }
    }
}

private void ReportPageSetup_Click(object sender, RoutedEventArgs e)
{
    // Optional dialog handler or direct focus to size selector
    MessageBox.Show("Report page setup can be selected directly from the dropdown menu on the top-right of the Report window.", "Report Page Setup", MessageBoxButton.OK, MessageBoxImage.Information);
}
