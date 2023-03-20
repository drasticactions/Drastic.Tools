// <copyright file="FlareView.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

// Port from https://github.com/StanlyHardy/FlareView

using CoreAnimation;

namespace Drastic.Flare;

public class FlareView
{
    private static FlareView? sharedFlareViewCenter = null;
    private UIColor? flareColor;

    public static FlareView SharedCenter
    {
        get
        {
            if (sharedFlareViewCenter == null)
            {
                sharedFlareViewCenter = new FlareView();
            }

            return sharedFlareViewCenter;
        }
    }

    public void Flarify(UIView childView, UIView rootView, UIColor fillColor)
    {
        childView.UserInteractionEnabled = false;
        this.flareColor = fillColor;
        childView.Transform = CGAffineTransform.MakeTranslation(0, 20);
        UIView.AnimateNotify(0.4, 0.0, .2f, 1.0f, UIViewAnimationOptions.CurveEaseInOut, () =>
        {
            childView.Transform = CGAffineTransform.MakeScale(0.01f, 0.01f);
        },
        new UICompletionHandler((bool finished) => {
            var vortexView = new UIView(new CGRect(childView.Frame.X, childView.Frame.Y, childView.Frame.Width, childView.Frame.Height));
            var vortexLayer = new CAShapeLayer();
            vortexView.Bounds = childView.Bounds;

            vortexLayer.StrokeColor = flareColor.CGColor;
            vortexLayer.FillColor = UIColor.Clear.CGColor;
            vortexLayer.Path = UIBezierPath.FromOval(vortexView.Bounds).CGPath;

            vortexView.Layer.AddSublayer(vortexLayer);
            rootView.AddSubview(vortexView);

            vortexView.Transform = CGAffineTransform.MakeScale(0, 0);

            UIView.AnimateNotify(1, () =>
            {
                vortexView.Transform = CGAffineTransform.MakeScale(1.3f, 1.3f);
            }, new UICompletionHandler((bool finished) =>
            {
                vortexView.Hidden = true;
                CreateFlares(childView, rootView);
            }));
        }));
    }

    private void CreateFlares(UIView childView, UIView rootView)
    {
        childView.Transform = CGAffineTransform.MakeScale(0, 0);

        UIView.AnimateNotify(0.3 / 1.5, () =>
        {
            var test = CGAffineTransform.MakeIdentity();
            test.Scale(1.1f, 1.1f);
            childView.Transform = test;
        }, new UICompletionHandler((bool finished) =>
        {
            UIView.AnimateNotify(0.3 / 2, () =>
            {
                var test = CGAffineTransform.MakeIdentity();
                test.Scale(0.9f, 0.9f);
                childView.Transform = test;
            }, new UICompletionHandler((bool finished) =>
            {
                UIView.Animate(0.3 / 2, () =>
                {
                    childView.Transform = CGAffineTransform.MakeIdentity();
                });
            }));
        }));

        int numberOfFlares = 20;
        CGPoint center = childView.Center;
        float radius = 55f;
        bool isBurlyFlare = true;

        for (int i = 0; i < numberOfFlares; i++)
        {
            nfloat x = radius * (float)Math.Cos(Math.PI / numberOfFlares * i * 2) + center.X;
            nfloat y = radius * (float)Math.Sin(Math.PI / numberOfFlares * i * 2) + center.Y;

            float circleRadius = 10;
            if (isBurlyFlare)
            {
                circleRadius = 5;
                isBurlyFlare = false;
            }
            else
            {
                isBurlyFlare = true;
            }

            var vortexView = new UIView(new CGRect(x, y, circleRadius, circleRadius));
            var circleLayer = new CAShapeLayer();

            circleLayer.StrokeColor = flareColor?.CGColor;
            circleLayer.FillColor = flareColor?.CGColor;
            circleLayer.Path = UIBezierPath.FromOval(vortexView.Bounds).CGPath;
            vortexView.Layer.AddSublayer(circleLayer);
            rootView.AddSubview(vortexView);

            UIView.AnimateNotify(0.8, 0, UIViewAnimationOptions.CurveEaseOut, () =>
            {
                vortexView.Transform = CGAffineTransform.MakeTranslation((float)(radius / 3 * Math.Cos(Math.PI / numberOfFlares * i * 2)), (float)(radius / 3 * Math.Sin(Math.PI / numberOfFlares * i * 2)));
                vortexView.Transform = CGAffineTransform.Scale(vortexView.Transform, 0.01f, 0.01f);
            }, new UICompletionHandler((bool finished) =>
            {
                vortexView.Transform = CGAffineTransform.MakeScale(0, 0);

                UIView.AnimateNotify(0.2, 0, UIViewAnimationOptions.CurveEaseOut, () =>
                {
                    childView.Transform = CGAffineTransform.MakeIdentity();
                }, new UICompletionHandler((bool finished) =>
                {
                    childView.UserInteractionEnabled = true;
                }));
            }));
        }
    }
}