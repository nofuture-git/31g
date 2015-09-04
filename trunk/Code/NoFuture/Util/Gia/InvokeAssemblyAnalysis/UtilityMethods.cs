using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Configuration;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis
{
    /// <summary>
    /// Utility methods specific to this console app.
    /// </summary>
    public static class UtilityMethods
    {
        private static string[] _gacAsmNames;

        /// <summary>
        /// Get a list of Assemby names from the <see cref="AppDomain.CurrentDomain"/>
        /// which are flagged as <see cref="System.Reflection.Assembly.GlobalAssemblyCache"/>
        /// </summary>
        /// <remarks>
        /// Results depend on the global switch <see cref="Constants.UseReflectionOnlyLoad"/>
        /// </remarks>
        internal static string[] GacAssemblyNames
        {
            get
            {
                if (_gacAsmNames != null)
                    return _gacAsmNames;

                _gacAsmNames = Constants.UseReflectionOnlyLoad
                    ? AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                        .Where(x => x.GlobalAssemblyCache)
                        .Select(x => x.FullName)
                        .ToArray()
                    : AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => x.GlobalAssemblyCache)
                        .Select(x => x.FullName)
                        .ToArray();

                return _gacAsmNames;
            }
        }

        /// <summary>
        /// Asserts that the <see cref="asmQualName"/> matches 
        /// a name <see cref="GacAssemblyNames"/>
        /// </summary>
        /// <param name="asmQualName"></param>
        /// <returns></returns>
        internal static bool IsIgnore(string asmQualName)
        {
            if (String.IsNullOrWhiteSpace(asmQualName))
                return true;
            var gacAsms = GacAssemblyNames;
            return gacAsms != null && gacAsms.Any(asmQualName.EndsWith);
        }

        /// <summary>
        /// Resolve a port number from the config file's appSettings
        /// on <see cref="appKey"/>
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        internal static int? ResolvePort(string appKey)
        {
            var cval = ConfigurationManager.AppSettings[appKey];
            return ResolveInt(cval);
        }

        internal static int? ResolveInt(string cval)
        {
            int valOut;
            if (!String.IsNullOrWhiteSpace(cval) && Int32.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        internal static bool? ResolveBool(string cval)
        {
            bool valOut;
            if (!String.IsNullOrWhiteSpace(cval) && Boolean.TryParse(cval, out valOut))
                return valOut;
            return null;
        }

        /// <summary>
        /// Transforms a <see cref="MemberInfo"/> into a <see cref="MetadataTokenName"/>
        /// getting as much info as possiable depending on which 
        /// child-type the <see cref="mi"/> resolves to.
        /// </summary>
        /// <param name="mi"></param>
        /// <returns></returns>
        internal static MetadataTokenName ToMetadataTokenName(this MemberInfo mi)
        {
            if (mi == null)
                return null;

            var tokenName = new MetadataTokenName { Name = mi.Name, Label = mi.GetType().Name };

            string asmQualName;
            string asmName;
            string expandedName;

            var type = mi as Type;

            if (type != null)
            {
                asmQualName = type.Assembly.GetName().FullName;
                //do not send back GAC asm's unless asked
                if (!Program.AreGacAssembliesResolved && IsIgnore(asmQualName))
                    return null;

                var t =
                    Program.AsmIndicies.Asms.FirstOrDefault(
                        x =>
                            String.Equals(x.AssemblyName, type.Assembly.GetName().FullName,
                                StringComparison.OrdinalIgnoreCase));

                asmName = type.Assembly.GetName().Name;
                expandedName = !String.IsNullOrEmpty(asmName)
                    ? type.FullName.Replace(String.Format("{0}", asmName), String.Empty)
                    : type.FullName;
                if (!String.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                    tokenName.Name = expandedName;

                tokenName.OwnAsmIdx = t != null ? t.IndexId : 0;
                return tokenName;
            }
            if (mi.DeclaringType == null) return tokenName;

            asmQualName = mi.DeclaringType.Assembly.GetName().FullName;
            //do not send back GAC asm's unless asked
            if (!Program.AreGacAssembliesResolved && IsIgnore(asmQualName))
                return null;

            var f =
                Program.AsmIndicies.Asms.FirstOrDefault(
                    x =>
                        String.Equals(x.AssemblyName, mi.DeclaringType.Assembly.GetName().FullName,
                            StringComparison.OrdinalIgnoreCase));

            asmName = mi.DeclaringType.Assembly.GetName().Name;
            expandedName = !String.IsNullOrEmpty(asmName)
                ? mi.DeclaringType.FullName.Replace(String.Format("{0}", asmName), String.Empty)
                : mi.DeclaringType.FullName;
            if (!String.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                tokenName.Name = String.Format("{0}{1}{2}", expandedName, Constants.TypeMethodNameSplitOn, tokenName.Name);

            tokenName.OwnAsmIdx = f != null ? f.IndexId : 0;

            var mti = mi as MethodInfo;
            if (mti == null)
                return tokenName;

            var mtiParams = mti.GetParameters();
            if (mtiParams.Length <= 0)
            {
                tokenName.Name = String.Format("{0}()", tokenName.Name);
                return tokenName;
            }
            tokenName.Name = String.Format("{0}({1})", tokenName.Name,
                String.Join(",", mtiParams.Select(x => x.ParameterType.FullName)));

            return tokenName;
        }

        /// <summary>
        /// Coupled with the <see cref="ToMetadataTokenName"/>, where the
        /// <see cref="MetadataTokenName.Name"/> is parsed for method arg types.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        /// <remarks>
        /// Presumes the <see cref="MemberInfo"/> was successfully cast as <see cref="MethodInfo"/>
        /// </remarks>
        internal static Type[] GetMethodArgTypes(string tokenName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;
            tokenName = tokenName.Trim();
            var idxSplt = tokenName.IndexOf('(');
            if (idxSplt < 0)
                return null;
            var argTypes = new List<Type>();

            var argNames = tokenName.Substring(tokenName.IndexOf('('));
            argNames = argNames.Replace(")", "");

            if (String.IsNullOrWhiteSpace(argNames))
                return Type.EmptyTypes;

            try
            {
                foreach (var argName in argNames.Split(',').Where(x => !String.IsNullOrWhiteSpace(x)))
                {
                    var pType = Type.GetType(argName);
                    if (pType != null)
                        argTypes.Add(pType);
                }
            }
            catch
            {
                return null;
            }
            return argTypes.ToArray();
        }

        /// <summary>
        /// Coupled with the <see cref="ToMetadataTokenName"/>, where the
        /// <see cref="MetadataTokenName.Name"/> is parsed namespace-qual type.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="owningAsmName"></param>
        /// <returns></returns>
        internal static string ParseTypeNameFromTokenName(string tokenName, string owningAsmName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;

            var idxSplt = tokenName.IndexOf(Constants.TypeMethodNameSplitOn, StringComparison.Ordinal);

            //assembly name and namespace being equal will have equal portion removed, add it back
            var typeName = tokenName.Substring(0, idxSplt);
            if (typeName.StartsWith("."))
                typeName = String.Format("{0}{1}", owningAsmName, typeName);
            return typeName.Trim();
        }

        /// <summary>
        /// Coupled with the <see cref="ToMetadataTokenName"/>, where the
        /// <see cref="MetadataTokenName.Name"/> is parsed for just the method name (no args)
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        internal static string ParseMethodNameFromTokenName(string tokenName)
        {
            var idxSplt = tokenName.IndexOf(Constants.TypeMethodNameSplitOn, StringComparison.Ordinal);

            idxSplt = idxSplt + (Constants.TypeMethodNameSplitOn).Length;
            if (idxSplt > tokenName.Length || idxSplt < 0)
                return null;

            var methodName = tokenName.Substring(idxSplt, tokenName.Length - idxSplt);

            idxSplt = methodName.IndexOf('(');
            if (idxSplt < 0)
                return methodName;

            methodName = methodName.Substring(0, methodName.IndexOf('(')).Trim();
            return methodName;
        }

        /// <summary>
        /// Attempts to resolve <see cref="tokenId"/> to a <see cref="MetadataTokenName"/>
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="tokenName"></param>
        /// <param name="useTokensRslvAsmIdx">Default to <see cref="Program.ManifestModule"/> for token resolution.</param>
        /// <returns></returns>
        internal static bool ResolveSingleTokenName(MetadataTokenId tokenId, out MetadataTokenName tokenName, bool useTokensRslvAsmIdx = false)
        {
            tokenName = null;
            if (tokenId.Id == 0)
            {
                return false;
            }
            if (Program.TokenId2NameCache.ContainsKey(tokenId))
            {
                tokenName = Program.TokenId2NameCache[tokenId];
                return true;
            }
            MemberInfo mi;
            var rtMiRslt = false;
            if (!useTokensRslvAsmIdx || (Program.AsmIndicies.Asms.All(x => x.IndexId != tokenId.RslvAsmIdx)))
            {
                rtMiRslt = TryResolveRtMemberInfo(tokenId, out mi);
            }
            else
            {
                var manifestAsm = Program.AsmIndicies.GetAssemblyByIndex(tokenId.RslvAsmIdx);
                rtMiRslt = TryResolveRtMemberInfo(manifestAsm.ManifestModule, tokenId, out mi);
            }
            if (!rtMiRslt)
                return false;
            if (mi == null)
            {
                return false;
            }

            tokenName = mi.ToMetadataTokenName();

            if (tokenName == null)
                return false;
            tokenName.Id = tokenId.Id;
            tokenName.RslvAsmIdx = tokenId.RslvAsmIdx;
            return true;
        }

        /// <summary>
        /// Resolves the <see cref="tokenId"/>to a <see cref="MemberInfo"/> using
        /// the current manifest module assigned to <see cref="Program.ManifestModule"/>
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="mi"></param>
        /// <returns></returns>
        internal static bool TryResolveRtMemberInfo(MetadataTokenId tokenId, out MemberInfo mi)
        {
            return TryResolveRtMemberInfo(Program.ManifestModule, tokenId, out mi);
        }

        /// <summary>
        /// Resolves the <see cref="tokenId"/>to a <see cref="MemberInfo"/> using
        /// the <see cref="manifestModule"/>
        /// </summary>
        /// <param name="manifestModule"></param>
        /// <param name="tokenId"></param>
        /// <param name="mi"></param>
        /// <returns></returns>
        internal static bool TryResolveRtMemberInfo(Module manifestModule, MetadataTokenId tokenId, out MemberInfo mi)
        {
            mi = null;

            if (manifestModule == null)
                return false;
            try
            {
                mi = manifestModule.ResolveMember(tokenId.Id);
            }
            catch (ArgumentException)//does not resolve the token
            {
                return false;
            }
            return mi != null;
        }

        /// <summary>
        /// Intended for resolving method calls of types defined in another assembly.
        /// </summary>
        /// <param name="owningAsm"></param>
        /// <param name="tokenName">The name as drafted from <see cref="ToMetadataTokenName"/></param>
        /// <param name="mi"></param>
        /// <param name="withArgs">
        /// Switch to have the a method resolved using the method signature args.
        /// Intended for resolving methods with overloads.
        /// </param>
        /// <returns></returns>
        internal static bool TryResolveRtMemberInfo(Assembly owningAsm, string tokenName, out MemberInfo mi, bool withArgs = false)
        {
            //default the out variable
            mi = null;

            //expect token name to match naming given herein
            if (String.IsNullOrWhiteSpace(tokenName) || !tokenName.Contains(Constants.TypeMethodNameSplitOn))
                return false;

            if (owningAsm == null)
                return false;

            var typeName = ParseTypeNameFromTokenName(tokenName, owningAsm.GetName().Name);

            if (String.IsNullOrWhiteSpace(typeName))
                return false;

            var asmType = owningAsm.GetType(typeName);
            if (asmType == null)
                return false;

            var methodName = ParseMethodNameFromTokenName(tokenName);
            if (String.IsNullOrWhiteSpace(methodName))
                return false;

            MethodInfo methodInfo = null;
            try
            {
                if (!withArgs)
                {
                    methodInfo = asmType.GetMethod(methodName, AssemblyAnalysis.DefaultFlags);
                }
                else
                {
                    var argTypes = GetMethodArgTypes(tokenName);
                    if (argTypes == null)
                        return false;
                    methodInfo = asmType.GetMethod(methodName, argTypes);
                }
            }
            catch (AmbiguousMatchException)
            {
                if (!withArgs)
                    return TryResolveRtMemberInfo(owningAsm, tokenName, out mi, true);
            }

            if (methodInfo == null)
                return false;

            mi = methodInfo;
            return true;
        }

        /// <summary>
        /// Helper method to loop ids to names.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        internal static List<MetadataTokenName> ResolveAllTokenNames(MetadataTokenId[] tokens)
        {
            var counter = 0;
            var total = tokens.Length;
            var names = new List<MetadataTokenName>();
            for (var i = 0; i < tokens.Length; i++)
            {
                counter += 1;
                try
                {
                    var cid = tokens[i];
                    Program.ReportProgress(new ProgressMessage
                    {
                        Activity = string.Format("{0}.{1}", cid.RslvAsmIdx, cid.Id),
                        ProcName = System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                        ProgressCounter = Etc.CalcProgressCounter(counter, total),
                        Status = "Resolving names"
                    });
                    if (Program.DisolutionCache.Contains(cid) ||
                        names.Any(x => x.Id == cid.Id && x.RslvAsmIdx == cid.RslvAsmIdx))
                        continue;

                    if (Program.TokenId2NameCache.ContainsKey(cid))
                    {
                        names.Add(Program.TokenId2NameCache[cid]);
                        continue;
                    }
                    MetadataTokenName tokenName;
                    var resolved = UtilityMethods.ResolveSingleTokenName(cid, out tokenName, true);

                    if (!resolved)
                    {
                        if (!Program.DisolutionCache.Contains(cid))
                            Program.DisolutionCache.Add(cid);
                        continue;
                    }
                    names.Add(tokenName);

                    if (!Program.TokenId2NameCache.ContainsKey(cid))
                    {
                        Program.TokenId2NameCache.Add(cid, tokenName);
                    }
                }
                catch
                {
                    continue;
                }
            }
            return names;
        }
    }
}
