using System.Text;

namespace Yetibyte.CSharpToUmlet.UmletElements.Enums;

public class UmletEnum(string name, IEnumerable<UmletEnumConstant> constants, UmletEnumUnderlyingType underlyingType) : IUmletElement
{
    public string Name { get; } = name;
    public IReadOnlyList<UmletEnumConstant> Constants { get; } = constants.ToArray();
    public UmletEnumUnderlyingType UnderlyingType { get; } = underlyingType;

    public string ToMarkup()
    {
        var markupBuilder = new StringBuilder();

        markupBuilder.AppendLine(new UmletStereotype("enum").ToMarkup());

        if (UnderlyingType != UmletEnumUnderlyingType.Int)
        {
            var typeStereotype = new UmletStereotype($"type: {UnderlyingType.ToString().ToLower()}");

            markupBuilder.AppendLine(typeStereotype.ToMarkup());
        }

        markupBuilder.AppendLine(Name);

        markupBuilder.AppendLine(UmletMarkup.HORIZONTAL_LINE);

        for (int i = 0; i < Constants.Count; i++)
        {
            markupBuilder.Append(Constants[i].ToMarkup());

            if (i < Constants.Count - 1)
            {
                markupBuilder.AppendLine();
            }
        }

        return markupBuilder.ToString();
    }
}
