using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.CSharpToUmlet.UmletElements;

namespace Yetibyte.CSharpToUmlet.Parsing
{
    public class SourceCodeParser : ISourceCodeParser
    {
        public IUmletElement Parse(string sourceCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

            var rootNode = syntaxTree.GetRoot();

            var parsableNode = FindTargetNode(rootNode);

            if (parsableNode is null)
            {
                throw new NoSupportedTypeFoundException("Could not find a supported type declaration in the source code.");
            }

            var parser = GetNodeParser(parsableNode);

            var markupElement = parser.Parse(parsableNode);

            return markupElement;
        }

        private SyntaxNode? FindTargetNode(SyntaxNode rootNode) => rootNode.DescendantNodesAndSelf().FirstOrDefault(IsParsable);

        private bool IsParsable(SyntaxNode rootNode)
        {
            return rootNode
                is TypeDeclarationSyntax
                or EnumDeclarationSyntax
                or DelegateDeclarationSyntax;
        }

        private ISyntaxNodeParser GetNodeParser(SyntaxNode node) => node switch
        {
            EnumDeclarationSyntax => new EnumNodeParser(),
            TypeDeclarationSyntax => new TypeNodeParser(),
            DelegateDeclarationSyntax => new DelegateNodeParser(),
            _ => throw new NotSupportedException("Could not find a parser for the given syntax node.")
        };
    }
}
