using Funcan.Domain;
using Funcan.Domain.Models;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ValidateMathFunctionTests
{

    [Test]
    public void SuccessValidationTests(
        [Values("sin(x)", "x ^ 2", "e ^ x", "x ^ 7 - 8 * x + 1 / (sin(x) / cos(x))")] string function)
    {
        MathFunction.TryCreate(function, out var func);
        Assert.AreEqual(func.Entity.Stringize(), function);
    }

    [Test]
    public void SuccessReplaceTest()
    {
        MathFunction.TryCreate("tan(x)", out var func);
        Assert.AreEqual("sin(x) / cos(x)", func.Entity.Stringize());

    }

    [Test]
    public void InvalidFunctionsTests([Values("Pow(x, 2)", "abc - 2", "x - .")] string function)
    {
        Assert.False(MathFunction.TryCreate(function, out var func));
    }
}