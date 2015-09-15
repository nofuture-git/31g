﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Configuration;
using System.Text;
using NoFuture.Shared;
using NoFuture.Util.Binary;

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
            if (!Program.AreGacAssembliesResolved)
                return false;

            if (String.IsNullOrWhiteSpace(asmQualName))
                return true;

            if (asmQualName.StartsWith("System."))
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
        /// Helper method which resolves the assembly using the <see cref="tokenId"/> RslvAsmIdx.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <param name="tokenName"></param>
        /// <param name="msgOut">Inteneded for debug trace from the Console.</param>
        /// <returns></returns>
        internal static bool ResolveSingleTokenName(MetadataTokenId tokenId, out MetadataTokenName tokenName, StringBuilder msgOut = null)
        {
            var manifestAsm = Program.AsmIndicies.GetAssemblyByIndex(tokenId.RslvAsmIdx);
            return ResolveSingleTokenName(manifestAsm, tokenId, out tokenName, msgOut);
        }

        /// <summary>
        /// Attempts to resolve <see cref="tokenId"/> to a <see cref="MetadataTokenName"/>
        /// </summary>
        /// <param name="resolvesWith"></param>
        /// <param name="tokenId"></param>
        /// <param name="tokenName"></param>
        /// <param name="msgOut">Inteneded for debug trace from the Console.</param>
        /// <returns></returns>
        internal static bool ResolveSingleTokenName(Assembly resolvesWith, MetadataTokenId tokenId,
            out MetadataTokenName tokenName, StringBuilder msgOut = null)
        {
            tokenName = null;
            if (tokenId.Id == 0)
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'the token id is zero'");
                return false;
            }
            if (Program.TokenId2NameCache.ContainsKey(tokenId))
            {
                tokenName = Program.TokenId2NameCache[tokenId];
                if (msgOut != null)
                    msgOut.Append(", Message:'token previously resolved'");

                return true;
            }
            if (resolvesWith == null)
            {
                if (msgOut != null)
                    msgOut.Append(string.Format(", Message:'resolve assembly idx {0} has no match'", tokenId.RslvAsmIdx));

                return false;
            }

            MemberInfo mi;
            var rtMiRslt = false;
            rtMiRslt = TryResolveRtMemberInfo(resolvesWith.ManifestModule, tokenId, out mi, msgOut);
            if (!rtMiRslt)
            {
                return false;
            }
            if (mi == null)
            {
                return false;
            }

            tokenName = AssemblyAnalysis.ConvertToMetadataTokenName(mi, Program.AsmIndicies, IsIgnore);

            if (tokenName == null)
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'could not construct a token name'");
                
                return false;
            }
            tokenName.Id = tokenId.Id;
            tokenName.RslvAsmIdx = tokenId.RslvAsmIdx;
            return true;            
        }

        /// <summary>
        /// Resolves the <see cref="tokenId"/>to a <see cref="MemberInfo"/> using
        /// the <see cref="manifestModule"/>
        /// </summary>
        /// <param name="manifestModule"></param>
        /// <param name="tokenId"></param>
        /// <param name="mi"></param>
        /// <param name="msgOut">Inteneded for debug trace from the Console.</param>
        /// <returns></returns>
        internal static bool TryResolveRtMemberInfo(Module manifestModule, MetadataTokenId tokenId, out MemberInfo mi, StringBuilder msgOut)
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
                if (msgOut != null)
                    msgOut.Append(", Message: 'manifest module could not resolve id'");
                return false;
            }
            return mi != null;
        }

        /// <summary>
        /// Intended for resolving method calls of types defined in another assembly.
        /// </summary>
        /// <param name="owningAsm"></param>
        /// <param name="tokenName">The name as drafted from <see cref="AssemblyAnalysis.ConvertToMetadataTokenName"/></param>
        /// <param name="mi"></param>
        /// <param name="msgOut">Inteneded for debug trace from the Console.</param>
        /// <returns></returns>
        internal static bool TryResolveRtMemberInfo(Assembly owningAsm, string tokenName, out MemberInfo mi, StringBuilder msgOut = null)
        {
            //default the out variable
            mi = null;

            //expect token name to match naming given herein
            if (String.IsNullOrWhiteSpace(tokenName))
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'the token name is null'");
                return false;
            }
            if (!tokenName.Contains(Constants.TypeMethodNameSplitOn))
            {
                if (msgOut != null)
                    msgOut.AppendFormat(", Message:'[{0}] does not contain {1}'", tokenName,
                        Constants.TypeMethodNameSplitOn);
            }

            if (owningAsm == null)
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'the owning assembly is null'");
                return false;
            }

            var assemblyName = owningAsm.GetName().Name;
            var typeName = AssemblyAnalysis.ParseTypeNameFromTokenName(tokenName, assemblyName);

            if (String.IsNullOrWhiteSpace(typeName))
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'could not parse type name'");
                return false;
            }
            Type asmType = null;
            try
            {
                //framework throwing null-ref ex despite null checks
                asmType = owningAsm.NfGetType(typeName, false, Program.LogFile);
            }
            catch
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'Assembly.GetType threw exception'");
                return false;
            }
            if (asmType == null)
            {
                if (msgOut != null)
                    msgOut.AppendFormat(", Message:'assembly {0} could not resolve {1}'", assemblyName,
                        typeName);
                return false;
            }

            var methodName = AssemblyAnalysis.ParseMethodNameFromTokenName(tokenName);
            if (String.IsNullOrWhiteSpace(methodName))
            {
                if (msgOut != null)
                    msgOut.Append(", Message:'could not parse method name'");
                return false;
            }

            MethodInfo methodInfo = null;

            //try easiest first
            try
            {
                methodInfo = asmType.NfGetMethod(methodName, AssemblyAnalysis.DefaultFlags);
            }
            catch (AmbiguousMatchException) { }//is overloaded 

            //try it the formal way
            if (methodInfo == null)
            {
                var args = AssemblyAnalysis.ParseArgsFromTokenName(tokenName).ToArray();
                var argTypes = args.Length <= 0
                    ? Type.EmptyTypes
                    : args.Select(Type.GetType).Where(x => x != null).ToArray();

                //there must be a one-for-one match of string names to first-class types
                if (args.Length == argTypes.Length)
                    methodInfo = asmType.NfGetMethod(methodName, AssemblyAnalysis.DefaultFlags, null, argTypes, null,
                        false, Program.LogFile);
            }

            //try it the very slow but certian way
            if (methodInfo == null)
            {
                var methodInfos = asmType.NfGetMethods(AssemblyAnalysis.DefaultFlags, false, Program.LogFile);
                if (methodInfos.Length <= 0)
                {
                    if (msgOut != null)
                    {
                        msgOut.AppendFormat(", Assembly:'{0}'\n", assemblyName);
                        msgOut.AppendFormat(", Type:'{0}'\n", typeName);
                        msgOut.Append(", Message:'does not have any methods'");
                    }
                    return false;
                }
                foreach (var info in methodInfos)
                {
                    var asTokenName = AssemblyAnalysis.ConvertToMetadataTokenName(info, Program.AsmIndicies, IsIgnore);
                    if (asTokenName == null || string.IsNullOrWhiteSpace(asTokenName.Name))
                        continue;
                    if (string.Equals(asTokenName.Name, tokenName))
                    {
                        methodInfo = info;
                        break;
                    }
                }
            }

            if (methodInfo == null)
            {
                if (msgOut != null)
                {
                    msgOut.AppendFormat(", Assembly:'{0}'\n", assemblyName);
                    msgOut.AppendFormat(", Type:'{0}'\n", typeName);
                    msgOut.AppendFormat(", Method:'{0}'\n", methodName);
                    msgOut.Append(", Message:'was not found'");
                }
                return false;
            }

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
                    var resolved = UtilityMethods.ResolveSingleTokenName(cid, out tokenName);

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
