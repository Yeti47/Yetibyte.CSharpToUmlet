namespace Yetibyte.CSharpToUmlet.UmletElements.Types
{
    public class UmletParameter(string name, string type) : IUmletElement
    {
        public string Name { get; } = name;
        public string Type { get; } = type;

        public string ToMarkup() => $"{Name}: {Type}";
    }

}
