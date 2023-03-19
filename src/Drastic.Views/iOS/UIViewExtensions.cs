// <copyright file="UIViewExtensions.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

using UIKit;

namespace Drastic.Views;

public enum ShakeDirection
{
    Vertical,
    Rotation,
    Horizontal
}

public static class UIViewExtensions
{
    public static void Shake(this UIView view)
    {
        view.Shake(5, 5, 0.03);
    }

    public static void Shake(this UIView view, int times, nfloat delta)
    {
        view.Shake(times, delta, null);
    }

    public static void Shake(this UIView view, int times, nfloat delta, Action completion)
    {
        view.Shake(times, delta, 0.03, completion);
    }

    public static void Shake(this UIView view, int times, nfloat delta, double interval)
    {
        view.Shake(times, delta, interval, null);
    }

    public static void Shake(this UIView view, int times, nfloat delta, double interval, Action completion)
    {
        view.Shake(times, delta, interval, ShakeDirection.Horizontal, completion);
    }

    public static void Shake(this UIView view, int times, nfloat delta, double interval, ShakeDirection shakeDirection)
    {
        view.Shake(times, delta, interval, shakeDirection, null);
    }

    public static void Shake(this UIView view, int times, nfloat delta, double interval, ShakeDirection shakeDirection,
        Action completion)
    {
        view.Shake(times, 1, 0, delta, interval, shakeDirection, completion);
    }

    static void Shake(this UIView view, int times, int direction, int current, nfloat delta, double interval,
        ShakeDirection shakeDirection, Action completionHandler)
    {
        void Animations()
        {
            switch (shakeDirection)
            {
                case ShakeDirection.Vertical:
                    view.Transform = CGAffineTransform.MakeTranslation(0, delta * direction);
                    break;
                case ShakeDirection.Rotation:
                    view.Transform = CGAffineTransform.MakeRotation((float)(Math.PI * delta / 1000.0f * direction));
                    break;
                case ShakeDirection.Horizontal:
                    view.Transform = CGAffineTransform.MakeTranslation(delta * direction, 0);
                    break;
                default:
                    break;
            }
        }

        UIView.AnimateNotify(interval, 0, 1, 0, UIViewAnimationOptions.BeginFromCurrentState,
            Animations,
            (finished) =>
            {
                if (current >= times)
                {
                    UIView.AnimateNotify(interval, 0, 1, 0, UIViewAnimationOptions.BeginFromCurrentState,
                        () => { view.Transform = CGAffineTransform.MakeIdentity(); },
                        (finished) => { completionHandler?.Invoke(); });
                    return;
                }

                view.Shake(times, direction * -1, current + 1, delta, interval, shakeDirection, completionHandler);
            });
    }
}