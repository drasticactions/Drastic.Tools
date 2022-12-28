// <copyright file="NativeMediaService.AllApple.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using AVFoundation;

namespace Drastic.Audio.Services
{
    /// <summary>
    /// Native Media Player.
    /// </summary>
    public partial class NativeMediaService
    {
        private NSObject? playedToEndObserver;
        private IDisposable? statusObserver;
        private IDisposable? rateObserver;
        private IDisposable? volumeObserver;

        private AVPlayer? avPlayer;
        private AVPlayerItem? playerItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeMediaService"/> class.
        /// </summary>
        public NativeMediaService()
        {
            this.positionTimer = new Timer(this.PositionTimerElapsed, null, 0, 500);
            this.avPlayer = new AVPlayer();
        }

        /// <inheritdoc/>
        public bool IsPlaying => this.avPlayer?.Rate != 0.0f;

        /// <inheritdoc/>
        public float CurrentPosition
        {
            get
            {
                if (this.avPlayer?.CurrentItem == null)
                {
                    return 0;
                }

                return (float)(this.avPlayer.CurrentTime.Seconds / this.avPlayer.CurrentItem.Duration.Seconds);
            }

            set
            {
                if (this.avPlayer?.CurrentItem != null)
                {
                    this.avPlayer.Seek(new CoreMedia.CMTime((long)(this.avPlayer.CurrentItem.Duration.Seconds * value), 1));
                }
            }
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            this.avPlayer?.Pause();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PauseAsync()
        {
            this.avPlayer?.Pause();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PlayAsync(double position = 0, bool fromPosition = false)
        {
#if !MACOS
            var audioSession = AVAudioSession.SharedInstance();
            var err = audioSession.SetCategory(AVAudioSession.CategoryPlayback);
            audioSession.SetMode(AVAudioSession.ModeMoviePlayback, out err);
            err = audioSession.SetActive(true);
#endif

            this.avPlayer?.Play();
            this.RaiseCanExecuteChanged?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task ResumeAsync()
        {
            this.avPlayer?.Play();
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

            NSUrl url = this.CurrentMedia.LocalPath != null ? NSUrl.CreateFileUrl(this.CurrentMedia.LocalPath, false, null) : new NSUrl(this.CurrentMedia.OnlinePath?.ToString() ?? string.Empty);

            var playerItem = new AVPlayerItem(url);
            this.avPlayer?.ReplaceCurrentItemWithPlayerItem(playerItem);
            this.AddRateObserver();
            this.AddVolumeObserver();
            this.AddPlayedToEndObserver();
            this.AddStatusObserver();
        }

        private void DisposeObservers(ref IDisposable? disposable)
        {
            disposable?.Dispose();
            disposable = null;
        }

        private void DisposeObservers(ref NSObject? disposable)
        {
            disposable?.Dispose();
            disposable = null;
        }

        private void AddVolumeObserver()
        {
            this.DestroyVolumeObserver();
            this.volumeObserver = this.avPlayer?.AddObserver("volume", NSKeyValueObservingOptions.New, this.ObserveVolume);
        }

        private void AddRateObserver()
        {
            this.DestroyRateObserver();
            this.rateObserver = this.avPlayer?.AddObserver("rate", NSKeyValueObservingOptions.New, this.ObserveRate);
        }

        private void AddStatusObserver()
        {
            this.DestroyStatusObserver();
            this.statusObserver = this.playerItem?.AddObserver("status", NSKeyValueObservingOptions.New, this.ObserveStatus);
        }

        private void AddPlayedToEndObserver()
        {
            this.DestroyPlayedToEndObserver();
            this.playedToEndObserver =
                NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, this.PlayedToEnd);
        }

        private void DestroyVolumeObserver() => this.DisposeObservers(ref this.volumeObserver);

        private void DestroyRateObserver() => this.DisposeObservers(ref this.rateObserver);

        private void DestroyStatusObserver() => this.DisposeObservers(ref this.statusObserver);

        private void DestroyPlayedToEndObserver()
        {
            if (this.playedToEndObserver == null)
            {
                return;
            }

            NSNotificationCenter.DefaultCenter.RemoveObserver(this.playedToEndObserver);
            this.DisposeObservers(ref this.playedToEndObserver);
        }

        private void PlayedToEnd(NSNotification notification)
        {
            if (notification.Object != this.avPlayer?.CurrentItem)
            {
                return;
            }

            this.EndCurrentItemReached?.Invoke(this, EventArgs.Empty);
        }

        private void ObserveStatus(NSObservedChange e)
        {
            if (this.avPlayer == null)
            {
                return;
            }

            switch (this.avPlayer.Status)
            {
                case AVPlayerStatus.Unknown:
                    break;
                case AVPlayerStatus.ReadyToPlay:
                    break;
                case AVPlayerStatus.Failed:
                    break;
            }

            this.RaiseCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private async Task<string> GetMetadata()
        {
            return string.Empty;
        }

        private void ObserveRate(NSObservedChange e)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ObserveVolume(NSObservedChange e)
        {
            this.RaiseCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
