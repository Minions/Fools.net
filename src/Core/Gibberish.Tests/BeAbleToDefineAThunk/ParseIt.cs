﻿using System.IO;
using ApprovalTests;
using FluentAssertions;
using Gibberish.AST;
using IronMeta.Matcher;
using MatchResultExtensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Gibberish.Tests.BeAbleToDefineAThunk
{
    [TestFixture]
    public class ParseIt
    {
        [Test]
        public void Can_parse_definethunk()
        {
            var input = @"define.thunk some.name:
	pass
";
            var subject = new ParseFasm();
            var result = subject.GetMatch(input, subject.DefineThunk);
            Approvals.VerifyJson(result.PrettyPrint());
        }

        [Test]
        public void Can_read_a_uselanguage_statement_and_parse_it_to_do_nothing()
        {
            var input = "use language fasm\n";
            var subject = new ParseFasm();
            var result = subject.GetMatch(input, subject.FasmFile).Result;
            result.Should().Be(ParseTree.Empty);
        }

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
        public void We_can_parse_arithmetic_expressions()
        {
            var subject = new ParseArithmetic();
            var result = subject.GetMatch("2 * 7", subject.Expression).Result;
            result.Should().Be(14);
        }
    }
}

namespace MatchResultExtensions
{
    internal static class _
    {
        public static string PrettyPrint(this MatchResult<char, ParseTree> self)
        {
            if (self.Success)
            {
                return "Success:\n" + (self.Result == null ? "<null>" : JsonConvert.SerializeObject(self.Result));
            }
            return "Error: " + (self.Error ?? "<null>");
        }
    }
}