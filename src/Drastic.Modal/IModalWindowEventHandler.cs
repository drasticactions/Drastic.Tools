// <copyright file="IModalWindowEventHandler.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Modal
{
    /// <summary>
    /// Handler for Modal Window Events.
    /// Implement this on your base Window elements.
    /// </summary>
    public interface IModalWindowEventHandler
    {
        /// <summary>
        /// Fired when window is enabled.
        /// </summary>
        public void Enabled();

        /// <summary>
        /// Fired when window is disabled.
        /// </summary>
        public void Disabled();
    }
}
