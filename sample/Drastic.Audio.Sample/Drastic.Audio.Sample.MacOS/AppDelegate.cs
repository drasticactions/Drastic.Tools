using Drastic.Audio.Model;
using Drastic.Audio.Services;
using Drastic.PureLayout;

namespace Drastic.Audio.Sample.MacOS;

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
    private NativeMediaService service;
    private NSButton playButton = new NSButton() { BezelStyle = NSBezelStyle.Rounded, Title = "Play" };
    private NSButton pauseButton = new NSButton() { BezelStyle = NSBezelStyle.Rounded, Title = "Pause" };

    public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation)
        : base(contentRect, aStyle, bufferingType, deferCreation)
    {
        var media = new SampleMedia() { OnlinePath = new Uri("https://cdn.discordapp.com/attachments/1047080059736952862/1057497416154226768/Upstate_-_TrackTribe.mp3") };
        this.service = new NativeMediaService();
        this.service.CurrentMedia = media;
        this.Title = "Drastic.Audio.Sample";
        this.pauseButton.Activated += PauseButton_PrimaryActionTriggered;
        this.playButton.Activated += PlayButton_PrimaryActionTriggered;
#pragma warning disable CA1416 // Validate platform compatibility
        // this.image.Layer = new CoreAnimation.CALayer() { BackgroundColor = CGColor.CreateSrgb(225, 0, 0, 1) };
#pragma warning restore CA1416 // Validate platform compatibility

        // Create the content view for the window and make it fill the window
        this.ContentView = new NSView(this.Frame);

        this.ContentView.AddSubview(this.playButton);
        this.ContentView.AddSubview(this.pauseButton);

        this.playButton.AutoCenterInSuperview();

        // this.pauseButton.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
        this.pauseButton.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.playButton, 15f);
        this.pauseButton.AutoAlignAxis(ALAxis.Vertical, this.playButton);
    }

    private void PauseButton_PrimaryActionTriggered(object? sender, EventArgs e)
    {
        this.service.PauseAsync();
    }

    private void PlayButton_PrimaryActionTriggered(object? sender, EventArgs e)
    {
        this.service.PlayAsync();
    }

    public class SampleMedia : IMediaItem
    {
        public string? LocalPath { get; set; }
        public Uri? OnlinePath { get; set; }
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
