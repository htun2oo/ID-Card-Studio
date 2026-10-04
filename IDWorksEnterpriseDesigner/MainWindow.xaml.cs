using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives; // UniformGrid အတွက် ထည့်သွင်းထားပါသည်
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
                // Window Title ပြောင်းလဲခြင်း
                this.Title = $"Datacard ID Works Enterprise Designer - {dialog.ProjectName} - [Card]";

                // Project သစ်ဖွင့်လိုက်ပါက Menu Items များကို ဖော်ပြပေးခြင်း
                MenuEdit.Visibility = Visibility.Visible;
                MenuInsert.Visibility = Visibility.Visible;
                MenuFormat.Visibility = Visibility.Visible;
                MenuTools.Visibility = Visibility.Visible;
                MenuWindow.Visibility = Visibility.Visible;

                // Workspace ကို ရှင်းလင်းခြင်း
                MdiHostGrid.Children.Clear();

                // MDI Layout ဖွဲ့စည်းပုံ
                Grid layoutGrid = new Grid();
                layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(275) });
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

                    // Front Card Window
                    Border cardFrontWindow = CreateDesignerSubWindow("Card", true, 3);
                    Grid.SetRow(cardFrontWindow, 0);
                    leftStack.Children.Add(cardFrontWindow);

                    // Back Card Window
                    Border cardBackWindow = CreateDesignerSubWindow("Card (back side)", true, 3);
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
                    Border reportWindow = CreateDesignerSubWindow("Report", false, 6);
                    Grid.SetColumn(reportWindow, 1);
                    Grid.SetRow(reportWindow, 1);
                    layoutGrid.Children.Add(reportWindow);
                }
            }
        }

        // Sub-Window (Card & Report) ဖန်တီးပေးသည့် နည်းလမ်း
        private Border CreateDesignerSubWindow(string title, bool isCard, int maxRulerUnit)
        {
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(180, 200, 230)),
                BorderThickness = new Thickness(3),
                Margin = new Thickness(2),
                Background = Brushes.White
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

            // Sub-Window Buttons (Minimize, Maximize, Close)
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

            // Horizontal Ruler
            UniformGrid topRuler = new UniformGrid { Rows = 1, Columns = maxRulerUnit, Background = Brushes.LightGray };
            for (int i = 1; i <= maxRulerUnit; i++)
            {
                topRuler.Children.Add(new TextBlock { Text = i.ToString(), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom, FontSize = 10, Margin = new Thickness(0, 0, 2, 0) });
            }
            Grid.SetColumn(topRuler, 1);
            Grid.SetRow(topRuler, 0);
            canvasGrid.Children.Add(topRuler);

            // Vertical Ruler
            UniformGrid leftRuler = new UniformGrid { Columns = 1, Rows = 2, Background = Brushes.LightGray };
            leftRuler.Children.Add(new TextBlock { Text = "1", VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 10 });
            leftRuler.Children.Add(new TextBlock { Text = "2", VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Center, FontSize = 10 });
            Grid.SetColumn(leftRuler, 0);
            Grid.SetRow(leftRuler, 1);
            canvasGrid.Children.Add(leftRuler);

            // White Canvas Container
            Border paperBorder = new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(5)
            };

            if (isCard)
            {
                paperBorder.Width = 210;
                paperBorder.Height = 130;
            }

            Grid.SetColumn(paperBorder, 1);
            Grid.SetRow(paperBorder, 1);
            canvasGrid.Children.Add(paperBorder);

            panel.Children.Add(canvasGrid);
            border.Child = panel;

            return border;
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
