using Foundation;
using Microsoft.Extensions.Logging.Debug;
using UIKit;

namespace Drastic.UITools.Sample;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        var logger = new DebugLoggerProvider();
       // Drastic.UITools.FPSViewer.Start(logger.CreateLogger("Drastic.UITools.Sample"));
        return base.FinishedLaunching(application, launchOptions);
    }
}