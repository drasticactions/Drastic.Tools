// <copyright file="Swizzle.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

using System;
using ObjCRuntime;
using System.Runtime.InteropServices;

namespace Drastic
{
    public class Swizzle<TDelegate> : IDisposable where TDelegate : class
    {
        protected IntPtr originalMethod;
        protected IntPtr originalImpl;
        protected IntPtr victimSel;
        protected IntPtr newImpl;
        protected bool isClassMethod;

        protected TDelegate dlg; // Your delegate

        public Swizzle(Class victim, IntPtr selector, TDelegate del, bool isClassMethod = false)
        {
            dlg = del;
            victimSel = selector;

            originalMethod = isClassMethod ? LibObjc.class_getClassMethod(victim.Handle, victimSel) : LibObjc.class_getInstanceMethod(victim.Handle, victimSel);
            originalImpl = LibObjc.method_getImplementation(originalMethod);

            newImpl = Marshal.GetFunctionPointerForDelegate(del as System.Delegate);
            LibObjc.method_setImplementation(originalMethod, newImpl);
        }

        public Swizzle(Type victim, string selector, TDelegate del, bool isClassMethod = false)
            : this(victim.GetClass(), Selector.GetHandle(selector), del, isClassMethod)
        {
        }

        public Swizzle(NSObject victim, string selector, TDelegate del, bool isClassMethod = false)
            : this(victim.Class, Selector.GetHandle(selector), del, isClassMethod)
        {
        }

        public Unswizzle Restore()
        {
            return new Unswizzle(this);
        }

        public virtual void Dispose()
        {
            LibObjc.method_setImplementation(originalMethod, originalImpl);
        }

        // Use this class to call the original implementation
        public class Unswizzle : IDisposable
        {
            Swizzle<TDelegate> swizzle;
            public Unswizzle(Swizzle<TDelegate> swizzle)
            {
                this.swizzle = swizzle;
                LibObjc.method_setImplementation(swizzle.originalMethod, swizzle.originalImpl);
            }

            public TDelegate Delegate
            {
                get
                {
                    return Marshal.GetDelegateForFunctionPointer(swizzle.originalImpl, swizzle.dlg.GetType()) as TDelegate;
                }
            }

            public void Dispose()
            {
                LibObjc.method_setImplementation(swizzle.originalMethod, swizzle.newImpl);
                swizzle = null;
            }
        }
    }
}