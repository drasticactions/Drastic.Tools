// <copyright file="SetRemoteNetworkSettings.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Drastic.LocalNetworkAddresses
{
    public class SetRemoteNetworkSettings : Microsoft.Build.Utilities.Task
    {
        public string DllPath { get; set; } = string.Empty;

        public override bool Execute()
        {
            Log.LogMessage("Setting Remote Network Settings...");
            var dllPath = Path.Combine(Path.GetDirectoryName(DllPath), "Drastic.LocalNetworkAddresses.dll") ?? string.Empty;

            if (!File.Exists(dllPath))
            {
                // Stop right away.
                Log.LogMessage($"{dllPath} does not exist, skipping...");
                return true;
            }

            var ogDllPath = Path.Combine(Path.GetDirectoryName(dllPath)!, Path.GetFileNameWithoutExtension(dllPath) + "-original.dll");
            File.Copy(dllPath, ogDllPath, true);

            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(ogDllPath);
            Log.LogMessage("Got Assembly");
            TypeDefinition type = assembly.MainModule.Types.Single(t => t.FullName == "Drastic.LocalNetworkAddresses.RemoteNetworkAddresses");
            Log.LogMessage("Got Type");
            MethodDefinition method = type.Methods.Single(m => m.Name == "GetLocalMachineAddresses");
            Log.LogMessage("Got Method");
            Instruction emptyStringAssignment = method.Body.Instructions
                .Single(i => i.OpCode == OpCodes.Ldstr);
            Log.LogMessage("Got Assignment");
            emptyStringAssignment.Operand = string.Join(";", DeviceIps);
            Log.LogMessage("Setting Assignment");
            assembly.Write(dllPath);
            Log.LogMessage("Writing Assembly");
            assembly.Dispose();
            File.Delete(ogDllPath);
            Log.LogMessage("Done!");

            return true;
        }

        static IEnumerable<string> DeviceIps =>
    GoodInterfaces()
    .SelectMany(x =>
        x.GetIPProperties().UnicastAddresses
            .Where(y => y.Address.AddressFamily == AddressFamily.InterNetwork)
            .Select(y => y.Address.ToString())).OrderBy(x => x);

        static IEnumerable<NetworkInterface> GoodInterfaces()
        {
            var allInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            return allInterfaces.Where(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                            !x.Name.StartsWith("pdp_ip", StringComparison.Ordinal) &&
                                            x.OperationalStatus == OperationalStatus.Up);
        }
    }
}
