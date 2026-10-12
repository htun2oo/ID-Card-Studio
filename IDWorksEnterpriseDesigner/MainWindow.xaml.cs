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
        // Front Card Dragging State
        private bool isDraggingFront = false;
        private Line? currentFrontLine;

        // Back Card Dragging State
        private bool isDraggingBack = false;
        private Line? currentBackLine;

        // Report Dragging State
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
                double newWidth = 378;
                double newHeight = 534;

                switch (tag)
                {
                    case "A4":
                        newWidth = 378; newHeight = 534;
                        break;
                    case "A5":
                        newWidth = 266; newHeight = 378;
                        break;
                    case "B5":
                        newWidth = 316; newHeight = 450;
                        break;
                    case "Letter":
                        newWidth = 324; newHeight = 420;
                        break;
                    case "Legal":
                        newWidth = 324; newHeight = 534;
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
