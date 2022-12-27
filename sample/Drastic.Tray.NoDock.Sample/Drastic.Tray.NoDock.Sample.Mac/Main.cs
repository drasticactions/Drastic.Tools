using Drastic.Tray.NoDock.Sample.Mac;

// This is the main entry point of the application.
NSApplication.Init ();

var application = NSApplication.SharedApplication!;
application.ActivationPolicy = NSApplicationActivationPolicy.Accessory;
application.Delegate = new AppDelegate();

NSApplication.Main (args);
