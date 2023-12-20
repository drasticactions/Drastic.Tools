namespace Drastic.Sidebar;

public interface ISidebarItem
{
    SidebarItemType SidebarItemType { get; }
    
    bool IsEnabled { get; set; }
}

public enum SidebarItemType
{
    Header,
    Row,
}