using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using IDWorksEnterpriseDesigner.Views;

namespace IDWorksEnterpriseDesigner
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow dialog = new NewProjectWindow();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                // Window Title ကို ပုံအတိုင်း ပြောင်းလဲခြင်း
                this.Title = $"Datacard ID Works Enterprise Designer - {dialog.ProjectName} - [Card]";

                // မျက်နှာပြင်ဟောင်းများကို ရှင်းလင်းခြင်း
                MdiHostGrid.Children.Clear();

                // 2x2 Layout ဖွဲ့စည်းရန် Grid Layout ထည့်သွင်းခြင်း
                Grid layoutGrid = new Grid();
                layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(280) }); // Left Column (Card Windows)
                layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) }); // Right Column

                layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) }); // Top Row
                layoutGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(220) }); // Bottom Row

                MdiHostGrid.Children.Add(layoutGrid);

                // 1. Card & Card (back side) - Left Column
                if (dialog.HasCardDesign)
                {
                    Grid leftStack = new Grid();
                    leftStack.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    leftStack.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });

                    Grid.SetColumn(leftStack, 0);
                    Grid.SetRowSpan(leftStack, 2);
                    layoutGrid.Children.Add(leftStack);

                    // Card (Front Side) Window
                    Border cardFrontWindow = CreateDesignerWindow("Card", true);
                    Grid.SetRow(cardFrontWindow, 0);
                    leftStack.Children.Add(cardFrontWindow);

                    // Card (Back Side) Window
                    Border cardBackWindow = CreateDesignerWindow("Card (back side)", true);
                    Grid.SetRow(cardBackWindow, 1);
                    leftStack.Children.Add(cardBackWindow);
                }

                // 2. Production Form Window - Top Right
                if (dialog.HasProductionForm)
                {
                    Border prodFormWindow = CreateProductionFormWindow("Production Form");
                    Grid.SetColumn(prodFormWindow, 1);
                    Grid.SetRow(prodFormWindow, 0);
                    layoutGrid.Children.Add(prodFormWindow);
                }

                // 3. Report Window - Bottom Right
                if (dialog.HasReportDesign)
                {
                    Border reportWindow = CreateDesignerWindow("Report", false);
                    Grid.SetColumn(reportWindow, 1);
                    Grid.SetRow(reportWindow, 1);
                    layoutGrid.Children.Add(reportWindow);
                }
            }
        }

        // Card နှင့် Report တို့အတွက် Ruler ပါသော Window ဖန်တီးပေးသည့် Helper Method
        private Border CreateDesignerWindow(string title, bool isCard)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(160, 180, 210)),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(4),
                Background = new SolidColorBrush(Color.FromRgb(240, 240, 240))
            };

            DockPanel panel = new DockPanel();

            // Title Bar
            Grid titleBar = new Grid { Background = new SolidColorBrush(Color.FromRgb(210, 225, 245)), Height = 22 };
            DockPanel.SetDock(titleBar, DockPanel.Dock.Top);
            
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

            // Window Control Buttons (Minimize, Maximize, Close)
            StackPanel winBtns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 4, 0) };
            winBtns.Children.Add(new Button { Content = " ", Width = 14, Height = 14, Margin = new Thickness(1) });
            winBtns.Children.Add(new Button { Content = " ", Width = 14, Height = 14, Margin = new Thickness(1) });
            winBtns.Children.Add(new Button { Content = "r", FontFamily = new FontFamily("Marlett"), FontSize = 9, Width = 14, Height = 14, Margin = new Thickness(1) });
            titleBar.Children.Add(winBtns);

            panel.Children.Add(titleBar);

            // Designer Canvas Area (Ruler & White Canvas)
            Grid canvasArea = new Grid { Background = new SolidColorBrush(Color.FromRgb(200, 200, 200)) };
            canvasArea.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20) });
            canvasArea.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            canvasArea.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            canvasArea.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Horizontal & Vertical Rulers
            Rectangle topRuler = new Rectangle { Fill = new SolidColorBrush(Color.FromRgb(225, 225, 225)), Stroke = Brushes.Gray };
            Grid.SetColumn(topRuler, 1);
            Grid.SetRow(topRuler, 0);
            canvasArea.Children.Add(topRuler);

            Rectangle leftRuler = new Rectangle { Fill = new SolidColorBrush(Color.FromRgb(225, 225, 225)), Stroke = Brushes.Gray };
            Grid.SetColumn(leftRuler, 0);
            Grid.SetRow(leftRuler, 1);
            canvasArea.Children.Add(leftRuler);

            // White Working Paper Canvas
            Border paper = new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10)
            };

            if (isCard)
            {
                paper.Width = 200;
                paper.Height = 125;
            }

            Grid.SetColumn(paper, 1);
            Grid.SetRow(paper, 1);
            canvasArea.Children.Add(paper);

            panel.Children.Add(canvasArea);
            border.Child = panel;

            return border;
        }

        // Production Form Window ဖန်တီးပေးသည့် Helper Method
        private Border CreateProductionFormWindow(string title)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(160, 180, 210)),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(4),
                Background = Brushes.White
            };

            DockPanel panel = new DockPanel();

            // Title Bar
            Grid titleBar = new Grid { Background = new SolidColorBrush(Color.FromRgb(210, 225, 245)), Height = 22 };
            DockPanel.SetDock(titleBar, DockPanel.Dock.Top);

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

            // Window Control Buttons
            StackPanel winBtns = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 0, 4, 0) };
            winBtns.Children.Add(new Button { Content = " ", Width = 14, Height = 14, Margin = new Thickness(1) });
            winBtns.Children.Add(new Button { Content = " ", Width = 14, Height = 14, Margin = new Thickness(1) });
            winBtns.Children.Add(new Button { Content = "r", FontFamily = new FontFamily("Marlett"), FontSize = 9, Width = 14, Height = 14, Margin = new Thickness(1) });
            titleBar.Children.Add(winBtns);

            panel.Children.Add(titleBar);
            border.Child = panel;

            return border;
        }
    }
}
