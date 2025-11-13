namespace Connect.Data.Caches.Algorithms;

public sealed class BipartiteGraph<TGroup, TChild>
    where TGroup : notnull
    where TChild : notnull
{
#region Variables

    private readonly Dictionary<TGroup, HashSet<TChild>> _groupToChildren = [];

#endregion

#region Functions

    /// <summary>
    /// Traverses the graph according to the given <see cref="SearchMode"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// When an unsupported <see cref="SearchMode"/> is provided.
    /// </exception>
    /// <seealso cref="SearchMode.Bfs"/>
    /// <seealso cref="SearchMode.Dfs"/>
    public IEnumerable<TChild> Search(SearchMode mode, TGroup group) {
        return (mode) switch {
            SearchMode.Bfs => Bfs(group),
            SearchMode.Dfs => Dfs(group),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }


    /// <summary>
    /// Queries a direct lookup of <see cref="TChild"/> nodes in the graph that
    /// are connected to the given <see cref="TGroup"/> node. This method won't
    /// traverse the graph at all, it directly looks up the values in the
    /// collection for efficiency.
    /// </summary>
    public IEnumerable<TChild> LookupChildNodesOf(TGroup group) {
        return _groupToChildren.TryGetValue(group, out var children)
            ? children
            : Enumerable.Empty<TChild>();
    }


    /// <summary>
    /// Adds a <see cref="TChild"/> node within the given <see cref="TGroup"/>.
    /// </summary>
    public void InsertEdge(TGroup group, TChild child) {
        if (!_groupToChildren.TryGetValue(group, out _))
            _groupToChildren[group] = [];

        _groupToChildren[group].Add(child);
    }

#endregion

#region Internals

    /// <summary>
    /// Yields all <see cref="TChild"/> in the collection connected to the given
    /// <see cref="TGroup"/> nodes.
    /// </summary>
    private IEnumerable<TChild> Bfs(TGroup group) {
        var visited = new HashSet<TChild>();
        var remains = new Queue<TChild>();

        if (_groupToChildren.TryGetValue(group, out var children)) {
            foreach (var instance in children)
                remains.Enqueue(instance);

            while (remains.Count > 0) {
                var node = remains.Dequeue();
                if (visited.Add(node))
                    yield return node;
            }
        }
    }


    /// <summary>
    /// Yields all <see cref="TChild"/> in the collection connected to the given
    /// <see cref="TGroup"/> nodes.
    /// </summary>
    private IEnumerable<TChild> Dfs(TGroup group) {
        var visited = new HashSet<TChild>();
        var remains = new Stack<TChild>();

        if (_groupToChildren.TryGetValue(group, out var children)) {
            foreach (var instance in children)
                remains.Push(instance);

            while (remains.Count > 0) {
                var node = remains.Pop();
                if (visited.Add(node))
                    yield return node;
            }
        }
    }

#endregion

#region Helpers

    /// <summary>
    /// Used as a flag for <see cref="BipartiteGraph{TGroup,TChild}.Search"/>.
    /// </summary>
    public enum SearchMode
    {
        Bfs,
        Dfs,
    }

#endregion
}
