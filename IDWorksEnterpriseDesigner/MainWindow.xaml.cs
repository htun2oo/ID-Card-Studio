using System;
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
        public static readonly RoutedCommand NewProjectCommand = new RoutedCommand();
        public static readonly RoutedCommand OpenProjectCommand = new RoutedCommand();
        public static readonly RoutedCommand OnlineHelpCommand = new RoutedCommand();

        // Photoshop Guide Dragging State Variables
        private bool isDraggingGuide = false;
        private bool isVerticalGuide = false; 
        private Line? activeGuideLine = null;
        private Canvas? activeGuidesCanvas = null;

        public MainWindow()
        {
            InitializeComponent();

            // Ctrl+N, Ctrl+O နှင့် F1 Shortcuts များအတွက် CommandBindings များကို ပုံစံမှန်ကန်စွာ ချိတ်ဆက်ခြင်း
            CommandBindings.Add(new CommandBinding(NewProjectCommand, (s, e) => NewProject_Click(s, new RoutedEventArgs())));
            CommandBindings.Add(new CommandBinding(OpenProjectCommand, (s, e) => OpenProject_Click(s, new RoutedEventArgs())));
            CommandBindings.Add(new CommandBinding(OnlineHelpCommand, (s, e) => OnlineHelp_Click(s, new RoutedEventArgs())));
        }

        // --- Menu and Toolbar Click Event Handlers ---
        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            // New Project ဒိုင်ယာလော့ခ် မပေါ်လာမီ Workspace ကို ဦးစွာပိတ်ထားမည် (Collapsed)
            CloseProject_Click(sender, e);
            
            NewProjectWindow dlg = new NewProjectWindow();
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                // Portrait သို့မဟုတ် Landscape အပေါ်မူတည်၍ Card ၏ Width နှင့် Height ကို သတ်မှတ်ခြင်း
                if (dlg.IsPortrait)
                {
                    // Portrait (အမြင့်ပိုရှည်မည် - ဥပမာ Width: 204.04, Height: 323.57)
                    if (CardDesigner != null) { CardDesigner.Width = 204.04; CardDesigner.Height = 323.57; }
                    if (FrontCardOverlayCanvas != null) { FrontCardOverlayCanvas.Width = 204.04; FrontCardOverlayCanvas.Height = 323.57; }

                    if (CardDesignerBack != null) { CardDesignerBack.Width = 204.04; CardDesignerBack.Height = 323.57; }
                    if (BackCardOverlayCanvas != null) { BackCardOverlayCanvas.Width = 204.04; BackCardOverlayCanvas.Height = 323.57; }
                }
                else
                {
                    // Landscape (အကျယ်ပိုရှည်မည် - မူလအတိုင်း Width: 323.57, Height: 204.04)
                    if (CardDesigner != null) { CardDesigner.Width = 323.57; CardDesigner.Height = 204.04; }
                    if (FrontCardOverlayCanvas != null) { FrontCardOverlayCanvas.Width = 323.57; FrontCardOverlayCanvas.Height = 204.04; }

                    if (CardDesignerBack != null) { CardDesignerBack.Width = 323.57; CardDesignerBack.Height = 204.04; }
                    if (BackCardOverlayCanvas != null) { BackCardOverlayCanvas.Width = 323.57; BackCardOverlayCanvas.Height = 204.04; }
                }
                
                ShowWorkspaceUI();
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            // Open Project ဒိုင်ယာလော့ခ် မပေါ်လာမီ Workspace ကို ဦးစွာပိတ်ထားမည် (Collapsed)
            CloseProject_Click(sender, e);
            
            OpenProjectWindow dlg = new OpenProjectWindow();
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                // OK နှိပ်မှသာ Workspace ကို ပြသမည်
                ShowWorkspaceUI();
            }
        }

        public void ShowWorkspaceUI()
        {
            ProjectWorkspace.Visibility = Visibility.Visible;

            // Designer surface window တွင် Recent File အပို မပေါ်စေရန် Collapsed လုပ်ခြင်း
            MenuRecentFileItem.Visibility = Visibility.Collapsed;
            MenuSepRecentTop.Visibility = Visibility.Collapsed;
            MenuSepRecentBottom.Visibility = Visibility.Collapsed;

            // Project Workspace ပွင့်ချိန်တွင် ပေါ်လာမည့် မီနူးများ
            MenuCloseProject.Visibility = Visibility.Visible;
            MenuSaveProject.Visibility = Visibility.Visible;
            MenuSaveProjectAs.Visibility = Visibility.Visible;
            MenuProjectProperties.Visibility = Visibility.Visible;
            MenuPrintCard.Visibility = Visibility.Visible;
            MenuDeleteProject.Visibility = Visibility.Visible;
            MenuSampleFilePath.Visibility = Visibility.Visible; // နမူနာ ဖိုင်လမ်းကြောင်း
            
            MenuEdit.Visibility = Visibility.Visible;
            MenuInsert.Visibility = Visibility.Visible;
            MenuFormat.Visibility = Visibility.Visible;
            MenuTools.Visibility = Visibility.Visible;
            MenuWindow.Visibility = Visibility.Visible;
            
            MenuSep1.Visibility = Visibility.Visible;
            MenuSep2.Visibility = Visibility.Visible;
            MenuSep3.Visibility = Visibility.Visible;
            MenuSep4.Visibility = Visibility.Visible;
            MenuSep5.Visibility = Visibility.Visible;
            MenuSep6.Visibility = Visibility.Visible;            
        }

        private void CloseProject_Click(object sender, RoutedEventArgs e) 
        { 
            ProjectWorkspace.Visibility = Visibility.Collapsed;

            // Software စစချင်း Main Window အခြေအနေသို့ ပြန်ရောက်ပါက Recent File ကို ပြန်ပြခြင်း
            MenuRecentFileItem.Visibility = Visibility.Visible;
            MenuSepRecentTop.Visibility = Visibility.Visible;
            MenuSepRecentBottom.Visibility = Visibility.Visible;

            // Project ဆိုင်ရာ မီနူးများကို ပြန်ဖျောက်ခြင်း
            MenuCloseProject.Visibility = Visibility.Collapsed; 
            MenuSaveProject.Visibility = Visibility.Collapsed;
            MenuSaveProjectAs.Visibility = Visibility.Collapsed;
            MenuProjectProperties.Visibility = Visibility.Collapsed;
            MenuPrintCard.Visibility = Visibility.Collapsed;
            MenuDeleteProject.Visibility = Visibility.Collapsed;
            MenuSampleFilePath.Visibility = Visibility.Collapsed;
            
            MenuEdit.Visibility = Visibility.Collapsed;
            MenuInsert.Visibility = Visibility.Collapsed;
            MenuFormat.Visibility = Visibility.Collapsed;
            MenuTools.Visibility = Visibility.Collapsed;
            MenuWindow.Visibility = Visibility.Collapsed;
            
            MenuSep1.Visibility = Visibility.Collapsed;
            MenuSep2.Visibility = Visibility.Collapsed;
            MenuSep3.Visibility = Visibility.Collapsed;
            MenuSep4.Visibility = Visibility.Collapsed;
            MenuSep5.Visibility = Visibility.Collapsed;
            MenuSep6.Visibility = Visibility.Collapsed;            
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e) { }
        private void SaveProjectAs_Click(object sender, RoutedEventArgs e) { }
        private void ProjectProperties_Click(object sender, RoutedEventArgs e) { }
        private void PrintSampleCard_Click(object sender, RoutedEventArgs e) { }
        private void ReportPageSetup_Click(object sender, RoutedEventArgs e) { }
        private void DeleteProject_Click(object sender, RoutedEventArgs e) { CloseProject_Click(sender, e); }
        private void RecentFile_Click(object sender, RoutedEventArgs e) { }
        private void Exit_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }
        
        private void OnlineHelp_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Online Help..."); }
        private void About_Click(object sender, RoutedEventArgs e) { MessageBox.Show("Datacard ID Works Enterprise Designer"); }

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

        private void ReportSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        // --- ScrollViewer ScrollChanged Events ---
        private void FrontMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }
        private void BackMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }
        private void ReportMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }

        // --- Ruler Drawing Logic ---
        private void DrawHorizontalRuler(Canvas canvas)
        {
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                if (canvas.Children[i] is Line)
                {
                    canvas.Children.RemoveAt(i);
                }
            }

            double step = 37.8;
            double width = Math.Max(canvas.ActualWidth, 1000);

            for (double x = 0; x <= width; x += step / 10.0)
            {
                bool isMajor = Math.Abs((x % step)) < 0.1 || Math.Abs((x % step) - step) < 0.1;
                bool isHalf = Math.Abs((x % (step / 2.0))) < 0.1 || Math.Abs((x % (step / 2.0)) - (step / 2.0)) < 0.1;
                double tickHeight = isMajor ? 12 : (isHalf ? 8 : 4);

                Line tick = new Line
                {
                    X1 = x,
                    Y1 = canvas.ActualHeight - tickHeight,
                    X2 = x,
                    Y2 = canvas.ActualHeight,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(tick);
            }
        }

        private void DrawVerticalRuler(Canvas canvas)
        {
            for (int i = canvas.Children.Count - 1; i >= 0; i--)
            {
                if (canvas.Children[i] is Line)
                {
                    canvas.Children.RemoveAt(i);
                }
            }

            double step = 37.8;
            double height = Math.Max(canvas.ActualHeight, 1000);

            for (double y = 0; y <= height; y += step / 10.0)
            {
                bool isMajor = Math.Abs((y % step)) < 0.1 || Math.Abs((y % step) - step) < 0.1;
                bool isHalf = Math.Abs((y % (step / 2.0))) < 0.1 || Math.Abs((y % (step / 2.0)) - (step / 2.0)) < 0.1;
                double tickWidth = isMajor ? 12 : (isHalf ? 8 : 4);

                Line tick = new Line
                {
                    X1 = canvas.ActualWidth - tickWidth,
                    Y1 = y,
                    X2 = canvas.ActualWidth,
                    Y2 = y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                canvas.Children.Add(tick);
            }
        }

        private void FrontTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { DrawHorizontalRuler(FrontTopRulerCanvas); }
        private void FrontLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { DrawVerticalRuler(FrontLeftRulerCanvas); }
        private void BackTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { DrawHorizontalRuler(BackTopRulerCanvas); }
        private void BackLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { DrawVerticalRuler(BackLeftRulerCanvas); }
        private void ReportTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { DrawHorizontalRuler(ReportTopRulerCanvas); }
        private void ReportLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { DrawVerticalRuler(ReportLeftRulerCanvas); }

        // --- Photoshop-Style Interactive Guide Line Dragging & Moving Logic ---

        private void TopRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas ruler && ruler.Tag is Canvas guidesCanvas)
            {
                activeGuidesCanvas = guidesCanvas;
                isVerticalGuide = false; // Horizontal Guide
                isDraggingGuide = true;
                activeGuideLine = null;

                Mouse.OverrideCursor = Cursors.SizeNS;

                this.CaptureMouse();
                e.Handled = true;
            }
        }

        private void LeftRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas ruler && ruler.Tag is Canvas guidesCanvas)
            {
                activeGuidesCanvas = guidesCanvas;
                isVerticalGuide = true; // Vertical Guide
                isDraggingGuide = true;
                activeGuideLine = null;

                Mouse.OverrideCursor = Cursors.SizeWE;

                this.CaptureMouse();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isDraggingGuide && activeGuidesCanvas != null)
            {
                Point pos = e.GetPosition(activeGuidesCanvas);

                bool hasLeftRuler = isVerticalGuide ? (pos.X >= 0) : (pos.Y >= 0);

                if (activeGuideLine == null && hasLeftRuler)
                {
                    activeGuideLine = new Line
                    {
                        Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B4F0")),
                        StrokeThickness = 1,
                        Cursor = isVerticalGuide ? Cursors.SizeWE : Cursors.SizeNS
                    };

                    if (isVerticalGuide)
                    {
                        activeGuideLine.Y1 = 0;
                        activeGuideLine.Y2 = activeGuidesCanvas.ActualHeight;
                    }
                    else
                    {
                        activeGuideLine.X1 = 0;
                        activeGuideLine.X2 = activeGuidesCanvas.ActualWidth;
                    }

                    activeGuidesCanvas.Children.Add(activeGuideLine);
                }

                if (activeGuideLine != null)
                {
                    if (isVerticalGuide)
                    {
                        activeGuideLine.X1 = pos.X;
                        activeGuideLine.X2 = pos.X;
                    }
                    else
                    {
                        activeGuideLine.Y1 = pos.Y;
                        activeGuideLine.Y2 = pos.Y;
                    }
                }
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (isDraggingGuide && activeGuidesCanvas != null)
            {
                Point pos = e.GetPosition(activeGuidesCanvas);

                bool outOfBounds = isVerticalGuide ? (pos.X < 0 || pos.X > activeGuidesCanvas.ActualWidth) 
                                                   : (pos.Y < 0 || pos.Y > activeGuidesCanvas.ActualHeight);

                if (outOfBounds && activeGuideLine != null)
                {
                    activeGuidesCanvas.Children.Remove(activeGuideLine);
                }

                Canvas canvasToUpdate = activeGuidesCanvas;

                Mouse.OverrideCursor = null;

                this.ReleaseMouseCapture();
                isDraggingGuide = false;
                activeGuideLine = null;
                activeGuidesCanvas = null;

                if (canvasToUpdate != null)
                {
                    bool foundCloseLine = false;
                    foreach (var child in canvasToUpdate.Children)
                    {
                        if (child is Line line)
                        {
                            bool isVert = (line.X1 == line.X2);
                            if ((isVert && Math.Abs(pos.X - line.X1) < 3) || (!isVert && Math.Abs(pos.Y - line.Y1) < 3))
                            {
                                canvasToUpdate.Cursor = isVert ? Cursors.SizeWE : Cursors.SizeNS;
                                foundCloseLine = true;
                                break;
                            }
                        }
                    }
                    if (!foundCloseLine)
                    {
                        canvasToUpdate.Cursor = Cursors.Arrow;
                    }
                }
            }
        }

        // --- Guide Line Hover & Click-to-Move Logic on Canvas ---

        private void GuidesCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Canvas guidesCanvas && !isDraggingGuide)
            {
                Point pos = e.GetPosition(guidesCanvas);
                bool foundCloseLine = false;

                foreach (var child in guidesCanvas.Children)
                {
                    if (child is Line line)
                    {
                        bool isVert = (line.X1 == line.X2);
                        if ((isVert && Math.Abs(pos.X - line.X1) < 3) || (!isVert && Math.Abs(pos.Y - line.Y1) < 3))
                        {
                            guidesCanvas.Cursor = isVert ? Cursors.SizeWE : Cursors.SizeNS;
                            foundCloseLine = true;
                            break;
                        }
                    }
                }

                if (!foundCloseLine)
                {
                    guidesCanvas.Cursor = Cursors.Arrow;
                }
            }
        }

        private void GuidesCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas guidesCanvas)
            {
                Point pos = e.GetPosition(guidesCanvas);

                foreach (var child in guidesCanvas.Children)
                {
                    if (child is Line line)
                    {
                        bool isVert = (line.X1 == line.X2);
                        if ((isVert && Math.Abs(pos.X - line.X1) < 3) || (!isVert && Math.Abs(pos.Y - line.Y1) < 3))
                        {
                            activeGuidesCanvas = guidesCanvas;
                            activeGuideLine = line;
                            isVerticalGuide = isVert;
                            isDraggingGuide = true;

                            Mouse.OverrideCursor = isVert ? Cursors.SizeWE : Cursors.SizeNS;

                            this.CaptureMouse();
                            e.Handled = true;
                            return;
                        }
                    }
                }
            }
        }
    }
}
