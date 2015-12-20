//
// IronMeta ParseFasm Parser; Generated 2015-12-20 23:11:51Z UTC
//

using System;
using System.Collections.Generic;
using System.Linq;

using IronMeta.Matcher;

#pragma warning disable 0219
#pragma warning disable 1591

namespace Gibberish
{

    using _ParseFasm_Inputs = IEnumerable<char>;
    using _ParseFasm_Results = IEnumerable<ParseTree>;
    using _ParseFasm_Item = IronMeta.Matcher.MatchItem<char, ParseTree>;
    using _ParseFasm_Args = IEnumerable<IronMeta.Matcher.MatchItem<char, ParseTree>>;
    using _ParseFasm_Memo = IronMeta.Matcher.MatchState<char, ParseTree>;
    using _ParseFasm_Rule = System.Action<IronMeta.Matcher.MatchState<char, ParseTree>, int, IEnumerable<IronMeta.Matcher.MatchItem<char, ParseTree>>>;
    using _ParseFasm_Base = IronMeta.Matcher.Matcher<char, ParseTree>;

    public partial class ParseFasm : IronMeta.Matcher.Matcher<char, ParseTree>
    {
        public ParseFasm()
            : base()
        {
            _setTerminals();
        }

        public ParseFasm(bool handle_left_recursion)
            : base(handle_left_recursion)
        {
            _setTerminals();
        }

        void _setTerminals()
        {
            this.Terminals = new HashSet<string>()
            {
                "FasmFile",
                "WS",
            };
        }


        public void FasmFile(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // AND 1
            int _start_i1 = _index;

            // LITERAL "use language fasm"
            _ParseLiteralString(_memo, ref _index, "use language fasm");

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // LITERAL '\n'
            _ParseLiteralChar(_memo, ref _index, '\n');

        label1: // AND
            var _r1_2 = _memo.Results.Pop();
            var _r1_1 = _memo.Results.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i1, _index, _memo.InputEnumerable, _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i1;
            }

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return ParseTree.Empty; }, _r0), true) );
            }

        }


        public void KW(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item str = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND str
            str = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 3
            int _start_i3 = _index;

            // CALLORVAR str
            _ParseFasm_Item _r4;

            if (str.Production != null)
            {
                var _p4 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)str.Production; // what type safety?
                _r4 = _MemoCall(_memo, str.Production.Method.Name, _index, _p4, null);
            }
            else
            {
                _r4 = _ParseLiteralObj(_memo, ref _index, str.Inputs);
            }

            if (_r4 != null) _index = _r4.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // STAR 5
            int _start_i5 = _index;
            var _res5 = Enumerable.Empty<ParseTree>();
        label5:

            // CALLORVAR WS
            _ParseFasm_Item _r6;

            _r6 = _MemoCall(_memo, "WS", _index, WS, null);

            if (_r6 != null) _index = _r6.NextIndex;

            // STAR 5
            var _r5 = _memo.Results.Pop();
            if (_r5 != null)
            {
                _res5 = _res5.Concat(_r5.Results);
                goto label5;
            }
            else
            {
                _memo.Results.Push(new _ParseFasm_Item(_start_i5, _index, _memo.InputEnumerable, _res5.Where(_NON_NULL), true));
            }

        label3: // AND
            var _r3_2 = _memo.Results.Pop();
            var _r3_1 = _memo.Results.Pop();

            if (_r3_1 != null && _r3_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i3, _index, _memo.InputEnumerable, _r3_1.Results.Concat(_r3_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i3;
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void WS(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // LITERAL '\n'
            _ParseLiteralChar(_memo, ref _index, '\n');

        }


    } // class ParseFasm

} // namespace Gibberish

