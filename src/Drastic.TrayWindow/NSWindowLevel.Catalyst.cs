// <copyright file="NSWindowLevel.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Runtime.Versioning;

namespace Drastic.TrayWindow
{
#pragma warning disable SA1600
#pragma warning disable SA1602
    internal enum NSWindowLevel : long
    {
        Normal = 0L,
        Dock = 20L,
        Floating = 3L,
        MainMenu = 24L,
        ModalPanel = 8L,
        PopUpMenu = 101L,
        ScreenSaver = 1000L,
        Status = 25L,
        Submenu = 3L,
        TornOffMenu = 3L,
    }
#pragma warning restore SA1600
#pragma warning restore SA1602
}