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
        private ParseTreeProperty<string> _parseTreeProperty = new ParseTreeProperty<string>();
        public override void EnterMethod_member_name(CSharp4Parser.Method_member_nameContext context)
        {
            Results.MethodNames.Add(context.GetText());
        }

        public override void EnterNamespace_name(CSharp4Parser.Namespace_nameContext context)
        {
            Results.NamespaceNames.Add(context.GetText());
        }

        public override void EnterClass_declaration(CSharp4Parser.Class_declarationContext context)
        {
            Results.ClassNames.Add(context.identifier().GetText());
        }

        public override void EnterClass_definition(CSharp4Parser.Class_definitionContext context)
        {
            Results.ClassNames.Add(context.identifier().GetText());
        }

        public override void EnterMethod_declaration(CSharp4Parser.Method_declarationContext context)
        {
            
            var nmContext = context.children.FirstOrDefault(x => x is CSharp4Parser.Method_member_nameContext);
            if (string.IsNullOrWhiteSpace(nmContext?.GetText()))
                return;
        }

        public override void EnterMethod_declaration2(CSharp4Parser.Method_declaration2Context context)
        {
            var nmContext = context.children.FirstOrDefault(x => x is CSharp4Parser.Method_member_nameContext);
            if (string.IsNullOrWhiteSpace(nmContext?.GetText()))
                return;
        }

        public override void EnterClass_member_declaration(CSharp4Parser.Class_member_declarationContext context)
        {

            var cmdecl = context.common_member_declaration();

            if (cmdecl == null)
                return;
            var nm = GetMemberName(cmdecl);
            if (string.IsNullOrWhiteSpace(nm))
                return;
            var methodBody = new CsharpParseItem {Name = nm};

            var acMods = context.all_member_modifiers();
            if (acMods != null)
            {
                foreach(var m in acMods.children)
                    methodBody.AccessModifiers.Add(m.GetText());
            }

            var attrs = context.attributes();
            if (attrs != null)
            {
                foreach(var a in attrs.children)
                    methodBody.Attributes.Add(a.GetText());
            }

            var paramNames = GetParameterType2Names(cmdecl);
            if(paramNames != null && paramNames.Any())
                methodBody.Parameters.AddRange(paramNames);

            methodBody.Start = new Tuple<int,int>(context.Start.Line, context.Start.Column);
            methodBody.End = new Tuple<int, int>(context.Stop.Line, context.Stop.Column);
            Results.ClassMemberBodies.Add(methodBody);
        }

        public string GetMemberName(IParseTree context)
        {
            if (context == null)
                return null;
            if (context is CSharp4Parser.Method_member_nameContext)
            {
                return context.GetText();
            }

            if (context.ChildCount <= 0)
                return null;

            for (var i = 0; i < context.ChildCount; i++)
            {
                var nm = GetMemberName(context.GetChild(i));
                if (!string.IsNullOrWhiteSpace(nm))
                    return nm;
            }

            return null;
        }

        public string[] GetParameterType2Names(IParseTree context)
        {
            if (context == null)
                return null;
            var plCtx = context as CSharp4Parser.Formal_parameter_listContext;
            if (plCtx!= null)
            {
                var sl = new List<string>();
                for (var i = 0; i < plCtx.ChildCount; i++)
                {
                    var plChild = plCtx.GetChild(i) as ParserRuleContext;
                    if(plChild == null)
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

            return results;
        }
    }
}
