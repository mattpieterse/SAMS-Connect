using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using Connect.Data.Caches;
using Connect.Data.Models;
using Connect.UI.Models;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Connect.UI.Views.Forum;

public sealed partial class ForumViewModel
    : ReactiveObject, IActivatableViewModel
{
#region Variables

    public ViewModelActivator Activator { get; } = new();
    public ForumViewFilters ListFilters { get; } = new();


    public ReactiveCommand<Unit, Unit> ClearFiltersCommand { get; }


    [Reactive]
    private int _appliedFiltersCount;


    [Reactive]
    private ObservableCollection<ForumBroadcastWrapper> _cachedBroadcasts = [];


    [Reactive]
    private ObservableCollection<IForumBroadcast> _searchBroadcasts = [];


    [Reactive]
    private string _searchText = string.Empty;

#endregion

#region Lifecycle

    private readonly ForumCache _forumCache;


    /// <summary>
    /// Constructor for <see cref="ForumViewModel"/>
    /// </summary>
    public ForumViewModel(
        ForumCache dbContext
    ) {
        _forumCache = dbContext;
        CachedBroadcasts = new ObservableCollection<ForumBroadcastWrapper>(
            _forumCache.FetchAll<IForumBroadcast>()
                .Select(item => new ForumBroadcastWrapper(item, _forumCache.IsRecommended(item)))
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
        observeFilters.BindTo(this, p => p.CachedBroadcasts);
        observeFilters
            .Select(results =>
                new ObservableCollection<IForumBroadcast>(results.Select(item => item.Instance).Take(10))
            )
            .BindTo(this, p => p.SearchBroadcasts);

        ClearFiltersCommand = ReactiveCommand.Create(ListFilters.Clear);

        TrackDepartmentFilterUsage();
    }

#endregion

#region Internals

    private void TrackDepartmentFilterUsage() {
        HashSet<MunicipalDepartment> previousSelection = [];

        ListFilters.SelectedDepartments
            .Throttle(TimeSpan.FromMilliseconds(200))
            .DistinctUntilChanged()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(selectedOptions => {
                var currentSelection = selectedOptions
                    .Where(opt => opt.IsSelected)
                    .Select(opt => opt.Value)
                    .ToHashSet();

                var newlySelected = currentSelection.Except(previousSelection).ToArray();
                if (newlySelected.Length > 0) {
                    _forumCache.LogUsageOfDepartmentFilter(newlySelected);
                }

                previousSelection = currentSelection;
            });
    }


    private IObservable<ObservableCollection<ForumBroadcastWrapper>> ObserveFilters() {
        return ListFilters.SelectedDepartments
            .CombineLatest(
                this.WhenAnyValue(
                    vm => vm.ListFilters.FilterStartDate,
                    vm => vm.ListFilters.FilterFinalDate,
                    vm => vm.SearchText
                ),
                (departments, observables) => (
                    Departments: departments.Select(d => d.Value).ToArray(),
                    EarliestDate: observables.Item1,
                    FurthestDate: observables.Item2,
                    SearchTerm: observables.Item3
                )
            )
            .Throttle(TimeSpan.FromMilliseconds(200))
            .DistinctUntilChanged()
            .Select(parameters => {
                var latest = _forumCache.FetchAll<IForumBroadcast>();
                var result = latest
                    .FilterByDateBlanket(parameters.EarliestDate, parameters.FurthestDate)
                    .FilterByDepartments(parameters.Departments)
                    .FilterBySearch(parameters.SearchTerm)
                    .OrderByCreated();

                var wrappedCollection = result
                    .Select(item => new ForumBroadcastWrapper(item, _forumCache.IsRecommended(item)));

                return new ObservableCollection<ForumBroadcastWrapper>(wrappedCollection);
            })
            .ObserveOn(RxApp.MainThreadScheduler)
            .Publish()
            .RefCount();
    }

#endregion
}
