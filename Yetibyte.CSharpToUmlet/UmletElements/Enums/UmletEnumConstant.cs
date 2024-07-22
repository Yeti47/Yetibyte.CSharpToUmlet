namespace Yetibyte.CSharpToUmlet.UmletElements.Enums;

public class UmletEnumConstant(string name, string? value = null) : IUmletElement
{
    public string Name { get; } = name;
    public string? Value { get; } = value;

    public string ToMarkup() => Value is null ? Name : $"{Name} = {Value}";
}
