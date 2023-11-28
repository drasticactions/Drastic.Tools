namespace Drastic.Sidebar;

public class SidebarItem : NSObject
{
    public SidebarItem(string title)
    {
        this.Title = title;
        this.SidebarItemType = SidebarItemType.Row;
    }

    public SidebarItem(string title, List<SidebarItem> items)
    {
        this.Title = title;
        this.MenuItems = items;
        this.SidebarItemType = SidebarItemType.Header;
    }

    public string Title { get; }

    public UIImage? Image { get; }

    public List<SidebarItem> MenuItems { get; } = new();

    public SidebarItemType SidebarItemType { get; }
}

public enum SidebarItemType
{
    Unknown,
    Header,
    Row,
}