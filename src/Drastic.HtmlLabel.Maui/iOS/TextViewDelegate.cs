// <copyright file="TextViewDelegate.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using UIKit;

namespace Drastic.HtmlLabel.Maui
{
    internal class TextViewDelegate : UITextViewDelegate
    {
        private Func<NSUrl, bool> navigateTo;

        public TextViewDelegate(Func<NSUrl, bool> navigateTo)
        {
            this.navigateTo = navigateTo;
        }

        public override bool ShouldInteractWithUrl(UITextView textView, NSUrl URL, NSRange characterRange)
        {
            if (this.navigateTo != null)
            {
                return this.navigateTo(URL);
            }

            return true;
        }
    }
}
