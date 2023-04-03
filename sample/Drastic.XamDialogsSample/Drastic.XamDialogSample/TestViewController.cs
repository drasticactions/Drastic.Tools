using System;
using XamDialogs;
#nullable disable

namespace Drastic.XamDialogSample
{
    public class TestViewController : UIViewController
    {
        public TestViewController()
        {
            var button = new UIButton(this.View!.Frame) { AutoresizingMask = UIViewAutoresizing.All };
            button.SetTitle("Open Dialog", UIControlState.Normal);

            this.View!.AddSubview(button);

            button.PrimaryActionTriggered += Button_PrimaryActionTriggered;
        }

        private void Button_PrimaryActionTriggered(object sender, EventArgs e)
        {
            this.ShowSimplePicker();
        }

        private void ShowDatePicker()
        {

            var dialog = new XamDatePickerDialog(UIDatePickerMode.DateAndTime)
            {
                Title = "Date Picker",
                Message = "Please Pick a date and time",
                BlurEffectStyle = UIBlurEffectStyle.Dark,
                CancelButtonText = "Cancel",
                ConstantUpdates = false,
            };

            dialog.SelectedDate = new DateTime(1969, 7, 20, 20, 18, 00, 00);

            dialog.ButtonMode = ButtonMode.OkAndCancel;

            dialog.ValidateSubmit = (DateTime data) =>
            {
                return true;
            };

            dialog.OnSelectedDateChanged += (object s, DateTime e) =>
            {
                Console.WriteLine(e);
            };

            dialog.Show(this);

            // Static methods
            //			var result = await XamDatePickerDialog.ShowDialogAsync(UIDatePickerMode.DateAndTime,"Date of Birth","Select your Date of Birth", new DateTime(1969,7,20,20,18,00,00) );
            //
            //			Console.WriteLine(result);
        }

        private void ShowSimplePicker()
        {
            var dialog = new XamSimplePickerDialog(new List<String>() { "Ringo", "John", "Paul", "George" })
            {
                Title = "Favorite Beatle",
                Message = "Pick your favorite beatle",
                BlurEffectStyle = UIBlurEffectStyle.Dark,
                CancelButtonText = "Cancel",
                ConstantUpdates = false,
            };

            dialog.OnSelectedItemChanged += (object s, string e) =>
            {
                Console.WriteLine(e);
            };

            dialog.SelectedItem = "John";

            dialog.Show(this);

            // Static methods
            //			var result = await XamSimplePickerDialog.ShowDialogAsync("Who are you?","Select your name", new List<String>(){"Dave","Rob","Jamie"}, "Rob");
            //			Console.WriteLine(result);
        }
    }
}