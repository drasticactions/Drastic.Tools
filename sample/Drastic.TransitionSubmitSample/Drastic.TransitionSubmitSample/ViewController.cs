using System;
using AnimatedButtons;
using Masonry;

namespace Drastic.TransitionSubmitSample
{
    public class ViewController : UIViewController
    {
        private AnimatedButtons.TransitionSubmitButton button;

        public ViewController()
        {
            this.button = new TransitionSubmitButton();
            this.button.SetTitle("Push Button", UIControlState.Normal);

            this.View!.AddSubview(this.button);

            this.button.MakeConstraints(make => {
                make.Center.EqualTo(this.View);
            });

            this.button.PrimaryActionTriggered += Button_PrimaryActionTriggered;
        }

        private async void Button_PrimaryActionTriggered(object sender, EventArgs e)
        {
            var button = sender as TransitionSubmitButton;

            // only allow a single click
            button.UserInteractionEnabled = false;

            // do some work, while the button animates
            await button.AnimateAsync(async () =>
            {
                // the work ...
                await Task.Delay(1000);
            });

            button.UserInteractionEnabled = true;
        }
    }
}