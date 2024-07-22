using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yetibyte.CSharpToUmlet.Parsing;

public class NoSupportedTypeFoundException(string message) : Exception(message)
{
}
