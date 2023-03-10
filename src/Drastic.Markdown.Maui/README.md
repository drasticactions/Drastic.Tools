[![NuGet Version](https://img.shields.io/nuget/v/Drastic.Markdown.Maui.svg)](https://www.nuget.org/packages/Drastic.Markdown.Maui/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.Markdown.Maui

Drastic.Markdown.Maui is a fork of [MarkdownView for Xamarin Forms](https://github.com/bares43/MarkdownView). It implements some common Markdown controls using standard MAUI controls. It should work on all supported MAUI platforms.

## How to use

- Install the Nuget
- Reference the control

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:controls="clr-namespace:Drastic.Markdown;assembly=Drastic.Markdown.Maui"
             x:Class="Drastic.Markdown.Maui.Sample.MainPage">
    <ContentPage.Resources>
         <controls:DarkMarkdownTheme x:Key="DarkTheme" />
         <controls:LightMarkdownTheme x:Key="LightTheme" />
    </ContentPage.Resources>
  <ScrollView>
        <controls:MarkdownView Theme="{AppThemeBinding Dark={StaticResource DarkTheme}, Light={StaticResource LightTheme}}" Background="Transparent" Markdown="{Binding Markdown}" />
  </ScrollView>
</ContentPage>
```