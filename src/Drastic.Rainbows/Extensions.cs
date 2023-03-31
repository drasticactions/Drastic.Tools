// <copyright file="Extensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using ObjCRuntime;

#nullable disable

namespace Drastic.Rainbows
{
    internal static class Extensions
    {
        public static Class GetClass(this Type type)
        {
            var handle = Class.GetHandle(type);
            return handle != IntPtr.Zero ? new Class(handle) : null;
        }
    }
}