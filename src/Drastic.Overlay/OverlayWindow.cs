// <copyright file="OverlayWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Overlay
{
    /// <summary>
    /// Overlay Window.
    /// Includes an event to add overlays to the given window after the Window Handler has been created or changed.
    /// </summary>
    public class OverlayWindow : Window
    {
        public virtual Task AddOverlaysAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            this.AddOverlaysAsync();
        }
    }
}
