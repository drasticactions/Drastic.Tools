// <copyright file="ImageSpinWheel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using CoreAnimation;

#nullable disable

// Port from https://github.com/unixpickle/SpinWheel/blob/master/SpinWheel/ANImageWheel.m
namespace Drastic.SpinWheel
{
    public class ImageSpinWheel : SpinWheel
    {
        private readonly UIImageView imageView;

        public ImageSpinWheel()
        {
        }

        public ImageSpinWheel(CGRect frame)
            : base(frame)
        {
            this.imageView = new UIImageView(this.Frame);
            this.imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            this.AddSubview(this.imageView);
        }

        [Export("initWithCoder:")]
        public ImageSpinWheel(NSCoder coder)
            : base(coder)
        {
            this.imageView = new UIImageView(this.Frame);
            this.imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            this.AddSubview(this.imageView);
        }

        public UIImage Image
        {
            get => this.imageView.Image;
            set => this.imageView.Image = value;
        }

        public ImageSpinWheel(CGRect frame, UIImage image)
            : this(frame)
        {
            this.Image = image;
        }

        public override double Angle
        {
            set
            {
                base.Angle = value;
                this.imageView.Layer.Transform = CATransform3D.MakeRotation((float)value, 0, 0, 1);
            }
        }
    }
}