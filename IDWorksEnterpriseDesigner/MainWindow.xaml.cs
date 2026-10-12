using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        private bool isDraggingGuide = false;
        private Line? activeGuideLine = null;
        private Canvas? activeCanvas = null;
        private Point startPoint;

        public MainWindow()
        {
            InitializeComponent();
            
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Collapsed;
            }
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow newProjectDlg = new NewProjectWindow();
            newProjectDlg.Owner = this;
            
            if (newProjectDlg.ShowDialog() == true)
            {
                if (ProjectWorkspace != null) ProjectWorkspace.Visibility = Visibility.Visible;

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
                string? tag = selectedItem.Tag?.ToString();
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

        // Top Ruler (Horizontal) - Ruler မှနေ၍ Card ပေါ်သို့ မျဉ်းဆွဲချခြင်း
        private void TopRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                activeCanvas = canvas;
                isDraggingGuide = true;
                canvas.CaptureMouse();

                startPoint = e.GetPosition(canvas);

                // FrontCardOverlayCanvas ထဲသို့ ဒေါင်လိုက် (Vertical) Guide Line အသစ်ထည့်သွင်းခြင်း
                activeGuideLine = new Line()
                {
                    X1 = startPoint.X,
                    Y1 = 0,
                    X2 = startPoint.X,
                    Y2 = FrontCardOverlayCanvas.ActualHeight > 0 ? FrontCardOverlayCanvas.ActualHeight : 204.04,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                };

                FrontCardOverlayCanvas.Children.Add(activeGuideLine);
                e.Handled = true;
            }
        }

        // Left Ruler (Vertical) - Ruler မှနေ၍ Card ပေါ်သို့ အလျားလိုက် မျဉ်းဆွဲချခြင်း
        private void LeftRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                activeCanvas = canvas;
                isDraggingGuide = true;
                canvas.CaptureMouse();

                startPoint = e.GetPosition(canvas);

                // FrontCardOverlayCanvas ထဲသို့ အလျားလိုက် (Horizontal) Guide Line အသစ်ထည့်သွင်းခြင်း
                activeGuideLine = new Line()
                {
                    X1 = 0,
                    Y1 = startPoint.Y,
                    X2 = FrontCardOverlayCanvas.ActualWidth > 0 ? FrontCardOverlayCanvas.ActualWidth : 323.57,
                    Y2 = startPoint.Y,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                };

                FrontCardOverlayCanvas.Children.Add(activeGuideLine);
                e.Handled = true;
            }
        }

        private void Ruler_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingGuide && activeGuideLine != null && activeCanvas != null)
            {
                Point pos = e.GetPosition(activeCanvas);
                
                // ဆွဲနေစဉ် Mouse တည်နေရာအလိုက် Guide Line ရွေ့လျားစေရန်
                if (activeGuideLine.X1 == activeGuideLine.X2) // Vertical Line (Top Ruler မှဆွဲလျှင်)
                {
                    activeGuideLine.X1 = pos.X;
                    activeGuideLine.X2 = pos.X;
                }
                else // Horizontal Line (Left Ruler မှဆွဲလျှင်)
                {
                    activeGuideLine.Y1 = pos.Y;
                    activeGuideLine.Y2 = pos.Y;
                }
                e.Handled = true;
            }
        }

        private void Ruler_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDraggingGuide && activeCanvas != null)
            {
                isDraggingGuide = false;
                activeCanvas.ReleaseMouseCapture();
                activeGuideLine = null;
                activeCanvas = null;
                e.Handled = true;
            }
        }
    }
}
