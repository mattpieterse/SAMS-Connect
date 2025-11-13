using System.Text.RegularExpressions;
using Connect.Data.Caches.Algorithms;
using Connect.Data.Models;

namespace Connect.Data.Caches;

public sealed class TicketCache
{
#region Context

    private readonly SortedDictionary<(DateTime CreatedAt, Guid Id), Ticket> _store = BuildSortedCompositeContext();
    private readonly (
        BipartiteGraph<MunicipalDepartment, Ticket> Department,
        BipartiteGraph<DateOnly, Ticket> CreatedAtDateOnly,
        BipartiteGraph<DateOnly, Ticket> UpdatedAtDateOnly,
        DateOnlyTree CreatedAtDateOnlyBst,
        DateOnlyTree UpdatedAtDateOnlyBst
        ) _indexes = (
            new BipartiteGraph<MunicipalDepartment, Ticket>(),
            new BipartiteGraph<DateOnly, Ticket>(),
            new BipartiteGraph<DateOnly, Ticket>(),
            new DateOnlyTree(),
            new DateOnlyTree()
        );

#endregion

#region Queries

    /// <summary>
    /// CRUD Function.
    /// </summary>
    public IReadOnlyList<Ticket> FetchAll() {
        var collection = _store.Values.ToList().AsReadOnly();
        return collection;
    }


    public IReadOnlyList<Ticket> FetchAllFilteredByDateBlanket(
        DateTime? earliestDateInclusive,
        DateTime? furthestDateInclusive
    ) {
        if (!earliestDateInclusive.HasValue && !furthestDateInclusive.HasValue) {
            return _store.Values.ToList().AsReadOnly();
        }

        // Usage of b-graph indexes for O(1)

        if (earliestDateInclusive == furthestDateInclusive) {
            return _indexes.CreatedAtDateOnly
                .LookupChildNodesOf(DateOnly.FromDateTime((DateTime) earliestDateInclusive!))
                .ToList()
                .AsReadOnly();
        }

        // Usage of AVL-BST indexes for O(log(n) + k)

        var earliestToken = earliestDateInclusive.HasValue
            ? DateOnly.FromDateTime((DateTime) earliestDateInclusive)
            : DateOnly.MinValue;

        var furthestToken = furthestDateInclusive.HasValue
            ? DateOnly.FromDateTime((DateTime) furthestDateInclusive)
            : DateOnly.MaxValue;

        var results = _indexes.CreatedAtDateOnlyBst
            .Search(earliestToken, furthestToken)
            .ToList()
            .AsReadOnly();

        return results;
    }


    public IReadOnlyList<Ticket> FetchAllFilteredByDepartments(
        params MunicipalDepartment[]? filters
    ) {
        switch (filters) {
        case { Length: 0 } or null: {
            return _store.Values.ToList().AsReadOnly();
        }
        case { Length: 1 }: {
            return _indexes.Department.LookupChildNodesOf(filters[0]).ToList().AsReadOnly();
        }
        }

        var unique = new HashSet<MunicipalDepartment>(filters);
        var result = unique
            .SelectMany(department => _indexes.Department.LookupChildNodesOf(department))
            .Distinct()
            .ToList()
            .AsReadOnly();

        return result;
    }


    /// <summary>
    /// CRUD Function.
    /// </summary>
    public void Insert(
        Ticket instance
    ) {
        _store.TryAdd((instance.CreatedAt, instance.Id), instance);
        _indexes.CreatedAtDateOnly.InsertEdge(DateOnly.FromDateTime(instance.CreatedAt), instance);
        _indexes.UpdatedAtDateOnly.InsertEdge(DateOnly.FromDateTime(instance.UpdatedAt), instance);
        _indexes.CreatedAtDateOnlyBst.Insert(instance);
        _indexes.UpdatedAtDateOnlyBst.Insert(instance);
        if (instance.Category is { } department) {
            _indexes.Department.InsertEdge(department, instance);
        }
    }


    public DateOnlyTree GetCreatedAtDateOnlyBstIndex() =>
        _indexes.CreatedAtDateOnlyBst;


    public DateOnlyTree GetUpdatedAtDateOnlyBstIndex() =>
        _indexes.UpdatedAtDateOnlyBst;


    public BipartiteGraph<MunicipalDepartment, Ticket> GetDepartmentIndex() =>
        _indexes.Department;


    public BipartiteGraph<DateOnly, Ticket> GetCreatedAtDateOnlyIndex() =>
        _indexes.CreatedAtDateOnly;


    public BipartiteGraph<DateOnly, Ticket> GetUpdatedAtDateOnlyIndex() =>
        _indexes.UpdatedAtDateOnly;


    public static MunicipalDepartment GetHighestPriorityDepartment() {
        var customHeap =
            new MaxHeap<(MunicipalDepartment department, int priority)>((x, y) => x.priority.CompareTo(y.priority));

        foreach (MunicipalDepartment department in Enum.GetValues(typeof(MunicipalDepartment))) {
            var priority = new Random().Next(1, 101);
            customHeap.Insert((department, priority));
        }

        var topDepartment = customHeap.ExtractMax();
        return topDepartment.department;
    }

#endregion

#region Lifecycle

    public TicketCache() { }

#endregion

#region Internals

    /// <summary>
    /// Descending comparer for the composite key on <see cref="_store"/>.
    /// <br />
    /// Items in the collection will always be sorted newest first based on each
    /// items time of addition. If a conflict arises, the comparer will fallback
    /// to using the <see cref="Guid"/> to determine a more fine-grained time of
    /// creation from the entity itself, as a resolution.
    /// </summary>
    /// <returns>
    /// An empty collection with a custom descending comparer.
    /// </returns>
    private static SortedDictionary<(DateTime CreatedAt, Guid Id), Ticket> BuildSortedCompositeContext() {
        return new SortedDictionary<(DateTime CreatedAt, Guid Id), Ticket>(
            Comparer<(DateTime, Guid)>.Create((x, y) => {
                var result = y.Item1.CompareTo(x.Item1);
                return result == 0
                    ? x.Item2.CompareTo(y.Item2)
                    : result;
            })
        );
    }

#endregion
}
