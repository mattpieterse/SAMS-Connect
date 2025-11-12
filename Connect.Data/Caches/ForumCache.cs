using System.Collections.Concurrent;
using Connect.Data.Caches.Development;
using Connect.Data.Models;

namespace Connect.Data.Caches;

public class ForumCache
{
#region Context

    private readonly ConcurrentDictionary<DateTime, IForumBroadcast> _store = ForumCacheSeeder.Init();

    private readonly ConcurrentDictionary<MunicipalDepartment, List<(DateTime timestamp, int weighting)>>
        _departmentFilterHistory = [];

    private readonly HashSet<MunicipalDepartment> _availableDepartments = [];
    private readonly HashSet<MunicipalProvincial> _availableProvincials = [];
    private readonly HashSet<DateOnly> _uniqueDates = [];

    private MunicipalDepartment? _recommendedCategory;

#endregion

#region Queries

    /// <summary>
    /// CRUD Function.
    /// </summary>
    public IReadOnlyList<IForumBroadcast> FetchAll() {
        var collection = _store.Values.ToList().AsReadOnly();
        return collection;
    }


    /// <summary>
    /// CRUD Function.
    /// </summary>
    public IReadOnlyList<T> FetchAll<T>() where T : IForumBroadcast {
        var collection = _store.Values.OfType<T>().ToList().AsReadOnly();
        return collection;
    }


    /// <summary>
    /// CRUD Function.
    /// </summary>
    public void Insert(IForumBroadcast instance) {
        if (!_store.TryAdd(instance.CreatedAt, instance)) return;
        UpdateTrackingSets(instance);
    }


    /// <summary>
    /// CRUD Function.
    /// </summary>
    public void Update(IForumBroadcast instance) {
        if (!_store.ContainsKey(instance.CreatedAt)) return;
        _store[instance.CreatedAt] = instance;
        HydrateTrackingSets();
    }


    /// <summary>
    /// CRUD Function.
    /// </summary>
    public void Delete(DateTime key) {
        if (_store.TryRemove(key, out _)) {
            HydrateTrackingSets();
        }
    }

#endregion

#region Functions

    /// <summary>
    /// Queries the <see cref="MunicipalDepartment"/> array that actually appear
    /// within items in the cache. This should be used for filtering UI, so that
    /// only valid items are rendered.
    /// </summary>
    /// <returns>
    /// <see cref="IReadOnlySet{MunicipalDepartment}"/>
    /// </returns>
    public IReadOnlySet<MunicipalDepartment> GetAvailableDepartments() => _availableDepartments;


    /// <summary>
    /// Queries the <see cref="MunicipalProvincial"/> array that actually appear
    /// within items in the cache. This should be used for filtering UI, so that
    /// only valid items are rendered.
    /// </summary>
    /// <returns>
    /// <see cref="IReadOnlySet{MunicipalProvincial}"/>
    /// </returns>
    public IReadOnlySet<MunicipalProvincial> GetAvailableProvincials() => _availableProvincials;


    public MunicipalDepartment? GetRecommendedCategory() => _recommendedCategory;


    public bool IsRecommended(IForumBroadcast instance) {
        return (
            _recommendedCategory.HasValue &&
            instance.Category.HasValue &&
            instance.Category.Value == _recommendedCategory.Value
        );
    }

#endregion

#region Internals

    private static int CalculateWeight(DateTime timestamp) {
        var age = DateTime.UtcNow - timestamp;
        return (age.TotalDays) switch {
            <= 07 => 3,
            <= 30 => 1,
            _ => 0
        };
    }


    private void UpdateTrackingSets(IForumBroadcast instance) {
        _uniqueDates.Add(DateOnly.FromDateTime(instance.CreatedAt));

        if (instance.Category.HasValue)
            _availableDepartments.Add(instance.Category.Value);

        if (instance is ForumEvent { Location: { } location })
            _availableProvincials.Add(location);
    }


    private void HydrateTrackingSets() {
        _availableDepartments.Clear();
        _availableProvincials.Clear();
        _uniqueDates.Clear();

        foreach (var instance in _store.Values) {
            UpdateTrackingSets(instance);
        }
    }


    private void UpdateRecommendedCategory() {
        if (_departmentFilterHistory.IsEmpty) {
            _recommendedCategory = null;
            return;
        }

        var scores = new Dictionary<MunicipalDepartment, int>();
        foreach (var (department, history) in _departmentFilterHistory) {
            var score = history
                .Select(h => CalculateWeight(h.timestamp))
                .Sum();

            if (score > 0)
                scores[department] = score;

            _recommendedCategory = (scores.Count) switch {
                > 0 => scores.OrderByDescending(i => i.Value).First().Key,
                _ => null
            };
        }
    }


    public void LogUsageOfDepartmentFilter(
        IEnumerable<MunicipalDepartment> departments
    ) {
        var now = DateTime.UtcNow;
        foreach (var usage in departments) {
            if (!_departmentFilterHistory.TryGetValue(usage, out var value)) {
                value = [];
                _departmentFilterHistory[usage] = value;
            }

            value.Add((now, CalculateWeight(now)));
        }

        UpdateRecommendedCategory();
    }

#endregion
}
