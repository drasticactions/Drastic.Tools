// <copyright file="MediaPlayerPositionChangedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Audio
{
    /// <summary>
    /// The mediaplayer's position changed.
    /// </summary>
    public class MediaPlayerPositionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Mediaplayer's current position.
        /// </summary>
        public readonly TimeSpan Position;

        /// <summary>
        /// Mediaplayer's duration of the current item.
        /// </summary>
        public readonly TimeSpan? Duration;

        /// <summary>
        /// Gets the total precent completed of the item, if duration is available.
        /// </summary>
        public readonly double PrecentCompleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerPositionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="position">Position.</param>
        public MediaPlayerPositionChangedEventArgs(TimeSpan position, TimeSpan? duration = default)
        {
            this.Position = position;
            this.Duration = duration;
            this.PrecentCompleted = 0;
            if (duration is TimeSpan real)
            {
                this.PrecentCompleted = this.Position.TotalSeconds / real.TotalSeconds;
            }
        }
    }
}
