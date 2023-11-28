namespace Drastic.Sidebar.Sample;

public class SampleRootViewController : UISplitViewController
{
    private SidebarUIViewController sidebar;
    private UIViewController testCollectionViewController;
    private UINavigationController navController;

    public SampleRootViewController()
        : base(UISplitViewControllerStyle.DoubleColumn)
    {
        var options = new SidebarUIViewControllerOptions();
        var menu = new SidebarItem("Foobar", new List<SidebarItem>() { new SidebarItem("Hoge"), new SidebarItem("Hoge 2"), new SidebarItem("Hoge 3") });
        var menu2 = new SidebarItem("Foobar 2", new List<SidebarItem>() { new SidebarItem("Hoge 4"), new SidebarItem("Hoge 5"), new SidebarItem("Hoge 6") });
        var menu3 = new SidebarItem("Foobar 3", new List<SidebarItem>() { new SidebarItem("Hoge 7"), new SidebarItem("Hoge 8"), new SidebarItem("Hoge 9") });
        options.MenuItems = new List<SidebarItem>() { menu, menu2, menu3 };
        this.sidebar = new SidebarUIViewController(options);
        this.testCollectionViewController = new BasicViewController();
        this.navController = new UINavigationController(this.testCollectionViewController);
        this.SetViewController(this.sidebar, UISplitViewControllerColumn.Primary);
        this.SetViewController(this.navController, UISplitViewControllerColumn.Secondary);
#if !TVOS
        this.PrimaryBackgroundStyle = UISplitViewControllerBackgroundStyle.Sidebar;
#endif
    }
}

public class BasicViewController : UIViewController
{
    public BasicViewController()
    {
        this.View!.AddSubview(new UILabel(View!.Frame)
        {
#if !TVOS
            BackgroundColor = UIColor.SystemBackground,
#endif
            TextAlignment = UITextAlignment.Center,
            Text = "Hello, Apple!",
            AutoresizingMask = UIViewAutoresizing.All,
        });
    }
}
