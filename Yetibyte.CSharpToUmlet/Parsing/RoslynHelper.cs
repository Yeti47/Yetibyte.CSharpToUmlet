using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Yetibyte.CSharpToUmlet.UmletElements;

namespace Yetibyte.CSharpToUmlet.Parsing
{
    public static class RoslynHelper
    {
        public static class Modifiers
        {
            public const string PRIVATE = "private";
            public const string PUBLIC = "public";
            public const string PROTECTD = "protected";

            public const string STATIC = "static";
            public const string ABSTRACT = "abstract";
            public const string SEALED = "sealed";

            public const string READONLY = "readonly";
        }

        public static class Accessors
        {
            public const string GET = "get";
            public const string SET = "set";
            public const string INIT = "init";
        }

        public static UmletAccessModifier GetAccessModifier(MemberDeclarationSyntax memberNode, UmletAccessModifier defaultModifier)
        {
            var accessModifier = defaultModifier;

            if (memberNode.Modifiers.Any(m => m.Text == "private"))
            {
                accessModifier = UmletAccessModifier.Private;
            }
            else if (memberNode.Modifiers.Any(m => m.Text == "protected"))
            {
                accessModifier = UmletAccessModifier.Protected;
            }
            else if (memberNode.Modifiers.Any(m => m.Text == "public"))
            {
                accessModifier = UmletAccessModifier.Public;
            }
            else if (memberNode.Modifiers.Any(m => m.Text == "internal"))
            {
                accessModifier = UmletAccessModifier.Package;
            }

            return accessModifier;
        }

        public static bool HasModifier(this MemberDeclarationSyntax node, string modifier) => node.Modifiers.Any(m => m.Text == modifier);

        public static bool HasAccessor(this BasePropertyDeclarationSyntax node, string accessor) => node.AccessorList?.Accessors.Any(a => a.Keyword.Text == accessor) ?? false;

        public static string GetTypeName(this TypeSyntax typeNode) => typeNode switch
        {
            IdentifierNameSyntax identifierNode => identifierNode.Identifier.Text,
            PredefinedTypeSyntax predefinedTypeNode => predefinedTypeNode.Keyword.Text,
            _ => typeNode.NormalizeWhitespace().ToString().Split(" ").LastOrDefault() ?? string.Empty
        };
        
    }
}
