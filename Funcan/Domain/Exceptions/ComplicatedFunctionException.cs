using System;

namespace Funcan.Domain.Exceptions;

public class ComplicatedFunctionException : Exception
{
    public ComplicatedFunctionException(string message = "Too Complicated function") : base(message)
    {
    }
}