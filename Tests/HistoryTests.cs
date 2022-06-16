using Funcan.Domain.Models;
using Funcan.Domain.Repository;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class HistoryTests {
    [Test]
    public void Test(){
        var userId = 1;
        var history = new MemoryHistoryRepository();
        var function = "x^2";
        var analysisOptions = new List<string> { "function", "extrema" };
        var expected = new HistoryEntry(function, -5, 5, analysisOptions, DateTime.Now);

        history.Save(userId, expected);
        var actual = history.Get(userId).First();
        Assert.True(string.Equals(expected.Function, actual.Function, StringComparison.CurrentCultureIgnoreCase));
        Assert.AreEqual(expected.From, actual.From);
        Assert.AreEqual(expected.To, actual.To);
        Assert.AreEqual(expected.plotters.Count, actual.plotters.Count);

        var count = expected.plotters.Count;
        for (var i = 0; i < count; i++)
            Assert.AreEqual(expected.plotters[i], actual.plotters[i]);
    }
}