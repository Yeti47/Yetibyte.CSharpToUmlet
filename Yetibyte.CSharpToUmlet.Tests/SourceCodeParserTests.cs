using Yetibyte.CSharpToUmlet.Parsing;
using Yetibyte.CSharpToUmlet.UmletElements;
using Yetibyte.CSharpToUmlet.UmletElements.Enums;
using Yetibyte.CSharpToUmlet.UmletElements.Types;

namespace Yetibyte.CSharpToUmlet.Tests
{
    [TestClass]
    public class SourceCodeParserTests
    {
        [TestMethod]
        public void ParseIntEnumInferred()
        {
            string sourceCode =
                """
                public enum Species
                {
                    Cat,
                    Dog,
                    Bird
                }
                """;

            var parser = new SourceCodeParser();

            var umletElement = parser.Parse(sourceCode);

            Assert.IsTrue(
                   umletElement is UmletEnum enumElement 
                && enumElement.Name == "Species"
                && enumElement.Constants.Count == 3
                && enumElement.Constants[0].Name == "Cat"
                && enumElement.Constants[1].Name == "Dog"
                && enumElement.Constants[2].Name == "Bird"
                && enumElement.UnderlyingType == UmletEnumUnderlyingType.Int
            );
        }

        [TestMethod]
        public void ParseIntEnumExplicit()
        {
            string sourceCode =
                """
                public enum Species : int
                {
                    Cat,
                    Dog,
                    Bird
                }
                """;

            var parser = new SourceCodeParser();

            var umletElement = parser.Parse(sourceCode);

            Assert.IsTrue(
                   umletElement is UmletEnum enumElement
                && enumElement.Name == "Species"
                && enumElement.Constants.Count == 3
                && enumElement.Constants[0].Name == "Cat"
                && enumElement.Constants[1].Name == "Dog"
                && enumElement.Constants[2].Name == "Bird"
                && enumElement.UnderlyingType == UmletEnumUnderlyingType.Int
            );
        }

        [TestMethod]
        public void ParseSbyteEnum()
        {
            string sourceCode =
                """
                public enum Species : sbyte
                {
                    Cat
                    Dog,
                    Bird
                }
                """;

            var parser = new SourceCodeParser();

            var umletElement = parser.Parse(sourceCode);

            Assert.IsTrue(
                   umletElement is UmletEnum enumElement
                && enumElement.Name == "Species"
                && enumElement.Constants.Count == 3
                && enumElement.Constants[0].Name == "Cat"
                && enumElement.Constants[1].Name == "Dog"
                && enumElement.Constants[2].Name == "Bird"
                && enumElement.UnderlyingType == UmletEnumUnderlyingType.Sbyte
            );
        }

        [TestMethod]
        public void ParseIntEnumWithValues()
        {
            string sourceCode =
                """
                public enum Species
                {
                    Cat = 0,
                    Dog = 1,
                    Bird = 2
                }
                """;

            var parser = new SourceCodeParser();

            var umletElement = parser.Parse(sourceCode);

            Assert.IsTrue(
                   umletElement is UmletEnum enumElement
                && enumElement.Name == "Species"
                && enumElement.Constants.Count == 3
                && enumElement.Constants[0].Name == "Cat"
                && enumElement.Constants[0].Value == "0"
                && enumElement.Constants[1].Name == "Dog"
                && enumElement.Constants[1].Value == "1"
                && enumElement.Constants[2].Name == "Bird"
                && enumElement.Constants[2].Value == "2"
                && enumElement.UnderlyingType == UmletEnumUnderlyingType.Int
            );
        }

        [TestMethod]
        public void ParseClass()
        {
            string sourceCode =
                """
                public abstract class Person
                {
                    public string Name { get; set; }
                    public int Age { get; init; }

                    public abstract int SomeAbstractProp { get; set; }

                    public Person Parent { get; }

                    public string Greet() => "Hi, there!";

                    public void DoStuff(int someParam) 
                    {

                    }

                    public static Person Create(string name, int age) => new Person { Name = name, Age = age };
                }
                """;

            var parser = new SourceCodeParser();

            var umletElement = parser.Parse(sourceCode);

            Assert.IsTrue(umletElement is UmletType umletType && umletType.Kind == UmletTypeKind.Class);
        }

        [TestMethod]
        public void ParseDelegate()
        {
            string sourceCode =
                """
                public delegate int Calculator(int a, double b);
                """;

            var parser = new SourceCodeParser();

            var umletElement = parser.Parse(sourceCode);

            string markup = umletElement.ToMarkup();

            Assert.IsTrue(
                   umletElement is UmletDelegate umletDelegate 
                && umletDelegate.Name == "Calculator"
                && umletDelegate.Parameters.Count == 2
                && umletDelegate.Parameters[0].Name == "a"
                && umletDelegate.Parameters[0].Type == "int"
                && umletDelegate.Parameters[1].Name == "b"
                && umletDelegate.Parameters[1].Type == "double"
                && umletDelegate.ReturnType == "int"
            );
        }
    }
}