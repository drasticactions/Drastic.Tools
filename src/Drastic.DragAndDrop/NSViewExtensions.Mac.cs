// <copyright file="NSViewExtensions.Mac.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.DragAndDrop
{
    /// <summary>
    /// NS View Extensions.
    /// </summary>
    internal static class NSViewExtensions
    {
        /// <summary>
        /// Bring Subviews to Front.
        /// </summary>
        /// <param name="view">Base View.</param>
        /// <param name="parentContainer">View to move forward.</param>
        public static void BringSubviewToFront(this NSView view, NSView parentContainer)
        {
            parentContainer.SortSubviews((viewA, viewB) =>
            {
                if (viewA == view)
                {
                    System.Diagnostics.Debug.WriteLine(nameof(BringSubviewToFront) + NSComparisonResult.Descending);
                    return NSComparisonResult.Descending;
                }
                else if (viewB == view)
                {
                    System.Diagnostics.Debug.WriteLine(nameof(BringSubviewToFront) + NSComparisonResult.Ascending);
                    return NSComparisonResult.Ascending;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(nameof(BringSubviewToFront) + NSComparisonResult.Same);
                    return NSComparisonResult.Same;
                }
            });
        }
    }
}