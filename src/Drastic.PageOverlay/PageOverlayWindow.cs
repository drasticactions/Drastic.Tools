// <copyright file="PageOverlayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.Overlay;

namespace Drastic.PageOverlay
{
    /// <summary>
    /// Page Overlay Window.
    /// </summary>
    public class PageOverlayWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageOverlayWindow"/> class.
        /// </summary>
        public PageOverlayWindow()
        {
            this.PageOverlay = new PageOverlay(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageOverlayWindow"/> class.
        /// </summary>
        /// <param name="page">Page to render on load.</param>
        public PageOverlayWindow(Page page)
            : base(page)
        {
            this.PageOverlay = new PageOverlay(this);
        }

        /// <summary>
        /// Gets the page overlay.
        /// </summary>
        public PageOverlay PageOverlay { get; }

        /// <inheritdoc/>
        protected override void OnCreated()
        {
            this.AddOverlay(this.PageOverlay);
        }
    }
}