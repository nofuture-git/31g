using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoFuture.Exceptions;
using NoFuture.Shared;
using System.Diagnostics;
using NoFuture.Util.Binary;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenIds : CmdBase<TokenIds>
    {
        public override byte[] Execute(byte[] arg)
        {
            Program.PrintToConsole("GetTokenIds invoked");
            Program.ProgressMessageState = null;
            try
            {
                var cProc = Process.GetCurrentProcess();
                var asm = GetAssembly(arg);

                var asmTypes = asm.NfGetTypes(false, Program.LogFile);
                var tokens = new List<MetadataTokenId>();
                var counter = 0;
                var total = asmTypes.Length;
                foreach (var asmType in asmTypes)
                {
                    counter += 1;
                    Program.ReportProgress(new ProgressMessage
                    {
                        Activity = asmType.FullName,
                        ProcName = cProc.ProcessName,
                        ProgressCounter = Etc.CalcProgressCounter(counter, total),
                        Status = "Getting top-level types"
                    });
                    tokens.Add(AssemblyAnalysis.GetMetadataToken(asmType,0));

                }

                if (string.IsNullOrWhiteSpace(Program.AssemblyNameRegexPattern))
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
                foreach (var iToken in callTokens)
                {
                    counter += 1;
                    Program.ReportProgress(new ProgressMessage
                    {
                        Activity = string.Format("{0}.{1}", iToken.RslvAsmIdx, iToken.Id),
                        ProcName = cProc.ProcessName,
                        ProgressCounter = Etc.CalcProgressCounter(counter, total),
                        Status = "Getting call-of-calls"
                    });
                    var stackTrc = new Stack<MetadataTokenId>();
                    ResolveCallOfCall(iToken, ref countDepth, stackTrc);
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
                Program.PrintToConsole(ex);
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
        internal static void ResolveCallOfCall(MetadataTokenId token, ref int depth, Stack<MetadataTokenId> stackTrc)
        {
            //when no pattern on which to match - no call-of-call to resolve
            if (string.IsNullOrWhiteSpace(Program.AssemblyNameRegexPattern))
                return;

            //detect if we are in a recursive call
            if (stackTrc.Any(x => x.Equals(token)))
                return;

            stackTrc.Push(new MetadataTokenId {Id = token.Id, RslvAsmIdx = token.RslvAsmIdx});

            //increment the current depth
            depth += 1;

            //abort if max depth has been reached
            if (depth > Program.MaxRecursionDepth)
            {
                depth -= 1;
                Program.PrintToConsole(
                    String.Format("Max Recursion Depth @ {0}.{1}\n", token.RslvAsmIdx, token.Id));
                return;
            }

            //when there are already Items then leave them as is
            if (token.Items != null && token.Items.Length > 0)
            {
                depth -= 1;
                return;
            }

            //don't waste clock cycles on Ignore types
            if (Program.DisolutionCache.Contains(token))
            {
                depth -= 1;
                return;
            }

            //resolve token to name
            MetadataTokenName tokenName = null;
            if (Program.TokenId2NameCache.ContainsKey(token))
            {
                tokenName = Program.TokenId2NameCache[token];
            }
            else
            {
                // the token must be resolvable with the its manifest module
                var resolveRslt = UtilityMethods.ResolveSingleTokenName(token, out tokenName);
                if (!resolveRslt || tokenName == null)
                {
                    Program.DisolutionCache.Add(token);
                    depth -= 1;
                    return;
                }
                Program.TokenId2NameCache.Add(token, tokenName);
            }

            //only proceed to find calls of calls, types are resolved elsewhere
            if (!tokenName.IsMethodName())
            {
                depth -= 1;
                return;
            }

            //match is on Asm Name, not type nor member name
            var owningAsmName = Program.AsmIndicies.Asms.FirstOrDefault(x => x.IndexId == tokenName.OwnAsmIdx);
            if (owningAsmName == null)
            {
                Program.DisolutionCache.Add(token);
                depth -= 1;
                return;
            }

            //check for match of asm name to user defined regex
            if (!Regex.IsMatch(owningAsmName.AssemblyName, Program.AssemblyNameRegexPattern))
            {
                Program.DisolutionCache.Add(token);
                depth -= 1;
                return;
            }

            //resolve token to runtime member info
            MemberInfo mi;
            var rtMiRslt =
                UtilityMethods.TryResolveRtMemberInfo(Program.AsmIndicies.GetAssemblyByIndex(owningAsmName.IndexId),
                    tokenName.Name, out mi);
            if (!rtMiRslt)
            {
                Program.DisolutionCache.Add(token);
                depth -= 1;
                return;
            }

            //get the token as it is called in its own assembly
            var r = AssemblyAnalysis.GetMetadataToken(mi, true, tokenName.OwnAsmIdx);
            if (stackTrc.Any(x => x.Equals(r)))
            {
                depth -= 1;
                return;
            }
            
            //this token differs from the method arg 'token'
            stackTrc.Push(new MetadataTokenId{Id = r.Id, RslvAsmIdx = r.RslvAsmIdx});

            //unwind when its a terminal token
            if (r.Items == null || r.Items.Length <= 0)
            {
                depth -= 1;
                return;
            }

            //these token ids are only resolvable to the asm who owns MemberInfo(mi)
            token.Items = r.Items;

            //recurse each of these calls-of-calls[...]-of-calls
            foreach (var iToken in token.Items)
            {
                ResolveCallOfCall(iToken, ref depth, stackTrc);
            }

            depth -= 1;
        }


        internal static Assembly GetAssembly(byte[] arg)
        {
            Assembly asm;
            if (Program.AsmInited != true)
            {
                throw new ItsDeadJim("no assemblies are loaded - call GetAsmIndices");

            }
            if (arg == null || arg.Length <= 0)
            {
                asm = Program.RootAssembly;
            }
            else
            {
                var crit = JsonConvert.DeserializeObject<GetTokenIdsCriteria>(Encoding.UTF8.GetString(arg));

                var asmName = crit.AsmName;
                Program.AssemblyNameRegexPattern = crit.ResolveAllNamedLike;

                if (string.IsNullOrWhiteSpace(asmName))
                {
                    throw new ItsDeadJim("could not deserialize the byte array");
                }

                var allAsms = Constants.UseReflectionOnlyLoad
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
