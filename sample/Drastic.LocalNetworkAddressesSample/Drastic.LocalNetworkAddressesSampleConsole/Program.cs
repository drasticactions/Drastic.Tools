// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Addresses!");

foreach(var address in Drastic.LocalNetworkAddresses.RemoteNetworkAddresses.GetLocalMachineAddresses())
{
    Console.WriteLine(address);
}