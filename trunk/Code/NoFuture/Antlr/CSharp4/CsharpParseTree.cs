using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace NoFuture.Antlr.CSharp4
{
    public class CsharpParseTree : CSharp4BaseListener
    {
        public CsharpParseResults Results { get; } = new CsharpParseResults();
        private string _typeNameCurrent;
        private string _namespaceNameCurrent;
        private Tuple<int, int> _typeStartCurrent;
        private Tuple<int, int> _typeBodyStartCurrent;
        private Tuple<int, int> _namespaceStartCurrent;
        private Tuple<int, int> _namespaceBodyStartCurrent;
        private Tuple<int, int> _typeEndCurrent;
        private Tuple<int, int> _namespaceEndCurrent;

        public override void EnterMethod_member_name(CSharp4Parser.Method_member_nameContext context)
        {
            Results.MethodNames.Add(context.GetText());
        }

        public override void EnterNamespace_declaration(CSharp4Parser.Namespace_declarationContext context)
        {
            _namespaceNameCurrent = context.qualified_identifier().GetText();

            var nmspSt = GetBodyStart(context);
            _namespaceBodyStartCurrent = nmspSt;
            _namespaceStartCurrent = new Tuple<int, int>(context.Start.Line, context.Start.Column);
            _namespaceEndCurrent = new Tuple<int, int>(context.Stop.Line, context.Stop.Column);
        }

        public override void ExitNamespace_declaration(CSharp4Parser.Namespace_declarationContext context)
        {
            _namespaceNameCurrent = null;
            _namespaceStartCurrent = null;
            _namespaceBodyStartCurrent = null;
            _namespaceEndCurrent = null;
        }

        public override void EnterNamespace_name(CSharp4Parser.Namespace_nameContext context)
        {
            Results.NamespaceNames.Add(context.GetText());
        }

        public override void EnterType_declaration(CSharp4Parser.Type_declarationContext context)
        {
            string nm = null;
            foreach (var td in new ParserRuleContext[]
            {
                context.class_definition(), context.interface_definition(), context.delegate_definition(),
                context.enum_definition(), context.struct_definition()
            })
            {
                if (td == null)
                    continue;
                nm = GetContextName(td);
                if (!string.IsNullOrWhiteSpace(nm))
                    break;
            }
            Results.TypeNames.Add(nm);
            _typeNameCurrent = nm;

            var typeNameStart = GetBodyStart(context);
            _typeBodyStartCurrent = typeNameStart;
            _typeStartCurrent = new Tuple<int, int>(context.Start.Line, context.Start.Column);
            _typeEndCurrent = new Tuple<int, int>(context.Stop.Line, context.Stop.Column);
        }

        public override void ExitType_declaration(CSharp4Parser.Type_declarationContext context)
        {
            _typeNameCurrent = null;
            _typeStartCurrent = null;
            _typeBodyStartCurrent = null;
            _typeEndCurrent = null;
        }

        public override void EnterInterface_member_declaration(CSharp4Parser.Interface_member_declarationContext context)
        {
            var nm = context.identifier().GetText();
            var parseItem = new CsharpParseItem { Name = nm };
            AssignAttributes(parseItem, context.attributes());
            AssignParameterType2Names(parseItem, context.formal_parameter_list());
            AssignStartAndStop(parseItem, context);
            AssignNsAndTypeValues(parseItem);
            Results.TypeMemberBodies.Add(parseItem);
        }

        public override void EnterStruct_member_declaration(CSharp4Parser.Struct_member_declarationContext context)
        {
            var cmdecl = context.common_member_declaration();

            if (cmdecl == null)
                return;
            var nm = GetContextName(cmdecl);

            var parseItem = new CsharpParseItem { Name = nm };
            AssignAccessMods(parseItem, context.all_member_modifiers());
            AssignAttributes(parseItem, context.attributes());
            AssignParameterType2Names(parseItem, cmdecl); ;
            AssignStartAndStop(parseItem, context);
            AssignNsAndTypeValues(parseItem);
        }

        public override void EnterEnum_member_declaration(CSharp4Parser.Enum_member_declarationContext context)
        {
            var nm = context.identifier().GetText();
            var parseItem = new CsharpParseItem { Name = nm };
            AssignAttributes(parseItem, context.attributes());
            AssignStartAndStop(parseItem, context);
            AssignNsAndTypeValues(parseItem);
            Results.TypeMemberBodies.Add(parseItem);
        }

        public override void EnterClass_member_declaration(CSharp4Parser.Class_member_declarationContext context)
        {
            var cmdecl = context.common_member_declaration();

            if (cmdecl == null)
                return;
            var nm = GetContextName(cmdecl);
            
            var parseItem = new CsharpParseItem {Name = nm};
            AssignAccessMods(parseItem, context.all_member_modifiers());
            AssignAttributes(parseItem, context.attributes());
            AssignParameterType2Names(parseItem, cmdecl);;
            AssignStartAndStop(parseItem, context);
            AssignNsAndTypeValues(parseItem);
            Results.TypeMemberBodies.Add(parseItem);
        }

        protected internal void AssignParameterType2Names(CsharpParseItem parseItem, IParseTree context)
        {
            if (parseItem == null)
                return;

            var paramNames = GetParameterType2Names(context);
            if (paramNames != null && paramNames.Any())
                parseItem.Parameters.AddRange(paramNames);
        }

        protected internal void AssignAccessMods(CsharpParseItem parseItem,
            CSharp4Parser.All_member_modifiersContext acMods)
        {
            if (parseItem == null)
                return;

            if (acMods == null)
                return;
            foreach (var m in acMods.children)
                parseItem.AccessModifiers.Add(m.GetText());
        }

        protected internal void AssignAttributes(CsharpParseItem parseItem, CSharp4Parser.AttributesContext attrs)
        {
            if (parseItem == null)
                return;
            if (attrs == null)
                return;
            foreach (var a in attrs.children)
                parseItem.Attributes.Add(a.GetText());
        }

        protected internal void AssignStartAndStop(CsharpParseItem parseItem, ParserRuleContext context)
        {
            if (parseItem == null)
                return;
            parseItem.BodyStart = GetBodyStart(context);
            parseItem.Start = new Tuple<int, int>(context.Start.Line, context.Start.Column);
            parseItem.End = new Tuple<int, int>(context.Stop.Line, context.Stop.Column);
        }

        protected internal void AssignNsAndTypeValues(CsharpParseItem parseItem)
        {
            if (parseItem == null)
                return;
            parseItem.DeclTypeName = _typeNameCurrent;
            parseItem.Namespace = _namespaceNameCurrent;
            parseItem.DeclBodyStart = _typeBodyStartCurrent;
            parseItem.DeclEnd = _typeEndCurrent;
            parseItem.NamespaceBodyStart = _namespaceBodyStartCurrent;
            parseItem.NamespaceEnd = _namespaceEndCurrent;
            parseItem.DeclStart = _typeStartCurrent;
            parseItem.NamespaceStart = _namespaceStartCurrent;
        }

        public string GetContextName(IParseTree context)
        {
            if (context == null)
                return null;
            if (context is CSharp4Parser.Member_nameContext)
            {
                return context.GetText();
            }
            if (context is CSharp4Parser.Method_member_nameContext)
            {
                return context.GetText();
            }
            if (context is CSharp4Parser.Field_declaration2Context flCtx && flCtx.variable_declarators() != null)
            {
                return flCtx.variable_declarators().GetText();
            }
            if (context is CSharp4Parser.Class_definitionContext)
            {
                return ((CSharp4Parser.Class_definitionContext) context).identifier().GetText();
            }
            if (context is CSharp4Parser.Delegate_definitionContext)
            {
                return ((CSharp4Parser.Delegate_definitionContext)context).identifier().GetText();
            }
            if (context is CSharp4Parser.Enum_definitionContext)
            {
                return ((CSharp4Parser.Enum_definitionContext)context).identifier().GetText();
            }
            if (context is CSharp4Parser.Interface_definitionContext)
            {
                return ((CSharp4Parser.Interface_definitionContext)context).identifier().GetText();
            }
            if (context is CSharp4Parser.Struct_definitionContext)
            {
                return ((CSharp4Parser.Struct_definitionContext)context).identifier().GetText();
            }

            if (context.ChildCount <= 0)
                return null;

            for (var i = 0; i < context.ChildCount; i++)
            {
                var nm = GetContextName(context.GetChild(i));
                if (!string.IsNullOrWhiteSpace(nm))
                    return nm;
            }

            return null;
        }

        public Tuple<int, int> GetBodyStart(IParseTree context)
        {
            if (context is CSharp4Parser.BodyContext
                || context is CSharp4Parser.Method_bodyContext
                || context is CSharp4Parser.Namespace_bodyContext
                || context is CSharp4Parser.Class_bodyContext
                || context is CSharp4Parser.Interface_bodyContext
                || context is CSharp4Parser.Enum_bodyContext
                || context is CSharp4Parser.Struct_bodyContext)
            {
                var plCtx = (ParserRuleContext) context;
                return new Tuple<int, int>(plCtx.Start.Line, plCtx.Start.Column);
            }

            if (context.ChildCount <= 0)
                return null;

            for (var i = 0; i < context.ChildCount; i++)
            {
                var bodyStart = GetBodyStart(context.GetChild(i));
                if (bodyStart != null)
                    return bodyStart;
            }

            return null;
        }

        public string[] GetParameterType2Names(IParseTree context)
        {
            if (context == null)
                return null;
            if (context is CSharp4Parser.Formal_parameter_listContext plCtx)
            {
                var sl = new List<string>();

                if (plCtx.ChildCount == 1 && plCtx.GetChild(0) is CSharp4Parser.Fixed_parametersContext)
                {
                    for (var j = 0; j < plCtx.GetChild(0).ChildCount; j++)
                    {
                        var plChild = plCtx.GetChild(0).GetChild(j) as ParserRuleContext;
                        if (plChild == null)
                            continue;
                        sl.Add($"{plChild.Start.Text} {plChild.Stop.Text}");
                    }

                    if (sl.Any())
                        return sl.ToArray();
                }

                for (var i = 0; i < plCtx.ChildCount; i++)
                {
                    if(!(plCtx.GetChild(i) is ParserRuleContext plChild))
                        continue;
                    sl.Add($"{plChild.Start.Text} {plChild.Stop.Text}");
                }

                return sl.Any() ? sl.ToArray() : null;
            }

            if (context.ChildCount <= 0)
                return null;

            for (var i = 0; i < context.ChildCount; i++)
            {
                var pNames = GetParameterType2Names(context.GetChild(i));
                if (pNames != null)
                    return pNames;
            }

            return null;
        }

        public override void ExitSpecific_catch_clause(CSharp4Parser.Specific_catch_clauseContext context)
        {
            var slk = new StringBuilder();

            slk.Append(context.CATCH().GetText());
            slk.Append(" ");
            slk.Append(context.OPEN_PARENS().GetText());
            slk.Append(context.class_type().GetText());

            if (context.identifier() != null)
            {
                slk.Append(" ");
                slk.Append(context.identifier().GetText());
            }
            slk.Append(context.CLOSE_PARENS().GetText());
            slk.AppendLine();
            var cBlock = context.block();
            

            PrintBlockStatements(slk, cBlock);
            Results.CatchBlocks.Add(slk.ToString());
        }

        public override void ExitGeneral_catch_clause(CSharp4Parser.General_catch_clauseContext context)
        {
            var slk = new StringBuilder();
            slk.Append(context.CATCH().GetText());
            slk.AppendLine();

            var cBlock = context.block();

            PrintBlockStatements(slk, cBlock);
        }

        private void PrintBlockStatements(StringBuilder slk, CSharp4Parser.BlockContext cBlock)
        {
            slk.AppendLine(cBlock.OPEN_BRACE().GetText());
            if (cBlock.statement_list() != null)
            {
                foreach (var stmt in cBlock.statement_list().statement())
                {
                    slk.AppendLine(stmt.GetText());
                }
            }
            slk.AppendLine(cBlock.CLOSE_BRACE().GetText());
            Results.CatchBlocks.Add(slk.ToString());
        }

        public static CsharpParseResults InvokeParse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;
            if (!System.IO.File.Exists(fileName))
                return null;

            var tr = System.IO.File.OpenRead(fileName);
            var input = new AntlrInputStream(tr);
            var lexer = new CSharp4Lexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new CSharp4Parser(tokens);

            var tree = parser.compilation_unit();

            var walker = new ParseTreeWalker();
            var loader = new CsharpParseTree();

            walker.Walk(loader, tree);

            var results = loader.Results;

            tr.Close();
            results.SourceFile = fileName;
            return results;
        }
    }
}
