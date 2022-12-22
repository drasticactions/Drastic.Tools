using Drastic.PureLayout;
using Drastic.Tray;

namespace Drastic.TrayWindow.Sample.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    public static Drastic.Tray.TrayIcon? Icon;
    public static Drastic.TrayWindow.UITrayWindow? TrayWindow;

    public AppDelegate()
    {
        var image = UIImage.GetSystemImage("trophy.circle");
        var trayImage = new TrayImage(image!);
        Icon = new Tray.TrayIcon("Drastic.Sample", trayImage);
        Icon.LeftClicked += Icon_LeftClicked;
    }

    private async void Icon_LeftClicked(object? sender, EventArgs e)
    {
        await TrayWindow?.ToggleVisibilityAsync()!;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        return true;
    }
}

public class SampleViewController : UIViewController
{
    public UILabel label = new UILabel();

    public SampleViewController()
    {
        this.label = new UILabel()
        {
            BackgroundColor = UIColor.Clear,
            TextAlignment = UITextAlignment.Center,
            Text = "Hello, Mac Catalyst Tray Window!",
            AutoresizingMask = UIViewAutoresizing.All,
        };

        this.View!.AddSubview(label);
        this.label.AutoCenterInSuperview();
    }
}
