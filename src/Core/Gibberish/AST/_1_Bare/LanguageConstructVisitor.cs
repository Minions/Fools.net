using System.Collections.Generic;

namespace Gibberish.AST._1_Bare
{
	public interface LanguageConstructVisitor
	{
		bool Visit(BlankLine line, int level, List<LanguageConstruct> result);

		bool Visit(UnknownStatement statement, int level, List<LanguageConstruct> result);

		bool Visit(CommentDefinition commentDefinition, int level, List<LanguageConstruct> result);

		bool Visit(UnknownPrelude prelude, int level, List<LanguageConstruct> result);

		bool Visit(UnknownBlock block, int level, List<LanguageConstruct> result);

		bool Visit(MultilineCommentDefinitionPrelude prelude, int level, List<LanguageConstruct> result);

		bool Visit(MultilineCommentDefinitionStatement statement, int level, List<LanguageConstruct> result);
	}
}