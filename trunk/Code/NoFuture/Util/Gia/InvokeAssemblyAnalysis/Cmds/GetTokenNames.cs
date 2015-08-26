using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds
{
    public class GetTokenNames : CmdBase<TokenNames>
    {
        public override byte[] Execute(byte[] arg)
        {
            Program.PrintToConsole("GetTokenNames invoked");
            try
            {
                if (Program.AsmInited != true)
                {
                    Program.PrintToConsole("no assemblies are loaded - call GetAsmIndices");
                    return EncodedResponse(
                            new TokenNames
                            {
                                Msg = "no assemblies are loaded - call GetAsmIndices",
                                St = MetadataTokenStatus.Error
                            });

                }

                var tokens = JsonConvert.DeserializeObject<int[]>(Encoding.UTF8.GetString(arg));

                if (tokens == null || tokens.Length <= 0)
                {
                    return EncodedResponse(new TokenNames
                    {
                        Msg = "parse failed",
                        St = MetadataTokenStatus.Error
                    });
                }

                if (Program.ManifestModule == null)
                    return EncodedResponse(new TokenNames
                    {
                        Msg = "the Manifest Module is null",
                        St = MetadataTokenStatus.Error
                    });
                var names = new List<MetadataTokenName>();
                for (var i = 0; i < tokens.Length; i++)
                {
                    try
                    {
                        var cid = tokens[i];
                        
                        if (Program.DisolutionCache.Contains(cid))
                            continue;

                        if (Program.ResolutionCache.ContainsKey(cid))
                        {
                            names.Add(Program.ResolutionCache[cid]);
                            continue;
                        }
                        var tokenName = new MetadataTokenName() {Id = cid};
                        var resolved = ResolveSingleToken(tokenName);

                        if (!resolved)
                        {
                            if(!Program.DisolutionCache.Contains(cid))
                                Program.DisolutionCache.Add(cid);
                            continue;
                        }

                        if (!Program.ResolutionCache.ContainsKey(cid))
                        {
                            Program.ResolutionCache.Add(cid, tokenName);
                            names.Add(tokenName);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                
                return EncodedResponse(new TokenNames(){Names = names.ToArray()});
            }
            catch (Exception ex)
            {
                Program.PrintToConsole(ex);
                return EncodedResponse(new TokenNames
                {
                    Msg = string.Format(ex.Message),
                    St = MetadataTokenStatus.Error
                });
            }
        }
    }
}
