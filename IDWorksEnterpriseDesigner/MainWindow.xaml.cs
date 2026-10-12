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
        private bool isDraggingFront = false;
        private Line? currentFrontLine;
        private Point frontStartPoint;

        private bool isDraggingBack = false;
        private Line? currentBackLine;
        private Point backStartPoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Visible;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report Page Setup clicked.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportSizeComboBox == null || ReportDocumentBorder == null) return;

            ComboBoxItem? selectedItem = ReportSizeComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string? tag = selectedItem.Tag?.ToString();
                switch (tag)
                {
                    case "A4":
                        ReportDocumentBorder.Width = 314;
                        ReportDocumentBorder.Height = 442;
                        break;
                    case "A5":
                        ReportDocumentBorder.Width = 221;
                        ReportDocumentBorder.Height = 314;
                        break;
                    case "B5":
                        ReportDocumentBorder.Width = 263;
                        ReportDocumentBorder.Height = 372;
                        break;
                    case "Letter":
                        ReportDocumentBorder.Width = 324;
                        ReportDocumentBorder.Height = 420;
                        break;
                    case "Legal":
                        ReportDocumentBorder.Width = 324;
                        ReportDocumentBorder.Height = 534;
                        break;
                }
            }
        }

        private void TopRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void LeftRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void Ruler_MouseMove(object sender, MouseEventArgs e) { }
        private void Ruler_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void FrontCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FrontCardOverlayCanvas == null) return;

            isDraggingFront = true;
            frontStartPoint = e.GetPosition(FrontCardOverlayCanvas);

            currentFrontLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 },
                X1 = frontStartPoint.X,
                Y1 = 0,
                X2 = frontStartPoint.X,
                Y2 = FrontCardOverlayCanvas.ActualHeight
            };

            FrontCardOverlayCanvas.Children.Add(currentFrontLine);
            FrontCardOverlayCanvas.CaptureMouse();
        }

        private void FrontCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingFront || currentFrontLine == null || FrontCardOverlayCanvas == null) return;

            Point currentPoint = e.GetPosition(FrontCardOverlayCanvas);
            currentFrontLine.X1 = currentPoint.X;
            currentFrontLine.X2 = currentPoint.X;
        }

        private void FrontCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDraggingFront)
            {
                isDraggingFront = false;
                if (FrontCardOverlayCanvas != null)
                {
                    FrontCardOverlayCanvas.ReleaseMouseCapture();
                }
                currentFrontLine = null;
            }
        }

        private void BackCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (BackCardOverlayCanvas == null) return;

            isDraggingBack = true;
            backStartPoint = e.GetPosition(BackCardOverlayCanvas);

            currentBackLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 },
                X1 = backStartPoint.X,
                Y1 = 0,
                X2 = backStartPoint.X,
                Y2 = BackCardOverlayCanvas.ActualHeight
            };

            BackCardOverlayCanvas.Children.Add(currentBackLine);
            BackCardOverlayCanvas.CaptureMouse();
        }

        private void BackCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingBack || currentBackLine == null || BackCardOverlayCanvas == null) return;

            Point currentPoint = e.GetPosition(BackCardOverlayCanvas);
            currentBackLine.X1 = currentPoint.X;
            currentBackLine.X2 = currentPoint.X;
        }

        private void BackCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDraggingBack)
            {
                isDraggingBack = false;
                if (BackCardOverlayCanvas != null)
                {
                    BackCardOverlayCanvas.ReleaseName("BackCardOverlayCanvas"); // Safe release check
                    BackCardOverlayCanvas.ReleaseMouseCapture();
                }
                currentBackLine = null;
            }
        }
    }
}
