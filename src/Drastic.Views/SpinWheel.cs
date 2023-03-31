// <copyright file="SpinWheel.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

using CoreAnimation;
using ObjCRuntime;

// Port from https://github.com/unixpickle/SpinWheel/blob/master/SpinWheel/ANSpinWheel.m
namespace Drastic.SpinWheel
{
    public class SpinWheel : UIView
    {
        private CADisplayLink displayTimer;
        private NSDate lastTimerDate;
        private CGPoint initialPoint;
        private double initialAngle;
        private double[] previousAngles = new double[2];
        private NSDate[] previousDates = new NSDate[2];
        private CGPoint[] previousPoints = new CGPoint[2];

        public SpinWheel()
        {
        }

        public SpinWheel(NSCoder coder)
            : base(coder)
        {
        }

        public SpinWheel(CGRect frame)
            : base(frame)
        {
        }

        protected internal SpinWheel(NativeHandle handle)
            : base(handle)
        {
        }

        protected SpinWheel(NSObjectFlag t)
            : base(t)
        {
        }

        public virtual double Angle { get; set; }

        public double AngularVelocity { get; set; }

        public double Drag { get; set; }

        public void StartAnimating()
        {
            if (this.displayTimer != null)
            {
                return;
            }

            this.lastTimerDate = null;
            this.displayTimer = CADisplayLink.Create(this.AnimationTimer);
            this.displayTimer.AddToRunLoop(NSRunLoop.Main, NSRunLoopMode.Common);
        }

        public void StopAnimating()
        {
            if (this.displayTimer == null)
            {
                return;
            }

            this.displayTimer.Invalidate();
            this.displayTimer = null;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            this.AngularVelocity = 0;
            this.initialAngle = this.Angle;
            this.initialPoint = ((UITouch)touches.AnyObject).LocationInView(this);
            this.PushTouchPoint(this.initialPoint, NSDate.Now);
        }

        private void AnimationTimer()
        {
            var newDate = NSDate.Now;

            if (this.lastTimerDate == null || this.AngularVelocity == 0)
            {
                this.lastTimerDate = newDate;
                return;
            }

            var passed = newDate.SecondsSinceReferenceDate - this.lastTimerDate.SecondsSinceReferenceDate;

            var angleReduction = this.Drag * passed * Math.Abs(this.AngularVelocity);

            if (this.AngularVelocity < 0)
            {
                this.AngularVelocity += angleReduction;

                if (this.AngularVelocity > 0)
                {
                    this.AngularVelocity = 0;
                }
            }
            else if (this.AngularVelocity > 0)
            {
                this.AngularVelocity -= angleReduction;

                if (this.AngularVelocity < 0)
                {
                    this.AngularVelocity = 0;
                }
            }

            if (Math.Abs(this.AngularVelocity) < 0.01)
            {
                this.AngularVelocity = 0;
            }

            var useAngle = this.Angle;
            useAngle += this.AngularVelocity * passed;

            // limit useAngle to +/- 2*PI
            if (useAngle < 0)
            {
                while (useAngle < -2 * Math.PI)
                {
                    useAngle += 2 * Math.PI;
                }
            }
            else
            {
                while (useAngle > 2 * Math.PI)
                {
                    useAngle -= 2 * Math.PI;
                }
            }

            this.Angle = useAngle;
            this.lastTimerDate = newDate;
            this.SetNeedsDisplay();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            var thePoint = ((UITouch)touches.AnyObject).LocationInView(this);
            this.PushTouchPoint(thePoint, NSDate.Now);
            var angleDiff = this.AngleForPoint(thePoint) - this.AngleForPoint(this.initialPoint);
            this.Angle = this.initialAngle + angleDiff;
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            this.AngularVelocity = this.CalculateFinalAngularVelocity(NSDate.Now);
            this.ClearTouchData();
        }

        private void PushTouchPoint(CGPoint point, NSDate date)
        {
            this.previousDates[0] = this.previousDates[1];
            this.previousPoints[0] = this.previousPoints[1];
            this.previousDates[1] = date;
            this.previousPoints[1] = point;
        }

        private void ClearTouchData()
        {
            this.previousDates[0] = null;
            this.previousDates[1] = null;
            this.previousPoints[0] = CGPoint.Empty;
            this.previousPoints[1] = CGPoint.Empty;
        }

        private double CalculateFinalAngularVelocity(NSDate finalDate)
        {
            if (this.previousDates[0] == null)
            {
                return 0;
            }

            var delay = finalDate.SecondsSinceReferenceDate - this.previousDates[0].SecondsSinceReferenceDate;
            var prevAngle = this.AngleForPoint(this.previousPoints[0]);
            var endAngle = this.AngleForPoint(this.previousPoints[1]);
            return (endAngle - prevAngle) / delay;
        }

        private double AngleForPoint(CGPoint point)
        {
            var center = new CGPoint(this.Frame.Size.Width / 2, this.Frame.Size.Height / 2);
            return Math.Atan2(point.Y - center.Y, point.X - center.X);
        }
    }
}