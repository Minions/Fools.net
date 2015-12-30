using System.Collections.Generic;

namespace Gibberish.AST
{
    internal class BodyNode : ParseTree
    {
        public readonly List<Statement> Statements = new List<Statement>();

        public BodyNode(PassStatement statement)
        {
            Statements.Add(statement);
        }
    }
}