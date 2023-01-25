[![NuGet Version](https://img.shields.io/nuget/v/Drastic.Modal.svg)](https://www.nuget.org/packages/Drastic.Modal/) ![License](https://img.shields.io/badge/License-MIT-blue.svg)

# Drastic.Modal

Drastic.Modal are controls for helping create Modal Windows for WinUI.

## How To Use

Implement a new `Window` based on `Drastic.Modal.ModalWindow`

```c#
    public sealed partial class TestModalWindow : ModalWindow
    {
        public TestModalWindow(Window parent, ModalWindowOptions? options = default)
            : base(parent, options: options)
        {
            this.InitializeComponent();

            this.ExtendsContentIntoAppTitleBar(true);
            this.SetTitleBar(this.AppTitleBar);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
```

