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

        private bool isDraggingBack = false;
        private Line? currentBackLine;

        private bool isDraggingReport = false;
        private Line? currentReportLine;

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
            if (ReportSizeComboBox == null || ReportDocumentBorder == null || ReportOverlayCanvas == null) return;

            ComboBoxItem? selectedItem = ReportSizeComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string? tag = selectedItem.Tag?.ToString();
                double newWidth = 793.8;
                double newHeight = 1122.6;

                switch (tag)
                {
                    case "A4": // 21.0 x 29.7 cm
                        newWidth = 793.8; newHeight = 1122.6;
                        break;
                    case "A5": // 14.8 x 21.0 cm
                        newWidth = 559.4; newHeight = 793.8;
                        break;
                    case "B5": // 17.6 x 25.0 cm
                        newWidth = 665.3; newHeight = 945.0;
                        break;
                    case "Letter": // 8.5 x 11 in
                        newWidth = 816.0; newHeight = 1056.0;
                        break;
                    case "Legal": // 8.5 x 14 in
                        newWidth = 816.0; newHeight = 1344.0;
                        break;
                }

                ReportDocumentBorder.Width = newWidth;
                ReportDocumentBorder.Height = newHeight;
                ReportOverlayCanvas.Width = newWidth;
                ReportOverlayCanvas.Height = newHeight;
            }
        }

        // --- Front Card Events ---
        private void FrontCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FrontCardOverlayCanvas == null) return;
            isDraggingFront = true;
            Point startPoint = e.GetPosition(FrontCardOverlayCanvas);

            currentFrontLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 },
                X1 = startPoint.X,
                Y1 = 0,
                X2 = startPoint.X,
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
                FrontCardOverlayCanvas?.ReleaseMouseCapture();
                currentFrontLine = null;
            }
        }

        // --- Back Card Events ---
        private void BackCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (BackCardOverlayCanvas == null) return;
            isDraggingBack = true;
            Point startPoint = e.GetPosition(BackCardOverlayCanvas);

            currentBackLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 },
                X1 = startPoint.X,
                Y1 = 0,
                X2 = startPoint.X,
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
                BackCardOverlayCanvas?.ReleaseMouseCapture();
                currentBackLine = null;
            }
        }

        // --- Report Window Events ---
        private void ReportOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ReportOverlayCanvas == null) return;
            isDraggingReport = true;
            Point startPoint = e.GetPosition(ReportOverlayCanvas);

            currentReportLine = new Line
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 2, 2 },
                X1 = startPoint.X,
                Y1 = 0,
                X2 = startPoint.X,
                Y2 = ReportOverlayCanvas.ActualHeight
            };

            ReportOverlayCanvas.Children.Add(currentReportLine);
            ReportOverlayCanvas.CaptureMouse();
        }

        private void ReportOverlayCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingReport || currentReportLine == null || ReportOverlayCanvas == null) return;
            Point currentPoint = e.GetPosition(ReportOverlayCanvas);
            currentReportLine.X1 = currentPoint.X;
            currentReportLine.X2 = currentPoint.X;
        }

        private void ReportOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDraggingReport)
            {
                isDraggingReport = false;
                ReportOverlayCanvas?.ReleaseMouseCapture();
                currentReportLine = null;
            }
        }
    }
}
