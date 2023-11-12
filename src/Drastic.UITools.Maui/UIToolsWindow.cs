using Microsoft.Extensions.Logging;

namespace Microsoft.Maui.Controls;

public class UIToolsWindow : Window
{
    private ILogger? logger;

    public UIToolsWindow(ILogger? logger = default)
        : base()
    {
        this.logger = logger;
    }

    public UIToolsWindow(Page page, ILogger? logger = default)
        : base(page)
    {
        this.logger = logger;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        this.EnableFPSViewer(this.logger);
    }
}