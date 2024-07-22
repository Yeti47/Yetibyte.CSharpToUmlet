using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.CSharpToUmlet.UmletElements;
using Yetibyte.CSharpToUmlet.UmletElements.Enums;

namespace Yetibyte.CSharpToUmlet.Parsing
{
    public class EnumNodeParser : ISyntaxNodeParser
    {
        public IUmletElement Parse(SyntaxNode node)
        {
            if (node is not EnumDeclarationSyntax enumNode)
                throw new ArgumentException("The given node is not an enum declaration.", nameof(node));

            var constants = enumNode.Members.Select(ParseEnumConstant);

            var baseTypeName = (enumNode.BaseList?.Types.FirstOrDefault()?.Type as PredefinedTypeSyntax)?.Keyword.Text ?? "int";

            var underlyingType = Enum.TryParse(baseTypeName, true, out UmletEnumUnderlyingType underlyingTypeTemp) 
                ? underlyingTypeTemp 
                : UmletEnumUnderlyingType.Int;

            return new UmletEnum(enumNode.Identifier.Text, constants, underlyingType);
        }

        private UmletEnumConstant ParseEnumConstant(EnumMemberDeclarationSyntax memberDeclaration) => new UmletEnumConstant(memberDeclaration.Identifier.Text, memberDeclaration.EqualsValue?.Value?.ToString());
    }
}
