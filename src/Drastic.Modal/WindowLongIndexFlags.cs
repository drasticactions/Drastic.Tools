// <copyright file="WindowLongIndexFlags.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace Drastic.Modal;

/// <summary>
/// Window Long Index Flag.
/// </summary>
[Flags]
internal enum WindowLongIndexFlags : int
{
#pragma warning disable SA1602 // Enumeration items should be documented
    GWL_EXSTYLE = -20,
    GWLP_HINSTANCE = -6,
    GWLP_HWNDPARENT = -8,
    GWL_ID = -12,
    GWLP_ID = GWL_ID,
    GWL_STYLE = -16,
    GWL_USERDATA = -21,
    GWLP_USERDATA = GWL_USERDATA,
    GWL_WNDPROC = -4,
    GWLP_WNDPROC = GWL_WNDPROC,
    DWLP_USER = 0x8,
    DWLP_MSGRESULT = 0x0,
    DWLP_DLGPROC = 0x4,
#pragma warning restore SA1602 // Enumeration items should be documented
}