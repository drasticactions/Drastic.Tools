// <copyright file="IMediaService.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drastic.Audio.Model;

namespace Drastic.Audio.Services
{
    /// <summary>
    /// IMediaService.
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Position Changed.
        /// </summary>
        event EventHandler<MediaPlayerPositionChangedEventArgs>? PositionChanged;

        /// <summary>
        /// End Current Item Reached.
        /// </summary>
        event EventHandler<EventArgs>? EndCurrentItemReached;

        /// <summary>
        /// Media Changed.
        /// </summary>
        event EventHandler<EventArgs>? MediaChanged;

        /// <summary>
        /// RaiseCanExecuteChanged.
        /// </summary>
        event EventHandler<EventArgs>? RaiseCanExecuteChanged;

        /// <summary>
        /// Gets or sets the current media.
        /// </summary>
        IMediaItem? CurrentMedia { get; set; }

        TimeSpan? Duration { get; }

        /// <summary>
        /// Gets a value indicating whether the stream is playing.
        /// </summary>
        bool IsPlaying { get; }

        /// <summary>
        /// Gets or sets the current position of the stream.
        /// </summary>
        float CurrentPosition { get; set; }

        /// <summary>
        /// Gets the current media album URI.
        /// </summary>
        /// <returns>String.</returns>
        Task<string> GetArtworkUrl();

        /// <summary>
        /// Play the current stream.
        /// </summary>
        /// <param name="position">The position to start the stream from.</param>
        /// <param name="fromPosition">Play the item from the position.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        Task PlayAsync(double position = 0, bool fromPosition = false);

        /// <summary>
        /// Stops the current item.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        Task StopAsync();

        /// <summary>
        /// Resumes the current item.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        Task ResumeAsync();

        /// <summary>
        /// Pause the current stream.
        /// </summary>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        Task PauseAsync();

        /// <summary>
        /// Skip ahead in the current stream.
        /// </summary>
        /// <param name="amount">Amount of time to skip ahead.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        Task SkipAhead(double amount = 0);

        /// <summary>
        /// Skip back in the current stream.
        /// </summary>
        /// <param name="amount">Amount of time to skip ahead.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        Task SkipBack(double amount = 0);
    }
}
