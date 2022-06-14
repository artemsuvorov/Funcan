using Funcan.Domain.Models;
using Funcan.Domain.Repository;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class HistoryTests
{
    [Test]
    public void Test()
    {
        var userId = 1;
        var history = new MemoryHistoryRepository();
        var function = "x^2";
        var analysisOptions = new List<PlotterInfo>
        {
            new("function", DrawType.Line),
            new("extrema", DrawType.Dots)
        };
        var expected = new HistoryEntry(function, -5, 5, analysisOptions);

        history.Save(userId, expected);
        var actual = history.Get(userId).First();
        Assert.True(string.Equals(expected.Function, actual.Function, StringComparison.CurrentCultureIgnoreCase));
        Assert.AreEqual(expected.From, actual.From);
        Assert.AreEqual(expected.To, actual.To);
        Assert.AreEqual(expected.AnalysisOptions.Count, actual.AnalysisOptions.Count);

        var count = expected.AnalysisOptions.Count;
        for (var i = 0; i < count; i++)
        {
            Assert.AreEqual(expected.AnalysisOptions[i].Name, actual.AnalysisOptions[i].Name);
            Assert.AreEqual(expected.AnalysisOptions[i].DrawType, actual.AnalysisOptions[i].DrawType);
        }
    }
}