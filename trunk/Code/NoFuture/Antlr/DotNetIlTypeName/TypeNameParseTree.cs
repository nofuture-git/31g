using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace NoFuture.Antlr.DotNetIlTypeName
{
    public class TypeNameParseTree : DotNetIlTypeNameBaseListener
    {
        /// <summary>
        /// Helper method to get parsed results without having a reference to NoFuture.Antlr
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static NfTypeNameParseItem ParseIl(string typeName)
        {
            if (String.IsNullOrWhiteSpace(typeName))
                return null;

            var rslts = TypeNameParseTree.InvokeParse(typeName);
            if (rslts == null || !rslts.Any())
                return null;

            return rslts.First();
        }

        protected ParseTreeProperty<NfTypeNameParseItem> MyDotNetNames = new ParseTreeProperty<NfTypeNameParseItem>();
        protected ParseTreeProperty<NfTypeNameParseItem> MyGenericArgs = new ParseTreeProperty<NfTypeNameParseItem>();

        public IEnumerable<NfTypeNameParseItem> Results { get; set; }

        public override void ExitUtPrius(DotNetIlTypeNameParser.UtPriusContext context)
        {
            var results = new List<NfTypeNameParseItem>();
            if (context.dotNetGenericName() != null)
            {
                var tnpi = MyDotNetNames.Get(context.dotNetGenericName());
                if (tnpi != null)
                    results.Add(tnpi);
            }
            if (context.dotNetAsmTypeName() != null)
            {
                var tnpi = MyDotNetNames.Get(context.dotNetAsmTypeName());
                if (tnpi != null)
                    results.Add(tnpi);
            }
            if (context.dotNetAsmName() != null)
            {
                var netName = new NfTypeNameParseItem
                {
                    AssemblyFullName = ConcatDotNetAsmName(context.dotNetAsmName()),
                    PublicKeyTokenValue = GetPublicKeyTokenValue(context.dotNetAsmName())
                };
                results.Add(netName);
            }
            if (context.dotNetName() != null)
            {
                var netName = new NfTypeNameParseItem {FullName = ConcatDotNetName(context.dotNetName())};
                results.Add(netName);
            }

            Results = results;
        }

        public override void ExitDotNetAsmTypeName(DotNetIlTypeNameParser.DotNetAsmTypeNameContext context)
        {
            var netName = MyDotNetNames.RemoveFrom(context) ?? new NfTypeNameParseItem();
            netName.AssemblyFullName = ConcatDotNetAsmName(context.dotNetAsmName());
            netName.PublicKeyTokenValue = GetPublicKeyTokenValue(context.dotNetAsmName());
            netName.FullName = ConcatDotNetName(context.dotNetName());
            MyDotNetNames.Put(context, netName);
        }

        public override void ExitDotNetGenericName(DotNetIlTypeNameParser.DotNetGenericNameContext context)
        {
            var netName = MyDotNetNames.RemoveFrom(context) ?? new NfTypeNameParseItem();
            netName.FullName = ConcatDotNetName(context.dotNetName());
            netName.AssemblyFullName = ConcatDotNetAsmName(context.dotNetAsmName());
            netName.PublicKeyTokenValue = GetPublicKeyTokenValue(context.dotNetAsmName());
            var fk = context.GENERIC_COUNTER().GetText() ?? "0";
            fk = fk.Replace("`", String.Empty);
            byte genCount;
            if (Byte.TryParse(fk, out genCount) && genCount > 0)
                netName.GenericCounter = genCount;

            var genericArgs = new List<NfTypeNameParseItem>();
            foreach (var genArg in context.dotNetGenericArg())
            {
                var genNetName = MyGenericArgs.Get(genArg);
                if (genNetName == null)
                    continue;
                genericArgs.Add(genNetName);
            }
            netName.GenericItems = genericArgs;
            MyDotNetNames.Put(context, netName);
        }

        public override void ExitDotNetGenericArg(DotNetIlTypeNameParser.DotNetGenericArgContext context)
        {
            if (context.dotNetAsmTypeName() != null && MyDotNetNames.Get(context.dotNetAsmTypeName()) != null)
            {
                var myName = MyDotNetNames.RemoveFrom(context.dotNetAsmTypeName());
                if (myName != null)
                    MyGenericArgs.Put(context, myName);

            }
            if (context.dotNetGenericName() != null && MyDotNetNames.Get(context.dotNetGenericName()) != null)
            {
                var myName = MyDotNetNames.RemoveFrom(context.dotNetGenericName());
                if(myName != null)
                    MyGenericArgs.Put(context, myName);
            }

            if (context.dotNetName() != null)
            {
                var netName = new NfTypeNameParseItem {FullName = ConcatDotNetName(context.dotNetName())};
                MyGenericArgs.Put(context, netName);
            }

        }

        internal static string GetPublicKeyTokenValue(DotNetIlTypeNameParser.DotNetAsmNameContext context)
        {
            if (context?.asmPubToken() == null)
                return "null";
            return context.asmPubToken().tokenValue().GetText() ?? "null";
        }

        internal static string ConcatDotNetAsmName(DotNetIlTypeNameParser.DotNetAsmNameContext context)
        {
            return context?.GetText();
        }

        internal static string ConcatDotNetName(DotNetIlTypeNameParser.DotNetNameContext context)
        {
            return context == null ? String.Empty : context.GetText();
        }

        public static IEnumerable<NfTypeNameParseItem> InvokeParse(string typeName)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(typeName));
            return InvokeParse(ms);
        }

        public static IEnumerable<NfTypeNameParseItem> InvokeParse(Stream stream)
        {
            var input = new AntlrInputStream(stream);
            var lexer = new DotNetIlTypeNameLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new DotNetIlTypeNameParser(tokens);

            var tree = parser.utPrius();

            var walker = new ParseTreeWalker();
            var loader = new TypeNameParseTree();

            walker.Walk(loader, tree);

            return loader.Results;
        }
    }
}
