using System;
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

        // Top Ruler (အပေါ်ဘက် ပေတံ) - အောက်သို့ဆွဲချလျှင် Horizontal Line ထွက်မည်
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

        // Left Ruler (ဘယ်ဘက် ပေတံ) - ညာဘက်သို့ဆွဲထုတ်လျှင် Vertical Line ထွက်မည်
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

        // Mouse Drag လုပ်နေစဉ် မျဉ်းကြောင်း အသစ်ပေါ်လာပြီး လိုက်ပါရွေ့လျားမည်
        private void Ruler_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDownOnRuler && activeCanvas != null)
            {
                Point currentPos = e.GetPosition(activeCanvas);

                if (!isDraggingGuide && (Math.Abs(currentPos.X - mouseDownPos.X) > 3 || Math.Abs(currentPos.Y - mouseDownPos.Y) > 3))
                {
                    isDraggingGuide = true;

                    activeGuideLine = new Line()
                    {
                        Stroke = Brushes.Red,
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection(new double[] { 2, 2 })
                    };

                    Point cardPos = e.GetPosition(FrontCardOverlayCanvas);

                    if (rulerType == "Top") // Top ruler မှဆွဲလျှင် အလျားလိုက် (Horizontal) မျဉ်း
                    {
                        activeGuideLine.X1 = 0;
                        activeGuideLine.Y1 = cardPos.Y;
                        activeGuideLine.X2 = FrontCardOverlayCanvas.ActualWidth;
                        activeGuideLine.Y2 = cardPos.Y;
                    }
                    else if (rulerType == "Left") // Left ruler မှဆွဲလျှင် ဒေါင်လိုက် (Vertical) မျဉ်း
                    {
                        activeGuideLine.X1 = cardPos.X;
                        activeGuideLine.Y1 = 0;
                        activeGuideLine.X2 = cardPos.X;
                        activeGuideLine.Y2 = FrontCardOverlayCanvas.ActualHeight;
                    }

                    FrontCardOverlayCanvas.Children.Add(activeGuideLine);
                }
                else if (isDraggingGuide && activeGuideLine != null)
                {
                    Point cardPos = e.GetPosition(FrontCardOverlayCanvas);

                    if (rulerType == "Top")
                    {
                        activeGuideLine.Y1 = cardPos.Y;
                        activeGuideLine.Y2 = cardPos.Y;
                    }
                    else if (rulerType == "Left")
                    {
                        activeGuideLine.X1 = cardPos.X;
                        activeGuideLine.X2 = cardPos.X;
                    }
                }
            }
            e.Handled = true;
        }

        // Mouse လွှတ်လိုက်ချိန်
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
