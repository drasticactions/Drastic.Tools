// <copyright file="XamCustomViewDialog.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

namespace XamDialogs
{
    /// <summary>
    /// DHDialog with customView
    /// </summary>
    internal class XamCustomViewDialog : XamDialogView
    {
        #region implemented abstract members of DHDialogView

        protected override bool CanSubmit()
        {
            throw new NotImplementedException();
        }

        protected override void HandleCancel()
        {
            throw new NotImplementedException();
        }

        protected override void HandleSubmit()
        {
            throw new NotImplementedException();
        }

        #endregion

        protected override UIKit.UIView ContentView
        {
            get
            {
                var aView = new UIView(new CGRect(0, 0, 320, 240));

                aView.BackgroundColor = UIColor.SystemBackground;

                return aView;
            }
        }


        public XamCustomViewDialog()
            : base(XamDialogType.CustomView)
        {

        }
    }
}