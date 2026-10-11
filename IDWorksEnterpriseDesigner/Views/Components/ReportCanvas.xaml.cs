using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IDWorksEnterpriseDesigner.Views.Components
{
    public partial class ReportCanvas : UserControl
    {
        public ReportCanvas()
        {
            InitializeComponent();
            this.Loaded += ReportCanvas_Loaded;
        }

        private void ReportCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            DrawRulers();
        }

        private void DrawRulers()
        {
            double oneCm = 37.8; // 1 cm = 37.8 pixels

            // Top Horizontal Ruler for Report
            TopReportRulerBorder.Child = null;
            Canvas topCanvas = new Canvas { ClipToBounds = true };
            for (int i = 0; i <= 20; i++)
            {
                double x = i * oneCm;
                Line line = new Line { X1 = x, Y1 = 14, X2 = x, Y2 = 24, Stroke = Brushes.Black, StrokeThickness = 1 };
                topCanvas.Children.Add(line);

                TextBlock tb = new TextBlock { Text = i.ToString(), FontSize = 9, Foreground = Brushes.Black };
                Canvas.SetLeft(tb, x + 2);
                Canvas.SetTop(tb, 2);
                topCanvas.Children.Add(tb);
            }
            TopReportRulerBorder.Child = topCanvas;

            // Left Vertical Ruler for Report
            LeftReportRulerBorder.Child = null;
            Canvas leftCanvas = new Canvas { ClipToBounds = true };
            for (int i = 0; i <= 15; i++)
            {
                double y = i * oneCm;
                Line line = new Line { X1 = 14, Y1 = y, X2 = 24, Y2 = y, Stroke = Brushes.Black, StrokeThickness = 1 };
                leftCanvas.Children.Add(line);

                TextBlock tb = new TextBlock { Text = i.ToString(), FontSize = 9, Foreground = Brushes.Black };
                Canvas.SetLeft(tb, 2);
                Canvas.SetTop(tb, y + 2);
                leftCanvas.Children.Add(tb);
            }
            LeftReportRulerBorder.Child = leftCanvas;
        }
    }
}
