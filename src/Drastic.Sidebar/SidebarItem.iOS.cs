namespace Drastic.Sidebar;

public class SidebarHeaderItem : SidebarItem
{
    public SidebarHeaderItem(string title, List<SidebarItem> items, UIImage? image = default)
        : base(title, image, SidebarItemType.Header)
    {
        this.MenuItems = items;
        this.IsEnabled = false;
    }
    
    public List<SidebarItem> MenuItems { get; }
}

public class SidebarItem : NSObject, ISidebarItem
{
    public SidebarItem(string title, UIImage? image = default)
        : this(title, image, SidebarItemType.Row)
    {
    }

    internal SidebarItem(string title, UIImage? image = default, SidebarItemType sidebarItemType = SidebarItemType.Row)
    {
        this.Title = title;
        this.UIImage = image;
        this.SidebarItemType = sidebarItemType;
        this.IsEnabled = true;
    }
    
    public string Title { get; }

    public UIImage? UIImage { get; }
    
    public bool IsEnabled { get; set; }

    public SidebarItemType SidebarItemType { get; }
}