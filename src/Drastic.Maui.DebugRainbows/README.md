[![NuGet Version](https://img.shields.io/nuget/v/Drastic.Maui.DebugRainbows.svg)](https://www.nuget.org/packages/Drastic.Maui.DebugRainbows/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.Maui.DebugRainbows

Drastic.Maui.DebugRainbows is a fork of Xamarin.Forms.DebugRainbows, a UI Library for helping visualize layout issues. It currently supports iOS, Catalyst, and Android.

<img width="1483" alt="スクリーンショット 2023-03-07 234603" src="https://user-images.githubusercontent.com/898335/223463670-74d38741-b35f-43fe-8b9e-cf170a3cb6a2.png">
<img width="1054" alt="2023-03-07_23 50 34" src="https://user-images.githubusercontent.com/898335/223463719-986277d2-904d-40ad-a01c-d3195017c348.png">
<img width="381" alt="2023-03-07_23 53 53" src="https://user-images.githubusercontent.com/898335/223463729-5963131f-fe4f-4be2-8bd9-40b25fc9178e.png">

## How To Use

Add `AddDebugRainbowsSupport` to your MauiBuilder.

```c#
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.AddDebugRainbowsSupport()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
```

Then, add the following setters to your Application Resources.

```
    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>

            <Style ApplyToDerivedTypes="True" TargetType="ContentPage">
                <Setter Property="debug:DebugRainbow.ShowColors" Value="true"/>
                <Setter Property="debug:DebugRainbow.ShowGrid" Value="false" />
                <Setter Property="debug:DebugRainbow.HorizontalItemSize" Value="20" />
                <Setter Property="debug:DebugRainbow.VerticalItemSize" Value="20" />
                <Setter Property="debug:DebugRainbow.MajorGridLineColor" Value="Red" />
                <Setter Property="debug:DebugRainbow.MajorGridLineInterval" Value="8" />
                <Setter Property="debug:DebugRainbow.MajorGridLineOpacity" Value=".5" />
                <Setter Property="debug:DebugRainbow.MajorGridLineWidth" Value="6" />
                <Setter Property="debug:DebugRainbow.GridLineColor" Value="Red" />
                <Setter Property="debug:DebugRainbow.GridLineOpacity" Value=".5" />
                <Setter Property="debug:DebugRainbow.GridLineWidth" Value="1" />
                <Setter Property="debug:DebugRainbow.GridPadding" Value="0" />
                <Setter Property="debug:DebugRainbow.GridOrigin" Value="Center" />
                <Setter Property="debug:DebugRainbow.MakeGridRainbows" Value="true" />
                <Setter Property="debug:DebugRainbow.Inverse" Value="false" />
            </Style>
        </ResourceDictionary>
    </Application.Resources>
```