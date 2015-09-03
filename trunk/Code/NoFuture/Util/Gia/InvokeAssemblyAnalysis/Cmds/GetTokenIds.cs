using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using NoFuture.Shared;
using NoFuture.Util.Binary;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenIds : CmdBase<TokenIds>
    {
        public override byte[] Execute(byte[] arg)
        {
            Program.PrintToConsole("GetTokenIds invoked");
            try
            {
                Assembly asm;
                if (Program.AsmInited != true)
                {
                    Program.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return EncodedResponse(
                            new TokenIds
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

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
                        Program.PrintToConsole("could not deserialize the byte array");
                        return EncodedResponse(
                            new TokenIds
                            {
                                Msg = "could not deserialize the byte array",
                                St = MetadataTokenStatus.Error
                            });

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
                    Program.PrintToConsole("could not locate a matching assembly");
                    return EncodedResponse(
                        new TokenIds
                        {
                            Msg = "could not locate a matching assembly",
                            St = MetadataTokenStatus.Error
                        });
                }

                var asmTypes = asm.GetTypes();
                var tokens = asmTypes.Select(x => AssemblyAnalysis.GetMetadataToken(x,0)).ToArray();

                if (string.IsNullOrWhiteSpace(Program.AssemblyNameRegexPattern))
                {
                    return EncodedResponse(
                        new TokenIds
                        {
                            Tokens = tokens
                        });
                }

                int countDepth = 0;
                foreach (var iToken in tokens.SelectMany(x => x.Items.SelectMany(y => y.Items)))
                {
                    ResolveCallOfCall(iToken, ref countDepth);
                }
                return EncodedResponse(
                    new TokenIds
                    {
                        Tokens = tokens
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
        internal static void ResolveCallOfCall(MetadataTokenId token, ref int depth)
        {
            //when no pattern on which to match - no call-of-call to resolve
            if (string.IsNullOrWhiteSpace(Program.AssemblyNameRegexPattern))
                return;
            //increment the current depth
            depth += 1;

            //abort if max depth has been reached
            if (depth > Program.MaxRecursionDepth)
            {
                depth -= 1;
                return;
            }

            //when there are already Items then leave them as is
            if (token.Items != null && token.Items.Length > 0)
            {
                depth -= 1;
                return;
            }

            //don't waste clock cycles on Ignore types
            if (Program.DisolutionCache.Contains(token) || Program.ResolutionCache.Contains(token))
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
                // the token must be resolvable with the current manifest module
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
            var owningAsmName = Program.AsmIndicies.Asms.FirstOrDefault(x => x.IndexId == tokenName.ObyAsmIdx);
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
            var rtMiRslt = UtilityMethods.TryResolveRtMemberInfo(Program.AsmIndicies.GetAssemblyByIndex(owningAsmName.IndexId), tokenName.Name, out mi);
            if (!rtMiRslt)
            {
                Program.DisolutionCache.Add(token);
                depth -= 1;
                return;
            }

            //get this types implementation calls
            var r = AssemblyAnalysis.GetMetadataToken(mi, true, tokenName.ObyAsmIdx);
            if (r.Items == null || r.Items.Length <= 0)
            {
                depth -= 1;
                return;
            }

            //these token ids are only resolvable to the asm who owns MemberInfo(mi)
            token.Items = r.Items;

            //push this types assembly's manifest module to the top
            Program.PushManifestModuleByIdxId(owningAsmName.IndexId);

            //recurse each of these calls-of-calls[...]-of-calls
            foreach (var iToken in token.Items)
            {
                ResolveCallOfCall(iToken, ref depth);
            }

            //pop the manifest module back to what it was upon entry
            Program.PopManifestModuleByIdxId();

            Program.ResolutionCache.Add(token);
            depth -= 1;
        }
    }
}
