// <copyright file="DragAndDrop.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Drastic.DragAndDrop
{
    /// <summary>
    /// Drag And Drop View.
    /// </summary>
    public partial class DragAndDrop : IDisposable, INotifyPropertyChanged
    {
        private bool disposedValue;

        /// <summary>
        /// Fired when files are dropped on the overlay.
        /// </summary>
        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        /// <summary>
        /// Fired when a user has started or stopped dragging to a surface.
        /// </summary>
        public event EventHandler<DragAndDropIsDraggingEventArgs>? Dragging;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called on Dispose.
        /// </summary>
        /// <param name="disposing">Is Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.DisposePlatformElements();
                }

                this.disposedValue = true;
            }
        }

#pragma warning disable SA1600 // Elements should be documented
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
#pragma warning restore SA1600 // Elements should be documented
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// On Property Changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = this.PropertyChanged;
            if (changed == null)
            {
                return;
            }

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

#if !WINDOWS && !MACCATALYST && !IOS && !ANDROID
        /// <summary>
        /// Dispose Elements.
        /// </summary>
        internal void DisposePlatformElements()
        {

        }
#endif
    }
}
