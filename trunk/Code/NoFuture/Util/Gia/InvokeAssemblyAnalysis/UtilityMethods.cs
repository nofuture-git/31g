using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Configuration;
using NoFuture.Shared;

namespace NoFuture.Util.Gia.InvokeAssemblyAnalysis
{
    public static class UtilityMethods
    {
        private static string[] _gacAsmNames;
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
        internal static bool IsIgnore(string asmQualName)
        {
            if (String.IsNullOrWhiteSpace(asmQualName))
                return true;
            var gacAsms = GacAssemblyNames;
            return gacAsms != null && gacAsms.Any(asmQualName.EndsWith);
        }

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
                if (!Program.ResolveGacAssemblies && IsIgnore(asmQualName))
                    return null;

                var t =
                    Program.AsmIndicies.Asms.FirstOrDefault(
                        x =>
                            String.Equals(x.AssemblyName, type.Assembly.GetName().FullName,
                                StringComparison.OrdinalIgnoreCase));

                asmName = type.Assembly.GetName().Name;
                expandedName = !String.IsNullOrEmpty(asmName)
                    ? type.FullName.Replace(String.Format("{0}.", asmName), String.Empty)
                    : type.Name;
                if (!String.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                    tokenName.Name = expandedName;

                tokenName.ObyAsmIdx = t != null ? t.IndexId : 0;
                return tokenName;
            }
            if (mi.DeclaringType == null) return tokenName;

            asmQualName = mi.DeclaringType.Assembly.GetName().FullName;
            //do not send back GAC asm's unless asked
            if (!Program.ResolveGacAssemblies && IsIgnore(asmQualName))
                return null;

            var f =
                Program.AsmIndicies.Asms.FirstOrDefault(
                    x =>
                        String.Equals(x.AssemblyName, mi.DeclaringType.Assembly.GetName().FullName,
                            StringComparison.OrdinalIgnoreCase));

            asmName = mi.DeclaringType.Assembly.GetName().Name;
            expandedName = !String.IsNullOrEmpty(asmName)
                ? mi.DeclaringType.FullName.Replace(String.Format("{0}", asmName), String.Empty)
                : mi.DeclaringType.Name;
            if (!String.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                tokenName.Name = String.Format("{0}{1}{2}", expandedName, Constants.TypeMethodNameSplitOn, tokenName.Name);

            tokenName.ObyAsmIdx = f != null ? f.IndexId : 0;

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

        internal static bool ResolveSingleTokenName(MetadataTokenId tokenId, out MetadataTokenName tokenName)
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
            var rtMiRslt = TryResolveRtMemberInfo(tokenId, out mi);
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
            return true;
        }

        internal static bool TryResolveRtMemberInfo(MetadataTokenId tokenId, out MemberInfo mi)
        {
            mi = null;

            if (Program.ManifestModule == null)
                return false;
            try
            {
                mi = Program.ManifestModule.ResolveMember(tokenId.Id);
            }
            catch (ArgumentException)//does not resolve the token
            {
                return false;
            }
            return mi != null;
        }

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
    }

}
