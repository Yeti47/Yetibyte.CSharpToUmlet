using System.Text;

namespace Yetibyte.CSharpToUmlet.UmletElements.Types
{
    public class UmletProperty(UmletAccessModifier accessModifier, string name, string type) : UmletMember(accessModifier, name, type)
    {
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }

        public override string ToMarkup()
        {
            var markupBuilder = new StringBuilder();

            string formatter = "";

            if (IsStatic)
            {
                formatter += UmletMarkup.UNDERLINE;
            }

            if (IsAbstract)
            {
                formatter += UmletMarkup.ITALIC;
            }

            var reverseFormatter = new string(formatter.Reverse().ToArray());

            markupBuilder.Append(formatter);

            markupBuilder.Append((char)AccessModifier);
            markupBuilder.Append(' ');

            UmletStereotype? readWriteStereotype = null;

            if (CanRead && !CanWrite)
            {
                readWriteStereotype = new UmletStereotype("read-only");
            }
            else if (!CanRead && CanWrite)
            {
                readWriteStereotype = new UmletStereotype("write-only");
            }

            if (readWriteStereotype is not null)
            {
                markupBuilder.Append(readWriteStereotype.ToMarkup());
                markupBuilder.Append(' ');
            }

            markupBuilder.Append(Name);

            if (!string.IsNullOrWhiteSpace(Type))
            {
                markupBuilder.Append(": ");
                markupBuilder.Append(Type);
            }

            markupBuilder.Append(reverseFormatter);

            return markupBuilder.ToString();
        }
    }

}
