using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        // Routed Commands များ ကြေညာခြင်း
        public static RoutedCommand NewProjectCommand = new RoutedCommand();
        public static RoutedCommand OpenProjectCommand = new RoutedCommand();
        public static RoutedCommand OnlineHelpCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();

            // Shortcut KeyBindings များကို သက်ဆိုင်ရာ Methods များသို့ ချိတ်ဆက်ခြင်း
            CommandBindings.Add(new CommandBinding(NewProjectCommand, (s, e) => NewProject_Click(s, e)));
            CommandBindings.Add(new CommandBinding(OpenProjectCommand, (s, e) => OpenProject_Click(s, e)));
            CommandBindings.Add(new CommandBinding(OnlineHelpCommand, (s, e) => OnlineHelp_Click(s, e)));
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // Views ဖိုင်တွဲအောက်ရှိ NewProjectWindow ကို ခေါ်ယူခြင်း
            var dialog = new IDWorksEnterpriseDesigner.Views.NewProjectWindow();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                // OK နှိပ်ပြီးပါက Workspace ကို ပေါ်လာစေရန်
                ProjectWorkspace.Visibility = Visibility.Visible;
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            // Views ဖိုင်တွဲအောက်ရှိ OpenProjectWindow ကို ခေါ်ယူခြင်း
            var dialog = new IDWorksEnterpriseDesigner.Views.OpenProjectWindow();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                // Open နှိပ်ပြီးပါက Workspace ကို ပေါ်လာစေရန်
                ProjectWorkspace.Visibility = Visibility.Visible;
            }
        }

        private void CloseProject_Click(object sender, RoutedEventArgs e)
        {
            // Close Project Event Handler
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            // Save Project Event Handler
        }

        private void SaveProjectAs_Click(object sender, RoutedEventArgs e)
        {
            // Save Project As Event Handler
        }

        private void ProjectProperties_Click(object sender, RoutedEventArgs e)
        {
            // Project Properties Event Handler
        }

        private void PrintSampleCard_Click(object sender, RoutedEventArgs e)
        {
            // Print Sample Card Event Handler
        }

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e)
        {
            // Report Page Setup Event Handler
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            // Delete Project Event Handler
        }

        private void RecentFile_Click(object sender, RoutedEventArgs e)
        {
            // Recent File Event Handler
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LargeToolbar_Click(object sender, RoutedEventArgs e)
        {
            LargeToolbarMenu.IsChecked = true;
            SmallToolbarMenu.IsChecked = false;
        }

        private void SmallToolbar_Click(object sender, RoutedEventArgs e)
        {
            SmallToolbarMenu.IsChecked = true;
            LargeToolbarMenu.IsChecked = false;
        }

        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Report Size Selection Changed
        }

        // --- Ruler Drawing Logic (Enhanced Centimeter Scale: 1 cm = 37.8 pixels) ---

        private void DrawTopRuler(Canvas canvas, double width)
        {
            if (canvas == null) return;
            canvas.Children.Clear();
            canvas.Width = Math.Max(width, 1000);

            double cmPixels = 37.8;

            // Draw baseline for the ruler
            var baseLine = new Line
            {
                X1 = 0, Y1 = 24,
                X2 = canvas.Width, Y2 = 24,
                Stroke = Brushes.Gray,
                StrokeThickness = 0.5
            };
            canvas.Children.Add(baseLine);

            for (double i = 0; i < canvas.Width; i += (cmPixels / 4))
            {
                int tickIndex = (int)Math.Round(i / (cmPixels / 4));
                double tickHeight = 4;

                if (tickIndex % 4 == 0) tickHeight = 12;      // 1 cm mark (Long)
                else if (tickIndex % 2 == 0) tickHeight = 8;  // 0.5 cm mark (Medium)

                var line = new Line
                {
                    X1 = i, Y1 = 24,
                    X2 = i, Y2 = 24 - tickHeight,
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.7
                };
                canvas.Children.Add(line);

                if (tickIndex % 4 == 0)
                {
                    int cmValue = tickIndex / 4;
                    var textBlock = new TextBlock
                    {
                        Text = cmValue.ToString(),
                        FontSize = 10,
                        FontWeight = FontWeights.SemiBold,
                        FontFamily = new FontFamily("Arial"),
                        Foreground = Brushes.Black
                    };
                    
                    double xOffset = (cmValue == 0) ? 2 : (cmValue < 10 ? 4 : 7);
                    Canvas.SetLeft(textBlock, i - xOffset);
                    Canvas.SetTop(textBlock, 1);
                    canvas.Children.Add(textBlock);
                }
            }
        }

        private void DrawLeftRuler(Canvas canvas, double height)
        {
            if (canvas == null) return;
            canvas.Children.Clear();
            canvas.Height = Math.Max(height, 1000);

            double cmPixels = 37.8;

            // Draw baseline for the vertical ruler
            var baseLine = new Line
            {
                X1 = 24, Y1 = 0,
                X2 = 24, Y2 = canvas.Height,
                Stroke = Brushes.Gray,
                StrokeThickness = 0.5
            };
            canvas.Children.Add(baseLine);

            for (double i = 0; i < canvas.Height; i += (cmPixels / 4))
            {
                int tickIndex = (int)Math.Round(i / (cmPixels / 4));
                double tickWidth = 4;

                if (tickIndex % 4 == 0) tickWidth = 12;
                else if (tickIndex % 2 == 0) tickWidth = 8;

                var line = new Line
                {
                    X1 = 24, Y1 = i,
                    X2 = 24 - tickWidth, Y2 = i,
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.7
                };
                canvas.Children.Add(line);

                if (tickIndex % 4 == 0)
                {
                    int cmValue = tickIndex / 4;
                    var textBlock = new TextBlock
                    {
                        Text = cmValue.ToString(),
                        FontSize = 10,
                        FontWeight = FontWeights.SemiBold,
                        FontFamily = new FontFamily("Arial"),
                        Foreground = Brushes.Black
                    };
                    
                    Canvas.SetLeft(textBlock, 2);
                    Canvas.SetTop(textBlock, i - 6);
                    canvas.Children.Add(textBlock);
                }
            }
        }

        private void FrontTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawTopRuler(FrontTopRulerCanvas, e.NewSize.Width);
        private void FrontLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawLeftRuler(FrontLeftRulerCanvas, e.NewSize.Height);
        private void FrontMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }
        
        private void FrontCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void FrontCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void FrontCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void BackTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawTopRuler(BackTopRulerCanvas, e.NewSize.Width);
        private void BackLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawLeftRuler(BackLeftRulerCanvas, e.NewSize.Height);
        private void BackMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }

        private void BackCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void BackCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void BackCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void ReportTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawTopRuler(ReportTopRulerCanvas, e.NewSize.Width);
        private void ReportLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawLeftRuler(ReportLeftRulerCanvas, e.NewSize.Height);
        private void ReportMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }

        private void ReportOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void ReportOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void ReportOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        // Help Menu & F1 Shortcut Event Handler
        private void OnlineHelp_Click(object sender, RoutedEventArgs e)
        {
            var helpWindow = new IDWorksEnterpriseDesigner.Views.HelpWindow();
            helpWindow.Owner = this;
            helpWindow.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Datacard ID Works Enterprise Designer\nVersion 1.0.0\n\nAll rights reserved.", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
