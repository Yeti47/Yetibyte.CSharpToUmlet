namespace Yetibyte.CSharpToUmlet.UmletElements.Types
{
    public abstract class UmletMember(UmletAccessModifier accessModifier, string name, string type) : IUmletElement
    {
        public UmletAccessModifier AccessModifier { get; } = accessModifier;
        public string Name { get; } = name;
        public string Type { get; } = type;

        public bool IsAbstract { get; set; }
        public bool IsStatic { get; set; }

        public abstract string ToMarkup();
    }

}
