using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.CSharpToUmlet.UmletElements.Types;

namespace Yetibyte.CSharpToUmlet.UmletElements
{
    public class UmletDelegate(string name, IEnumerable<UmletParameter> parameters, string returnType) : IUmletElement
    {
        public string Name { get; } = name;
        public IReadOnlyList<UmletParameter> Parameters { get; } = parameters.ToArray();
        public string ReturnType { get; } = returnType;


        public string ToMarkup()
        {
            var markupBuilder = new StringBuilder();

            markupBuilder.AppendLine(new UmletStereotype("delegate").ToMarkup());

            markupBuilder.AppendLine(Name);

            markupBuilder.AppendLine(UmletMarkup.HORIZONTAL_LINE);

            foreach (var parameter in Parameters)
            {
                markupBuilder.Append(new UmletStereotype("parameter").ToMarkup());
                markupBuilder.Append(' ');
                markupBuilder.AppendLine(parameter.ToMarkup());
            }

            markupBuilder.Append(new UmletStereotype("return").ToMarkup()); 
            markupBuilder.Append(' ');
            markupBuilder.Append(ReturnType);

            return markupBuilder.ToString();
        }
    }
}
