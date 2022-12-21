// <copyright file="NSEventModiferMask.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace Drastic.Tray
{
    public enum NSEventModifierMask : ulong
    {
        AlphaShiftKeyMask = 0x10000uL,
        ShiftKeyMask = 0x20000uL,
        ControlKeyMask = 0x40000uL,
        AlternateKeyMask = 0x80000uL,
        CommandKeyMask = 0x100000uL,
        NumericPadKeyMask = 0x200000uL,
        HelpKeyMask = 0x400000uL,
        FunctionKeyMask = 0x800000uL,
        DeviceIndependentModifierFlagsMask = 0xFFFF0000uL
    }
}