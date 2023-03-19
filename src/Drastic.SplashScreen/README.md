[![NuGet Version](https://img.shields.io/nuget/v/Drastic.SplashScreen.svg)](https://www.nuget.org/packages/Drastic.SplashScreen/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.SplashScreen

Drastic.SplashScreen is a simple Twitter-style Splash Screen view for iOS, Catalyst, and tvOS. It was ported from [MSTwitterSplashScreen](https://github.com/mateuszszklarek/MSTwitterSplashScreen).

![test](https://user-images.githubusercontent.com/898335/226174108-fa59f98a-6ca8-4161-bfb2-f25d15c0173a.gif)

# How To:

- Install the Drastic.SplashScreen Nuget
- Add the `SplashScreen` to a UIViewController

```csharp
        public TestViewController(CGRect frame)
        {
            this.splashScreen = new Drastic.SplashScreen.SplashScreen(this.TwitterPath(), UIColor.Blue, UIColor.White);
            this.splashScreen.DurationAnimation = 2f;
            this.View!.AddSubview(this.splashScreen);
        }
```

- The `SplashScreen` takes a `UIBezierPath`. There is an example in the sample.
- Invoke `SplashScreen.StartAnimation` to play out the animation.

```csharp
        public override void ViewDidAppear(bool animated)
        {
            this.splashScreen.StartAnimation();
            base.ViewDidAppear(animated);
        }
```

