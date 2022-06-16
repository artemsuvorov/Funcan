namespace Funcan.Domain.Models;

public record DrawType(string Value)
{
    public static DrawType Line => new("Lines");
    public static DrawType Dots => new("Dots");
}