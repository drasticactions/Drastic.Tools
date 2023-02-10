// <copyright file="ModalWindowOptions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Windows.Graphics;

namespace Drastic.Modal
{
    /// <summary>
    /// Modal Window Options.
    /// </summary>
    public class ModalWindowOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow the window to be resized.
        /// </summary>
        public bool IsResizable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the window to be minimized.
        /// </summary>
        public bool IsMinimizable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the window to be maximized.
        /// </summary>
        public bool IsMaximizable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the close button on the window.
        /// </summary>
        public bool HideCloseButton { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to always show the modal window on top.
        /// </summary>
        public bool AlwaysOnTop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the window to be shown in the taskbar. Defaults to true.
        /// </summary>
        public bool IsShownInSwitchers { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to disable input on all windows. Defaults to true.
        /// </summary>
        public bool DisableOtherWindows { get; set; } = true;

        /// <summary>
        /// Gets or sets the min size of the window.
        /// </summary>
        public SizeInt32? MinSize { get; set; }

        /// <summary>
        /// Gets or sets the position of the window.
        /// Use the helper command <see cref="ModalWindowExtensions.PositionModalInCenter"/> to set the window to appear in the center of a given window.
        /// </summary>
        public PointInt32? Position { get; set; }
    }
}
