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
        private const double CmToPixel = 37.8;
        private readonly Brush subTickColor = new SolidColorBrush(Color.FromRgb(70, 130, 180)); // Steel Blue

        // Drag and Drop Guidelines အတွက် လိုအပ်သော Variables များနှင့် State များ
        private bool isDraggingGuide = false;
        private bool isHorizontalGuide = true; // True က Top Ruler မှ Horizontal line, False က Left Ruler မှ Vertical line
        private Line draggedGuideLine = null;
        private Canvas activeOverlayCanvas = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            
            // Window တစ်ခုလုံးတွင် Mouse Move နှင့် Up များကို ဖမ်းယူရန်
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
            DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
            DrawReportRulers(21.0, 29.7);

            // Ruler Canvas များတွင် Mouse Down Event များ ချိတ်ဆက်ခြင်း
            FrontTopRulerCanvas.MouseLeftButtonDown += (s, ev) => StartDragGuide(ev, true, FrontCardOverlayCanvas);
            FrontLeftRulerCanvas.MouseLeftButtonDown += (s, ev) => StartDragGuide(ev, false, FrontCardOverlayCanvas);

            BackTopRulerCanvas.MouseLeftButtonDown += (s, ev) => StartDragGuide(ev, true, BackCardOverlayCanvas);
            BackLeftRulerCanvas.MouseLeftButtonDown += (s, ev) => StartDragGuide(ev, false, BackCardOverlayCanvas);

            ReportTopRulerCanvas.MouseLeftButtonDown += (s, ev) => StartDragGuide(ev, true, ReportOverlayCanvas);
            ReportLeftRulerCanvas.MouseLeftButtonDown += (s, ev) => StartDragGuide(ev, false, ReportOverlayCanvas);
        }

        // Guide Line ဆွဲတင်ခြင်း စတင်ရန်
        private void StartDragGuide(MouseButtonEventArgs e, bool isHorizontal, Canvas targetCanvas)
        {
            if (targetCanvas == null) return;

            isDraggingGuide = true;
            isHorizontalGuide = isHorizontal;
            activeOverlayCanvas = targetCanvas;

            Point mousePos = e.GetPosition(activeOverlayCanvas);

            // ယာယီ Guide Line အသစ် ဖန်တီးခြင်း (အနီရောင် သို့မဟုတ် Cyan ဖြင့် ထင်းရှားစေရန်)
            draggedGuideLine = new Line
            {
                Stroke = new SolidColorBrush(Color.FromRgb(0, 120, 215)), // Windows Accent Blue
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 4, 2 } // Dashed line ပုံစံ
            };

            if (isHorizontalGuide)
            {
                draggedGuideLine.X1 = 0;
                draggedGuideLine.X2 = activeOverlayCanvas.Width;
                draggedGuideLine.Y1 = Math.Clamp(mousePos.Y, 0, activeOverlayCanvas.Height);
                draggedGuideLine.Y2 = draggedGuideLine.Y1;
                activeOverlayCanvas.Cursor = Cursors.SizeNS;
            }
            else
            {
                draggedGuideLine.Y1 = 0;
                draggedGuideLine.Y2 = activeOverlayCanvas.Height;
                draggedGuideLine.X1 = Math.Clamp(mousePos.X, 0, activeOverlayCanvas.Width);
                draggedGuideLine.X2 = draggedGuideLine.X1;
                activeOverlayCanvas.Cursor = Cursors.SizeWE;
            }

            activeOverlayCanvas.Children.Add(draggedGuideLine);
            Mouse.Capture(this);
            e.Handled = true;
        }

        // Mouse ရွှေ့နေစဉ် Guide Line ပါ လိုက်ပါရွှေ့လျားစေရန်
        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingGuide || draggedGuideLine == null || activeOverlayCanvas == null) return;

            Point mousePos = e.GetPosition(activeOverlayCanvas);

            if (isHorizontalGuide)
            {
                double newY = Math.Clamp(mousePos.Y, 0, activeOverlayCanvas.Height);
                draggedGuideLine.Y1 = newY;
                draggedGuideLine.Y2 = newY;
            }
            else
            {
                double newX = Math.Clamp(mousePos.X, 0, activeOverlayCanvas.Width);
                draggedGuideLine.X1 = newX;
                draggedGuideLine.X2 = newX;
            }
        }

        // Mouse လွှတ်လိုက်သည့်အခါ Guide Line ကို Canvas ပေါ်တွင် ချမှတ်ရန်
        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDraggingGuide) return;

            if (activeOverlayCanvas != null)
            {
                activeOverlayCanvas.Cursor = Cursors.Arrow;
            }

            isDraggingGuide = false;
            draggedGuideLine = null;
            activeOverlayCanvas = null;
            ReleaseMouseCapture();
        }

        // Window Size ပြောင်းလဲသည့်အခါ ရူလာများ အလိုအလျောက် ပြန်ဆွဲရန်
        private void FrontTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
        private void FrontLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
        private void BackTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
        private void BackLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
        
        private void ReportTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ReportSizeComboBox != null && ReportSizeComboBox.SelectedItem is ComboBoxItem item)
            {
                string tag = item.Tag?.ToString();
                double w = 21.0, h = 29.7;
                if (tag == "A5") { w = 14.8; h = 21.0; }
                else if (tag == "B5") { w = 17.6; h = 25.0; }
                else if (tag == "Letter") { w = 21.59; h = 27.94; }
                else if (tag == "Legal") { w = 21.59; h = 35.56; }
                DrawReportRulers(w, h);
            }
        }
        private void ReportLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }

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

        // Card ရူလာများဆွဲသည့် Function
        private void DrawCardRulers(Canvas topCanvas, Canvas leftCanvas, double maxCmX, double maxCmY)
        {
            if (topCanvas == null || leftCanvas == null) return;

            topCanvas.Children.Clear();
            leftCanvas.Children.Clear();

            double topWidth = Math.Max(topCanvas.ActualWidth, maxCmX * CmToPixel + 100);
            double leftHeight = Math.Max(leftCanvas.ActualHeight, maxCmY * CmToPixel + 100);

            topCanvas.Width = topWidth;
            leftCanvas.Height = leftHeight;

            int limitX = (int)Math.Ceiling(topWidth / CmToPixel);
            int limitY = (int)Math.Ceiling(leftHeight / CmToPixel);

            for (int cm = 0; cm <= limitX; cm++)
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

                if (cm < limitX)
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

            for (int cm = 0; cm <= limitY; cm++)
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

                if (cm < limitY)
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

            ReportTopRulerCanvas.Width = Math.Max(ReportTopRulerCanvas.ActualWidth, totalWidthPx + 100);
            ReportLeftRulerCanvas.Height = Math.Max(ReportLeftRulerCanvas.ActualHeight, totalHeightPx + 100);

            int limitX = (int)Math.Ceiling(ReportTopRulerCanvas.Width / CmToPixel);
            int limitY = (int)Math.Ceiling(ReportLeftRulerCanvas.Height / CmToPixel);

            for (int cm = 0; cm <= limitX; cm++)
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

                if (cm < limitX)
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

            for (int cm = 0; cm <= limitY; cm++)
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

                if (cm < limitY)
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
