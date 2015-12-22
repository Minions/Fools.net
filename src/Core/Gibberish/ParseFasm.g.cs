//
// IronMeta ParseFasm Parser; Generated 2015-12-22 03:47:18Z UTC
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
                "AtRoot",
                "FasmFile",
                "Name",
                "NL",
                "TopLevelStatement",
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


        public void TopLevelStatement(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // CALL DefineThunk
            var _start_i0 = _index;
            _ParseFasm_Item _r0;

            _r0 = _MemoCall(_memo, "DefineThunk", _index, DefineThunk, new _ParseFasm_Item[] { new _ParseFasm_Item(AtRoot) });

            if (_r0 != null) _index = _r0.NextIndex;

        }


        public void DefineThunk(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indentation = null;
            _ParseFasm_Item name = null;
            _ParseFasm_Item body = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indentation
            indentation = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 4
            int _start_i4 = _index;

            // CALL DefineThunkPrelude
            var _start_i6 = _index;
            _ParseFasm_Item _r6;

            _r6 = _MemoCall(_memo, "DefineThunkPrelude", _index, DefineThunkPrelude, new _ParseFasm_Item[] { indentation });

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND name
            name = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // CALL Body
            var _start_i8 = _index;
            _ParseFasm_Item _r8;

            _r8 = _MemoCall(_memo, "Body", _index, Body, new _ParseFasm_Item[] { indentation });

            if (_r8 != null) _index = _r8.NextIndex;

            // BIND body
            body = _memo.Results.Peek();

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

            // ACT
            var _r3 = _memo.Results.Peek();
            if (_r3 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new DefineThunkNode(((NameNode)name).Name, ((BodyNode)body).Statements); }, _r3), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void DefineThunkPrelude(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indentation = null;
            _ParseFasm_Item name = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indentation
            indentation = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 4
            int _start_i4 = _index;

            // AND 5
            int _start_i5 = _index;

            // AND 6
            int _start_i6 = _index;

            // AND 7
            int _start_i7 = _index;

            // CALLORVAR indentation
            _ParseFasm_Item _r8;

            if (indentation.Production != null)
            {
                var _p8 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)indentation.Production; // what type safety?
                _r8 = _MemoCall(_memo, indentation.Production.Method.Name, _index, _p8, null);
            }
            else
            {
                _r8 = _ParseLiteralObj(_memo, ref _index, indentation.Inputs);
            }

            if (_r8 != null) _index = _r8.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label7; }

            // CALL KW
            var _start_i9 = _index;
            _ParseFasm_Item _r9;
            var _arg9_0 = "define.thunk";

            _r9 = _MemoCall(_memo, "KW", _index, KW, new _ParseFasm_Item[] { new _ParseFasm_Item(_arg9_0) });

            if (_r9 != null) _index = _r9.NextIndex;

        label7: // AND
            var _r7_2 = _memo.Results.Pop();
            var _r7_1 = _memo.Results.Pop();

            if (_r7_1 != null && _r7_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i7, _index, _memo.InputEnumerable, _r7_1.Results.Concat(_r7_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i7;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label6; }

            // CALLORVAR Name
            _ParseFasm_Item _r11;

            _r11 = _MemoCall(_memo, "Name", _index, Name, null);

            if (_r11 != null) _index = _r11.NextIndex;

            // BIND name
            name = _memo.Results.Peek();

        label6: // AND
            var _r6_2 = _memo.Results.Pop();
            var _r6_1 = _memo.Results.Pop();

            if (_r6_1 != null && _r6_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i6, _index, _memo.InputEnumerable, _r6_1.Results.Concat(_r6_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i6;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // LITERAL ":"
            _ParseLiteralString(_memo, ref _index, ":");

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // CALLORVAR NL
            _ParseFasm_Item _r13;

            _r13 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r13 != null) _index = _r13.NextIndex;

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

            // ACT
            var _r3 = _memo.Results.Peek();
            if (_r3 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return name; }, _r3), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void Name(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            _ParseFasm_Item name = null;

            // REGEXP [a-zA-z0-9_.]+
            _ParseRegexp(_memo, ref _index, _re0);

            // BIND name
            name = _memo.Results.Peek();

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new NameNode(name.Inputs); }, _r0), true) );
            }

        }


        public void Body(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indentation = null;
            _ParseFasm_Item pass = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indentation
            indentation = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // CALL PassStatement
            var _start_i5 = _index;
            _ParseFasm_Item _r5;

            _r5 = _MemoCall(_memo, "PassStatement", _index, PassStatement, new _ParseFasm_Item[] { indentation });

            if (_r5 != null) _index = _r5.NextIndex;

            // BIND pass
            pass = _memo.Results.Peek();

            // ACT
            var _r3 = _memo.Results.Peek();
            if (_r3 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return pass; }, _r3), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void PassStatement(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indentation = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indentation
            indentation = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 4
            int _start_i4 = _index;

            // AND 5
            int _start_i5 = _index;

            // CALL Indent
            var _start_i6 = _index;
            _ParseFasm_Item _r6;

            _r6 = _MemoCall(_memo, "Indent", _index, Indent, new _ParseFasm_Item[] { indentation });

            if (_r6 != null) _index = _r6.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label5; }

            // LITERAL "pass"
            _ParseLiteralString(_memo, ref _index, "pass");

        label5: // AND
            var _r5_2 = _memo.Results.Pop();
            var _r5_1 = _memo.Results.Pop();

            if (_r5_1 != null && _r5_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i5, _index, _memo.InputEnumerable, _r5_1.Results.Concat(_r5_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i5;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // CALLORVAR NL
            _ParseFasm_Item _r8;

            _r8 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r8 != null) _index = _r8.NextIndex;

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

            // ACT
            var _r3 = _memo.Results.Peek();
            if (_r3 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new BodyNode(new PassStatement()); }, _r3), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void AtRoot(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // LOOK 0
            int _start_i0 = _index;

            // ANY
            _ParseAny(_memo, ref _index);

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _ParseFasm_Item(_index, _memo.InputEnumerable)); }

            // LOOK 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 != null ? new _ParseFasm_Item(_start_i0, _memo.InputEnumerable) : null );
            _index = _start_i0;

        }


        public void Indent(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indents = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indents
            indents = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 3
            int _start_i3 = _index;

            // LITERAL '\t'
            _ParseLiteralChar(_memo, ref _index, '\t');

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label3; }

            // CALLORVAR indents
            _ParseFasm_Item _r5;

            if (indents.Production != null)
            {
                var _p5 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)indents.Production; // what type safety?
                _r5 = _MemoCall(_memo, indents.Production.Method.Name, _index, _p5, null);
            }
            else
            {
                _r5 = _ParseLiteralObj(_memo, ref _index, indents.Inputs);
            }

            if (_r5 != null) _index = _r5.NextIndex;

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

        static readonly Verophyle.Regexp.StringRegexp _re0 = new Verophyle.Regexp.StringRegexp(@"[a-zA-z0-9_.]+");

    } // class ParseFasm

} // namespace Gibberish

