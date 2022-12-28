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
        private UIProgressView progressView = new UIProgressView(UIProgressViewStyle.Default);
        private UIView progressHolderView = new UIView() { };
        private UILabel progressTime = new UILabel() { Text = "00:00:00" };
        private UILabel totalTime = new UILabel() { Text = "00:00:00" };

        public MainUIViewController()
        {
            var media = new SampleMedia() { OnlinePath = new Uri("https://cdn.discordapp.com/attachments/1047080059736952862/1057497416154226768/Upstate_-_TrackTribe.mp3") };
            this.service = new NativeMediaService();
            this.service.CurrentMedia = media;
            this.service.PositionChanged += Service_PositionChanged;
            this.SetupUI();
            this.SetupLayout();
            this.playButton.PrimaryActionTriggered += PlayButton_PrimaryActionTriggered;
            this.pauseButton.PrimaryActionTriggered += PauseButton_PrimaryActionTriggered;

        }

        private void Service_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            this.totalTime.Text = e.Duration?.ToString("g") ?? string.Empty;
            this.progressTime.Text = e.Position.ToString("g");
            this.progressView.Progress = (float)e.PrecentCompleted;
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
            this.View!.AddSubview(this.progressHolderView);
            this.progressHolderView.AddSubview(this.progressTime);
            this.progressHolderView.AddSubview(this.totalTime);
            this.progressHolderView.AddSubview(this.progressView);
        }

        private void SetupLayout()
        {
            this.playButton.AutoCenterInSuperview();

           // this.pauseButton.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            this.pauseButton.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.playButton, 15f);
            //this.pauseButton.AutoPinEdge(ALEdge.Bottom, ALEdge.Top, this.progressHolderView, 15f);

            this.pauseButton.AutoAlignAxis(ALAxis.Vertical, this.playButton);
            this.progressHolderView.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            //this.progressHolderView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.pauseButton);

            this.progressTime.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Right);
            this.totalTime.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Left);
            this.progressView.AutoPinEdge(ALEdge.Left, ALEdge.Right, this.progressTime);
            this.progressView.AutoPinEdge(ALEdge.Right, ALEdge.Left, this.totalTime);
            this.progressView.AutoAlignAxisToSuperviewAxis(ALAxis.Vertical);
            this.progressView.AutoAlignAxisToSuperviewAxis(ALAxis.Horizontal);
            //this.progressView.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.pauseButton, 20f);
            //this.progressView.AutoAlignAxis(ALAxis.Vertical, this.playButton);
            //this.progressView.AutoPinEdge(ALEdge.Left, ALEdge.Left, this.View!, 25f);
            //this.progressView.AutoPinEdge(ALEdge.Right, ALEdge.Right, this.View!, 25f);
        }

        public class SampleMedia : IMediaItem
        {
            public string? LocalPath { get; set; }

            public Uri? OnlinePath { get; set; }
        }
    }
}