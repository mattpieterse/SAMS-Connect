using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Connect.UI.Views.Forum;
using Connect.UI.Views.Home;
using Connect.UI.Views.Ticket;
using Microsoft.Extensions.Localization;
using ReactiveUI;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace Connect.UI.Shells;

public sealed partial class ShellViewModel
    : ObservableObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    [ObservableProperty]
    private ObservableCollection<object> _navigationHeaderItems;


    [ObservableProperty]
    private ObservableCollection<object> _navigationFooterItems;

#endregion

#region Lifecycle

    private readonly IStringLocalizer _localizer;


    public ShellViewModel(
        IStringLocalizer localizer
    ) {
        _localizer = localizer;
        NavigationHeaderItems = new ObservableCollection<object>(GetNavigationHeaderItems());
        NavigationFooterItems = new ObservableCollection<object>(GetNavigationFooterItems());
    }

#endregion

#region ICommands

    [RelayCommand]
    private static void ToggleTheme() {
        ApplicationThemeManager.Apply(
            applicationTheme: ApplicationThemeManager.GetAppTheme() == ApplicationTheme.Dark
                ? ApplicationTheme.Light
                : ApplicationTheme.Dark
        );
    }

#endregion

#region Internals

    /// <summary>
    /// Constructs and returns the navigation items for the sidebar header area.
    /// </summary>
    private IEnumerable<object> GetNavigationHeaderItems() {
        return [
            new NavigationViewItem() {
                Content = _localizer["navigation.header.item.home"],
                TargetPageType = typeof(HomeView),
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.Home24
                }
            },
            new NavigationViewItem() {
                Content = _localizer["navigation.header.item.event"],
                TargetPageType = typeof(ForumView),
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.Alert24
                }
            },
            new NavigationViewItem() {
                Content = _localizer["navigation.header.item.issue"],
                TargetPageType = typeof(TicketControlView),
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.PeopleCommunity24
                }
            },
        ];
    }


    /// <summary>
    /// Constructs and returns the navigation items for the sidebar footer area.
    /// </summary>
    private IEnumerable<object> GetNavigationFooterItems() {
        return [
            new NavigationViewItem() {
                Content = _localizer["navigation.footer.item.theme"],
                Command = ToggleThemeCommand,
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.Color24
                }
            },
            new NavigationViewItem() {
                Content = _localizer["navigation.footer.item.about"],
                TargetPageType = typeof(AboutView),
                Icon = new SymbolIcon() {
                    Symbol = SymbolRegular.Info24
                }
            }
        ];
    }

#endregion
}
