// <copyright file="ModalWindowExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Runtime.InteropServices;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.Graphics;

namespace Drastic.Modal;

public static class ModalWindowExtensions
{
    public static bool EnableWindow(this Window window, bool enabled)
        => InternalModalWindowExtensions.EnableWindow(window.GetWindowHandle(), enabled);
}