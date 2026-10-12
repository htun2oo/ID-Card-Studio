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

        // Card ပေါ်မှ Line ကို ပြန်လည်ဆွဲထုတ်/ရွှေ့ရန်အတွက်
        private Line? selectedGuideLine = null;
        private bool isDraggingExistingLine = false;

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

        // --- Ruler Drag Handling (Ruler ပေါ်မှ လိုင်းအသစ် ဆွဲထုတ်ခြင်း) ---

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

                    if (rulerType == "Top")
                    {
                        activeGuideLine.X1 = 0;
                        activeGuideLine.Y1 = cardPos.Y;
                        activeGuideLine.X2 = FrontCardOverlayCanvas.ActualWidth;
                        activeGuideLine.Y2 = cardPos.Y;
                    }
                    else if (rulerType == "Left")
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

        // --- Canvas Handling (Card ပေါ်ရှိ Line များကို ရွှေ့ခြင်း, ဖျက်ခြင်း နှင့် Mouse Cursor ပြောင်းလဲခြင်း) ---

        private void FrontCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                Point pos = e.GetPosition(canvas);

                foreach (var child in canvas.Children)
                {
                    if (child is Line line)
                    {
                        if (line.X1 == line.X2) // Vertical Line
                        {
                            if (Math.Abs(pos.X - line.X1) <= 8)
                            {
                                selectedGuideLine = line;
                                isDraggingExistingLine = true;
                                canvas.CaptureMouse();
                                e.Handled = true;
                                break;
                            }
                        }
                        else if (line.Y1 == line.Y2) // Horizontal Line
                        {
                            if (Math.Abs(pos.Y - line.Y1) <= 8)
                            {
                                selectedGuideLine = line;
                                isDraggingExistingLine = true;
                                canvas.CaptureMouse();
                                e.Handled = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void FrontCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                Point pos = e.GetPosition(canvas);

                if (isDraggingExistingLine && selectedGuideLine != null)
                {
                    // အကယ်၍ Line ကို Card ဧရိယာအပြင်ဘက်သို့ ဆွဲထုတ်ပါက ဖျက်ပစ်မည်
                    if (pos.X < -5 || pos.Y < -5 || pos.X > canvas.ActualWidth + 5 || pos.Y > canvas.ActualHeight + 5)
                    {
                        canvas.Children.Remove(selectedGuideLine);
                        canvas.ReleaseMouseCapture();
                        isDraggingExistingLine = false;
                        selectedGuideLine = null;
                        canvas.Cursor = Cursors.Arrow;
                    }
                    else
                    {
                        // Card အတွင်း၌ တည်နေရာ ပြန်လည်ရွှေ့ပေးမည်
                        if (selectedGuideLine.X1 == selectedGuideLine.X2) // Vertical Line
                        {
                            selectedGuideLine.X1 = pos.X;
                            selectedGuideLine.X2 = pos.X;
                        }
                        else // Horizontal Line
                        {
                            selectedGuideLine.Y1 = pos.Y;
                            selectedGuideLine.Y2 = pos.Y;
                        }
                    }
                }
                else
                {
                    // Photoshop ကဲ့သို့ Line ပေါ်သို့ Mouse တင်လိုက်လျှင် Cursor ပုံစံပြောင်းပေးခြင်း
                    bool isOverLine = false;
                    foreach (var child in canvas.Children)
                    {
                        if (child is Line line)
                        {
                            if (line.X1 == line.X2 && Math.Abs(pos.X - line.X1) <= 6) // Vertical Line (ဘယ်/ညာ မြားပုံစံ)
                            {
                                canvas.Cursor = Cursors.SizeWE;
                                isOverLine = true;
                                break;
                            }
                            else if (line.Y1 == line.Y2 && Math.Abs(pos.Y - line.Y1) <= 6) // Horizontal Line (အပေါ်/အောက် မြားပုံစံ)
                            {
                                canvas.Cursor = Cursors.SizeNS;
                                isOverLine = true;
                                break;
                            }
                        }
                    }

                    if (!isOverLine)
                    {
                        canvas.Cursor = Cursors.Arrow;
                    }
                }
            }
            e.Handled = true;
        }

        private void FrontCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas canvas)
            {
                if (isDraggingExistingLine)
                {
                    canvas.ReleaseMouseCapture();
                    isDraggingExistingLine = false;
                    selectedGuideLine = null;
                }
                canvas.Cursor = Cursors.Arrow;
                e.Handled = true;
            }
        }
    }
}
