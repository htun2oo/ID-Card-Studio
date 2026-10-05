using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using IDWorksEnterpriseDesigner.Views;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        private string _projectName = "Sample";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow dialog = new NewProjectWindow();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                _projectName = dialog.ProjectName;

                // Window Title ပြောင်းလဲခြင်း
                this.Title = $"Datacard ID Works Enterprise Designer - {_projectName} - [Card]";

                // File Menu တွင်းရှိ Separators များကို ဖော်ပြပေးခြင်း
                Sep1.Visibility = Visibility.Visible;
                Sep2.Visibility = Visibility.Visible;
                Sep3.Visibility = Visibility.Visible;
                Sep4.Visibility = Visibility.Visible;

                // File Menu တွင်းရှိ Project သက်ဆိုင်ရာ Menu Items များကို ဖော်ပြပေးခြင်း
                MenuCloseProject.Visibility = Visibility.Visible;
                MenuSaveProject.Visibility = Visibility.Visible;
                MenuSaveProjectAs.Visibility = Visibility.Visible;
                MenuProjectProperties.Visibility = Visibility.Visible;
                MenuPrintSampleCard.Visibility = Visibility.Visible;
                MenuPreviewSampleReport.Visibility = Visibility.Visible;
                MenuReportPageSetup.Visibility = Visibility.Visible;
                MenuDeleteProject.Visibility = Visibility.Visible;

                // Recent File
                MenuRecentFile.Header = $@"1 C:\Users\...\Sample\{_projectName}.iwp";
                MenuRecentFile.IsEnabled = true;

                // Menu Items များ ဖော်ပြပေးခြင်း
                MenuEdit.Visibility = Visibility.Visible;
                MenuInsert.Visibility = Visibility.Visible;
                MenuFormat.Visibility = Visibility.Visible;
                MenuTools.Visibility = Visibility.Visible;
                MenuWindow.Visibility = Visibility.Visible;

                // Workspace ကို ရှင်းလင်းခြင်း
                MdiHostGrid.Children.Clear();

                // MDI Layout ဖွဲ့စည်းပုံ
                Grid layoutGrid = new Grid();
                layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(360) });
                layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(210) });

                MdiHostGrid.Children.Add(layoutGrid);

                // 1. Left Side: Card & Card (back side)
                if (dialog.HasCardDesign)
                {
                    Grid leftStack = new Grid();
                    leftStack.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    leftStack.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                    Grid.SetColumn(leftStack, 0);
                    Grid.SetRowSpan(leftStack, 2);
                    layoutGrid.Children.Add(leftStack);

                    // Front Card Window (ISO ID-1 standard: 8.56 cm x 5.398 cm, Ruler horizontal 8 cm)
                    Border cardFrontWindow = CreateDesignerSubWindow("Card", true, 8, 5);
                    Grid.SetRow(cardFrontWindow, 0);
                    leftStack.Children.Add(cardFrontWindow);

                    // Back Card Window (ISO ID-1 standard)
                    Border cardBackWindow = CreateDesignerSubWindow("Card (back side)", true, 8, 5);
                    Grid.SetRow(cardBackWindow, 1);
                    leftStack.Children.Add(cardBackWindow);
                }

                // 2. Top Right: Production Form Window
                if (dialog.HasProductionForm)
                {
                    Border prodFormWindow = CreateProductionFormSubWindow("Production Form");
                    Grid.SetColumn(prodFormWindow, 1);
                    Grid.SetRow(prodFormWindow, 0);
                    layoutGrid.Children.Add(prodFormWindow);
                }

                // 3. Bottom Right: Report Window
                if (dialog.HasReportDesign)
                {
                    Border reportWindow = CreateDesignerSubWindow("Report", false, 18, 10);
                    Grid.SetColumn(reportWindow, 1);
                    Grid.SetRow(reportWindow, 1);
                    layoutGrid.Children.Add(reportWindow);
                }
            }
        }

        // Sub-Window (Card & Report) ဖန်တီးပေးသည့် နည်းလမ်း
        private Border CreateDesignerSubWindow(string title, bool isCard, int maxRulerCmX, int maxRulerCmY)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(180, 200, 230)),
                BorderThickness = new Thickness(3),
                Margin = new Thickness(2),
                Background = Brushes.White
            };

            // Window Active ဖြစ်လျှင် Title Bar ပြောင်းလဲရန်
            border.MouseLeftButtonDown += (s, e) =>
            {
                this.Title = $"Datacard ID Works Enterprise Designer - {_projectName} - [{title}]";
            };

            DockPanel panel = new DockPanel();

            // Sub-Window Header / Title Bar
            Grid titleBar = new Grid { Background = new SolidColorBrush(Color.FromRgb(215, 228, 245)), Height = 22 };
            DockPanel.SetDock(titleBar, Dock.Top);

            TextBlock titleText = new TextBlock
            {
                Text = title,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(6, 0, 0, 0),
                Foreground = Brushes.DarkBlue
            };
            titleBar.Children.Add(titleText);

            // Sub-Window Buttons
            StackPanel winBtns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 2, 0) };
            winBtns.Children.Add(new Button { Content = "_", Width = 16, Height = 14, FontSize = 8, Margin = new Thickness(1), Background = Brushes.LightGray });
            winBtns.Children.Add(new Button { Content = "☐", Width = 16, Height = 14, FontSize = 8, Margin = new Thickness(1), Background = Brushes.LightGray });
            winBtns.Children.Add(new Button { Content = "✕", Width = 16, Height = 14, FontSize = 8, Margin = new Thickness(1), Background = Brushes.LightGray });
            titleBar.Children.Add(winBtns);

            panel.Children.Add(titleBar);

            // Canvas Area with Ruler
            Grid canvasGrid = new Grid { Background = new SolidColorBrush(Color.FromRgb(210, 210, 210)) };
            canvasGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(22) });
            canvasGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            canvasGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(22) });
            canvasGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // 1 CM = 37.7952 Pixels (Standard 96 DPI precise conversion)
            double cmDpi = 37.7952;

            // Ruler & Grid Container Canvas
            Canvas designAreaCanvas = new Canvas { Background = new SolidColorBrush(Color.FromRgb(210, 210, 210)), ClipToBounds = true };
            Grid.SetColumn(designAreaCanvas, 1);
            Grid.SetRow(designAreaCanvas, 1);

            // White Paper Container
            Border paperBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1)
            };

            if (isCard)
            {
                // ISO ID-1 Standard: 85.60 mm x 53.98 mm (8.56 cm x 5.398 cm)
                paperBorder.Width = 8.56 * cmDpi;
                paperBorder.Height = 5.398 * cmDpi;
            }
            else
            {
                paperBorder.Width = 15 * cmDpi;
                paperBorder.Height = 10 * cmDpi;
            }

            // Card / Paper Margin - Left Offset 15px
            Canvas.SetLeft(paperBorder, 15);
            Canvas.SetTop(paperBorder, 15);
            designAreaCanvas.Children.Add(paperBorder);

            // Horizontal Ruler Canvas
            Canvas topRulerCanvas = new Canvas { Background = Brushes.LightGray, Height = 22, Cursor = Cursors.SizeNS };
            Grid.SetColumn(topRulerCanvas, 1);
            Grid.SetRow(topRulerCanvas, 0);

            // Card Window များ၏ ညာဘက်အစွန်းတွင် Margin ခွာပေးရန် MaxWidth သတ်မှတ်ခြင်း
            if (isCard)
            {
                double cardContentWidth = 15 + (8.56 * cmDpi) + 15; // Left margin + Card width + Right margin (15px)
                topRulerCanvas.MaxWidth = cardContentWidth;
                topRulerCanvas.HorizontalAlignment = HorizontalAlignment.Left;
                designAreaCanvas.MaxWidth = cardContentWidth;
                designAreaCanvas.HorizontalAlignment = HorizontalAlignment.Left;
            }

            DrawHorizontalRuler(topRulerCanvas, maxRulerCmX, cmDpi);

            // Vertical Ruler Canvas
            Canvas leftRulerCanvas = new Canvas { Background = Brushes.LightGray, Width = 22, Cursor = Cursors.SizeWE };
            Grid.SetColumn(leftRulerCanvas, 0);
            Grid.SetRow(leftRulerCanvas, 1);
            DrawVerticalRuler(leftRulerCanvas, maxRulerCmY, cmDpi);

            // Photoshop-style Guide Lines Dragging System
            SetupGuideLineDragging(topRulerCanvas, leftRulerCanvas, designAreaCanvas);

            canvasGrid.Children.Add(topRulerCanvas);
            canvasGrid.Children.Add(leftRulerCanvas);
            canvasGrid.Children.Add(designAreaCanvas);

            panel.Children.Add(canvasGrid);
            border.Child = panel;

            return border;
        }

        // Horizontal Ruler အမှတ်အသား (Centimeter Ticks) ဆွဲပေးခြင်း
        private void DrawHorizontalRuler(Canvas canvas, int maxCm, double cmPx)
        {
            double startOffset = 15; // Align with paper margin
            for (int cm = 1; cm <= maxCm; cm++)
            {
                double posX = startOffset + (cm * cmPx);

                // Sub-ticks (mm lines)
                for (int mm = 1; mm < 10; mm++)
                {
                    double mmX = startOffset + ((cm - 1) * cmPx) + (mm * (cmPx / 10));
                    double h = (mm == 5) ? 8 : 4;
                    Line subLine = new Line { X1 = mmX, Y1 = 22, X2 = mmX, Y2 = 22 - h, Stroke = Brushes.DimGray, StrokeThickness = 0.8 };
                    canvas.Children.Add(subLine);
                }

                // Main CM Line & Text
                Line mainLine = new Line { X1 = posX, Y1 = 22, X2 = posX, Y2 = 10, Stroke = Brushes.Black, StrokeThickness = 1 };
                TextBlock txt = new TextBlock { Text = cm.ToString(), FontSize = 9, Foreground = Brushes.Black };
                Canvas.SetLeft(txt, posX - 8);
                Canvas.SetTop(txt, 0);

                canvas.Children.Add(mainLine);
                canvas.Children.Add(txt);
            }
        }

        // Vertical Ruler အမှတ်အသား (Centimeter Ticks) ဆွဲပေးခြင်း
        private void DrawVerticalRuler(Canvas canvas, int maxCm, double cmPx)
        {
            double startOffset = 15; // Align with paper margin
            for (int cm = 1; cm <= maxCm; cm++)
            {
                double posY = startOffset + (cm * cmPx);

                // Sub-ticks (mm lines)
                for (int mm = 1; mm < 10; mm++)
                {
                    double mmY = startOffset + ((cm - 1) * cmPx) + (mm * (cmPx / 10));
                    double w = (mm == 5) ? 8 : 4;
                    Line subLine = new Line { X1 = 22, Y1 = mmY, X2 = 22 - w, Y2 = mmY, Stroke = Brushes.DimGray, StrokeThickness = 0.8 };
                    canvas.Children.Add(subLine);
                }

                // Main CM Line & Text
                Line mainLine = new Line { X1 = 22, Y1 = posY, X2 = 10, Y2 = posY, Stroke = Brushes.Black, StrokeThickness = 1 };
                TextBlock txt = new TextBlock { Text = cm.ToString(), FontSize = 9, Foreground = Brushes.Black };
                Canvas.SetLeft(txt, 1);
                Canvas.SetTop(txt, posY - 7);

                canvas.Children.Add(mainLine);
                canvas.Children.Add(txt);
            }
        }

        // Photoshop Style Grid / Guide Lines Drag & Drop Logic
        private void SetupGuideLineDragging(Canvas topRuler, Canvas leftRuler, Canvas designCanvas)
        {
            Line? currentGuide = null;

            // Horizontal Guide Line Drag from Top Ruler
            topRuler.MouseLeftButtonDown += (s, e) =>
            {
                Point p = e.GetPosition(designCanvas);
                currentGuide = new Line
                {
                    X1 = 0,
                    Y1 = p.Y,
                    X2 = 2000,
                    Y2 = p.Y,
                    Stroke = Brushes.Cyan,
                    StrokeThickness = 1,
                    Cursor = Cursors.SizeNS
                };

                designCanvas.Children.Add(currentGuide);
                topRuler.CaptureMouse();
            };

            topRuler.MouseMove += (s, e) =>
            {
                if (currentGuide != null && topRuler.IsMouseCaptured)
                {
                    Point p = e.GetPosition(designCanvas);
                    currentGuide.Y1 = p.Y;
                    currentGuide.Y2 = p.Y;
                }
            };

            topRuler.MouseLeftButtonUp += (s, e) =>
            {
                if (topRuler.IsMouseCaptured)
                {
                    topRuler.ReleaseMouseCapture();
                    if (currentGuide != null)
                    {
                        MakeGuideDraggable(currentGuide, designCanvas, false);
                        currentGuide = null;
                    }
                }
            };

            // Vertical Guide Line Drag from Left Ruler
            leftRuler.MouseLeftButtonDown += (s, e) =>
            {
                Point p = e.GetPosition(designCanvas);
                currentGuide = new Line
                {
                    X1 = p.X,
                    Y1 = 0,
                    X2 = p.X,
                    Y2 = 2000,
                    Stroke = Brushes.Cyan,
                    StrokeThickness = 1,
                    Cursor = Cursors.SizeWE
                };

                designCanvas.Children.Add(currentGuide);
                leftRuler.CaptureMouse();
            };

            leftRuler.MouseMove += (s, e) =>
            {
                if (currentGuide != null && leftRuler.IsMouseCaptured)
                {
                    Point p = e.GetPosition(designCanvas);
                    currentGuide.X1 = p.X;
                    currentGuide.X2 = p.X;
                }
            };

            leftRuler.MouseLeftButtonUp += (s, e) =>
            {
                if (leftRuler.IsMouseCaptured)
                {
                    leftRuler.ReleaseMouseCapture();
                    if (currentGuide != null)
                    {
                        MakeGuideDraggable(currentGuide, designCanvas, true);
                        currentGuide = null;
                    }
                }
            };
        }

        // Guide Line များကို Canvas ထဲသို့ ရောက်သွားပါက အပေါ်တွင် ပြန်လည် Drag ဆွဲရွှေ့နိုင်ရန် ဖန်တီးပေးခြင်း
        private void MakeGuideDraggable(Line? guide, Canvas designCanvas, bool isVertical)
        {
            if (guide == null) return;

            bool isDragging = false;

            guide.MouseLeftButtonDown += (s, e) =>
            {
                isDragging = true;
                guide.CaptureMouse();
                e.Handled = true;
            };

            guide.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    Point p = e.GetPosition(designCanvas);
                    if (isVertical)
                    {
                        guide.X1 = p.X;
                        guide.X2 = p.X;
                    }
                    else
                    {
                        guide.Y1 = p.Y;
                        guide.Y2 = p.Y;
                    }
                }
            };

            guide.MouseLeftButtonUp += (s, e) =>
            {
                if (isDragging)
                {
                    isDragging = false;
                    guide.ReleaseMouseCapture();
                }
            };
        }

        // Production Form Sub-Window ဖန်တီးပေးသည့် နည်းလမ်း
        private Border CreateProductionFormSubWindow(string title)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(180, 200, 230)),
                BorderThickness = new Thickness(3),
                Margin = new Thickness(2),
                Background = Brushes.White
            };

            border.MouseLeftButtonDown += (s, e) =>
            {
                this.Title = $"Datacard ID Works Enterprise Designer - {_projectName} - [{title}]";
            };

            DockPanel panel = new DockPanel();

            // Header
            Grid titleBar = new Grid { Background = new SolidColorBrush(Color.FromRgb(215, 228, 245)), Height = 22 };
            DockPanel.SetDock(titleBar, Dock.Top);

            TextBlock titleText = new TextBlock
            {
                Text = title,
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(6, 0, 0, 0),
                Foreground = Brushes.SteelBlue
            };
            titleBar.Children.Add(titleText);

            // Control Buttons
            StackPanel winBtns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 2, 0) };
            winBtns.Children.Add(new Button { Content = "_", Width = 16, Height = 14, FontSize = 8, Margin = new Thickness(1), Background = Brushes.LightGray });
            winBtns.Children.Add(new Button { Content = "☐", Width = 16, Height = 14, FontSize = 8, Margin = new Thickness(1), Background = Brushes.LightGray });
            winBtns.Children.Add(new Button { Content = "✕", Width = 16, Height = 14, FontSize = 8, Margin = new Thickness(1), Background = Brushes.LightGray });
            titleBar.Children.Add(winBtns);

            panel.Children.Add(titleBar);

            // White Area
            Grid whiteArea = new Grid { Background = Brushes.White };
            panel.Children.Add(whiteArea);

            border.Child = panel;
            return border;
        }
    }
}
