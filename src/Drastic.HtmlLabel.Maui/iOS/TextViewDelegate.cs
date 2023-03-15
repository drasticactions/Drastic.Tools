using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;

namespace Drastic.HtmlLabel.Maui
{
    internal class TextViewDelegate : UITextViewDelegate
    {
        private Func<NSUrl, bool> _navigateTo;

        public TextViewDelegate(Func<NSUrl, bool> navigateTo)
        {
            _navigateTo = navigateTo;
        }

        public override bool ShouldInteractWithUrl(UITextView textView, NSUrl URL, NSRange characterRange)
        {
            if (_navigateTo != null)
            {
                return _navigateTo(URL);
            }
            return true;
        }
    }

}
