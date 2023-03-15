// <copyright file="BaseTextViewRenderer.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Drastic.HtmlLabel.Maui;
using Foundation;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using Microsoft.Maui.Controls.Platform;
using UIKit;
using NativeTextView = UIKit.UITextView;

namespace Drastic.HtmlLabel.Maui
{
    public abstract class BaseTextViewRenderer<TElement> : ViewRenderer<TElement, NativeTextView>
        where TElement : Label
    {
        private SizeRequest perfectSize;
        private bool perfectSizeValid;

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            if (!this.perfectSizeValid)
            {
                this.perfectSize = base.GetDesiredSize(double.PositiveInfinity, double.PositiveInfinity);
                this.perfectSize.Minimum = new Size(Math.Min(10, this.perfectSize.Request.Width), this.perfectSize.Request.Height);
                this.perfectSizeValid = true;
            }

            var widthFits = widthConstraint >= this.perfectSize.Request.Width;
            var heightFits = heightConstraint >= this.perfectSize.Request.Height;

            if (widthFits && heightFits)
            {
                return this.perfectSize;
            }

            var result = base.GetDesiredSize(widthConstraint, heightConstraint);
            var tinyWidth = Math.Min(10, result.Request.Width);
            result.Minimum = new Size(tinyWidth, result.Request.Height);

            if (widthFits || this.Element.LineBreakMode == LineBreakMode.NoWrap)
            {
                return result;
            }

            var containerIsNotInfinitelyWide = !double.IsInfinity(widthConstraint);

            if (containerIsNotInfinitelyWide)
            {
                var textCouldHaveWrapped = this.Element.LineBreakMode == LineBreakMode.WordWrap || this.Element.LineBreakMode == LineBreakMode.CharacterWrap;
                var textExceedsContainer = result.Request.Width > widthConstraint;

                if (textExceedsContainer || textCouldHaveWrapped)
                {
                    var expandedWidth = Math.Max(tinyWidth, widthConstraint);
                    result.Request = new Size(expandedWidth, result.Request.Height);
                }
            }

            return result;
        }

        protected override NativeTextView CreateNativeControl()
        {
            var control = new NativeTextView(CGRect.Empty)
            {
                Editable = false,
                ScrollEnabled = false,
                ShowsVerticalScrollIndicator = false,
                BackgroundColor = UIColor.Clear,
            };
            return control;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TElement> e)
        {
            this.perfectSizeValid = false;

            if (e.NewElement != null)
            {
                try
                {
                    if (this.Control == null)
                    {
                        this.SetNativeControl(this.CreateNativeControl());
                    }

                    bool shouldInteractWithUrl = !this.Element.GestureRecognizers.Any();
                    if (shouldInteractWithUrl)
                    {
                        // Setting the data detector types mask to capture all types of link-able data
                        this.Control.DataDetectorTypes = UIDataDetectorType.All;
                        this.Control.Selectable = true;
                        this.Control.Delegate = new TextViewDelegate(this.NavigateToUrl);
                    }
                    else
                    {
                        this.Control.Selectable = false;
                        foreach (var recognizer in this.Element.GestureRecognizers.OfType<TapGestureRecognizer>())
                        {
                            if (recognizer.Command != null)
                            {
                                var command = recognizer.Command;
                                this.Control.AddGestureRecognizer(new UITapGestureRecognizer(() => { command.Execute(recognizer.CommandParameter); }));
                            }
                        }
                    }

                    this.UpdateLineBreakMode();
                    this.UpdateHorizontalTextAlignment();
                    this.ProcessText();
                    this.UpdatePadding();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                }
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e != null && RendererHelper.RequireProcess(e.PropertyName))
            {
                try
                {
                    this.UpdateLineBreakMode();
                    this.UpdateHorizontalTextAlignment();
                    this.ProcessText();
                    this.UpdatePadding();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"            ERROR: ", ex.Message);
                }
            }
        }

        protected abstract void ProcessText();

        protected abstract bool NavigateToUrl(NSUrl url);

        private void UpdateLineBreakMode()
        {
            switch (this.Element.LineBreakMode)
            {
                case LineBreakMode.NoWrap:
                    this.Control.TextContainer.LineBreakMode = UILineBreakMode.Clip;
                    break;
                case LineBreakMode.WordWrap:
                    this.Control.TextContainer.LineBreakMode = UILineBreakMode.WordWrap;
                    break;
                case LineBreakMode.CharacterWrap:
                    this.Control.TextContainer.LineBreakMode = UILineBreakMode.CharacterWrap;
                    break;
                case LineBreakMode.HeadTruncation:
                    this.Control.TextContainer.LineBreakMode = UILineBreakMode.HeadTruncation;
                    break;
                case LineBreakMode.MiddleTruncation:
                    this.Control.TextContainer.LineBreakMode = UILineBreakMode.MiddleTruncation;
                    break;
                case LineBreakMode.TailTruncation:
                    this.Control.TextContainer.LineBreakMode = UILineBreakMode.TailTruncation;
                    break;
            }
        }

        private void UpdatePadding()
        {
            this.Control.TextContainerInset = new UIEdgeInsets(
                (float)this.Element.Padding.Top,
                (float)this.Element.Padding.Left,
                (float)this.Element.Padding.Bottom,
                (float)this.Element.Padding.Right);

            this.UpdateLayout();
        }

        private void UpdateLayout()
        {
            this.LayoutSubviews();
        }

        private void UpdateHorizontalTextAlignment()
        {
            this.Control.TextAlignment = this.Element.HorizontalTextAlignment.ToNativeTextAlignment(((IVisualElementController)this.Element).EffectiveFlowDirection);
        }
    }
}
