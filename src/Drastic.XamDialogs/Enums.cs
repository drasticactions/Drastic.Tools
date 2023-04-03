// <copyright file="Enums.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace XamDialogs
{


    public enum DatetimeFormat
    {
        AM_PM,
        FULL
    }

    /// <summary>
    /// Type of dialog
    /// </summary>
    public enum XamDialogType
    {

        PlainTextInput,
        NumberInput,
        PhoneNumberInput,
        EmailInput,
        SecureTextInput,
        LoginAndPasswordInput,
        CustomView,
        DatePicker,
        PickerView,
    }

    /// <summary>
    /// Button mode.
    /// </summary>
    public enum ButtonMode
    {
        OkAndCancel,
        Ok,
        Cancel,
    }
}
