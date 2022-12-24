// <copyright file="DragAndDrop.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Drastic.DragAndDrop
{
    public partial class DragAndDrop : IDisposable, INotifyPropertyChanged
    {
        private bool disposedValue;
        private bool isDragging;

        /// <summary>
        /// Fired when files are dropped on the overlay.
        /// </summary>
        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets a value indicating whether the user is dragging onto the view.
        /// </summary>
        public bool IsDragging
        {
            get { return this.isDragging; }
            set { this.SetProperty(ref this.isDragging, value); }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.DisposeNativeElements();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
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

#if !WINDOWS && !MACCATALYST && !IOS
        internal void DisposeNativeElements()
        {

        }
#endif
    }
}
