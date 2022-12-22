// <copyright file="TrayClickedEventArgs.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Tray
{
    public class TrayClickedEventArgs : EventArgs
    {
        public static readonly TrayClickedEventArgs Empty = new TrayClickedEventArgs();
    }
}