// <copyright file="TrayIcon.Catalyst.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AddressBook;
using AppKit;
using CoreFoundation;
using Drastic.Interop;
using ObjCRuntime;
using UIKit;

namespace Drastic.Tray
{
    /// <summary>
    /// Tray Icon.
    /// </summary>
    public partial class TrayIcon : NSObject
    {
        private NSObject statusBarItem;
        private ShimNSMenu menu;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrayIcon"/> class.
        /// </summary>
        /// <param name="name">Name of the icon.</param>
        /// <param name="image">Icon Image Stream. Optional.</param>
        /// <param name="menuItems">Items to populate context menu. Optional.</param>
        public TrayIcon(TrayImage? image = null, List<TrayMenuItem>? menuItems = null)
        {
            this.menuItems = menuItems ?? new List<TrayMenuItem>();

            this.menu = new ShimNSMenu();

            var systemStatusBarObj = GetNSStatusBar().PerformSelector(new Selector("systemStatusBar"));
            this.statusBarItem = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(systemStatusBarObj.Handle, "statusItemWithLength:", -1f))!;
            var statusBarButton = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(this.statusBarItem.Handle, "button"));

            if (statusBarButton is not null && image is not null)
            {
                Drastic.Interop.ObjC.Call(statusBarButton.Handle, "setImage:", image.Image.Handle);
                Drastic.Interop.ObjC.Call(image.Image.Handle, "setTemplate:", true);

                image.Image.Size = new CoreGraphics.CGSize(24, 24);
            }

            if (statusBarButton is not null)
            {
                // Handle click
                // 26 = NSEventType.OtherMouseUp
                Drastic.Interop.ObjC.Call(statusBarButton.Handle, "sendActionOn:", 26);
                Drastic.Interop.ObjC.Call(statusBarButton.Handle, "setTarget:", this.Handle);
                Drastic.Interop.ObjC.Call(statusBarButton.Handle, "setAction:", new Selector("handleButtonClick:").Handle);
            }

