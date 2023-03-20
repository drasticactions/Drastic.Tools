// <copyright file="TestViewController.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Reflection;
using Masonry;

namespace Drastic.Flare.Sample
{
    public class TestViewController : UIViewController
    {
        private UIButton button;

        public TestViewController()
        {
            this.View!.BackgroundColor = UIColor.White;
            var icon = GetResourceFileContent("Icon.dotnet-bot-button.png");
            var image = UIImage.LoadFromData(NSData.FromStream(icon!)!);
            this.button = new UIButton();
            this.button.Frame = new CGRect(0, 0, image!.Size.Width, image!.Size.Height);
            this.button.SetImage(image, UIControlState.Normal);
            this.button.TouchDown += Button_TouchDown;
            this.View!.AddSubview(this.button);
            this.button.MakeConstraints(make =>
            {
                make.Center.EqualTo(this.View);
            });
        }

        private void Button_TouchDown(object? sender, EventArgs e)
        {
            Drastic.Flare.FlareView.SharedCenter.Flarify(this.button, this.View!, UIColor.Purple);
        }

        /// <summary>
        /// Get Resource File Content via FileName.
        /// </summary>
        /// <param name="fileName">Filename.</param>
        /// <returns>Stream.</returns>
        public static Stream? GetResourceFileContent(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Drastic.Flare.Sample." + fileName;
            if (assembly is null)
            {
                return null;
            }

            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}