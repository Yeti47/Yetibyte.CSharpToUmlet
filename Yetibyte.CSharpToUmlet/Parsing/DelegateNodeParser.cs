using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.CSharpToUmlet.UmletElements;
using Yetibyte.CSharpToUmlet.UmletElements.Types;

namespace Yetibyte.CSharpToUmlet.Parsing;

public class DelegateNodeParser : ISyntaxNodeParser
{
    public IUmletElement Parse(SyntaxNode node)
    {
        if (node is not DelegateDeclarationSyntax delegateNode)
            throw new ArgumentException("The given node is not a delegate declaration.", nameof(node));

        string name = delegateNode.Identifier.Text;

        string returnType = delegateNode.ReturnType.GetTypeName();

        var parameters = delegateNode.ParameterList.Parameters.Select(p => new UmletParameter(p.Identifier.Text, p.Type?.GetTypeName() ?? string.Empty));

        return new UmletDelegate(name, parameters, returnType);
    }
}
