[![NuGet Version](https://img.shields.io/nuget/v/Drastic.LocalNetworkAddresses.svg)](https://www.nuget.org/packages/Drastic.LocalNetworkAddresses/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.LocalNetworkAddresses

Drastic.LocalNetworkAddresses is a simple library that passes the build machines local IP addresses to your built application.

## How To

- Reference the `Drastic.LocalNetworkAddresses` nuget
- In your application, call `Drastic.LocalNetworkAddresses.RemoteNetworkAddresses.GetLocalMachineAddresses()`
- This will return an array of IP addresses from the host machine.

## How it works

Drastic.LocalNetworkAddresses works by editing the `Drastic.LocalNetworkAddresses.dll` as part of the build process. This is done via `Mono.Cecil` and an MSBuild task that happens whenever you build and deploy your application.

For up to date IPs, you need to build or rebuild your application. If you build on one network connection and deploy, it will still contain your old IP addresses.
