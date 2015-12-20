using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Gibberish.Tests
{
    [TestFixture]
    public class Class1
    {
        [Ignore("It will be a while.")]
        [Test]
        public void FullAcceptanceTest()
        {
            var contents = File.ReadAllText("...");
            // work hard to parse & stuff
            // emit something
            // assert that what was emitted is what we expected
        }

        [Test]
        public void Can_read_a_uselanguage_statement_and_parse_it_to_do_nothing()
        {
            var input = "use language fool\n";
            ParseTree parseTree = Parse(input);
            parseTree.Should().Be(ParseTree.Empty);
        }

        private ParseTree Parse(string input)
        {
            return ParseTree.Empty;
        }
    }

    internal class ParseTree
    {
        public static readonly ParseTree Empty = new ParseTree();
    }
}
