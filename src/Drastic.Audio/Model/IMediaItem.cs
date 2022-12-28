// <copyright file="IMediaItem.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drastic.Audio.Model
{
    /// <summary>
    /// Media Element.
    /// </summary>
    public interface IMediaItem
    {
        /// <summary>
        /// Gets or sets the local path to the media.
        /// </summary>
        public string? LocalPath { get; set; }

        /// <summary>
        /// Gets or sets the online path to the media.
        /// </summary>
        public Uri? OnlinePath { get; set; }
    }
}
