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

        // Card ပေါ်မှ Line ကို ပြန်လည်ဆွဲထုတ်ရန်အတွက်
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

                    // Line ကို Mouse ဖြင့် ထပ်မံဖမ်းဆွဲနိုင်ရန် Event ချိတ်ပေးခြင်း
                    activeGuideLine.MouseLeftButtonDown += GuideLine_MouseLeftButtonDown;

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

        // Card ပေါ်ရှိ ဇယားကွက်လိုင်းကို Mouse ဖြင့် ကလစ်နှိပ်ပြီး ဖမ်းဆွဲရန်
        private void GuideLine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Line line)
            {
                selectedGuideLine = line;
                isDraggingExistingLine = true;
                line.CaptureMouse();
                e.Handled = true;
            }
        }

        // Card ပေါ်တွင် Mouse ဖြင့် Line ကို ဆွဲရွှေ့နေစဉ် (Ruler သို့မဟုတ် အပြင်ဘက်သို့ ရောက်လျှင် ဖျက်ပစ်ရန်)
        private void FrontCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingExistingLine && selectedGuideLine != null)
            {
                Point pos = e.GetPosition(FrontCardOverlayCanvas);

                // အကယ်၍ Line ကို Card ဧရိယာ၏ အနားစွန်းများ (သို့မဟုတ် ပေတံတည်ရှိရာ အပြင်ဘက်) သို့ ပြန်ဆွဲထုတ်သွားပါက ဖျက်ပစ်မည်
                if (pos.X < 5 || pos.Y < 5 || pos.X > FrontCardOverlayCanvas.ActualWidth - 5 || pos.Y > FrontCardOverlayCanvas.ActualHeight - 5)
                {
                    FrontCardOverlayCanvas.Children.Remove(selectedGuideLine);
                    isDraggingExistingLine = false;
                    selectedGuideLine = null;
                }
                else
                {
                    // Card အတွင်းတွင် ဆက်လက်ရွှေ့ပြောင်းပေးမည်
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
                e.Handled = true;
            }
        }

        // Mouse ကို လွှတ်လိုက်ချိန်
        private void FrontCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDraggingExistingLine && selectedGuideLine != null)
            {
                selectedGuideLine.ReleaseMouseCapture();
                isDraggingExistingLine = false;
                selectedGuideLine = null;
            }
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
    }
}
