// <copyright file="PageOverlaySample.xaml.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Overlay;

namespace Drastic.PageOverlay.Sample;

public partial class PageOverlaySample : ContentPage, IHitTestView
{
    private PageOverlayWindow window;

    public PageOverlaySample(PageOverlayWindow overlayWindow)
    {
        this.InitializeComponent();
        this.window = overlayWindow;
        this.HitTestViews.Add(this.ControlLayout);
    }

    /// <summary>
    /// Gets the hit test views.
    /// </summary>
    public List<IView> HitTestViews { get; } = new List<IView>();

    private void OnPageOverlay(object sender, EventArgs e)
    {
        this.window.PageOverlay.RemoveView(this);
    }
}
