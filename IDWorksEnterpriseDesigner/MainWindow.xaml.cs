using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;

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

        private void FrontTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }
        private void FrontLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }
        private void FrontMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }
        
        private void FrontCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void FrontCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void FrontCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void BackTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }
        private void BackLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }
        private void BackMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }

        private void BackCardOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void BackCardOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void BackCardOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        private void ReportTopRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }
        private void ReportLeftRulerCanvas_SizeChanged(object sender, SizeChangedEventArgs e) { }
        private void ReportMainScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) { }

        private void ReportOverlayCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { }
        private void ReportOverlayCanvas_MouseMove(object sender, MouseEventArgs e) { }
        private void ReportOverlayCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) { }

        // Help Menu Event Handlers
        private void OnlineHelp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.google.com",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open help: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Datacard ID Works Enterprise Designer\nVersion 1.0.0\n\nAll rights reserved.", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
