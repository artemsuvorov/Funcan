using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Repository;

public class SimpleHistory : IHistory
{
    private Dictionary<int, List<HistoryEntry>> HistoryEntries { get; } = new();

    public void Save(int userId, HistoryEntry historyEntry)
    {
        if (!HistoryEntries.ContainsKey(userId))
            HistoryEntries.Add(userId, new List<HistoryEntry>());
        HistoryEntries[userId].Add(historyEntry);
    }

    public List<HistoryEntry> Get(int userId) =>
        !HistoryEntries.ContainsKey(userId) ? null : HistoryEntries[userId];
}