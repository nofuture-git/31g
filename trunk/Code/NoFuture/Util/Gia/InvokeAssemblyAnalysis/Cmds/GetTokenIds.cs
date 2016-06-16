using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;
using System.Diagnostics;
using NoFuture.Globals;
using NoFuture.Util.Binary;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenIds : CmdBase<TokenIds>, ICmd
    {
        public GetTokenIds(Program myProgram)
            : base(myProgram)
        {
        }

        public override byte[] Execute(byte[] arg)
        {
            MyProgram.PrintToConsole("GetTokenIds invoked");
            MyProgram.ProgressMessageState = null;
            try
            {
                var cProc = Process.GetCurrentProcess();
                var asm = GetAssembly(arg);

                var asmTypes = asm.NfGetTypes(false, MyProgram.LogFile);
                var tokens = new List<MetadataTokenId>();
                var counter = 0;
                var total = asmTypes.Length;
                MyProgram.PrintToConsole(string.Format("There are {0} types in the assembly", total));
                foreach (var asmType in asmTypes)
                {
                    counter += 1;
                    ((IaaProgram)MyProgram).ReportProgress(new ProgressMessage
                    {
                        Activity = asmType.FullName,
                        ProcName = cProc.ProcessName,
                        ProgressCounter = Etc.CalcProgressCounter(counter, total),
                        Status = "Getting top-level types"
                    });
                    tokens.Add(AssemblyAnalysis.GetMetadataToken(asmType,0));

                }

                Console.Write('\n');
                if (string.IsNullOrWhiteSpace(((IaaProgram)MyProgram).AssemblyNameRegexPattern))
                {
                    return EncodedResponse(
                        new TokenIds
                        {
                            Tokens = tokens.ToArray()
                        });
                }

                var countDepth = 0;
                var callTokens = tokens.SelectMany(x => x.Items.SelectMany(y => y.Items)).ToArray();
                counter = 0;
                total = callTokens.Length;
                MyProgram.PrintToConsole(string.Format("There are {0} call-of-call tokens", total));
                foreach (var iToken in callTokens)
                {
                    counter += 1;
                    ((IaaProgram)MyProgram).ReportProgress(new ProgressMessage
                    {
                        Activity = string.Format("{0}.{1}", iToken.RslvAsmIdx, iToken.Id),
                        ProcName = cProc.ProcessName,
                        ProgressCounter = Etc.CalcProgressCounter(counter, total),
                        Status = "Getting call-of-calls"
                    });
                    var stackTrc = new Stack<MetadataTokenId>();
                    ResolveCallOfCall(iToken, ref countDepth, stackTrc, null);
                }
                Console.Write('\n');
                return EncodedResponse(
                    new TokenIds
                    {
                        Tokens = tokens.ToArray()
                    });
            }
            catch (Exception ex)
            {
                Console.Write('\n');
                MyProgram.PrintToConsole(ex);
                return EncodedResponse(
                    new TokenIds
                    {
                        Msg = ex.Message,
                        St = MetadataTokenStatus.Error
                    });
            }
        }

        /// <summary>
        /// Resolves the outbound call stack for the given <see cref="token"/>
        /// </summary>
        /// <param name="token"></param>
        /// <param name="depth"></param>
        /// <param name="stackTrc">For detecting recursive call patterns</param>
        /// <param name="msgOut">For getting details on recursion.</param>
        internal void ResolveCallOfCall(MetadataTokenId token, ref int depth, Stack<MetadataTokenId> stackTrc, StringBuilder msgOut)
        {
            if (msgOut != null)
            {
                if(depth > 0)
                    msgOut.Append(new string(' ', depth));

                msgOut.AppendFormat("Depth:{0}", depth);
                msgOut.AppendFormat(", Token:{0}.0x{1}", token.RslvAsmIdx, token.Id.ToString("X4"));
            }
            //detect if we are in a recursive call
            if (stackTrc.Any(x => x.Equals(token)))
            {
                if (msgOut != null)
                    msgOut.AppendLine(", Message:'present in stack trace'");
                return;
            }

            stackTrc.Push(new MetadataTokenId {Id = token.Id, RslvAsmIdx = token.RslvAsmIdx});

            //increment the current depth
            depth += 1;

            //abort if max depth has been reached
            if (depth > ((IaaProgram)MyProgram).MaxRecursionDepth)
            {
                depth -= 1;
                MyProgram.PrintToConsole(
                    String.Format("Max Recursion Depth @ {0}.{1}\n", token.RslvAsmIdx, token.Id));
                if(msgOut != null)
                    msgOut.AppendLine(", Message:'max recursion depth'");
                return;
            }

            //when there are already Items then leave them as is
            if (token.Items != null && token.Items.Length > 0)
            {
                depth -= 1;
                if (msgOut != null)
                    msgOut.AppendLine(", Message:'Items already present'");
                return;
            }

            //don't waste clock cycles on Ignore types
            if (((IaaProgram)MyProgram).DisolutionCache.Contains(token))
            {
                depth -= 1;
                if (msgOut != null)
                    msgOut.AppendLine(", Message:'token in DisolutionCache'");
                return;
            }

            //resolve token to name
            MetadataTokenName tokenName = null;
            if (((IaaProgram)MyProgram).TokenId2NameCache.ContainsKey(token))
            {
                tokenName = ((IaaProgram)MyProgram).TokenId2NameCache[token];
            }
            else
            {
                // the token must be resolvable with the its manifest module
                var resolveRslt = ((IaaProgram)MyProgram).UtilityMethods.ResolveSingleTokenName(token, out tokenName, msgOut);
                if (!resolveRslt || tokenName == null)
                {
                    ((IaaProgram)MyProgram).DisolutionCache.Add(token);
                    depth -= 1;
                    if (msgOut != null)
                        msgOut.AppendLine(", Message:'ResolveSingleTokenName failed'");
                    return;
                }
                ((IaaProgram)MyProgram).TokenId2NameCache.Add(token, tokenName);
            }

            //only proceed to find calls of calls, types are resolved elsewhere
            if (!tokenName.IsMethodName())
            {
                depth -= 1;
                if (msgOut != null)
                    msgOut.AppendLine(", Message:'token is not a method'");
                return;
            }

            //match is on Asm Name, not type nor member name
            var owningAsmName = ((IaaProgram)MyProgram).AsmIndicies.Asms.FirstOrDefault(x => x.IndexId == tokenName.OwnAsmIdx);
            if (owningAsmName == null)
            {
                ((IaaProgram)MyProgram).DisolutionCache.Add(token);
                depth -= 1;
                if (msgOut != null)
                    msgOut.AppendLine(string.Format(", Message:'owning assembly idx {0} has no match'",tokenName.OwnAsmIdx));
                return;
            }

            //check for match of asm name to user defined regex
            if (!Regex.IsMatch(owningAsmName.AssemblyName, ((IaaProgram)MyProgram).AssemblyNameRegexPattern))
            {
                ((IaaProgram)MyProgram).DisolutionCache.Add(token);
                if (msgOut != null)
                    msgOut.AppendLine(string.Format(", Message:'assembly name [{1}] does not match regex [{0}]'",
                        ((IaaProgram)MyProgram).AssemblyNameRegexPattern, owningAsmName.AssemblyName));

                depth -= 1;
                return;
            }

            //resolve token to runtime member info
            MemberInfo mi;
            var rtMiRslt =
                ((IaaProgram)MyProgram).UtilityMethods.TryResolveRtMemberInfo(((IaaProgram)MyProgram).AsmIndicies.GetAssemblyByIndex(owningAsmName.IndexId),
                    tokenName.Name, out mi, msgOut);
            if (!rtMiRslt)
            {
                ((IaaProgram)MyProgram).DisolutionCache.Add(token);
                depth -= 1;
                return;
            }

            //get the token as it is called in its own assembly
            var r = AssemblyAnalysis.GetMetadataToken(mi, true, tokenName.OwnAsmIdx);
            if (stackTrc.Any(x => x.Equals(r)))
            {
                depth -= 1;
                if (msgOut != null)
                    msgOut.AppendLine(string.Format(", Message:'name resolved token id {0}.{1} found in stack trace'",
                        r.RslvAsmIdx, r.Id.ToString("X4")));
                return;
            }
            
            //this token differs from the method arg 'token'
            stackTrc.Push(new MetadataTokenId{Id = r.Id, RslvAsmIdx = r.RslvAsmIdx});

            //unwind when its a terminal token
            if (r.Items == null || r.Items.Length <= 0)
            {
                depth -= 1;
                if (msgOut != null)
                    msgOut.AppendLine(string.Format(", Message:'name resolved token id {0}.{1} is a terminal node'",
                        r.RslvAsmIdx, r.Id.ToString("X4")));

                return;
            }

            //these token ids are only resolvable to the asm who owns MemberInfo(mi)
            token.Items = r.Items;

            //recurse each of these calls-of-calls[...]-of-calls
            foreach (var iToken in token.Items)
            {
                ResolveCallOfCall(iToken, ref depth, stackTrc, msgOut);
            }

            depth -= 1;
        }


        internal Assembly GetAssembly(byte[] arg)
        {
            Assembly asm;
            if (((IaaProgram)MyProgram).AsmInited != true)
            {
                throw new ItsDeadJim("no assemblies are loaded - call GetAsmIndices");

            }
            if (arg == null || arg.Length <= 0)
            {
                asm = ((IaaProgram)MyProgram).RootAssembly;
            }
            else
            {
                var crit = JsonConvert.DeserializeObject<GetTokenIdsCriteria>(Encoding.UTF8.GetString(arg));

                var asmName = crit.AsmName;
                ((IaaProgram)MyProgram).AssemblyNameRegexPattern = crit.ResolveAllNamedLike;

                if (string.IsNullOrWhiteSpace(asmName))
                {
                    throw new ItsDeadJim("could not deserialize the byte array");
                }

                var allAsms = NfConfig.UseReflectionOnlyLoad
                    ? AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    : AppDomain.CurrentDomain.GetAssemblies();

                asm =
                    allAsms.FirstOrDefault(
                        x => string.Equals(x.GetName().FullName, asmName, StringComparison.OrdinalIgnoreCase));

            }

            if (asm == null)
            {
                throw new ItsDeadJim("could not locate a matching assembly");
            }

            return asm;
        }
    }
}
