using AppKit;
using ObjCRuntime;
using ScreenCaptureKit;
using System.Reflection.Emit;

namespace Drastic.RainbowsSample.MacOS;

[Register ("AppDelegate")]
public class AppDelegate : NSApplicationDelegate {

    private MainWindowController? mainWindowController;

    public override void DidFinishLaunching (NSNotification notification)
	{
        Drastic.Rainbows.SwizzleCommands.Start();

        this.mainWindowController = new MainWindowController();
        this.mainWindowController.Window.MakeKeyAndOrderFront(this);
    }

	public override void WillTerminate (NSNotification notification)
	{
		// Insert code here to tear down your application
	}
}

public class MainWindow : NSWindow
{
    private NSViewController? controller;

    public MainWindow()
    {
    }

    public MainWindow(NSCoder coder) : base(coder)
    {
    }

    public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) : base(contentRect, aStyle, bufferingType, deferCreation)
    {
        this.controller = new TestViewController();
        this.ContentViewController = this.controller;
    }

    public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation, NSScreen screen) : base(contentRect, aStyle, bufferingType, deferCreation, screen)
    {
    }

    protected MainWindow(NSObjectFlag t) : base(t)
    {
    }

    protected internal MainWindow(NativeHandle handle) : base(handle)
    {
    }
}

public class TestViewController : NSViewController
{
    public TestViewController()
    {
        this.View = new NSView(new CGRect(0, 0, 1000, 500));
    }

    public override void AwakeFromNib()
    {
        base.AwakeFromNib();
    }
}

internal class TestView : NSView
{
    public TestView()
    {
    }

    public TestView(NSCoder coder) : base(coder)
    {
    }

    public TestView(CGRect frameRect) : base(frameRect)
    {
    }

    protected TestView(NSObjectFlag t) : base(t)
    {
    }

    protected internal TestView(NativeHandle handle) : base(handle)
    {
    }

    public override void ViewDidMoveToSuperview()
    {
        base.ViewDidMoveToSuperview();
    }
}

public class MainWindowController : NSWindowController
{
    public MainWindowController() : base()
    {
        // Construct the window from code here
        CGRect contentRect = new CGRect(0, 0, 1000, 500);
        base.Window = new MainWindow(contentRect, (NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable), NSBackingStore.Buffered, false);

        // Simulate Awaking from Nib
        Window.AwakeFromNib();
    }

    public new MainWindow Window
    {
        get { return (MainWindow)base.Window; }
    }
}
