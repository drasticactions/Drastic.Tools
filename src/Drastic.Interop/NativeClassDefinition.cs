// <copyright file="NativeClassDefinition.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Drastic.Interop
{
    internal class NativeClassDefinition
    {
        private readonly List<Delegate> callbacks;
        private readonly IntPtr[] protocols;

        public IntPtr Handle { get; private set; }

        private bool registered;
        private IntPtr ivar;

        private NativeClassDefinition(string name, IntPtr parent, IntPtr[] protocols)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.protocols = protocols ?? throw new ArgumentNullException(nameof(protocols));
            this.callbacks = new List<Delegate>();
            this.Handle = ObjC.AllocateClassPair(parent, name, IntPtr.Zero);
        }

        public static NativeClassDefinition FromObject(string name, params IntPtr[] protocols)
        {
            return FromClass(name, ObjC.GetClass("NSObject"), protocols);
        }

        public static NativeClassDefinition FromClass(string name, IntPtr parent, params IntPtr[] protocols)
        {
            return new NativeClassDefinition(name, parent, protocols);
        }

        public void AddMethod<T>(string name, string signature, T callback)
            where T : Delegate
        {
            if (this.registered)
            {
                throw new InvalidOperationException("Native class is already declared and registered");
            }

            // keep reference to callback or it will get garbage collected
            this.callbacks.Add(callback);

            ObjC.AddMethod(
                this.Handle,
                ObjC.RegisterName(name),
                callback,
                signature);
        }

        public void FinishDeclaration()
        {
            if (this.registered)
            {
                throw new InvalidOperationException("Native class is already declared and registered");
            }

            this.registered = true;

            // variable to hold reference to .NET object that creates an instance
            const string variableName = "_SEInstance";
            ObjC.AddVariable(this.Handle, variableName, new IntPtr(IntPtr.Size), (byte)Math.Log(IntPtr.Size, 2), "@");
            this.ivar = ObjC.GetVariable(this.Handle, variableName);

            foreach (IntPtr protocol in this.protocols)
            {
                if (protocol == IntPtr.Zero)
                {
                    // must not add null protocol, can cause runtime exception with conformsToProtocol check
                    continue;
                }

                ObjC.AddProtocol(this.Handle, protocol);
            }

            ObjC.RegisterClassPair(this.Handle);
        }

        public NativeClassInstance CreateInstance(object parent)
        {
            if (!this.registered)
            {
                throw new InvalidOperationException("Native class is not yet fully declared and registered");
            }

            IntPtr instance = ObjC.Call(this.Handle, "new");

            var parentHandle = GCHandle.Alloc(parent, GCHandleType.Normal);
            ObjC.SetVariableValue(instance, this.ivar, GCHandle.ToIntPtr(parentHandle));

            return new NativeClassInstance(instance, parentHandle);
        }

        public T GetParent<T>(IntPtr self)
        {
            IntPtr handle = ObjC.GetVariableValue(self, this.ivar);
            return (T)GCHandle.FromIntPtr(handle).Target!;
        }
    }
}
