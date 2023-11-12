// <copyright file="FPSViewerTimeoutException.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.UITools;

/// <summary>
/// Thrown when Timeout is exceeded.
/// </summary>
public class FPSViewerTimeoutException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FPSViewerTimeoutException"/> class.
    /// </summary>
    /// <param name="message">Message.</param>
    public FPSViewerTimeoutException(string message)
        : base(message)
    {
    }
}