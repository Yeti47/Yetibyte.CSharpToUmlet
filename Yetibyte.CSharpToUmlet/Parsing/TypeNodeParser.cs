using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.CSharpToUmlet.UmletElements;
using Yetibyte.CSharpToUmlet.UmletElements.Types;

namespace Yetibyte.CSharpToUmlet.Parsing;

public class TypeNodeParser : ISyntaxNodeParser
{
    public IUmletElement Parse(SyntaxNode node)
    {
        if (node is not TypeDeclarationSyntax typeNode)
            throw new ArgumentException("The given node is not a type declaration.", nameof(node));

        string typeName = typeNode.Identifier.Text;

        UmletTypeKind typeKind = typeNode switch
        {
            InterfaceDeclarationSyntax => UmletTypeKind.Interface,
            StructDeclarationSyntax => UmletTypeKind.Struct,
            RecordDeclarationSyntax recordNode when recordNode.ClassOrStructKeyword.Text == "struct" => UmletTypeKind.Struct,
            _ => UmletTypeKind.Class
        };

        bool isRecord = typeNode is RecordDeclarationSyntax;
        bool isAbstract = typeNode.HasModifier(RoslynHelper.Modifiers.Abstract);
        bool isSealed = typeNode.HasModifier(RoslynHelper.Modifiers.Sealed);
        bool isStatic = typeNode.HasModifier(RoslynHelper.Modifiers.Static);

        UmletAccessModifier accessModifier = RoslynHelper.GetAccessModifier(typeNode, UmletAccessModifier.Package);

        var members = typeNode
            .Members
            .Where(m => m is FieldDeclarationSyntax 
                          or PropertyDeclarationSyntax 
                          or MethodDeclarationSyntax)
            .Select(ParseMember);

        var umletType = new UmletType(typeKind, typeName, members)
        {
            IsAbstract = isAbstract,
            IsSealed = isSealed,
            IsStatic = isStatic,
            IsRecord = isRecord
        };

        return umletType;
    }

    private UmletMember ParseMember(MemberDeclarationSyntax memberNode) => memberNode switch
    {
        FieldDeclarationSyntax fieldNode => ParseField(fieldNode),
        PropertyDeclarationSyntax propertyNode => ParseProperty(propertyNode),
        MethodDeclarationSyntax methodNode => ParseMethod(methodNode),
        _ => throw new Exception("Unsupported member node type.")
    };

    private UmletProperty ParseField(FieldDeclarationSyntax fieldNode)
    {
        var variableNode = fieldNode.Declaration.Variables.First();

        string name = variableNode.Identifier.Text;

        var accessModifier = RoslynHelper.GetAccessModifier(fieldNode, UmletAccessModifier.Private);

        bool isReadonly = fieldNode.HasModifier(RoslynHelper.Modifiers.ReadOnly);

        string type = fieldNode.Declaration.Type.GetTypeName();

        var property = new UmletProperty(accessModifier, name, type)
        {
            CanRead = true,
            CanWrite = !isReadonly,
            IsAbstract = fieldNode.HasModifier(RoslynHelper.Modifiers.Abstract) || fieldNode.Parent is InterfaceDeclarationSyntax,
            IsStatic = fieldNode.HasModifier(RoslynHelper.Modifiers.Static)
        };

        return property;
    }

    private UmletProperty ParseProperty(PropertyDeclarationSyntax propertyNode)
    {
        string name = propertyNode.Identifier.Text;

        var accessModifier = RoslynHelper.GetAccessModifier(propertyNode, propertyNode.Parent is InterfaceDeclarationSyntax ? UmletAccessModifier.Public : UmletAccessModifier.Private);

        string type = propertyNode.Type.GetTypeName();

        var property = new UmletProperty(accessModifier, name, type)
        {
            CanRead = propertyNode.HasAccessor(RoslynHelper.Accessors.Get),
            CanWrite = propertyNode.HasAccessor(RoslynHelper.Accessors.Set),
            IsAbstract = propertyNode.HasModifier(RoslynHelper.Modifiers.Abstract) || propertyNode.Parent is InterfaceDeclarationSyntax,
            IsStatic = propertyNode.HasModifier(RoslynHelper.Modifiers.Static),
        };

        return property;
    }

    private UmletMethod ParseMethod(MethodDeclarationSyntax methodNode)
    {
        var name = methodNode.Identifier.Text;

        var accessModifier = RoslynHelper.GetAccessModifier(methodNode, methodNode.Parent is InterfaceDeclarationSyntax ? UmletAccessModifier.Public : UmletAccessModifier.Private);

        string type = methodNode.ReturnType.GetTypeName();

        var parameters = methodNode.ParameterList.Parameters.Select(p => new UmletParameter(p.Identifier.Text, p.Type?.GetTypeName() ?? string.Empty));

        var method = new UmletMethod(accessModifier, name, type, parameters)
        {
            IsAbstract = methodNode.HasModifier(RoslynHelper.Modifiers.Abstract) || methodNode.Parent is InterfaceDeclarationSyntax,
            IsStatic = methodNode.HasModifier(RoslynHelper.Modifiers.Static),
        };

        return method;
    }
}
