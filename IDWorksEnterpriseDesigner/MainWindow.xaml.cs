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

        // MainWindow.xaml ရှိ Click="NewProject_Click" အတွက် Event Handler
        private void NewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow dialog = new NewProjectWindow();
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                // Window Title ကို ပြောင်းလဲခြင်း
                this.Title = $"Datacard ID Works Enterprise Designer - {dialog.ProjectName} - [Card]";

                // New Project ရွေးချယ်မှုများအတိုင်း MDI Windows များကို ပြသရန်
                // (လိုအပ်ပါက Component များ ဖွင့်သည့် Logic ကို ဤနေရာတွင် ထည့်သွင်းနိုင်ပါသည်)
            }
        }
    }
}
