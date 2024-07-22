using Yetibyte.CSharpToUmlet.UmletElements;

namespace Yetibyte.CSharpToUmlet.Parsing;

public interface ISourceCodeParser
{
    IUmletElement Parse(string sourceCode);
}