private void NewProject_Click(object sender, RoutedEventArgs e)
{
    Views.NewProjectWindow newProjectWin = new Views.NewProjectWindow
    {
        Owner = this
    };

    if (newProjectWin.ShowDialog() == true)
    {
        // 1. Workspace ကို ပေါ်လာစေခြင်း
        if (ProjectWorkspace != null)
        {
            ProjectWorkspace.Visibility = Visibility.Visible;
        }

        // 2. ပရောဂျက်ဖွင့်ပြီးနောက် မီနူးအပြည့်အစုံ ပေါ်လာစေရန် Visibility ကို ဖွင့်ပေးခြင်း
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

        // 3. Ruler များ ဆွဲခြင်း
        DrawCardRulers(FrontTopRulerCanvas, FrontLeftRulerCanvas, 15.0, 10.0);
        DrawCardRulers(BackTopRulerCanvas, BackLeftRulerCanvas, 15.0, 10.0);
        DrawReportRulers(21.0, 29.7);
    }
}
