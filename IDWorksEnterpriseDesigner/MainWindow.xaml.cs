using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        private bool isMouseDownOnRuler = false;
        private bool isDraggingGuide = false;
        private Line? activeGuideLine = null;
        private Canvas? activeCanvas = null;
        private string rulerType = ""; // "Top" သို့မဟုတ် "Left"
        private Point mouseDownPos;

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

        // Top Ruler တွင် Click နှိပ်လိုက်ချိန် (လိုင်းချက်ချင်းမဆွဲပါ)
        private void TopRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                activeCanvas = canvas;
                isMouseDownOnRuler = true;
                rulerType = "Top";
                mouseDownPos = e.GetPosition(canvas);
                canvas.CaptureMouse();
                e.Handled = true;
            }
        }

        // Left Ruler တွင် Click နှိပ်လိုက်ချိန် (လိုင်းချက်ချင်းမဆွဲပါ)
        private void LeftRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                activeCanvas = canvas;
                isMouseDownOnRuler = true;
                rulerType = "Left";
                mouseDownPos = e.GetPosition(canvas);
                canvas.CaptureMouse();
                e.Handled = true;
            }
        }

        // Mouse ကို ဖိပြီး ရွှေ့နေစဉ် (Drag လုပ်မှသာ Line အသစ်စတင်ပေါ်လာမည်)
        private void Ruler_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDownOnRuler && activeCanvas != null)
            {
                Point currentPos = e.GetPosition(activeCanvas);

                // Mouse ကို အနည်းငယ် (ဥပမာ ၃ ပစ်ဇယ်ထက်ပို၍) ရွှေ့မှသာ Drag လုပ်သည်ဟု သတ်မှတ်မည်
                if (!isDraggingGuide && (Math.Abs(currentPos.X - mouseDownPos.X) > 3 || Math.Abs(currentPos.Y - mouseDownPos.Y) > 3))
                {
                    isDraggingGuide = true;

                    activeGuideLine = new Line()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                    };

                    if (rulerType == "Top")
                    {
                        Point cardPos = e.GetPosition(FrontCardOverlayCanvas);
                        activeGuideLine.X1 = cardPos.X;
                        activeGuideLine.Y1 = 0;
                        activeGuideLine.X2 = cardPos.X;
                        activeGuideLine.Y2 = FrontCardOverlayCanvas.ActualHeight;
                    }
                    else if (rulerType == "Left")
                    {
                        Point cardPos = e.GetPosition(FrontCardOverlayCanvas);
                        activeGuideLine.X1 = 0;
                        activeGuideLine.Y1 = cardPos.Y;
                        activeGuideLine.X2 = FrontCardOverlayCanvas.ActualWidth;
                        activeGuideLine.Y2 = cardPos.Y;
                    }

                    FrontCardOverlayCanvas.Children.Add(activeGuideLine);
                }
                else if (isDraggingGuide && activeGuideLine != null)
                {
                    // Drag ဆွဲနေစဉ် Mouse တည်နေရာအလိုက် လိုင်းရွေ့လျားစေရန်
                    Point cardPos = e.GetPosition(FrontCardOverlayCanvas);

                    if (rulerType == "Top")
                    {
                        activeGuideLine.X1 = cardPos.X;
                        activeGuideLine.X2 = cardPos.X;
                    }
                    else if (rulerType == "Left")
                    {
                        activeGuideLine.Y1 = cardPos.Y;
                        activeGuideLine.Y2 = cardPos.Y;
                    }
                }
            }
            e.Handled = true;
        }

        // Mouse ကို လွှတ်လိုက်ချိန်
        private void Ruler_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (activeCanvas != null)
            {
                activeCanvas.ReleaseMouseCapture();
            }

            isMouseDownOnRuler = false;
            isDraggingGuide = false;
            activeGuideLine = null;
            activeCanvas = null;
            rulerType = "";
            e.Handled = true;
        }
    }
}
