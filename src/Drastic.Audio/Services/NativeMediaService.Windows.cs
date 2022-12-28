// <copyright file="NativeMediaService.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Drastic.Audio.Services
{
    /// <summary>
    /// Native Media Service.
    /// </summary>
    public partial class NativeMediaService
    {
        private MediaPlayer mediaPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeMediaService"/> class.
        /// </summary>
        public NativeMediaService()
        {
            this.mediaPlayer = new MediaPlayer() { AudioCategory = MediaPlayerAudioCategory.Media };
            this.mediaPlayer.MediaEnded += this.MediaPlayer_MediaEnded;
            this.mediaPlayer.MediaOpened += this.MediaPlayer_MediaOpened;
            this.mediaPlayer.CurrentStateChanged += this.MediaPlayer_CurrentStateChanged;
        }

        /// <inheritdoc/>
        public float CurrentPosition
        {
            get
            {
                if (this.mediaPlayer == null || this.mediaPlayer.NaturalDuration.TotalSeconds == 0)
                {
                    return 0;
                }

                return (float)(this.mediaPlayer.Position.TotalSeconds / this.mediaPlayer.NaturalDuration.TotalSeconds);
            }

            set
            {
                if (this.mediaPlayer != null)
                {
                    this.mediaPlayer.Position = TimeSpan.FromSeconds(this.mediaPlayer.NaturalDuration.TotalSeconds * value);
                }
            }
        }

        public TimeSpan? Duration => null;

        /// <inheritdoc/>
        public bool IsPlaying => this.mediaPlayer?.CurrentState == MediaPlayerState.Playing;

        /// <inheritdoc/>
        public Task PauseAsync()
        {
            this.mediaPlayer?.Pause();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
            this.mediaPlayer?.Play();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ResumeAsync()
        {
            this.mediaPlayer?.Play();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SkipAhead(double amount = 0)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task SkipBack(double amount = 0)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            if (this.mediaPlayer != null)
            {
                this.mediaPlayer.Pause();
                this.mediaPlayer.Position = TimeSpan.MinValue;
            }

            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<string> GetArtworkUrl() => this.GetMetadata();

        /// <summary>
        /// Set current media native.
        /// </summary>
        internal void SetCurrentMediaNative()
        {
            if (this.CurrentMedia == null)
            {
                return;
            }

            MediaSource source = this.CurrentMedia.LocalPath != null ? MediaSource.CreateFromUri(new Uri(this.CurrentMedia.LocalPath)) : MediaSource.CreateFromUri(this.CurrentMedia.OnlinePath);
            this.mediaPlayer.Source = source;
        }

        private async Task<string> GetMetadata()
        {
            return string.Empty;
        }

        private void MediaPlayer_MediaEnded(MediaPlayer sender, object args)
        {
            this.EndCurrentItemReached?.Invoke(this, EventArgs.Empty);
        }

        private void MediaPlayer_CurrentStateChanged(MediaPlayer sender, object args)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MediaPlayer_MediaOpened(MediaPlayer sender, object args)
        {
            this.MediaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
