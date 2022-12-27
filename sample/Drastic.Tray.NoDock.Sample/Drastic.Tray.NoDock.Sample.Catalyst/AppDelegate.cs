using System.Reflection;
using Drastic.PureLayout;
using Drastic.TrayWindow;

namespace Drastic.Tray.NoDock.Sample.Catalyst;

[Register("AppDelegate")]
public class AppDelegate : TrayAppDelegate
{
    public override UIWindow? Window
    {
        get;
        set;
    }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        NSApplication.SetActivationPolicy(NSApplicationActivationPolicy.Accessory);

        var menuItems = new List<TrayMenuItem>();
        var trayImage = new TrayImage(GetResourceFileContent("favicon.png")!);
        menuItems.Add(new TrayMenuItem("Quit!", trayImage, async () => { Drastic.TrayWindow.NSApplication.Terminate(); }, "q"));

        var icon = new Tray.TrayIcon("Tray Window Sample", trayImage, menuItems, false);
        icon.RightClicked += (object? sender, TrayClickedEventArgs e) => icon.OpenMenu();
        this.CreateTrayWindow(icon, new TrayWindowOptions(), new SampleViewController("Welcome to the tray!"));

        return true;
    }

    /// <summary>
    /// Get Resource File Content via FileName.
    /// </summary>
    /// <param name="fileName">Filename.</param>
    /// <returns>Stream.</returns>
    public static Stream? GetResourceFileContent(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Drastic.Tray.NoDock.Sample.Catalyst." + fileName;
        if (assembly is null)
        {
            return null;
        }

        return assembly.GetManifestResourceStream(resourceName);
    }
}

/// <summary>
/// Sample View Controller.
/// </summary>
public class SampleViewController : UIViewController
{
    private UILabel label = new UILabel();

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleViewController"/> class.
    /// </summary>
    /// <param name="title">Sets the text on the label.</param>
    public SampleViewController(string title)
    {
        this.label = new UILabel()
        {
            BackgroundColor = UIColor.Clear,
            TextAlignment = UITextAlignment.Center,
            Text = title,
            AutoresizingMask = UIViewAutoresizing.All,
        };

        this.View!.AddSubview(this.label);
        this.label.AutoCenterInSuperview();
    }
}

