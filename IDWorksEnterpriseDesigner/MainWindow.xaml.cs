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

        // တင်းကြပ်စွာ ခွဲခြားပြသရန် ကာလာနှစ်မျိုး
        private readonly Brush primaryColor = new SolidColorBrush(Color.FromRgb(0, 0, 128));   // Navy/Dark Blue
        private readonly Brush secondaryColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));  // Black

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

        // ရူလာစကေးများကို ကာလာနှစ်မျိုးဖြင့် ထင်းရှားစွာ ဆွဲပေးသည့် Function
        private void DrawReportRulers(double widthCm, double heightCm)
        {
            if (ReportTopRulerCanvas == null || ReportLeftRulerCanvas == null) return;

            ReportTopRulerCanvas.Children.Clear();
            ReportLeftRulerCanvas.Children.Clear();

            double totalWidthPx = widthCm * CmToPixel;
            double totalHeightPx = heightCm * CmToPixel;

            ReportTopRulerCanvas.Width = totalWidthPx + 50;
            ReportLeftRulerCanvas.Height = totalHeightPx + 50;

            // 1. Top Horizontal Ruler (0 မှ 21 cm အထိ)
            for (int cm = 0; cm <= (int)Math.Ceiling(widthCm); cm++)
            {
                double startX = cm * CmToPixel;

                // Main Cm Tick Line (အပြာရင့်ရောင်)
                Line mainLine = new Line
                {
                    X1 = startX, Y1 = 12,
                    X2 = startX, Y2 = 24,
                    Stroke = primaryColor,
                    StrokeThickness = 1.2
                };
                ReportTopRulerCanvas.Children.Add(mainLine);

                // Cm Text Label (အပြာရင့်ရောင်)
                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 9,
                    Foreground = primaryColor,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, startX - (cm >= 10 ? 6 : 3));
                Canvas.SetTop(text, 1);
                ReportTopRulerCanvas.Children.Add(text);

                // Sub-ticks (0.1 cm / 1 mm အပိုင်းအစများ - အနက်ရောင်)
                if (cm < (int)Math.Ceiling(widthCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subX = startX + (mm * (CmToPixel / 10.0));
                        double tickHeight = (mm == 5) ? 7 : 4;

                        Line subLine = new Line
                        {
                            X1 = subX, Y1 = 24 - tickHeight,
                            X2 = subX, Y2 = 24,
                            Stroke = (mm == 5) ? primaryColor : secondaryColor,
                            StrokeThickness = (mm == 5) ? 1.0 : 0.7
                        };
                        ReportTopRulerCanvas.Children.Add(subLine);
                    }
                }
            }

            // 2. Left Vertical Ruler (0 မှ 29.7 cm အထိ)
            for (int cm = 0; cm <= (int)Math.Ceiling(heightCm); cm++)
            {
                double startY = cm * CmToPixel;

                // Main Cm Tick Line (အပြာရင့်ရောင်)
                Line mainLine = new Line
                {
                    X1 = 12, Y1 = startY,
                    X2 = 24, Y2 = startY,
                    Stroke = primaryColor,
                    StrokeThickness = 1.2
                };
                ReportLeftRulerCanvas.Children.Add(mainLine);

                // Cm Text Label (အပြာရင့်ရောင်)
                TextBlock text = new TextBlock
                {
                    Text = cm.ToString(),
                    FontSize = 9,
                    Foreground = primaryColor,
                    FontWeight = FontWeights.Bold
                };
                Canvas.SetLeft(text, 2);
                Canvas.SetTop(text, startY - 6);
                ReportLeftRulerCanvas.Children.Add(text);

                // Sub-ticks (0.1 cm / 1 mm အပိုင်းအစများ - အနက်ရောင်)
                if (cm < (int)Math.Ceiling(heightCm))
                {
                    for (int mm = 1; mm < 10; mm++)
                    {
                        double subY = startY + (mm * (CmToPixel / 10.0));
                        double tickWidth = (mm == 5) ? 7 : 4;

                        Line subLine = new Line
                        {
                            X1 = 24 - tickWidth, Y1 = subY,
                            X2 = 24, Y2 = subY,
                            Stroke = (mm == 5) ? primaryColor : secondaryColor,
                            StrokeThickness = (mm == 5) ? 1.0 : 0.7
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
