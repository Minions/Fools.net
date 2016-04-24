using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gibberish.AST;
using Gibberish.AST._1_Bare;
using JetBrains.Annotations;

namespace Gibberish.Tests.ZzTestHelpers
{
	internal class RecognitionAssertions<TConstruct> : ObjectAssertions
	{
		public RecognitionAssertions(bool success, [NotNull] IEnumerable<TConstruct> result) : base(result)
		{
			Success = success;
			Result = result.ToArray();
		}

		public bool Success { get; }
		[NotNull]
		public TConstruct[] Result { get; }

		public void BeRecognizedAs(AstBuilder<TConstruct> expected)
		{
			var statements = expected.Build();
			Success.Should()
				.BeTrue("parse should have fully matched the input. This is probably an error in the test");
			Result.ShouldBeEquivalentTo(
				statements,
				options => options.IncludingFields()
					.IncludingProperties()
					.IncludingNestedObjects()
					.RespectingRuntimeTypes().ComparingByValue<PossiblySpecified<int>>());
		}

		internal void BeRecognizedAs(params AstBuilder<TConstruct>[] expected)
		{
			var statements = expected.SelectMany(b => b.Build());
			Success.Should()
				.BeTrue("parse should have fully matched the input. This is probably an error in the test");
			Result.ShouldBeEquivalentTo(
				statements,
				options => options.IncludingFields()
					.IncludingProperties()
					.IncludingNestedObjects()
					.RespectingRuntimeTypes());
		}
	}
}
