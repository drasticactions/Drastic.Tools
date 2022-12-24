using Drastic.PureLayout;

namespace Drastic.DragAndDrop.Sample.MacCatalyst;

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

        // create a UIViewController with a single UILabel
        Window.RootViewController = new SampleViewController();

        // make the window visible
        Window.MakeKeyAndVisible();

        return true;
    }

    internal class SampleViewController : UIViewController
    {
        private UILabel label = new UILabel() { Text = "Drag and Drop an image onto the window..." };
        private UIImageView image = new UIImageView() { BackgroundColor = UIColor.Gray };
        private DragAndDrop dragAnDrop;

        public SampleViewController()
        {
            this.SetupUI();
            this.SetupLayout();

            this.dragAnDrop = new DragAndDrop(this);
            this.dragAnDrop.Drop += this.DragAnDrop_Drop;
        }

        private void DragAnDrop_Drop(object? sender, DragAndDropOverlayTappedEventArgs e)
        {
            try
            {
                if (e.Paths.Any())
                {
                    var path = e.Paths[0];
                    this.image.Image = UIImage.FromFile(path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public void SetupUI()
        {
            this.View!.AddSubview(this.label);
            this.View!.AddSubview(this.image);
        }

        public void SetupLayout()
        {
            this.image.AutoCenterInSuperview();
            this.image.AutoSetDimensionsToSize(new CGSize(300, 300));
            this.label.AutoPinEdgesToSuperviewEdgesExcludingEdge(UIEdgeInsets.Zero, ALEdge.Top);
            this.label.AutoPinEdge(ALEdge.Top, ALEdge.Bottom, this.image);
            this.label.TextAlignment = UITextAlignment.Center;
        }
    }
}