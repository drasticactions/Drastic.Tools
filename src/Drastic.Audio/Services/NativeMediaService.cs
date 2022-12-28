// <copyright file="NativeMediaService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Audio.Services
{
    /// <summary>
    /// Native Media Service.
    /// </summary>
    public partial class NativeMediaService : IMediaService
    {
        private System.Threading.Timer? positionTimer;

        private Model.IMediaItem? media;

        /// <inheritdoc/>
        public event EventHandler<MediaPlayerPositionChangedEventArgs>? PositionChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? EndCurrentItemReached;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? RaiseCanExecuteChanged;

        /// <inheritdoc/>
        public event EventHandler<EventArgs>? MediaChanged;

        /// <inheritdoc/>
        public Model.IMediaItem? CurrentMedia { get { return this.media; } set { this.media = value; this.SetCurrentMedia(); } }

        private void SetCurrentMedia()
        {
            var path = this.CurrentMedia?.LocalPath ?? this.CurrentMedia?.OnlinePath?.ToString();

            if (path == null)
            {
                throw new NullReferenceException(nameof(this.CurrentMedia));
            }

            this.SetCurrentMediaNative();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
        }

#if !ANDROID && !IOS && !MACCATALYST && !WINDOWS && !TVOS && !MACOS

        /// <inheritdoc/>
        public TimeSpan? Duration => throw new NotImplementedException();

        internal void SetCurrentMediaNative()
        {
        }

        /// <inheritdoc/>
        public Task PauseAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task ResumeAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SkipAhead(double amount = 0)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task SkipBack(double amount = 0)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string> GetArtworkUrl()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsPlaying => throw new NotImplementedException();

        /// <inheritdoc/>
        public float CurrentPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#endif
    }
}
