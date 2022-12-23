namespace Drastic.TrayWindow.Sample.Maui;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        var image = App.GetResourceFileContent("TrayIcon.ico");

        Drastic.TrayWindow.Maui.MauiTrayWindow.Generate("Test", image!, new TrayWindowOptions(), new SamplePage());
    }
}