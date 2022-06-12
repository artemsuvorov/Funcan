namespace Funcan.Domain.Models;

public class DrawType
{
    public string Value { get; }

    private DrawType(string value) => Value = value;
    public static DrawType Line => new("Lines");
    public static DrawType Dots => new("Dots");
}