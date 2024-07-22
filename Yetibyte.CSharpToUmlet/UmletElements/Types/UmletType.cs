using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.CSharpToUmlet.UmletElements.Types
{
    public enum UmletTypeKind
    {
        Class,
        Interface,
        Struct
    }

    public class UmletType(UmletTypeKind kind, string name, IEnumerable<UmletMember> members) : IUmletElement
    {
        public UmletTypeKind Kind { get; } = kind;
        public string Name { get; } = name;

        public bool IsAbstract { get; set; }
        public bool IsSealed { get; set; }
        public bool IsStatic { get; set; }
        public bool IsRecord { get; set; }

        public IReadOnlyList<UmletMember> Members { get; } = members.ToArray();

        public IEnumerable<UmletProperty> Properties => Members.OfType<UmletProperty>();
        public IEnumerable<UmletMethod> Methods => Members.OfType<UmletMethod>();

        public string ToMarkup()
        {
            var markupBuilder = new StringBuilder();

            var kindStereoType = Kind switch
            {
                UmletTypeKind.Interface => new UmletStereotype("interface"),
                UmletTypeKind.Struct => new UmletStereotype("struct"),
                _ => null
            };

            if (kindStereoType is not null)
            {
                markupBuilder.AppendLine(kindStereoType.ToMarkup());
            }

            if (IsAbstract)
            {
                markupBuilder.AppendLine(new UmletStereotype("abstract").ToMarkup());
            }

            if (IsRecord)
            {
                markupBuilder.AppendLine(new UmletStereotype("record").ToMarkup());
            }

            if (IsSealed)
            {
                markupBuilder.AppendLine(new UmletStereotype("sealed").ToMarkup());
            }

            string formatter = "";

            if (IsStatic)
            {
                formatter += UmletMarkup.UNDERLINE;
            }

            if (IsAbstract || Kind == UmletTypeKind.Interface)
            {
                formatter += UmletMarkup.ITALIC;
            }

            var reverseFormatter = new string(formatter.Reverse().ToArray());

            markupBuilder.Append($"{formatter}{Name}{reverseFormatter}");

            if (Members.Any())
            {
                markupBuilder.AppendLine();
                markupBuilder.AppendLine(UmletMarkup.HORIZONTAL_LINE);
            }

            var properties = Properties.ToArray();

            for (int i = 0; i < properties.Length; i++)
            {
                markupBuilder.Append(properties[i].ToMarkup());

                if (i < properties.Length - 1)
                {
                    markupBuilder.AppendLine();
                }
            }

            if (Methods.Any())
            {
                markupBuilder.AppendLine();
                markupBuilder.AppendLine(UmletMarkup.HORIZONTAL_LINE);
            }

            var methods = Methods.ToArray();

            for (int i = 0; i < methods.Length; i++)
            {
                markupBuilder.Append(methods[i].ToMarkup());

                if (i < methods.Length - 1)
                {
                    markupBuilder.AppendLine();
                }
            }

            return markupBuilder.ToString();
        }
    }

}
