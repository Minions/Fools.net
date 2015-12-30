using System.Collections.Generic;

namespace Gibberish.AST
{
    internal class DefineThunkNode : ParseTree
    {
        public readonly List<Statement> body;

        public readonly string name;
        // ReSharper disable once UnusedMember.Global
        public readonly string type = "define.thunk";

        public DefineThunkNode(string name, List<Statement> body)
        {
            this.name = name;
            this.body = body;
        }
    }
}