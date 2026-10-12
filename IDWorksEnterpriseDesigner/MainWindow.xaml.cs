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
        private readonly Brush subTickColor = new SolidColorBrush(Color.FromRgb(70, 130, 180)); // Steel Blue

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Card ရူလာများ (Front နှင့် Back)
            DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 10.0, 6.0);
            DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 10.0, 6.0);

            // Report ရူလာ (A4 အရွယ်အစား: 21cm x 29.7cm)
            DrawReportRulers(21.0, 29.7);
        }

        // Scroll ပြုလုပ်စဉ် ရူလာများပါ လိုက်ပါရွှေ့လျားစေရန်
        private void FrontMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (FrontTopRulerScrollViewer != null) FrontTopRulerScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
            if (FrontLeftRulerScrollViewer != null) FrontLeftRulerScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void BackMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (BackTopRulerScrollViewer != null) BackTopRulerScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
            if (BackLeftRulerScrollViewer != null) BackLeftRulerScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void ReportMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (ReportTopRulerScrollViewer != null) ReportTopRulerScrollViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
            if (ReportLeftRulerScrollViewer != null) ReportLeftRulerScrollViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }

        // Card ရူလာများဆွဲသည့် Function (Zero အမှတ်ချိန်ညှိပြီး)
        private void DrawCardRulers(Canvas topCanvas, Canvas leftCanvas, double widthCm, double heightCm)
        {
            if (topCanvas == null || leftCanvas == null) return;

            topCanvas.Children.Clear();
            leftCanvas.Children.Clear();

            double totalWidthPx = widthCm * CmToPixel;
            double totalHeightPx = heightCm * CmToPixel;

            topCanvas.Width = totalWidthPx + 50;
            leftCanvas.Height = totalHeightPx + 50;

            // 1. Top Horizontal Ruler
            for (int cm = 0; cm <= (int)Math.Ceiling(widthCm); cm++)
            {
                double startX = cm * CmToPixel;

                Line mainLine = new Line
                {
                    X1 = startX, Y1 = 15,
                    X2 = startX, Y2 = 24,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.2
                };
                topCanvas.Children.Add(mainLine);

                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, startX - (cm >= 10 ? 7 : 4));
                Canvas.SetTop(text, 1);
                topCanvas.Children.Add(text);

                if (cm < (int)Math.Ceiling(widthCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subX = startX + (mm * (CmToPixel / 10.0));
                        double tickHeight = (mm == 5) ? 6 : 3.5;

                        Line subLine = new Line
                        {
                            X1 = subX, Y1 = 24 - tickHeight,
                            X2 = subX, Y2 = 24,
                            Stroke = (mm == 5) ? Brushes.Black : subTickColor,
                            StrokeThickness = (mm == 5) ? 0.9 : 0.7
                        };
                        topCanvas.Children.Add(subLine);
                    }
                }
            }

            // 2. Left Vertical Ruler
            for (int cm = 0; cm <= (int)Math.Ceiling(heightCm); cm++)
            {
                double startY = cm * CmToPixel;

                Line mainLine = new Line
                {
                    X1 = 15, Y1 = startY,
                    X2 = 24, Y2 = startY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.2
                };
                leftCanvas.Children.Add(mainLine);

                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, cm >= 10 ? 1 : 3);
                Canvas.SetTop(text, startY - 7);
                leftCanvas.Children.Add(text);

                if (cm < (int)Math.Ceiling(heightCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subY = startY + (mm * (CmToPixel / 10.0));
                        double tickWidth = (mm == 5) ? 6 : 3.5;

                        Line subLine = new Line
                        {
                            X1 = 24 - tickWidth, Y1 = subY,
                            X2 = 24, Y2 = subY,
                            Stroke = (mm == 5) ? Brushes.Black : subTickColor,
                            StrokeThickness = (mm == 5) ? 0.9 : 0.7
                        };
                        leftCanvas.Children.Add(subLine);
                    }
                }
            }
        }

        // Report ရူလာများဆွဲသည့် Function
        private void DrawReportRulers(double widthCm, double heightCm)
        {
            if (ReportTopRulerCanvas == null || ReportLeftRulerCanvas == null) return;

            ReportTopRulerCanvas.Children.Clear();
            ReportLeftRulerCanvas.Children.Clear();

            double totalWidthPx = widthCm * CmToPixel;
            double totalHeightPx = heightCm * CmToPixel;

            ReportTopRulerCanvas.Width = totalWidthPx + 50;
            ReportLeftRulerCanvas.Height = totalHeightPx + 50;

            // 1. Top Horizontal Ruler
            for (int cm = 0; cm <= (int)Math.Ceiling(widthCm); cm++)
            {
                double startX = cm * CmToPixel;

                Line mainLine = new Line
                {
                    X1 = startX, Y1 = 15,
                    X2 = startX, Y2 = 24,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.2
                };
                ReportTopRulerCanvas.Children.Add(mainLine);

                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, startX - (cm >= 10 ? 7 : 4));
                Canvas.SetTop(text, 1);
                ReportTopRulerCanvas.Children.Add(text);

                if (cm < (int)Math.Ceiling(widthCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subX = startX + (mm * (CmToPixel / 10.0));
                        double tickHeight = (mm == 5) ? 6 : 3.5;

                        Line subLine = new Line
                        {
                            X1 = subX, Y1 = 24 - tickHeight,
                            X2 = subX, Y2 = 24,
                            Stroke = (mm == 5) ? Brushes.Black : subTickColor,
                            StrokeThickness = (mm == 5) ? 0.9 : 0.7
                        };
                        ReportTopRulerCanvas.Children.Add(subLine);
                    }
                }
            }

            // 2. Left Vertical Ruler
            for (int cm = 0; cm <= (int)Math.Ceiling(heightCm); cm++)
            {
                double startY = cm * CmToPixel;

                Line mainLine = new Line
                {
                    X1 = 15, Y1 = startY,
                    X2 = 24, Y2 = startY,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.2
                };
                ReportLeftRulerCanvas.Children.Add(mainLine);

                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 10,
                    Foreground = Brushes.Black,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, cm >= 10 ? 1 : 3);
                Canvas.SetTop(text, startY - 7);
                ReportLeftRulerCanvas.Children.Add(text);

                if (cm < (int)Math.Ceiling(heightCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subY = startY + (mm * (CmToPixel / 10.0));
                        double tickWidth = (mm == 5) ? 6 : 3.5;

                        Line subLine = new Line
                        {
                            X1 = 24 - tickWidth, Y1 = subY,
                            X2 = 24, Y2 = subY,
                            Stroke = (mm == 5) ? Brushes.Black : subTickColor,
                            StrokeThickness = (mm == 5) ? 0.9 : 0.7
                        };
                        ReportLeftRulerCanvas.Children.Add(subLine);
                    }
                }
            }
        }

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

        // ==========================================
        // MENU EVENT HANDLERS
        // ==========================================
        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            Window newProjectWin = new Window
            {
                Title = "New Project",
                Width = 350,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = Brushes.WhiteSmoke
            };

            StackPanel mainPanel = new StackPanel { Margin = new Thickness(15) };
            
            mainPanel.Children.Add(new TextBlock 
            { 
                Text = "Enter project name:", 
                FontWeight = FontWeights.Bold, 
                Margin = new Thickness(0, 0, 0, 5) 
            });

            TextBox txtName = new TextBox 
            { 
                Text = "Project1", 
                Height = 26, 
                VerticalContentAlignment = VerticalAlignment.Center, 
                Margin = new Thickness(0, 0, 0, 15) 
            };
            mainPanel.Children.Add(txtName);

            StackPanel btnPanel = new StackPanel 
            { 
                Orientation = Orientation.Horizontal, 
                HorizontalAlignment = HorizontalAlignment.Right 
            };

            Button btnOk = new Button { Content = "OK", Width = 75, Height = 25, Margin = new Thickness(0, 0, 10, 0) };
            btnOk.Click += (s, args) => {
                newProjectWin.DialogResult = true;
                newProjectWin.Close();
            };

            Button btnCancel = new Button { Content = "Cancel", Width = 75, Height = 25 };
            btnCancel.Click += (s, args) => {
                newProjectWin.Close();
            };

            btnPanel.Children.Add(btnOk);
            btnPanel.Children.Add(btnCancel);
            mainPanel.Children.Add(btnPanel);

            newProjectWin.Content = mainPanel;
            newProjectWin.ShowDialog();
        }

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e) { }
        private void Exit_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }

        // ==========================================
        // CANVAS MOUSE EVENT HANDLERS
        // ==========================================
        private void FrontCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void FrontCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void FrontCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void BackCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void BackCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void BackCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void ReportOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void ReportOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void ReportOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }
    }
}
