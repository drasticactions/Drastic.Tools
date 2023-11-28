namespace Drastic.Sidebar;

public class SidebarListItem
{
    public string Title { get; set; } = string.Empty;
}

public class SidebarListGroupItem
{
    public string Title { get; set; } = string.Empty;

    public List<SidebarItem> Items { get; set; } = new List<SidebarItem>();
}