using System.Text;

namespace Yetibyte.CSharpToUmlet.UmletElements.Types
{
    public class UmletMethod(UmletAccessModifier accessModifier, string name, string type, IEnumerable<UmletParameter> parameters) : UmletMember(accessModifier, name, type)
    {
        public IReadOnlyList<UmletParameter> Parameters { get; } = parameters.ToArray();

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

            markupBuilder.Append(Name);

            markupBuilder.Append('(');

            for (int i = 0; i < Parameters.Count; i++)
            {
                markupBuilder.Append(Parameters[i].ToMarkup());

                if (i < Parameters.Count - 1)
                {
                    markupBuilder.Append(", ");
                }
            }

            markupBuilder.Append(')');

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