            if (menuItems is not null)
            {
                this.SetupStatusBarMenu(menuItems);
            }
        }

        public CGRect GetFrame()
        {
            var statusBarButton = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(this.statusBarItem!.Handle, "button"));
            var nsButtonWindow = Runtime.GetNSObject(Drastic.Interop.ObjC.Call(statusBarButton!.Handle, "window"));
            if (nsButtonWindow is null)
            {
                return new CGRect(0, 0, 0, 0);
            }

            var windowFrame = (NSValue)nsButtonWindow.ValueForKey(new Foundation.NSString("frame"));

            return windowFrame.CGRectValue;
        }

        public void OpenMenu()
        {
            NativeHandle nonNullHandle = this.menu.GetNonNullHandle("menu");
            Drastic.Interop.ObjC.Call(this.statusBarItem.Handle, "popUpStatusItemMenu:", nonNullHandle);
        }

        public void SetupStatusBarMenu(List<TrayMenuItem> menuItems)
        {
            this.menu.RemoveAllItems();

            foreach (var item in this.menuItems)
            {
                var nsMenuItem = new ShimNSMenuItem(item);
                this.menu.AddItem(nsMenuItem);
            }
        }

        private static NSObject GetNSMenu()
           => Runtime.GetNSObject(Drastic.Interop.AppKit.Call("NSMenu", "new"))!;

        private static NSObject GetNSStatusBar()
           => Runtime.GetNSObject(Class.GetHandle("NSStatusBar"))!;

        private static NSObject GetNSMenuItem()
        {
            var item = Runtime.GetNSObject(Drastic.Interop.AppKit.Call("NSMenuItem", "alloc"))!;
            return item;
        }

        private static void NSApplicationActivateIgnoringOtherApps(bool ignoreSetting = true)
        {
            var sharedApp = Drastic.Interop.AppKit.Call("NSApplication", "sharedApplication");
            Drastic.Interop.ObjC.Call(sharedApp, "activateIgnoringOtherApps:", ignoreSetting);
        }

        private static NSObject GetNSApplicationSharedApplicationCurrentEvent()
        {
            var sharedApp = Drastic.Interop.AppKit.Call("NSApplication", "sharedApplication");
            return Runtime.GetNSObject<NSObject>(Drastic.Interop.ObjC.Call(sharedApp, "currentEvent"))!;
        }

        private void NativeElementDispose()
        {
            this.statusBarItem.Dispose();
            this.menu.Dispose();
        }

        [Export("handleButtonClick:")]
        private void HandleClick(NSObject senderStatusBarButton)
        {
            var test = GetNSApplicationSharedApplicationCurrentEvent()!;
            var type = (NSEventType)Drastic.Interop.ObjC.Call(test.Handle, "type");
            NSApplicationActivateIgnoringOtherApps(true);
            switch (type)
            {
                case NSEventType.LeftMouseDown:
                    this.LeftClicked?.Invoke(this, TrayClickedEventArgs.Empty);
                    break;
                case NSEventType.RightMouseDown:
                    this.RightClicked?.Invoke(this, TrayClickedEventArgs.Empty);
                    break;
            }
        }

        internal class ShimNSMenu : NSObject
        {
            public ShimNSMenu()
            {
                this.Handle = Drastic.Interop.AppKit.Call("NSMenu", "new");
            }

            public void RemoveAllItems()
            {
                Drastic.Interop.ObjC.Call(this.Handle, "removeAllItems");
            }

            public void AddItem(ShimNSMenuItem item)
            {
                NativeHandle nonNullHandle = item.GetNonNullHandle("newItem");
                Drastic.Interop.ObjC.Call(this.Handle, "addItem:", nonNullHandle);
            }
        }

        internal class ShimNSMenuItem : NSObject
        {
            internal TrayMenuItem Item;

            private static readonly NativeClassDefinition CallbackClassDefinition;

            private readonly NativeClassInstance callbackClass;

            static ShimNSMenuItem()
            {
                CallbackClassDefinition = CreateCallbackClass();
            }

            public ShimNSMenuItem(TrayMenuItem item)
            {
                this.Item = item;
                this.Handle = Drastic.Interop.AppKit.Call("NSMenuItem", "alloc");
                ObjC.Call(this.Handle, "initWithTitle:action:keyEquivalent:", Drastic.Interop.NSString.Create(item.Text), ObjC.RegisterName("menuCallback:"), Drastic.Interop.NSString.Create(string.Empty));
                this.callbackClass = CallbackClassDefinition.CreateInstance(this);
                this.SetTarget(this.callbackClass.Handle);

                if (this.Item.Icon is not null)
                {
                    this.Image = this.Item.Icon.Image;
                }

                this.KeyEquivalent = item.KeyEquivalent;
                this.KeyEquivalentModifierMask = item.KeyEquivalentModifierMask;
            }

            public AppKit.NSImage? Image
            {
                get
                {
                    return Runtime.GetNSObject<NSImage>(Drastic.Interop.ObjC.Call(this.Handle, "image"));
                }

                set
                {
                    NativeHandle arg = value.GetHandle();
                    Drastic.Interop.ObjC.Call(this.Handle, "setImage:", arg);
                }
            }

            public string? KeyEquivalent
            {
                get
                {
                    return CFString.FromHandle(Drastic.Interop.ObjC.Call(this.Handle, "keyEquivalent"));
                }

                set
                {
                    NativeHandle arg = CFString.CreateNative(value);
                    Drastic.Interop.ObjC.Call(this.Handle, "setKeyEquivalent:", arg);
                }
            }

            public NSEventModifierMask? KeyEquivalentModifierMask
            {
                get
                {
                    return (NSEventModifierMask)(nuint)Drastic.Interop.ObjC.Call(this.Handle, "keyEquivalentModifierMask");
                }

                set
                {
                    if (value is not null)
                    {
                        Drastic.Interop.ObjC.Call(this.Handle, "setKeyEquivalentModifierMask:", (nuint)value);
                    }
                }
            }

            private static NativeClassDefinition CreateCallbackClass()
            {
                var definition = NativeClassDefinition.FromObject("DrasticInteropMenuCallback");

                definition.AddMethod<MenuCallbackDelegate>(
                    "menuCallback:",
                    "v@:@",
                    (self, op, menu) =>
                    {
                        var instance = definition.GetParent<ShimNSMenuItem>(self);
                        instance.Item.Action?.Invoke();
                    });

                definition.FinishDeclaration();

                return definition;
            }

            private void SetTarget(IntPtr target)
            {
                ObjC.Call(this.Handle, "setTarget:", target);
            }

            private void SetTag(long tag)
            {
                ObjC.Call(this.Handle, "setTag:", new IntPtr(tag));
            }
        }
    }
}