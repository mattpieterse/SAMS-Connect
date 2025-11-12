using Connect.Data.Models;

namespace Connect.Data.Caches;

public static class ForumCacheExtensions
{
#region LINQ (Filters)

    public static IEnumerable<T> FilterByDateBlanket<T>(
        this IEnumerable<T> collection,
        DateTime? earliestDateInclusive,
        DateTime? furthestDateInclusive
    ) where T : IForumBroadcast {
        if (!earliestDateInclusive.HasValue && !furthestDateInclusive.HasValue)
            return collection;

        return collection.Where(item =>
            (!earliestDateInclusive.HasValue || item.CreatedAt.Date >= earliestDateInclusive.Value.Date) &&
            (!furthestDateInclusive.HasValue || item.CreatedAt.Date <= furthestDateInclusive.Value.Date)
        );
    }


    public static IEnumerable<T> FilterByDepartments<T>(
        this IEnumerable<T> collection,
        params MunicipalDepartment[]? departments
    ) where T : IForumBroadcast {
        if (departments is { Length: 0 } or null)
            return collection;

        var uniqueDepartments = new HashSet<MunicipalDepartment>(departments);
        return collection.Where(item =>
            item.Category.HasValue && uniqueDepartments.Contains(item.Category.Value)
        );
    }


    public static IEnumerable<IForumBroadcast> FilterBySearch(
        this IEnumerable<IForumBroadcast> collection,
        string? searchTerm
    ) {
        return string.IsNullOrWhiteSpace(searchTerm)
            ? collection
            : collection.Where(broadcast =>
                broadcast.Heading.Contains(searchTerm, StringComparison.InvariantCultureIgnoreCase)
            );
    }

#endregion

#region LINQ (Mutator)

    public static IEnumerable<T> OrderByCreated<T>(
        this IEnumerable<T> collection,
        bool descendingList = true
    ) where T : IForumBroadcast {
        return descendingList
            ? collection.OrderByDescending(item => item.CreatedAt)
            : collection.OrderBy(item => item.CreatedAt);
    }


    public static IEnumerable<T> OrderByUpdated<T>(
        this IEnumerable<T> collection,
        bool descendingList = true
    ) where T : IForumBroadcast {
        return descendingList
            ? collection.OrderByDescending(item => item.UpdatedAt)
            : collection.OrderBy(item => item.UpdatedAt);
    }

#endregion

#region Helpers

    private static IEnumerable<T> ApplyFilters<T>(
        this IEnumerable<T> collection,
        DateTime? earliestDateInclusive = null,
        DateTime? furthestDateInclusive = null,
        IEnumerable<MunicipalDepartment>? departments = null
    ) where T : IForumBroadcast {
        var filteredCollection = collection
            .FilterByDateBlanket(earliestDateInclusive, furthestDateInclusive)
            .FilterByDepartments(departments?.ToArray());

        return filteredCollection;
    }

#endregion
}
