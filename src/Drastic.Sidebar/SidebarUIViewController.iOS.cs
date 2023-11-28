using ObjCRuntime;

namespace Drastic.Sidebar;

public class SidebarUIViewControllerOptions
{
    public List<SidebarItem> MenuItems { get; set; } = new List<SidebarItem>();
    
    public UICollectionViewLayout? Layout { get; set; }
}

public class SidebarSelectionEventArgs : EventArgs
{
    private readonly SidebarItem item;

    /// <summary>
    /// Initializes a new instance of the <see cref="SidebarSelectionEventArgs"/> class.
    /// </summary>
    /// <param name="item">Item.</param>
    public SidebarSelectionEventArgs(SidebarItem item)
    {
        this.item = item;
    }

    /// <summary>
    /// Gets the Item.
    /// </summary>
    public SidebarItem Item => this.item;
}

public class SidebarUIViewController : UIViewController, IUICollectionViewDelegate
{
    private UICollectionView collectionView;
    
    private UICollectionViewDiffableDataSource<NSString, SidebarItem>? dataSource;

    private SidebarUIViewControllerOptions options;
    
    public SidebarUIViewController(SidebarUIViewControllerOptions options)
    {
        ArgumentNullException.ThrowIfNull(this.View);
        this.options = options;
        this.collectionView = new UICollectionView(this.View.Bounds, options.Layout ?? this.CreateLayout());
        this.collectionView.Delegate = this;

        this.View.AddSubview(this.collectionView);

        // Anchor collectionView
        this.collectionView.TranslatesAutoresizingMaskIntoConstraints = false;

        // Create constraints to pin the edges of myView to its superview's edges
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            this.collectionView.TopAnchor.ConstraintEqualTo(this.View.TopAnchor),
            this.collectionView.BottomAnchor.ConstraintEqualTo(this.View.BottomAnchor),
            this.collectionView.LeadingAnchor.ConstraintEqualTo(this.View.LeadingAnchor),
            this.collectionView.TrailingAnchor.ConstraintEqualTo(this.View.TrailingAnchor),
        });

        this.ConfigureDataSource();

        foreach (var item in options.MenuItems)
        {
            this.SetupNavigationItems(this.GetNavigationSnapshot(item));
        }
    }
    
    public event EventHandler<SidebarSelectionEventArgs>? OnItemSelected;
    
    /// <summary>
    /// Item Selected.
    /// </summary>
    /// <param name="collectionView">Collection View.</param>
    /// <param name="indexPath">Index Path.</param>
    [Export("collectionView:didSelectItemAtIndexPath:")]
    protected async void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
    {
        var item = this.dataSource?.GetItemIdentifier(indexPath);
        if (item is not null)
            this.OnItemSelected?.Invoke(this, new SidebarSelectionEventArgs(item));
    }

    /// <inheritdoc/>
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        if (this.View is null)
        {
            throw new NullReferenceException(nameof(this.View));
        }
    }
    
    private class CustomCell : UICollectionViewListCell
    {
        public CustomCell()
        {
        }

        public CustomCell(NSCoder coder) : base(coder)
        {
        }

        public CustomCell(CGRect frame) : base(frame)
        {
        }

        protected CustomCell(NSObjectFlag t) : base(t)
        {
        }

        protected internal CustomCell(NativeHandle handle) : base(handle)
        {
        }

    }
    
    private void ConfigureDataSource()
    {
        var headerRegistration = UICollectionViewCellRegistration.GetRegistration(
            typeof(UICollectionViewListCell),
            new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
            {
                var sidebarItem = (SidebarItem)item;
                var contentConfiguration = UIListContentConfiguration.SidebarHeaderConfiguration;
                contentConfiguration.Text = sidebarItem.Title;
                contentConfiguration.TextProperties.Font = UIFont.PreferredSubheadline;
                contentConfiguration.TextProperties.Color = UIColor.SecondaryLabel;
                cell.ContentConfiguration = contentConfiguration;
                ((UICollectionViewListCell)cell).Accessories = new[] { new UICellAccessoryOutlineDisclosure() };
            }));
        
        var rowRegistration = UICollectionViewCellRegistration.GetRegistration(typeof(CustomCell),
            new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
            {
                var sidebarItem = item as SidebarItem;
                if (sidebarItem is null)
                {
                    return;
                }

#if TVOS
                    var cfg = UIListContentConfiguration.CellConfiguration;
#else
                var cfg = UIListContentConfiguration.SidebarCellConfiguration;
#endif
                cfg.Text = sidebarItem.Title;
                // if (sidebarItem.SystemIcon is not null)
                // {
                //     cfg.Image = UIImage.GetSystemImage(sidebarItem.SystemIcon);
                // }

                cell.ContentConfiguration = cfg;
            })
        );

        if (this.collectionView is null)
        {
            throw new NullReferenceException(nameof(this.collectionView));
        }

        this.dataSource = new UICollectionViewDiffableDataSource<NSString, SidebarItem>(this.collectionView,
            new UICollectionViewDiffableDataSourceCellProvider((collectionView, indexPath, item) =>
            {
                var sidebarItem = item as SidebarItem;
                if (sidebarItem is null || collectionView is null)
                {
                    throw new Exception();
                }

                if (sidebarItem.SidebarItemType == SidebarItemType.Header)
                {
                    return collectionView.DequeueConfiguredReusableCell(headerRegistration, indexPath, item);
                }
                else
                {
                    return collectionView.DequeueConfiguredReusableCell(rowRegistration, indexPath, item);
                }
            })
        );
    }

    private void SetupNavigationItems(NSDiffableDataSourceSectionSnapshot<SidebarItem> snapshot)
    {
        if (this.dataSource is null)
        {
            return;
        }

        var sectionIdentifier = new NSString(Guid.NewGuid().ToString());
        this.dataSource.ApplySnapshot(snapshot, sectionIdentifier, false);
    }

    private NSDiffableDataSourceSectionSnapshot<SidebarItem> GetNavigationSnapshot(SidebarItem item)
    {
        var snapshot = new NSDiffableDataSourceSectionSnapshot<SidebarItem>();

        snapshot.AppendItems(new [] { item });
        snapshot.ExpandItems(new [] { item });
        snapshot.AppendItems(item.MenuItems.ToArray(), item);
        
        return snapshot;
    }

    private UICollectionViewLayout CreateLayout()
    {
        return new UICollectionViewCompositionalLayout((sectionIndex, layoutEnvironment) =>
        {
            var configuration = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Sidebar);
            configuration.ShowsSeparators = false;
            configuration.HeaderMode = UICollectionLayoutListHeaderMode.FirstItemInSection;
            return NSCollectionLayoutSection.GetSection(configuration, layoutEnvironment);
        });
    }
}