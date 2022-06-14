using System.Collections.Generic;
using System.Linq;
using Funcan.Domain.Models;

namespace Funcan.Domain.Repository;

public class MemoryHistoryRepository : IHistoryRepository
{
    private Dictionary<int, Dictionary<string, HistoryEntry>> HistoryEntries { get; } = new();

    public void Save(int userId, HistoryEntry historyEntry)
    {
        if (!HistoryEntries.ContainsKey(userId))
            HistoryEntries.Add(userId, new Dictionary<string, HistoryEntry>());
        HistoryEntries[userId][historyEntry.Function] = historyEntry;
    }

    public List<HistoryEntry> Get(int userId) =>
        !HistoryEntries.ContainsKey(userId) ? new List<HistoryEntry>() : HistoryEntries[userId].Values.ToList();
}