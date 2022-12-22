// <copyright file="SampleViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.PureLayout;
using Drastic.Tray;

namespace Drastic.TrayWindow.Sample.MacCatalyst;

/// <summary>
/// Sample View Controller.
/// </summary>
public class SampleViewController : UIViewController
{
    private UILabel label = new UILabel();

    /// <summary>
    /// Initializes a new instance of the <see cref="SampleViewController"/> class.
    /// </summary>
    /// <param name="title">Sets the text on the label.</param>
    public SampleViewController(string title)
    {
        this.label = new UILabel()
        {
            BackgroundColor = UIColor.Clear,
            TextAlignment = UITextAlignment.Center,
            Text = title,
            AutoresizingMask = UIViewAutoresizing.All,
        };

        this.View!.AddSubview(this.label);
        this.label.AutoCenterInSuperview();
    }
}
