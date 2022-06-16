using System;

namespace Funcan.Domain.Parsers;

public interface IFunctionParser
{
    Func<double, double> Parse(string function);
}