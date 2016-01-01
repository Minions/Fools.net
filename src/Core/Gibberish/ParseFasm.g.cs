//
// IronMeta ParseFasm Parser; Generated 2015-12-31 13:10:24Z UTC
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
                "AllowedStatementsInDefineThunk",
                "AtRoot",
                "Name",
                "NL",
                "TopLevelStatement",
                "WS",
            };
        }


        public void FasmFile(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // CALL Statement
            var _start_i1 = _index;
            _ParseFasm_Item _r1;

            _r1 = _MemoCall(_memo, "Statement", _index, Statement, new _ParseFasm_Item[] { new _ParseFasm_Item(UseLanguage) });

            if (_r1 != null) _index = _r1.NextIndex;

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new DeclarationSet(); }, _r0), true) );
            }

        }


        public void UseLanguage(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            _ParseFasm_Item lang = null;

            // AND 1
            int _start_i1 = _index;

            // CALL KW
            var _start_i2 = _index;
            _ParseFasm_Item _r2;
            var _arg2_0 = "use language";

            _r2 = _MemoCall(_memo, "KW", _index, KW, new _ParseFasm_Item[] { new _ParseFasm_Item(_arg2_0) });

            if (_r2 != null) _index = _r2.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Name
            _ParseFasm_Item _r4;

            _r4 = _MemoCall(_memo, "Name", _index, Name, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND lang
            lang = _memo.Results.Peek();

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
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return lang; }, _r0), true) );
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

            // CALL Block
            var _start_i5 = _index;
            _ParseFasm_Item _r5;

            _r5 = _MemoCall(_memo, "Block", _index, Block, new _ParseFasm_Item[] { indentation, new _ParseFasm_Item(DefineThunkPrelude), new _ParseFasm_Item(AllowedStatementsInDefineThunk) });

            if (_r5 != null) _index = _r5.NextIndex;

            // BIND body
            body = _memo.Results.Peek();

            // ACT
            var _r3 = _memo.Results.Peek();
            if (_r3 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { var inner = (Block) body;
         return new DefineThunkNode(((NameNode)inner.Prelude).Name, inner.Statements); }, _r3), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void DefineThunkPrelude(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            _ParseFasm_Item name = null;

            // AND 1
            int _start_i1 = _index;

            // CALL KW
            var _start_i2 = _index;
            _ParseFasm_Item _r2;
            var _arg2_0 = "define.thunk";

            _r2 = _MemoCall(_memo, "KW", _index, KW, new _ParseFasm_Item[] { new _ParseFasm_Item(_arg2_0) });

            if (_r2 != null) _index = _r2.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label1; }

            // CALLORVAR Name
            _ParseFasm_Item _r4;

            _r4 = _MemoCall(_memo, "Name", _index, Name, null);

            if (_r4 != null) _index = _r4.NextIndex;

            // BIND name
            name = _memo.Results.Peek();

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
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return name; }, _r0), true) );
            }

        }


        public void AllowedStatementsInDefineThunk(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // NOT 0
            int _start_i0 = _index;

            // ANY
            _ParseAny(_memo, ref _index);

            // QUES
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _memo.Results.Push(new _ParseFasm_Item(_index, _memo.InputEnumerable)); }

            // NOT 0
            var _r0 = _memo.Results.Pop();
            _memo.Results.Push( _r0 == null ? new _ParseFasm_Item(_start_i0, _memo.InputEnumerable) : null);
            _index = _start_i0;

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


        public void PassStatement(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            // CALL Statement
            var _start_i1 = _index;
            _ParseFasm_Item _r1;
            var _arg1_0 = "pass";

            _r1 = _MemoCall(_memo, "Statement", _index, Statement, new _ParseFasm_Item[] { new _ParseFasm_Item(_arg1_0) });

            if (_r1 != null) _index = _r1.NextIndex;

            // ACT
            var _r0 = _memo.Results.Peek();
            if (_r0 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r0.StartIndex, _r0.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new PassStatement(); }, _r0), true) );
            }

        }


        public void Statement(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item rule = null;
            _ParseFasm_Item r = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND rule
            rule = _memo.ArgResults.Peek();

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 4
            int _start_i4 = _index;

            // CALLORVAR rule
            _ParseFasm_Item _r6;

            if (rule.Production != null)
            {
                var _p6 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)rule.Production; // what type safety?
                _r6 = _MemoCall(_memo, rule.Production.Method.Name, _index, _p6, null);
            }
            else
            {
                _r6 = _ParseLiteralObj(_memo, ref _index, rule.Inputs);
            }

            if (_r6 != null) _index = _r6.NextIndex;

            // BIND r
            r = _memo.Results.Peek();

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label4; }

            // CALLORVAR NL
            _ParseFasm_Item _r7;

            _r7 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r7 != null) _index = _r7.NextIndex;

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
                _memo.Results.Push( new _ParseFasm_Item(_r3.StartIndex, _r3.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return r; }, _r3), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void Block(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indentation = null;
            _ParseFasm_Item prelude = null;
            _ParseFasm_Item allowed_statements = null;
            _ParseFasm_Item body = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // AND 1
            int _start_i1 = _arg_index;

            // AND 2
            int _start_i2 = _arg_index;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indentation
            indentation = _memo.ArgResults.Peek();

            // AND shortcut
            if (_memo.ArgResults.Peek() == null) { _memo.ArgResults.Push(null); goto label2; }

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND prelude
            prelude = _memo.ArgResults.Peek();

        label2: // AND
            var _r2_2 = _memo.ArgResults.Pop();
            var _r2_1 = _memo.ArgResults.Pop();

            if (_r2_1 != null && _r2_2 != null)
            {
                _memo.ArgResults.Push(new _ParseFasm_Item(_start_i2, _arg_index, _r2_1.Inputs.Concat(_r2_2.Inputs), _r2_1.Results.Concat(_r2_2.Results).Where(_NON_NULL), false));
            }
            else
            {
                _memo.ArgResults.Push(null);
                _arg_index = _start_i2;
            }

            // AND shortcut
            if (_memo.ArgResults.Peek() == null) { _memo.ArgResults.Push(null); goto label1; }

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND allowed_statements
            allowed_statements = _memo.ArgResults.Peek();

        label1: // AND
            var _r1_2 = _memo.ArgResults.Pop();
            var _r1_1 = _memo.ArgResults.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.ArgResults.Push(new _ParseFasm_Item(_start_i1, _arg_index, _r1_1.Inputs.Concat(_r1_2.Inputs), _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), false));
            }
            else
            {
                _memo.ArgResults.Push(null);
                _arg_index = _start_i1;
            }

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // AND 10
            int _start_i10 = _index;

            // AND 11
            int _start_i11 = _index;

            // AND 12
            int _start_i12 = _index;

            // AND 13
            int _start_i13 = _index;

            // CALLORVAR indentation
            _ParseFasm_Item _r14;

            if (indentation.Production != null)
            {
                var _p14 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)indentation.Production; // what type safety?
                _r14 = _MemoCall(_memo, indentation.Production.Method.Name, _index, _p14, null);
            }
            else
            {
                _r14 = _ParseLiteralObj(_memo, ref _index, indentation.Inputs);
            }

            if (_r14 != null) _index = _r14.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label13; }

            // CALLORVAR prelude
            _ParseFasm_Item _r16;

            if (prelude.Production != null)
            {
                var _p16 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)prelude.Production; // what type safety?
                _r16 = _MemoCall(_memo, prelude.Production.Method.Name, _index, _p16, null);
            }
            else
            {
                _r16 = _ParseLiteralObj(_memo, ref _index, prelude.Inputs);
            }

            if (_r16 != null) _index = _r16.NextIndex;

            // BIND prelude
            prelude = _memo.Results.Peek();

        label13: // AND
            var _r13_2 = _memo.Results.Pop();
            var _r13_1 = _memo.Results.Pop();

            if (_r13_1 != null && _r13_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i13, _index, _memo.InputEnumerable, _r13_1.Results.Concat(_r13_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i13;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label12; }

            // LITERAL ":"
            _ParseLiteralString(_memo, ref _index, ":");

        label12: // AND
            var _r12_2 = _memo.Results.Pop();
            var _r12_1 = _memo.Results.Pop();

            if (_r12_1 != null && _r12_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i12, _index, _memo.InputEnumerable, _r12_1.Results.Concat(_r12_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i12;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label11; }

            // CALLORVAR NL
            _ParseFasm_Item _r18;

            _r18 = _MemoCall(_memo, "NL", _index, NL, null);

            if (_r18 != null) _index = _r18.NextIndex;

        label11: // AND
            var _r11_2 = _memo.Results.Pop();
            var _r11_1 = _memo.Results.Pop();

            if (_r11_1 != null && _r11_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i11, _index, _memo.InputEnumerable, _r11_1.Results.Concat(_r11_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i11;
            }

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label10; }

            // CALL Body
            var _start_i20 = _index;
            _ParseFasm_Item _r20;

            _r20 = _MemoCall(_memo, "Body", _index, Body, new _ParseFasm_Item[] { indentation, allowed_statements });

            if (_r20 != null) _index = _r20.NextIndex;

            // BIND body
            body = _memo.Results.Peek();

        label10: // AND
            var _r10_2 = _memo.Results.Pop();
            var _r10_1 = _memo.Results.Pop();

            if (_r10_1 != null && _r10_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i10, _index, _memo.InputEnumerable, _r10_1.Results.Concat(_r10_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i10;
            }

            // ACT
            var _r9 = _memo.Results.Peek();
            if (_r9 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r9.StartIndex, _r9.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return new Block(prelude.Results.Single(), body.Results.Cast<Statement>()); }, _r9), true) );
            }

        label0: // ARGS 0
            _arg_input_index = _arg_index; // no-op for label

        }


        public void Body(_ParseFasm_Memo _memo, int _index, _ParseFasm_Args _args)
        {

            int _arg_index = 0;
            int _arg_input_index = 0;

            _ParseFasm_Item indentation = null;
            _ParseFasm_Item allowed_statements = null;
            _ParseFasm_Item statements = null;

            // ARGS 0
            _arg_index = 0;
            _arg_input_index = 0;

            // AND 1
            int _start_i1 = _arg_index;

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND indentation
            indentation = _memo.ArgResults.Peek();

            // AND shortcut
            if (_memo.ArgResults.Peek() == null) { _memo.ArgResults.Push(null); goto label1; }

            // ANY
            _ParseAnyArgs(_memo, ref _arg_index, ref _arg_input_index, _args);

            // BIND allowed_statements
            allowed_statements = _memo.ArgResults.Peek();

        label1: // AND
            var _r1_2 = _memo.ArgResults.Pop();
            var _r1_1 = _memo.ArgResults.Pop();

            if (_r1_1 != null && _r1_2 != null)
            {
                _memo.ArgResults.Push(new _ParseFasm_Item(_start_i1, _arg_index, _r1_1.Inputs.Concat(_r1_2.Inputs), _r1_1.Results.Concat(_r1_2.Results).Where(_NON_NULL), false));
            }
            else
            {
                _memo.ArgResults.Push(null);
                _arg_index = _start_i1;
            }

            if (_memo.ArgResults.Pop() == null)
            {
                _memo.Results.Push(null);
                goto label0;
            }

            // OR 8
            int _start_i8 = _index;

            // AND 9
            int _start_i9 = _index;

            // CALL Indent
            var _start_i10 = _index;
            _ParseFasm_Item _r10;

            _r10 = _MemoCall(_memo, "Indent", _index, Indent, new _ParseFasm_Item[] { indentation });

            if (_r10 != null) _index = _r10.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label9; }

            // CALLORVAR PassStatement
            _ParseFasm_Item _r11;

            _r11 = _MemoCall(_memo, "PassStatement", _index, PassStatement, null);

            if (_r11 != null) _index = _r11.NextIndex;

        label9: // AND
            var _r9_2 = _memo.Results.Pop();
            var _r9_1 = _memo.Results.Pop();

            if (_r9_1 != null && _r9_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i9, _index, _memo.InputEnumerable, _r9_1.Results.Concat(_r9_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i9;
            }

            // OR shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Pop(); _index = _start_i8; } else goto label8;

            // PLUS 12
            int _start_i12 = _index;
            var _res12 = Enumerable.Empty<ParseTree>();
        label12:

            // AND 13
            int _start_i13 = _index;

            // CALL Indent
            var _start_i14 = _index;
            _ParseFasm_Item _r14;

            _r14 = _MemoCall(_memo, "Indent", _index, Indent, new _ParseFasm_Item[] { indentation });

            if (_r14 != null) _index = _r14.NextIndex;

            // AND shortcut
            if (_memo.Results.Peek() == null) { _memo.Results.Push(null); goto label13; }

            // CALLORVAR allowed_statements
            _ParseFasm_Item _r15;

            if (allowed_statements.Production != null)
            {
                var _p15 = (System.Action<_ParseFasm_Memo, int, IEnumerable<_ParseFasm_Item>>)(object)allowed_statements.Production; // what type safety?
                _r15 = _MemoCall(_memo, allowed_statements.Production.Method.Name, _index, _p15, null);
            }
            else
            {
                _r15 = _ParseLiteralObj(_memo, ref _index, allowed_statements.Inputs);
            }

            if (_r15 != null) _index = _r15.NextIndex;

        label13: // AND
            var _r13_2 = _memo.Results.Pop();
            var _r13_1 = _memo.Results.Pop();

            if (_r13_1 != null && _r13_2 != null)
            {
                _memo.Results.Push( new _ParseFasm_Item(_start_i13, _index, _memo.InputEnumerable, _r13_1.Results.Concat(_r13_2.Results).Where(_NON_NULL), true) );
            }
            else
            {
                _memo.Results.Push(null);
                _index = _start_i13;
            }

            // PLUS 12
            var _r12 = _memo.Results.Pop();
            if (_r12 != null)
            {
                _res12 = _res12.Concat(_r12.Results);
                goto label12;
            }
            else
            {
                if (_index > _start_i12)
                    _memo.Results.Push(new _ParseFasm_Item(_start_i12, _index, _memo.InputEnumerable, _res12.Where(_NON_NULL), true));
                else
                    _memo.Results.Push(null);
            }

        label8: // OR
            int _dummy_i8 = _index; // no-op for label

            // BIND statements
            statements = _memo.Results.Peek();

            // ACT
            var _r6 = _memo.Results.Peek();
            if (_r6 != null)
            {
                _memo.Results.Pop();
                _memo.Results.Push( new _ParseFasm_Item(_r6.StartIndex, _r6.NextIndex, _memo.InputEnumerable, _Thunk(_IM_Result => { return statements; }, _r6), true) );
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

