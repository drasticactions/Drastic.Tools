using System.Reflection.Emit;
using AppKit;
using Drastic.PureLayout;

namespace Drastic.DragAndDrop.Sample.Mac;

[Register("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    private MainWindowController? mainWindowController;

    /// <inheritdoc/>
    public override void DidFinishLaunching(NSNotification notification)
    {
        this.mainWindowController = new MainWindowController();
        this.mainWindowController.Window.MakeKeyAndOrderFront(this);
    }

    /// <inheritdoc/>
    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }
}

public class MainWindow : NSWindow
{
    private NSTextField label = new NSTextField() { StringValue = "Drag and Drop an image onto the window...", Editable = false, Bezeled = false, BackgroundColor = NSColor.FromRgba(0, 0, 0, 0) };
    private NSImageView image = new NSImageView() { };
    private DragAndDrop dragAndDrop;

    public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation)
        : base(contentRect, aStyle, bufferingType, deferCreation)
    {
        this.Title = "Drastic.DragAndDrop.Sample";

#pragma warning disable CA1416 // Validate platform compatibility
       // this.image.Layer = new CoreAnimation.CALayer() { BackgroundColor = CGColor.CreateSrgb(225, 0, 0, 1) };
#pragma warning restore CA1416 // Validate platform compatibility

        // Create the content view for the window and make it fill the window
        this.ContentView = new NSView(this.Frame);

        this.ContentView.AddSubview(this.label);
        this.ContentView.AddSubview(this.image);

        this.dragAndDrop = new DragAndDrop(this.ContentView);
        this.dragAndDrop.Drop += this.DragAndDrop_Drop;

        this.image.AutoCenterInSuperview();
        this.image.AutoSetDimensionsToSize(new CGSize(300, 300));
        this.label.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.ContentView);
        this.label.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.ContentView);
        this.label.AutoPinEdge(ALEdge.Bottom, ALEdge.Bottom, this.ContentView, -25f);
        this.label.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.image, 25f);
        this.label.Alignment = NSTextAlignment.Center;
    }

    private void DragAndDrop_Drop(object? sender, DragAndDropOverlayTappedEventArgs e)
    {
        try
        {
            if (e.Paths.Any())
            {
                var path = e.Paths[0];
                this.image.Image = NSImage.FromStream(File.OpenRead(path));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
        }
    }
}

public class MainWindowController : NSWindowController
{
    public MainWindowController()
        : base()
    {
        // Construct the window from code here
        CGRect contentRect = new CGRect(0, 0, 1000, 500);
        base.Window = new MainWindow(contentRect, NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable, NSBackingStore.Buffered, false);

        // Simulate Awaking from Nib
        this.Window.AwakeFromNib();
    }

    public new MainWindow Window
    {
        get { return (MainWindow)base.Window; }
    }
}