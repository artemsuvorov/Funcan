using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;

namespace Funcan.Domain.Repository;

public class MemoryHistoryRepository : IHistoryRepository {
    private ConcurrentDictionary<int, ConcurrentDictionary<string, HistoryEntry>> HistoryEntries { get; } = new();

    public void Save(int userId, HistoryEntry historyEntry){
        if (!HistoryEntries.ContainsKey(userId))
            HistoryEntries.TryAdd(userId, new ConcurrentDictionary<string, HistoryEntry>());
        if (HistoryEntries.TryGetValue(userId, out var userHistory))
            userHistory.AddOrUpdate(historyEntry.Function, historyEntry, (_, _) => historyEntry);
    }

    public List<HistoryEntry> Get(int userId){
        if (!HistoryEntries.ContainsKey(userId))
            return new List<HistoryEntry>();

        return HistoryEntries.TryGetValue(userId, out var userHistory)
            ? userHistory.Values.ToList()
            : new List<HistoryEntry>();
    }
}