using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
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
    public TicketControlViewFilters ListFilters { get; } = new();

    public ReactiveCommand<Unit, Unit> ClearFiltersCommand { get; }


    [Reactive]
    private int _appliedFiltersCount;


    [Reactive]
    private ObservableCollection<Data.Models.Ticket> _cachedTickets = [];

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
        _toastService = toastService;
        CachedTickets = new ObservableCollection<Data.Models.Ticket>(
            _ticketCache.FetchAll()
        );

        this.WhenAnyValue(
                p => p.ListFilters.FilterStartDate,
                p => p.ListFilters.FilterFinalDate
            ).CombineLatest(ListFilters.SelectedDepartments, (dates, selectedDepartments) => {
                var count = 0;
                if (dates.Item1 is not null) count++;
                if (dates.Item2 is not null) count++;
                count += selectedDepartments.Count;
                return count;
            }).ObserveOn(RxApp.MainThreadScheduler)
            .BindTo(this, p => p.AppliedFiltersCount);

        var observeFilters = ObserveFilters();
        observeFilters.BindTo(this, p => p.CachedTickets);

        ClearFiltersCommand = ReactiveCommand.Create(ListFilters.Clear);

        ShowToast();
    }

#endregion

#region Internals

    private IObservable<ObservableCollection<Data.Models.Ticket>> ObserveFilters() {
        return ListFilters.SelectedDepartments
            .CombineLatest(
                this.WhenAnyValue(
                    vm => vm.ListFilters.FilterStartDate,
                    vm => vm.ListFilters.FilterFinalDate
                ),
                (departments, dates) => (
                    Departments: departments.Select(d => d.Value).ToArray(),
                    EarliestDate: dates.Item1,
                    FurthestDate: dates.Item2
                )
            )
            .Throttle(TimeSpan.FromMilliseconds(200))
            .DistinctUntilChanged()
            .Select(parameters => {
                IReadOnlyList<Data.Models.Ticket> results;
                var deptFilter = (parameters.Departments.Length > 0);
                var dateFilter = parameters is {
                    EarliestDate: not null,
                    FurthestDate: not null
                };

                switch (deptFilter) {
                case false when !dateFilter: {
                    results = _ticketCache.FetchAll();
                    break;
                }
                case false when dateFilter: {
                    results = _ticketCache.FetchAllFilteredByDepartments(parameters.Departments);
                    break;
                }
                case true when !dateFilter: {
                    results = _ticketCache.FetchAllFilteredByDateBlanket(
                        parameters.EarliestDate,
                        parameters.FurthestDate
                    );
                    break;
                }
                default: {
                    var deptsFiltered = _ticketCache
                        .FetchAllFilteredByDepartments(parameters.Departments)
                        .ToHashSet();

                    var datesFiltered = _ticketCache.FetchAllFilteredByDateBlanket(
                        parameters.EarliestDate,
                        parameters.FurthestDate
                    );

                    results = deptsFiltered
                        .Intersect(datesFiltered)
                        .ToList()
                        .AsReadOnly();

                    break;
                }
                }

                return new ObservableCollection<Data.Models.Ticket>(results);
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Publish()
            .RefCount();
    }


    private void ShowToast() {
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
