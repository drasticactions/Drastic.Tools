// <copyright file="FPSViewerViewController.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using CFTimeInterval = System.Double;

// Ported from https://github.com/tapwork/WatchdogInspector/tree/master
namespace Drastic.UITools;

internal class FPSViewerViewController : UIViewController
{
    private const double KBestFrameRate = 60.0;
    private const float KBarViewWidth = 10.0f;
    private const float KBarViewPaddingX = 8.0f;
    private const double KBarViewAnimationDuration = 2.0;
    private const float KLabelWidth = 150.0f;
    private double lastUpdateFPSTIme = 0.0;

    private UILabel? fpsLabel;
    private UILabel? timeLabel;
    private HashSet<UIView> barViews = new HashSet<UIView>();

    /// <inheritdoc/>
    public override void LoadView()
    {
        base.LoadView();
        this.View!.BackgroundColor = UIColor.LightGray;

        this.fpsLabel = new UILabel
        {
            BackgroundColor = UIColor.Clear,
            Font = UIFont.BoldSystemFontOfSize(14),
        };
        this.View.AddSubview(this.fpsLabel);

        this.timeLabel = new UILabel
        {
            BackgroundColor = UIColor.Clear,
            Font = UIFont.BoldSystemFontOfSize(14),
        };
        this.View.AddSubview(this.timeLabel);
    }

    /// <inheritdoc/>
    public override void ViewWillLayoutSubviews()
    {
        base.ViewWillLayoutSubviews();

        if (this.fpsLabel is not null)
        {
            this.fpsLabel.Frame = new CGRect(15, 15, KLabelWidth, this.View!.Bounds.Size.Height);
        }

        if (this.fpsLabel is not null && this.timeLabel is not null)
        {
            this.timeLabel.Frame = new CGRect(this.fpsLabel.Frame.Right, 0, KLabelWidth, this.View!.Bounds.Size.Height);
        }
    }

    /// <summary>
    /// Update the FPS on the screen.
    /// </summary>
    /// <param name="fps"></param>
    public void UpdateFPS(double fps)
    {
        if (this.fpsLabel is not null)
        {
            if (fps > 0)
            {
                this.fpsLabel.Text = $"fps: {fps:F2}";
            }
            else
            {
                this.fpsLabel.Text = null;
            }
        }

        this.UpdateColorWithFPS(fps);
        this.AddBarWithFPS(fps);
        this.lastUpdateFPSTIme = NSDate.Now.SecondsSinceReferenceDate;
    }

    public void UpdateStallingTime(double stallingTime)
    {
        if (this.timeLabel is null)
        {
            return;
        }

        this.timeLabel.Text = stallingTime > 0 ? $"Stalling: {stallingTime:F2} Sec" : null;
    }

    private void UpdateColorWithFPS(double fps)
    {
        // Fade from green to red
        var n = 1 - (fps / KBestFrameRate);
        var red = 255 * n;
        var green = 255 * (1 - n) / 2;
        var blue = 0;
        UIColor color = UIColor.FromRGBA((nfloat)(red / 255.0f), (nfloat)(green / 255.0f), blue / 255.0f, 1.0f);
        if (fps == 0.0)
        {
            color = UIColor.LightGray;
        }

        UIView.Animate(0.2, () => { this.View!.Layer.BackgroundColor = color.CGColor; });
    }

    private void AddBarWithFPS(double fps)
    {
        double duration = KBarViewAnimationDuration;
        if (this.lastUpdateFPSTIme > 0)
        {
            duration = NSDate.Now.SecondsSinceReferenceDate - this.lastUpdateFPSTIme;
        }

        nfloat xPos = this.View!.Bounds.Size.Width;
        nfloat height = this.View.Bounds.Size.Height * (nfloat)(fps / KBestFrameRate);
        nfloat yPos = this.View.Bounds.Size.Height - height;
        UIView barView = new UIView(new CGRect(xPos, yPos, KBarViewWidth, height))
        {
            BackgroundColor = UIColor.FromWhiteAlpha(1.0f, 0.2f),
        };
        this.View.AddSubview(barView);
        this.barViews.Add(barView);

        if (this.fpsLabel is not null)
        {
            this.View.BringSubviewToFront(this.fpsLabel);
        }

        if (this.timeLabel is not null)
        {
            this.View.BringSubviewToFront(this.timeLabel);
        }

        foreach (UIView view in this.barViews)
        {
            view.Layer.RemoveAllAnimations();
            CGRect rect = view.Frame;
            rect.X = rect.X - rect.Width - KBarViewPaddingX;
            UIView.Animate(duration, () => { view.Frame = rect; }, () => { this.RemoveBarViewIfNeeded(view); });
        }
    }

    private void RemoveBarViewIfNeeded(UIView barView)
    {
        if (barView.Frame.GetMaxX() <= -KBarViewPaddingX)
        {
            barView.RemoveFromSuperview();
            this.barViews.Remove(barView);
        }
    }
}