using System;
using System.Runtime.Versioning;

namespace Drastic.Tray
{
    public enum NSEventType : ulong
    {
        LeftMouseDown = 1uL,
        LeftMouseUp = 2uL,
        RightMouseDown = 3uL,
        RightMouseUp = 4uL,
        MouseMoved = 5uL,
        LeftMouseDragged = 6uL,
        RightMouseDragged = 7uL,
        MouseEntered = 8uL,
        MouseExited = 9uL,
        KeyDown = 10uL,
        KeyUp = 11uL,
        FlagsChanged = 12uL,
        AppKitDefined = 13uL,
        SystemDefined = 14uL,
        ApplicationDefined = 0xFuL,
        Periodic = 0x10uL,
        CursorUpdate = 17uL,
        ScrollWheel = 22uL,
        TabletPoint = 23uL,
        TabletProximity = 24uL,
        OtherMouseDown = 25uL,
        OtherMouseUp = 26uL,
        OtherMouseDragged = 27uL,
        Gesture = 29uL,
        Magnify = 30uL,
        Swipe = 0x1FuL,
        Rotate = 18uL,
        BeginGesture = 19uL,
        EndGesture = 20uL,
        SmartMagnify = 0x20uL,
        QuickLook = 33uL,
        Pressure = 34uL,
        DirectTouch = 37uL,
        ChangeMode = 38uL
    }
}