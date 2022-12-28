using Drastic.Audio.Model;
using Drastic.Audio.Services;
using Drastic.PureLayout;

namespace Drastic.Audio.Sample.tvOS;

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
        // Override point for customization after application launch.
        // If not required for your application you can safely delete this method
        this.Window = new MainWindow();
        this.Window.MakeKeyAndVisible();
        return true;
    }
}

public class MainWindow : UIWindow
{
    public MainWindow()
    {
        this.RootViewController = new MainUIViewController();
    }

    private class MainUIViewController : UIViewController
    {
        private NativeMediaService service;
        private UIButton playButton = new UIButton(UIButtonType.RoundedRect) { Enabled = true };
        private UIButton pauseButton = new UIButton(UIButtonType.RoundedRect) { Enabled = true };

        public MainUIViewController()
        {
            var media = new SampleMedia() { OnlinePath = new Uri("https://cdn.discordapp.com/attachments/1047080059736952862/1057497416154226768/Upstate_-_TrackTribe.mp3") };
            this.service = new NativeMediaService();
            this.service.CurrentMedia = media;
            this.SetupUI();
            this.SetupLayout();
            this.playButton.PrimaryActionTriggered += PlayButton_PrimaryActionTriggered;
            this.pauseButton.PrimaryActionTriggered += PauseButton_PrimaryActionTriggered;

        }

        private void PauseButton_PrimaryActionTriggered(object? sender, EventArgs e)
        {
            this.service.PauseAsync();
        }

        private void PlayButton_PrimaryActionTriggered(object? sender, EventArgs e)
        {
            this.service.PlayAsync();
        }

        private void SetupUI()
        {
            this.playButton.SetTitle("Play", UIControlState.Normal);
            this.pauseButton.SetTitle("Pause", UIControlState.Normal);
            this.View!.AddSubview(this.playButton);
            this.View!.AddSubview(this.pauseButton);
        }

        private void SetupLayout()
        {
            this.playButton.AutoCenterInSuperview();

           // this.pauseButton.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            this.pauseButton.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.playButton, 15f);
            this.pauseButton.AutoAlignAxis(ALAxis.Vertical, this.playButton);
        }

        public class SampleMedia : IMediaItem
        {
            public string? LocalPath { get; set; }
            public Uri? OnlinePath { get; set; }
        }
    }
}