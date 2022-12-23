using Drastic.TrayWindow.Maui;
using Foundation;

namespace Drastic.TrayWindow.Sample.Maui;

[Register("AppDelegate")]
public class AppDelegate : MauiTrayUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
