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
        // 1 cm = 37.8 pixels (96 DPI Standard)
        private const double CmToPixel = 37.8;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawReportRulers(21.0, 29.7); // Default A4 Size (21cm x 29.7cm)
        }

        // စာရွက်အကျယ် ရွှေ့လိုက်ပါက ရူလာပါ လိုက်ပါ ရွှေ့လျားစေရန်
        private void ReportMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ReportTopRulerScrollViewer != null)
                ReportTopRulerScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);

            if (ReportLeftRulerScrollViewer != null)
                ReportLeftRulerScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }

        // ရူလာစကေးနှင့် စကေးအသေးများ တိကျစွာ ဆွဲပေးသည့် Function
        private void DrawReportRulers(double widthCm, double heightCm)
        {
            if (ReportTopRulerCanvas == null || ReportLeftRulerCanvas == null) return;

            ReportTopRulerCanvas.Children.Clear();
            ReportLeftRulerCanvas.Children.Clear();

            double totalWidthPx = widthCm * CmToPixel;
            double totalHeightPx = heightCm * CmToPixel;

            ReportTopRulerCanvas.Width = totalWidthPx + 50;
            ReportLeftRulerCanvas.Height = totalHeightPx + 50;

            // 1. Top Horizontal Ruler (အလျား စကေးဆွဲခြင်း - 0 မှ 21 cm အထိ)
            for (int cm = 0; cm <= (int)Math.Ceiling(widthCm); cm++)
            {
                double startX = cm * CmToPixel;

                // Main Tick Line (1 cm)
                Line mainLine = new Line
                {
                    X1 = startX, Y1 = 14,
                    X2 = startX, Y2 = 24,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                ReportTopRulerCanvas.Children.Add(mainLine);

                // Cm Text Label
                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 9,
                    Foreground = Brushes.Black,
                    FontWeight = (cm == 0) ? FontWeights.Bold : FontWeights.Normal
                };
                Canvas.SetLeft(text, startX - (cm >= 10 ? 6 : 3));
                Canvas.SetTop(text, 1);
                ReportTopRulerCanvas.Children.Add(text);

                // Sub-ticks (0.1 cm / 1 mm အပိုင်းအစများ)
                if (cm < (int)Math.Ceiling(widthCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subX = startX + (mm * (CmToPixel / 10.0));
                        double tickHeight = (mm == 5) ? 6 : 3;

                        Line subLine = new Line
                        {
                            X1 = subX, Y1 = 24 - tickHeight,
                            X2 = subX, Y2 = 24,
                            Stroke = (mm == 5) ? Brushes.Black : Brushes.Gray,
                            StrokeThickness = (mm == 5) ? 0.8 : 0.5
                        };
                        ReportTopRulerCanvas.Children.Add(subLine);
                    }
                }
            }

            // 2. Left Vertical Ruler (အနံ စကေးဆွဲခြင်း - 0 မှ 29.7 cm အထိ)
            for (int cm = 0; cm <= (int)Math.Ceiling(heightCm); cm++)
            {
                double startY = cm * CmToPixel;

                // Main Tick Line (1 cm)
                Line mainLine = new Line
                {
                    X1 = 14, Y1 = startY,
                    X2 = 24, Y2 = startY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                ReportLeftRulerCanvas.Children.Add(mainLine);

                // Cm Text Label
                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 9,
                    Foreground = Brushes.Black,
                    FontWeight = (cm == 0) ? FontWeights.Bold : FontWeights.Normal
                };
                Canvas.SetLeft(text, 2);
                Canvas.SetTop(text, startY - 6);
                ReportLeftRulerCanvas.Children.Add(text);

                // Sub-ticks (0.1 cm / 1 mm အပိုင်းအစများ)
                if (cm < (int)Math.Ceiling(heightCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subY = startY + (mm * (CmToPixel / 10.0));
                        double tickWidth = (mm == 5) ? 6 : 3;

                        Line subLine = new Line
                        {
                            X1 = 24 - tickWidth, Y1 = subY,
                            X2 = 24, Y2 = subY,
                            Stroke = (mm == 5) ? Brushes.Black : Brushes.Gray,
                            StrokeThickness = (mm == 5) ? 0.8 : 0.5
                        };
                        ReportLeftRulerCanvas.Children.Add(subLine);
                    }
                }
            }
        }

        // Dropdown စာရွက်ဆိုဒ် ပြောင်းလဲပါက စကေးပါ လိုက်ပါပြောင်းလဲမည်
        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReportSizeComboBox == null || ReportDocumentBorder == null || ReportOverlayCanvas == null) return;

            ComboBoxItem selectedItem = ReportSizeComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                string tag = selectedItem.Tag?.ToString();
                double widthCm = 21.0;
                double heightCm = 29.7;

                switch (tag)
                {
                    case "A4": widthCm = 21.0; heightCm = 29.7; break;
                    case "A5": widthCm = 14.8; heightCm = 21.0; break;
                    case "B5": widthCm = 17.6; heightCm = 25.0; break;
                    case "Letter": widthCm = 21.59; heightCm = 27.94; break;
                    case "Legal": widthCm = 21.59; heightCm = 35.56; break;
                }

                double pxWidth = widthCm * CmToPixel;
                double pxHeight = heightCm * CmToPixel;

                ReportDocumentBorder.Width = pxWidth;
                ReportDocumentBorder.Height = pxHeight;
                ReportOverlayCanvas.Width = pxWidth;
                ReportOverlayCanvas.Height = pxHeight;

                DrawReportRulers(widthCm, heightCm);
            }
        }
    }
}
