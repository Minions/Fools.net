//
// IronMeta ParseFasm Parser; Generated 2015-12-22 03:02:12Z UTC
//

using System;
using System.Collections.Generic;
using System.Linq;

using IronMeta.Matcher;
using Gibberish.AST;

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
                "Body",
                "FasmFile",
                "Name",
                "NL",
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

            // CALLORVAR NL
            _ParseFasm_Item _r3;

            _r3 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r3 != null) _index = _r3.NextIndex;

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


        public void DefineThunk(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            _ParseFasm_Item name = null;
            _ParseFasm_Item body = null;

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // AND 3
            int _start_i3 = _index;

            // AND 4
            int _start_i4 = _index;

            // CALL KW
            var _start_i5 = _index;
            _ParseFasm_Item _r5;
            var _arg5_0 = "define.thunk";

            _r5 = _MemoCall(_memo, "KW", _index, KW, new _ParseFasm_Item[] { new _ParseFasm_Item(_arg5_0) });

            if (_r5 != null) _index = _r5.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // CALLORVAR Name
            _ParseFasm_Item _r7;

            _r7 = _MemoCall(_memo, "Name", _index, Name, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // BIND name
            name = _memo.Results.Peek();

        label4: // AND
            var _r4_2 = _memo.Results.Pop();
            var _r4_1 = _memo.Results.Pop();

            if (_r4_1 != null && _r4_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i4, _index, _memo.InputEnumerable, _r4_1.Results.Concat(_r4_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i4;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // LITERAL ":"
            _ParseLiteralString(_memo, ref _index, ":");

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

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // CALLORVAR NL
            _ParseFasm_Item _r9;

            _r9 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r9 != null) _index = _r9.NextIndex;

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Body
            _ParseFasm_Item _r11;

            _r11 = _MemoCall(_memo, "Body", _index, Body, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // BIND body
            body = _memo.Results.Peek();

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
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new DefineThunkNode(((NameNode)name).Name, ((BodyNode)body).Statements); }, _r0), true) );
            }

        }


        public void Name(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // LITERAL "some.name"
            _ParseLiteralString(_memo, ref _index, "some.name");

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new NameNode("some.name"); }, _r0), true) );
            }

        }


        public void Body(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // AND 1
            int _start_i1 = _index;

            // AND 2
            int _start_i2 = _index;

            // LITERAL '\t'
            _ParseLiteralChar(_memo, ref _index, '\t');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label2; }

            // LITERAL "pass"
            _ParseLiteralString(_memo, ref _index, "pass");

        label2: // AND
            var _r2_2 = _memo.Results.Pop();
            var _r2_1 = _memo.Results.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i2, _index, _memo.InputEnumerable, _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i2;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR NL
            _ParseFasm_Item _r5;

            _r5 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r5 != null) _index = _r5.NextIndex;

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
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new BodyNode(new PassStatement()); }, _r0), true) );
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

            // OR 5
            int _start_i5 = _index;

            // PLUS 6
            int _start_i6 = _index;
            var _res6 = Enumerable.Empty<ParseTree>();
        label6:

            // CALLORVAR WS
            _ParseFasm_Item _r7;

            _r7 = _MemoCall(_memo, "WS", _index, WS, null);

            if (_r7 != null) _index = _r7.NextIndex;

            // PLUS 6
            var _r6 = _memo.Results.Pop();
            if (_r6 != null)
            {
                _res6 = _res6.Concat(_r6.Results);
                goto label6;
            }
            else
            {
                if (_index > _start_i6)
                    _memo.Results.Push(new _ParseFasm_Item(_start_i6, _index, _memo.InputEnumerable, _res6.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i5; } else goto label5;

            // LOOK 8
            int _start_i8 = _index;

            // CALLORVAR NL
            _ParseFasm_Item _r9;

            _r9 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r9 != null) _index = _r9.NextIndex;

            // LOOK 8
            var _r8 = _memo.Results.Pop();
            _memo.Results.Push( _r8 != null ? new _ParseFasm_Item(_start_i8, _memo.InputEnumerable) : null );
            _index = _start_i8;

        label5: // OR
            int _dummy_i5 = _index; // no-op for label

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

            // LITERAL ' '
            _ParseLiteralChar(_memo, ref _index, ' ');

        }


        public void NL(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // OR 0
            int _start_i0 = _index;

            // OR 1
            int _start_i1 = _index;

            // LITERAL "\r\n"
            _ParseLiteralString(_memo, ref _index, "\r\n");

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i1; } else goto label1;

            // LITERAL "\n"
            _ParseLiteralString(_memo, ref _index, "\n");

        label1: // OR
            int _dummy_i1 = _index; // no-op for label

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i0; } else goto label0;

            // LITERAL "\r"
            _ParseLiteralString(_memo, ref _index, "\r");

        label0: // OR
            int _dummy_i0 = _index; // no-op for label

        }


    } // class ParseFasm

} // namespace Gibberish

