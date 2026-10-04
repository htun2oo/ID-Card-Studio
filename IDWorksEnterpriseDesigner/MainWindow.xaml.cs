using System.Windows;
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
                // Title ကို "Datacard ID Works Enterprise Designer - ProjectName - [Card]" သို့ ပြောင်းလဲခြင်း
                this.Title = $"Datacard ID Works Enterprise Designer - {dialog.ProjectName} - [Card]";

                // Card Design (Front/Back)
                PnlCardContainer.Visibility = dialog.HasCardDesign ? Visibility.Visible : Visibility.Collapsed;

                // Production Form Design
                WinProductionForm.Visibility = dialog.HasProductionForm ? Visibility.Visible : Visibility.Collapsed;

                // Report Design
                WinReport.Visibility = dialog.HasReportDesign ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
