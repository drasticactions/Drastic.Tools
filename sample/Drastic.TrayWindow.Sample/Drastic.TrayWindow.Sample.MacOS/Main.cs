// <copyright file="Main.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using Drastic.TrayWindow.Sample.MacOS;

// This is the main entry point of the application.
NSApplication.Init();

NSApplication.SharedApplication.Delegate = new AppDelegate();

NSApplication.Main(args);
