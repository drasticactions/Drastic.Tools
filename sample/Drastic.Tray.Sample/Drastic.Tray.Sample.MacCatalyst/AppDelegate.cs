using AppKit;
using Drastic.PureLayout;

namespace Drastic.Tray.Sample.MacCatalyst;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        // create a new window instance based on the screen size
        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        Window.RootViewController = new SampleViewController(Window!);

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;
    }
}

public class SampleViewController : UIViewController
{
    public UIButton trayButton = new UIButton(UIButtonType.RoundedRect);
    private UIWindow window;

    public SampleViewController(UIWindow window)
    {
        this.window = window;
        this.SetupUI();
        this.SetupLayout();
    }

    private void SetupUI()
    {
        this.View!.AddSubview(this.trayButton);
        this.trayButton.SetTitle("Add Tray Icon", UIControlState.Normal);
        this.trayButton.TouchUpInside += TrayButton_TouchUpInside;
    }

    private TrayIcon? trayIcon;

    private async void TrayButton_TouchUpInside(object? sender, EventArgs e)
    {
        var menuItems = new List<TrayMenuItem>();
        var image = UIImage.GetSystemImage("trophy.circle");
        var trayImage = new TrayImage(image!);
        menuItems.Add(new TrayMenuItem("Hello!", trayImage, async () => { }, "h"));
        menuItems.Add(new TrayMenuItem("From!", trayImage, async () => { }, "f"));
        menuItems.Add(new TrayMenuItem("Mac Catalyst!", trayImage, async () => { }, "m", NSEventModifierMask.ControlKeyMask | NSEventModifierMask.CommandKeyMask));
        this.trayIcon = new Drastic.Tray.TrayIcon("Tray Sample", trayImage, menuItems);
        trayIcon.RightClicked += (object? sender, EventArgs e) => { trayIcon.OpenMenu(); };
        trayIcon.LeftClicked += (object? sender, EventArgs e) => {
            var okAlertController = UIAlertController.Create("Drastic.Tray.Sample", "Welcome!", UIAlertControllerStyle.Alert);
            okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
            PresentViewController(okAlertController, true, null);

        };
    }

    private void SetupLayout()
    {
        this.trayButton.AutoCenterInSuperview();
    }
}
