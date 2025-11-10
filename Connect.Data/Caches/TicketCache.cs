using System.Collections.Concurrent;
using Connect.Data.Models;

namespace Connect.Data.Caches;

public sealed class TicketCache
{
#region Context

    private readonly ConcurrentDictionary<DateTime, Ticket> _store = [];

#endregion

#region Queries

    public Task Insert(Ticket instance) {
        _store.TryAdd(instance.CreatedAt, instance);
        return Task.CompletedTask;
    }

#endregion
}
