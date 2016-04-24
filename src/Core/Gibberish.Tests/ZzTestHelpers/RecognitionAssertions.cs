using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using Gibberish.AST;
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
			_Verify(expected.Build());
		}

		internal void BeRecognizedAs(params AstBuilder<TConstruct>[] expected)
		{
			_Verify(
				expected.SelectMany(b => b.Build())
					.ToList());
		}

		private void _Verify(List<TConstruct> statements)
		{
			Success.Should()
				.BeTrue("parse should have fully matched the input. This is probably an error in the test");
			Result.ShouldBeEquivalentTo(
				statements,
				options => options.IncludingFields()
					.IncludingProperties()
					.IncludingNestedObjects()
					.RespectingRuntimeTypes()
					.ComparingByValue<PossiblySpecified<bool>>()
					.ComparingByValue<PossiblySpecified<int>>());
		}
	}
}
