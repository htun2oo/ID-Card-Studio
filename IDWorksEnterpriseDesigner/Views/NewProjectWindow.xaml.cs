using System.Windows;

namespace IDWorksEnterpriseDesigner.Views
{
    public partial class NewProjectWindow : Window
    {
        public string ProjectName { get; set; } = string.Empty;

        public NewProjectWindow()
        {
            InitializeComponent();
        }
    }
}
