// <copyright file="TrayIcon.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.Tray
{
    public partial class TrayIcon : IDisposable
    {
        private List<TrayMenuItem> menuItems = new List<TrayMenuItem>();
        private bool disposedValue;

        /// <summary>
        /// Left Clicked Event.
        /// </summary>
        public event EventHandler<TrayClickedEventArgs>? LeftClicked;

        /// <summary>
        /// Right Clicked Event.
        /// </summary>
        public event EventHandler<TrayClickedEventArgs>? RightClicked;

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

#if !MACCATALYST && !WINDOWS && !MACOS
        /// <summary>
        /// Dispose Native Elements.
        /// </summary>
        public void NativeElementDispose()
        {
        }
#endif
    }
}