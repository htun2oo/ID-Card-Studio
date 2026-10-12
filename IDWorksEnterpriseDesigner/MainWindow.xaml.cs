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

        // Top Ruler မှ Mouse ဖြင့် နှိပ်ချလိုက်ချိန် (Horizontal Ruler)
        private void TopRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                activeCanvas = canvas;
                isDraggingGuide = true;
                canvas.CaptureMouse();

                Point pos = e.GetPosition(canvas);

                // Card Overlay Canvas ပေါ်တွင် ဒေါင်လိုက် (Vertical) မျဉ်းကြောင်း ဖန်တီးခြင်း
                activeGuideLine = new Line()
                {
                    X1 = pos.X,
                    Y1 = 0,
                    X2 = pos.X,
                    Y2 = FrontCardOverlayCanvas.ActualHeight,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                };

                FrontCardOverlayCanvas.Children.Add(activeGuideLine);
                e.Handled = true; // Event အခြားနေရာသို့ မပျံ့နှံ့စေရန် တားဆီးခြင်း
            }
        }

        // Left Ruler မှ Mouse ဖြင့် နှိပ်ချလိုက်ချိန် (Vertical Ruler)
        private void LeftRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                activeCanvas = canvas;
                isDraggingGuide = true;
                canvas.CaptureMouse();

                Point pos = e.GetPosition(canvas);

                // Card Overlay Canvas ပေါ်တွင် အလျားလိုက် (Horizontal) မျဉ်းကြောင်း ဖန်တီးခြင်း
                activeGuideLine = new Line()
                {
                    X1 = 0,
                    Y1 = pos.Y,
                    X2 = FrontCardOverlayCanvas.ActualWidth,
                    Y2 = pos.Y,
                    Stroke = Brushes.Red,
                    StrokeThickness = 1,
                    StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                };

                FrontCardOverlayCanvas.Children.Add(activeGuideLine);
                e.Handled = true;
            }
        }

        // Mouse ဆွဲနေစဉ် (Drag လုပ်နေစဉ်) မျဉ်းကြောင်း လိုက်ပါရွေ့လျားစေရန်
        private void Ruler_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingGuide && activeGuideLine != null && activeCanvas != null)
            {
                // Card Overlay Canvas ပေါ်ရှိ Mouse တည်နေရာကို တိုက်ရယူခြင်း
                Point pos = e.GetPosition(FrontCardOverlayCanvas);
                
                if (activeGuideLine.Y1 == activeGuideLine.Y2) // Horizontal Line (Left Ruler မှဆွဲလျှင်)
                {
                    activeGuideLine.Y1 = pos.Y;
                    activeGuideLine.Y2 = pos.Y;
                }
                else // Vertical Line (Top Ruler မှဆွဲလျှင်)
                {
                    activeGuideLine.X1 = pos.X;
                    activeGuideLine.X2 = pos.X;
                }
                e.Handled = true;
            }
        }

        // Mouse ကို လွှတ်လိုက်ချိန် (Drop လုပ်လိုက်ချိန်)
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
