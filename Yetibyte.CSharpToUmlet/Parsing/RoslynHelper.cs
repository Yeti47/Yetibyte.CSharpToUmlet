using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Yetibyte.CSharpToUmlet.UmletElements;

namespace Yetibyte.CSharpToUmlet.Parsing;

public static class RoslynHelper
{
    public static class Modifiers
    {
        public const string Private = "private";
        public const string Public = "public";
        public const string Protected = "protected";
        public const string Internal = "internal";

        public const string Static = "static";
        public const string Abstract = "abstract";
        public const string Sealed = "sealed";

        public const string ReadOnly = "readonly";
    }

    public static class Accessors
    {
        public const string Get = "get";
        public const string Set = "set";
        public const string Init = "init";
    }

    public static UmletAccessModifier GetAccessModifier(MemberDeclarationSyntax memberNode, UmletAccessModifier defaultModifier)
    {
        var accessModifier = defaultModifier;

        if (memberNode.HasModifier(Modifiers.Private))
        {
            accessModifier = UmletAccessModifier.Private;
        }
        else if (memberNode.HasModifier(Modifiers.Protected))
        {
            accessModifier = UmletAccessModifier.Protected;
        }
        else if (memberNode.HasModifier(Modifiers.Public))
        {
            accessModifier = UmletAccessModifier.Public;
        }
        else if (memberNode.HasModifier(Modifiers.Internal))
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
