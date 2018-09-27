﻿using System.Text;
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
