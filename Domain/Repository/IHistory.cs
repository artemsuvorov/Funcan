using System.Collections.Generic;
using Funcan.Domain.Models;

namespace Funcan.Domain.Repository;

public interface IHistory
{
    void Save(int userId, HistoryEntry historyEntry);
    List<HistoryEntry> Get(int userId);
}