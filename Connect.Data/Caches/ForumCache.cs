using System.Collections.Concurrent;
using Connect.Data.Models;

namespace Connect.Data.Caches;

public class ForumCache
{
#region Context

    private readonly ConcurrentDictionary<DateTime, IForumBroadcast> _store = [];

    private readonly ConcurrentDictionary<MunicipalDepartment, List<(DateTime timestamp, int weighting)>>
        _departmentFilterHistory = [];

    private readonly HashSet<MunicipalDepartment> _availableDepartments = [];
    private readonly HashSet<MunicipalProvincial> _availableProvincials = [];

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
        if (instance.Category.HasValue)
            _availableDepartments.Add(instance.Category.Value);

        if (instance is ForumEvent { Location: { } location })
            _availableProvincials.Add(location);
    }


    private void HydrateTrackingSets() {
        _availableDepartments.Clear();
        _availableProvincials.Clear();
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


    public void LogUsageOfDeparmentFilter(
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

#region Lifecycle

    public ForumCache() => SeedDevelopmentData();

#endregion

#region Development

    private void SeedDevelopmentData(
        int eventCount = 5,
        int letterCount = 5
    ) {
        var random = new Random();

        string[] sampleHeadings = [
            "System Update",
            "Community Meeting",
            "Policy Change",
            "Urgent Notice",
            "Weekly Newsletter"
        ];

        string[] sampleContents = [
            "Please be advised of the following changes.",
            "Join us for a discussion regarding upcoming projects.",
            "New policies will take effect next month.",
            "Immediate action is required by all departments.",
            "Highlights from this week's activities."
        ];

        var allDepartments = Enum.GetValues(typeof(MunicipalDepartment)).Cast<MunicipalDepartment>().ToArray();
        var allProvincials = Enum.GetValues(typeof(MunicipalProvincial)).Cast<MunicipalProvincial>().ToArray();

        for (var i = 0; i < eventCount; i++) {
            var forumEvent = new ForumEvent {
                Heading = sampleHeadings[random.Next(sampleHeadings.Length)],
                Content = sampleContents[random.Next(sampleContents.Length)],
                Category = allDepartments[random.Next(allDepartments.Length)],
                Location = allProvincials[random.Next(allProvincials.Length)],
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(30)),
                UpdatedAt = DateTime.UtcNow
            };

            Insert(forumEvent);
        }

        for (var i = 0; i < letterCount; i++) {
            var forumLetter = new ForumLetter {
                Heading = sampleHeadings[random.Next(sampleHeadings.Length)],
                Content = sampleContents[random.Next(sampleContents.Length)],
                Category = allDepartments[random.Next(allDepartments.Length)],
                Location = allProvincials[random.Next(allProvincials.Length)],
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(30)),
                UpdatedAt = DateTime.UtcNow
            };

            Insert(forumLetter);
        }
    }

#endregion
}
