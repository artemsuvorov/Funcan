using System.Collections.Generic;

namespace Funcan.Domain.Models;

public record HistoryEntry(string Function, double From, double To, List<PlotterInfo> analysisOptions);