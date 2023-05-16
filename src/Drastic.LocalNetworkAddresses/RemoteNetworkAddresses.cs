// <copyright file="RemoteNetworkAddresses.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace Drastic.LocalNetworkAddresses
{
    public static class RemoteNetworkAddresses
    {
        public static string[] GetLocalMachineAddresses()
        {
            var addresses = "";

            if (string.IsNullOrEmpty(addresses))
            {
                return new string[0];
            }

            return addresses.Split(';');
        }
    }
}
