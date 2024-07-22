namespace Yetibyte.CSharpToUmlet.UmletElements;

public class UmletStereotype(string name) : IUmletElement
{
    public string Name { get; } = name;

    public string ToMarkup() => $"<<{Name}>>";
}
