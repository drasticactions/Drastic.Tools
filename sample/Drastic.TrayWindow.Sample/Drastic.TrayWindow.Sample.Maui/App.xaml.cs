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

        var menuItems = new List<TrayMenuItem>();
        var image = UIImage.GetSystemImage("trophy.circle");
        var trayImage = new TrayImage(image!);
        menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
        menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
        menuItems.Add(new TrayMenuItem("Mac Catalyst!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
        Icon = new Tray.TrayIcon("Drastic.Sample", trayImage, menuItems);
        this.Icon.RightClicked += (object? sender, EventArgs e) => { this.Icon.OpenMenu(); };
        Icon.LeftClicked += (object? sender, EventArgs e) => { this.window?.ToggleVisibilityAsync(); };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return window = new SampleTrayWindow(this.Icon!) { Page = new AppShell() };
    }
}
