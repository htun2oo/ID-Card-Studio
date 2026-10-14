using System;
using System.Collections.Generic;
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
        private readonly Brush subTickColor = new SolidColorBrush(Color.FromRgb(70, 130, 180));

        private bool isDraggingGuide = false;
        private bool isHorizontalGuide = true;
        private bool hasEnteredCanvas = false;
        private Line? draggedGuideLine = null;
        private Canvas? activeOverlayCanvas = null;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            
            this.MouseMove += MainWindow_MouseMove;
            this.MouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FrontTopRulerCanvas.MouseLeftButtonDown += (s, ev) => { StartDragGuide(ev, true, FrontCardOverlayCanvas); ev.Handled = true; };
            FrontLeftRulerCanvas.MouseLeftButtonDown += (s, ev) => { StartDragGuide(ev, false, FrontCardOverlayCanvas); ev.Handled = true; };

            BackTopRulerCanvas.MouseLeftButtonDown += (s, ev) => { StartDragGuide(ev, true, BackCardOverlayCanvas); ev.Handled = true; };
            BackLeftRulerCanvas.MouseLeftButtonDown += (s, ev) => { StartDragGuide(ev, false, BackCardOverlayCanvas); ev.Handled = true; };

            ReportTopRulerCanvas.MouseLeftButtonDown += (s, ev) => { StartDragGuide(ev, true, ReportOverlayCanvas); ev.Handled = true; };
            ReportLeftRulerCanvas.MouseLeftButtonDown += (s, ev) => { StartDragGuide(ev, false, ReportOverlayCanvas); ev.Handled = true; };

            SetupCanvasGuideInteractions(FrontCardOverlayCanvas);
            SetupCanvasGuideInteractions(BackCardOverlayCanvas);
            SetupCanvasGuideInteractions(ReportOverlayCanvas);
        }

        private void SetupCanvasGuideInteractions(Canvas overlayCanvas)
        {
            if (overlayCanvas == null) return;

            overlayCanvas.MouseMove += (s, ev) =>
            {
                if (isDraggingGuide) return;

                Point mousePos = ev.GetPosition(overlayCanvas);
                Line? targetLine = null;

                foreach (var child in overlayCanvas.Children)
                {
                    if (child is Line line)
                    {
                        bool isHorizontal = (line.Y1 == line.Y2);
                        if (isHorizontal)
                        {
                            if (Math.Abs(mousePos.Y - line.Y1) <= 5 && mousePos.X >= 0 && mousePos.X <= overlayCanvas.Width)
                            {
                                targetLine = line;
                                break;
                            }
                        }
                        else
                        {
                            if (Math.Abs(mousePos.X - line.X1) <= 5 && mousePos.Y >= 0 && mousePos.Y <= overlayCanvas.Height)
                            {
                                targetLine = line;
                                break;
                            }
                        }
                    }
                }

                if (targetLine != null)
                {
                    overlayCanvas.Cursor = (targetLine.Y1 == targetLine.Y2) ? Cursors.SizeNS : Cursors.SizeWE;
                }
                else
                {
                    overlayCanvas.Cursor = Cursors.Arrow;
                }
            };

            overlayCanvas.MouseLeftButtonDown += (s, ev) =>
            {
                Point mousePos = ev.GetPosition(overlayCanvas);
                Line? targetLine = null;

                foreach (var child in overlayCanvas.Children)
                {
                    if (child is Line line)
                    {
                        bool isHorizontal = (line.Y1 == line.Y2);
                        if (isHorizontal)
                        {
                            if (Math.Abs(mousePos.Y - line.Y1) <= 6 && mousePos.X >= 0 && mousePos.X <= overlayCanvas.Width)
                            {
                                targetLine = line;
                                break;
                            }
                        }
                        else
                        {
                            if (Math.Abs(mousePos.X - line.X1) <= 6 && mousePos.Y >= 0 && mousePos.Y <= overlayCanvas.Height)
                            {
                                targetLine = line;
                                break;
                            }
                        }
                    }
                }

                if (targetLine != null && targetLine.Parent is Canvas parentCanvas)
                {
                    draggedGuideLine = targetLine;
                    activeOverlayCanvas = parentCanvas;
                    isDraggingGuide = true;
                    isHorizontalGuide = (targetLine.Y1 == targetLine.Y2);
                    hasEnteredCanvas = true;
                    ev.Handled = true;
                }
            };
        }

        private void StartDragGuide(MouseButtonEventArgs e, bool isHorizontal, Canvas targetCanvas)
        {
            if (targetCanvas == null) return;

            isDraggingGuide = true;
            isHorizontalGuide = isHorizontal;
            activeOverlayCanvas = targetCanvas;
            hasEnteredCanvas = false;

            Point mousePos = e.GetPosition(activeOverlayCanvas);

            draggedGuideLine = new Line
            {
                Stroke = new SolidColorBrush(Color.FromRgb(0, 120, 215)),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection { 4, 2 }
            };

            draggedGuideLine.MouseLeftButtonDown += (s, ev) =>
            {
                if (ev.ClickCount == 2 && s is Line line && line.Parent is Canvas parentCanvas)
                {
                    parentCanvas.Children.Remove(line);
                    ev.Handled = true;
                }
            };

            if (isHorizontalGuide)
            {
                draggedGuideLine.X1 = 0;
                draggedGuideLine.X2 = activeOverlayCanvas.Width;
                double initialY = Math.Clamp(mousePos.Y, 0, activeOverlayCanvas.Height);
                draggedGuideLine.Y1 = initialY;
                draggedGuideLine.Y2 = initialY;
                activeOverlayCanvas.Cursor = Cursors.SizeNS;
            }
            else
            {
                draggedGuideLine.Y1 = 0;
                draggedGuideLine.Y2 = activeOverlayCanvas.Height;
                double initialX = Math.Clamp(mousePos.X, 0, activeOverlayCanvas.Width);
                draggedGuideLine.X1 = initialX;
                draggedGuideLine.X2 = initialX;
                activeOverlayCanvas.Cursor = Cursors.SizeWE;
            }

            if (!activeOverlayCanvas.Children.Contains(draggedGuideLine))
            {
                activeOverlayCanvas.Children.Add(draggedGuideLine);
            }

            Mouse.Capture(this);
            e.Handled = true;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggingGuide || draggedGuideLine == null || activeOverlayCanvas == null) return;

            Point mousePos = e.GetPosition(activeOverlayCanvas);
            bool isInside = mousePos.X >= 0 && mousePos.Y >= 0 && mousePos.X <= activeOverlayCanvas.Width && mousePos.Y <= activeOverlayCanvas.Height;

            if (isInside)
            {
                hasEnteredCanvas = true;
            }

            if (hasEnteredCanvas)
            {
                if (isHorizontalGuide)
                {
                    if (mousePos.Y < -10 || mousePos.Y > activeOverlayCanvas.Height + 10)
                    {
                        activeOverlayCanvas.Children.Remove(draggedGuideLine);
                        isDraggingGuide = false;
                        draggedGuideLine = null;
                        activeOverlayCanvas.Cursor = Cursors.Arrow;
                        ReleaseMouseCapture();
                        return;
                    }
                }
                else
                {
                    if (mousePos.X < -10 || mousePos.X > activeOverlayCanvas.Width + 10)
                    {
                        activeOverlayCanvas.Children.Remove(draggedGuideLine);
                        isDraggingGuide = false;
                        draggedGuideLine = null;
                        activeOverlayCanvas.Cursor = Cursors.Arrow;
                        ReleaseMouseCapture();
                        return;
                    }
                }
            }

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

        private void FrontTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
        private void FrontLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
        private void BackTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
        private void BackLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) => DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
        
        private void ReportTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ReportSizeComboBox != null && ReportSizeComboBox.SelectedItem is ComboBoxItem item)
            {
                string? tag = item.Tag?.ToString();
                double w = 21.0, h = 29.7;
                if (tag == "A5") { w = 14.8; h = 21.0; }
                else if (tag == "B5") { w = 17.6; h = 25.0; }
                else if (tag == "Letter") { w = 21.59; h = 27.94; }
                else if (tag == "Legal") { w = 21.59; h = 35.56; }
                DrawReportRulers(w, h);
            }
        }
        private void ReportLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }

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

            if (ReportSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string? tag = selectedItem.Tag?.ToString();
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

        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            Views.NewProjectWindow newProjectWin = new Views.NewProjectWindow
            {
                Owner = this
            };

            if (newProjectWin.ShowDialog() == true)
            {
                ActivateProjectWorkspace();
            }
        }

        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            // အကယ်၍ Save လုပ်ထားသော Project များ မရှိသေးပါက list ကို null သို့မဟုတ် empty ထားနိုင်သည်
            List<string> existingProjects = new List<string>
            {
                "Sample",
                "Sample Badge",
                "Sample Corporate",
                "Sample Government",
                "Sample Healthcare",
                "Sample Loyalty",
                "Sample University"
            };

            Views.OpenProjectWindow openProjectWin = new Views.OpenProjectWindow(existingProjects)
            {
                Owner = this
            };

            if (openProjectWin.ShowDialog() == true)
            {
                ActivateProjectWorkspace();
            }
        }

        private void ActivateProjectWorkspace()
        {
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Visible;
            }

            MenuCloseProject.Visibility = Visibility.Visible;
            MenuSep1.Visibility = Visibility.Visible;
            MenuSaveProject.Visibility = Visibility.Visible;
            MenuSaveProjectAs.Visibility = Visibility.Visible;
            MenuSep2.Visibility = Visibility.Visible;
            MenuProjectProperties.Visibility = Visibility.Visible;
            MenuSep3.Visibility = Visibility.Visible;
            MenuPrintCard.Visibility = Visibility.Visible;
            MenuSep4.Visibility = Visibility.Visible;
            MenuDeleteProject.Visibility = Visibility.Visible;
            MenuSep5.Visibility = Visibility.Visible;
            MenuRecentFileItem.Visibility = Visibility.Visible;
            MenuSep6.Visibility = Visibility.Visible;

            MenuEdit.Visibility = Visibility.Visible;
            MenuInsert.Visibility = Visibility.Visible;
            MenuFormat.Visibility = Visibility.Visible;
            MenuTools.Visibility = Visibility.Visible;
            MenuWindow.Visibility = Visibility.Visible;

            DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
            DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
            DrawReportRulers(21.0, 29.7);
        }

        private void CloseProject_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectWorkspace != null)
            {
                ProjectWorkspace.Visibility = Visibility.Collapsed;
            }

            MenuCloseProject.Visibility = Visibility.Collapsed;
            MenuSep1.Visibility = Visibility.Collapsed;
            MenuSaveProject.Visibility = Visibility.Collapsed;
            MenuSaveProjectAs.Visibility = Visibility.Collapsed;
            MenuSep2.Visibility = Visibility.Collapsed;
            MenuProjectProperties.Visibility = Visibility.Collapsed;
            MenuSep3.Visibility = Visibility.Collapsed;
            MenuPrintCard.Visibility = Visibility.Collapsed;
            MenuSep4.Visibility = Visibility.Collapsed;
            MenuDeleteProject.Visibility = Visibility.Collapsed;
            MenuSep5.Visibility = Visibility.Collapsed;
            MenuRecentFileItem.Visibility = Visibility.Collapsed;
            MenuSep6.Visibility = Visibility.Collapsed;

            MenuEdit.Visibility = Visibility.Collapsed;
            MenuInsert.Visibility = Visibility.Collapsed;
            MenuFormat.Visibility = Visibility.Collapsed;
            MenuTools.Visibility = Visibility.Collapsed;
            MenuWindow.Visibility = Visibility.Collapsed;
        }

        private void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Project saved successfully.", "Save Project", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SaveProjectAs_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "ID Works Project Files (*.iwp)|*.iwp|All Files (*.*)|*.*"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                MessageBox.Show($"Project saved to: {saveFileDialog.FileName}", "Save Project As", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ProjectProperties_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Project Properties configuration window.", "Properties", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void PrintSampleCard_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Printing sample card...", "Print", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DeleteProject_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete this project?", "Delete Project", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                CloseProject_Click(sender, e);
            }
        }

        private void RecentFile_Click(object sender, RoutedEventArgs e)
        {
            ActivateProjectWorkspace();
        }

        private void ReportPageSetup_Click(object sender, RoutedEventArgs e) { }
        private void Exit_Click(object sender, RoutedEventArgs e) { Application.Current.Shutdown(); }

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
