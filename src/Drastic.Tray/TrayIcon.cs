// <copyright file="TrayIcon.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.Tray
{
	public partial class TrayIcon : IDisposable
    {
        private string? iconName;
        private List<TrayMenuItem> menuItems;
        private bool holdsWindowInTray;
        private bool disposedValue;

        /// <summary>
        /// Left Clicked Event.
        /// </summary>
        public event EventHandler<EventArgs>? LeftClicked;

        /// <summary>
        /// Right Clicked Event.
        /// </summary>
        public event EventHandler<EventArgs>? RightClicked;

        /// <summary>
        /// Menu Item Clicked.
        /// </summary>
        public event EventHandler<MenuClickedEventArgs>? MenuClicked;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.NativeElementDispose();
                }

                this.disposedValue = true;
            }
        }
    }
}