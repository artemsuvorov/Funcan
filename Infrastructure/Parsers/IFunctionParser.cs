using System;

namespace Funcan.Solvers;

public interface IFunctionParser
{
    Func<double, double> Parse(string function);
}