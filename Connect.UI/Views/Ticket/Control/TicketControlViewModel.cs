using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Connect.Data.Caches;
using Connect.UI.Services.Appearance;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using Wpf.Ui.Controls;

namespace Connect.UI.Views.Ticket.Control;

public sealed partial class TicketControlViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();


    [Reactive]
    private ObservableCollection<Data.Models.Ticket> _cachedTickets = [];


    [Reactive]
    private ObservableCollection<Data.Models.Ticket> _searchTickets = [];


    [Reactive]
    private string _searchText = string.Empty;

#endregion

#region Lifecycle

    private readonly TicketCache _ticketCache;
    private readonly IToastService _toastService;


    /// <summary>
    /// Constructor for <see cref="TicketControlViewModel"/>
    /// </summary>
    public TicketControlViewModel(
        TicketCache dbContext,
        IToastService toastService
    ) {
        _ticketCache = dbContext;
        CachedTickets = new ObservableCollection<Data.Models.Ticket>(
            _ticketCache.FetchAll()
        );

        _toastService = toastService;
        _toastService.Warning(
            heading: "Important notice to citizens",
            message:
            $"Currently, tickets for the Department of {MunicipalDepartmentEnumToDisplayStringConverter().Replace(TicketCache.GetHighestPriorityDepartment().ToString(), " $1")} are taking the highest priority in your area and will be addressed first. This is due to increased local demand, and changes to our policies. Thank you for your understanding, and thank you for building a brighter tomorrow with us. Every report you send makes a difference.",
            icon: new SymbolIcon {
                Symbol = SymbolRegular.ArrowTrendingLines24
            }
        );
    }

#endregion

#region Expression

    [
        GeneratedRegex("(\\B[A-Z])")
    ]
    private static partial Regex MunicipalDepartmentEnumToDisplayStringConverter();

#endregion
}
