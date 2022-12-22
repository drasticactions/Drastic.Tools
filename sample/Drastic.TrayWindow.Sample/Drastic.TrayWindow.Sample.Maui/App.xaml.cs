using Drastic.Tray;
using UIKit;

namespace Drastic.TrayWindow.Sample.Maui;

public partial class App : Application
{
    private Tray.TrayIcon? Icon;
    private SampleTrayWindow? window;

    public App()
    {
        InitializeComponent();

        var image = UIImage.GetSystemImage("trophy.circle");
        var trayImage = new TrayImage(image!);
        Icon = new Tray.TrayIcon("Drastic.Sample", trayImage);
        Icon.LeftClicked += Icon_LeftClicked;
    }

    private async void Icon_LeftClicked(object? sender, EventArgs e)
    {
        await this.window?.ToggleVisibilityAsync()!;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return window = new SampleTrayWindow(this.Icon!) { Page = new AppShell() };
    }
}
