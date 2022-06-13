using Funcan.Domain;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ValidateMathFunctionTests
{

    [Test]
    public void SuccessValidationTests(
        [Values("sin(x)", "x ^ 2", "e ^ x", "x ^ 7 - 8 * x + 1 / (sin(x) / cos(x))")] string function)
    {
        var func = new MathFunction(function);
        Assert.AreEqual(func.Function.Stringize(), function);
    }

    [Test]
    public void SuccessReplaceTest()
    {
        var func = new MathFunction("tan(x)");
        Assert.AreEqual(func.Function.Stringize(), "sin(x) / cos(x)");

    }

    [Test]
    public void InvalidFunctionsTests([Values("Pow(x, 2)", "abc - 2", "x - .")] string function)
    {
        try
        {
            var func = new MathFunction(function);
        }
        catch (ArgumentException e)
        {
            Assert.AreEqual(e.Message, "Некорректное выражение");
        }
    }
}