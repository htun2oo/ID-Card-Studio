using System.Windows;

namespace IDWorksEnterpriseDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Initial Status Message သတ်မှတ်ခြင်း
            SetStatusMessage("For Help, press F1");
        }

        /// <summary>
        /// Status Bar Text ကို ပြောင်းလဲရန် Helper Method
        /// </summary>
        public void SetStatusMessage(string message)
        {
            if (StatusText != null)
            {
                // TextBlock ဖြစ်သောကြောင့် Content အစား Text Property ကို သုံးထားပါသည်
                StatusText.Text = message;
            }
        }
    }
}
