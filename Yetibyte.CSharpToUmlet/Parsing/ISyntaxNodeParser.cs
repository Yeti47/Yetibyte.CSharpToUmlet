using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yetibyte.CSharpToUmlet.UmletElements;

namespace Yetibyte.CSharpToUmlet.Parsing;

public interface ISyntaxNodeParser
{
    IUmletElement Parse(SyntaxNode node);
}
