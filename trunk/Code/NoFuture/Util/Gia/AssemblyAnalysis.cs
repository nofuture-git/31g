﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.Binary;
using NoFuture.Tools;
using NoFuture.Util.Gia.InvokeCmds;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.Gia
{
    /// <summary>
    /// Acts a a wrapper around the independet process of similar name.
    /// </summary>
    public class AssemblyAnalysis : InvokeConsoleBase
    {
        #region constants
        public const string GET_TOKEN_IDS_PORT_CMD_SWITCH = "nfDumpAsmTokensPort";
        public const string GET_TOKEN_NAMES_PORT_CMD_SWITCH = "nfResolveTokensPort";
        public const string GET_ASM_INDICES_PORT_CMD_SWITCH = "nfGetAsmIndicies";
        public const string RESOLVE_GAC_ASM_SWITCH = "nfResolveGacAsm";
        public const int DF_START_PORT = 5059;
        #endregion

        #region fields
        private readonly AsmIndicies _asmIndices;
        private readonly InvokeGetAsmIndicies _getAsmIndicesCmd;
        private readonly InvokeGetTokenIds _getTokenIdsCmd;
        private readonly InvokeGetTokenNames _getTokenNamesCmd;
        #endregion

        #region properties
        /// <summary>
        /// A mapping of index ids to assembly names.
        /// </summary>
        public AsmIndicies AsmIndicies { get { return _asmIndices; } }
        #endregion

        #region ctors
        /// <summary>
        /// Launches the NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe and calls 
        /// GetAsmIndicies command.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="ports"></param>
        /// <param name="resolveGacAsmNames"></param>
        /// <returns></returns>
        /// <remarks>
        /// The ports used are defaulted to <see cref="DF_START_PORT"/>.
        /// </remarks>
        /// <remarks>
        /// <![CDATA[
        ///  +------------------------------------------------------------------------------------------------------------+
        ///  |                                           Using AssemblyAnalysis                                           |
        ///  ||+--operating-context(1)--+|+-----new-instance(2)----+|+------remote-exe(3)-----+|+---target-assembly(4)---+|
        ///  ||      new instance of     |                          |                          |                          |
        ///  ||                   ................>                 |                          |                          |
        ///  ||                          |     start new process    |                          |                          |
        ///  ||                          |   provide assembly name  |                          |                          |
        ///  ||                          |                    ................>                |                          |
        ///  ||                          |                          |      launch sockets      |                          |
        ///  ||                          |                          | get asm names on manifest|                          |
        ///  ||                          |                          |                     ................>               |
        ///  ||                          |                          |send AsmIndicies on socket|                          |
        ///  ||                          |                 <................                   |                          |
        ///  ||                          |    receive AsmIndices    |                          |                          |
        ///  ||                          |       save to disk       |                          |                          |
        ///  ||                          |      assign to prop      |                          |                          |
        ///  ||                <................                    |                          |                          |
        ///  ||  invoke GetTokenIds with |                          |                          |                          |
        ///  ||           regex          |                          |                          |                          |
        ///  ||                   ................>                 |                          |                          |
        ///  ||                          | send GetTokenIdsCriteria |                          |                          |
        ///  ||                          |         on socket        |                          |                          |
        ///  ||                          |                    ................>                |                          |
        ///  ||                          |                          |    get types as tokens   |                          |
        ///  ||                          |                          |                     ................>               |
        ///  ||                          |                          |   get members as tokens  |                          |
        ///  ||                          |                          |                     ................>               |
        ///  ||                          |                          |  get callvirts as tokens |                          |
        ///  ||                          |                          |                     ................>               |
        ///  ||                          |                          |  get tokens-of-tokens(a) |                          |
        ///  ||                          |                          |                     ................>               |
        ///  ||                          |                          |send TokenIds on socket(b)|                          |
        ///  ||                          |                 <................                   |                          |
        ///  ||                          |       get TokenIds       |                          |                          |
        ///  ||                          |       save to disk       |                          |                          |
        ///  ||                          |      return TokenIds     |                          |                          |
        ///  ||                <................                    |                          |                          |
        ///  ||   flatten the TokenIds   |                          |                          |                          |
        ///  ||   invoke GetTokenNames   |                          |                          |                          |
        ///  ||                   ................>                 |                          |                          |
        ///  ||                          | send MetadataTokenId[] on|                          |                          |
        ///  ||                          |          socket          |                          |                          |
        ///  ||                          |                    ................>                |                          |
        ///  ||                          |                          | resolve each to a runtime|                          |
        ///  ||                          |                          |           type           |                          |
        ///  ||                          |                          |                     ................>               |
        ///  ||                          |                          | send TokenNames on socket|                          |
        ///  ||                          |                 <................                   |                          |
        ///  ||                          |    receive TokenNames    |                          |                          |
        ///  ||                          |       save to disk       |                          |                          |
        ///  ||                          |     return TokenNames    |                          |                          |
        ///  ||                <................                    |                          |                          |
        ///  ||+------------------------+|+------------------------+|+------------------------+|+------------------------+|
        /// 
        /// (1) assume PowerShell
        /// (2) new instance of NoFuture.Util.Gia.AssemblyAnalysis
        /// (3) new process NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe
        /// (4) the assembly whose tokens we want
        /// 
        /// (a)  The top-level types already had all thier members resolved to tokens and every virtcall found within the body of those members.
        ///      This is now starting it all over again as if the types found on the callvirts in the body of these members were themselves the top-level types.
        ///      This will continue until we end up on a type which is contained in an assembly whose name doesn't match our regex pattern.
        /// (b)  This is a tree data-struct. Each token has itself a collection of tokens and so on.
        /// ]]>
        /// </remarks>
        /// <example>
        /// <![CDATA[
        ///  $myAsmAly = New-Object NoFuture.Util.Gia.AssemblyAnalysis($AssemblyPath,$false)
        ///  $myTokensIds = $myAsmAly.GetTokenIds(0, "NoFuture.*")
        ///  $myFlatTokens = $myTokensIds.FlattenToDistinct()
        ///  $myTokenNames = $myAsmAly.GetTokenNames($myFlatTokens)
        /// ]]>
        /// </example>
        public AssemblyAnalysis(string assemblyPath, bool resolveGacAsmNames, params int[] ports)
        {
            if (String.IsNullOrWhiteSpace(assemblyPath) || !File.Exists(assemblyPath))
                throw new ItsDeadJim("This isn't a valid assembly path");

            if (String.IsNullOrWhiteSpace(CustomTools.InvokeAssemblyAnalysis) || !File.Exists(CustomTools.InvokeAssemblyAnalysis))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.Gia.InvokeAssemblyAnalysis, assign " +
                                     "the global variable at NoFuture.CustomTools.InvokeAssemblyAnalysis.");

            var np = DF_START_PORT;
            var usePorts = new int[3];
            for (var i = 0; i < usePorts.Length; i++)
            {
                usePorts[i] = ports != null && ports.Length >= i + 1 ? ports[i] : np + i;
            }
            var args = String.Join(" ",
                ConsoleCmd.ConstructCmdLineArgs(GET_ASM_INDICES_PORT_CMD_SWITCH,
                    usePorts[0].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_IDS_PORT_CMD_SWITCH,
                    usePorts[1].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_NAMES_PORT_CMD_SWITCH,
                    usePorts[2].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(RESOLVE_GAC_ASM_SWITCH, resolveGacAsmNames.ToString()));

            MyProcess = StartRemoteProcess(CustomTools.InvokeAssemblyAnalysis, args);

            _getAsmIndicesCmd = new InvokeGetAsmIndicies()
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[0]
            };

            _asmIndices = _getAsmIndicesCmd.Receive(assemblyPath);

            _getTokenIdsCmd = new InvokeGetTokenIds(_asmIndices)
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[1]
            };

            _getTokenNamesCmd = new InvokeGetTokenNames()
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[2]
            };



        }
        #endregion

        #region instance methods
        /// <summary>
        /// Gets the manifold of Metadata Token Ids for the assembly at <see cref="asmIdx"/>
        /// </summary>
        /// <param name="asmIdx"></param>
        /// <param name="recurseAnyAsmNamedLike">
        /// Optional regex pattern for tokens-of-tokens resolution.
        /// Basically, "keep going while the token's containing assembly looks like this..."
        /// Default is to stop at the end of <see cref="asmIdx"/> tokens-of-tokens.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// Use the <see cref="AsmIndicies"/> property to get the index.
        /// </remarks>
        public TokenIds GetTokenIds(int asmIdx, string recurseAnyAsmNamedLike = null)
        {
            _getTokenIdsCmd.RecurseAnyAsmNamedLike = recurseAnyAsmNamedLike;
            return _getTokenIdsCmd.Receive(asmIdx);
        }

        /// <summary>
        /// Resolves the given Metadata Token Ids to thier names & types.
        /// </summary>
        /// <param name="metadataTokenIds"></param>
        /// <returns></returns>
        public TokenNames GetTokenNames(MetadataTokenId[] metadataTokenIds)
        {
            return _getTokenNamesCmd.Receive(metadataTokenIds);
        }

        #endregion

        #region token name parsers

        /// <summary>
        /// Transforms a <see cref="MemberInfo"/> into a <see cref="MetadataTokenName"/>
        /// getting as much info as possiable depending on which 
        /// child-type the <see cref="mi"/> resolves to.
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="indicies">
        /// optional parameter, used to abbreviate the name removing the redundant part shared 
        /// by both assembly and namespace.
        /// </param>
        /// <param name="isIgnore">optional f(x) pointer for calling assembly to specify some additional rules by token name</param>
        /// <returns></returns>
        public static MetadataTokenName ConvertToMetadataTokenName(MemberInfo mi, AsmIndicies indicies, Func<string, bool> isIgnore)
        {
            if (mi == null)
                return null;

            var localIsIgnore = isIgnore ?? (s => false);
            var localIndicies = indicies ?? new AsmIndicies { Asms = new MetadataTokenAsm[0] };

            var tokenName = new MetadataTokenName { Name = mi.Name, Label = mi.GetType().Name };

            string asmQualName;
            string asmName;
            string expandedName;

            var type = mi as Type;

            if (type != null)
            {
                asmQualName = type.Assembly.GetName().FullName;
                //do not send back GAC asm's unless asked
                if (localIsIgnore(asmQualName))
                    return null;

                var t =
                    localIndicies.Asms.FirstOrDefault(
                        x =>
                            String.Equals(x.AssemblyName, type.Assembly.GetName().FullName,
                                StringComparison.OrdinalIgnoreCase));

                if (t == null)
                    return null;

                asmName = type.Assembly.GetName().Name;
                expandedName = !String.IsNullOrEmpty(asmName)
                    ? type.FullName.Replace(String.Format("{0}", asmName), String.Empty)
                    : type.FullName;
                if (!String.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                    tokenName.Name = expandedName;

                tokenName.OwnAsmIdx = t.IndexId;
                return tokenName;
            }
            if (mi.DeclaringType == null) return tokenName;

            asmQualName = mi.DeclaringType.Assembly.GetName().FullName;
            //do not send back GAC asm's unless asked
            if (localIsIgnore(asmQualName) || localIsIgnore(mi.DeclaringType.FullName))
                return null;

            var f =
                localIndicies.Asms.FirstOrDefault(
                    x =>
                        String.Equals(x.AssemblyName, mi.DeclaringType.Assembly.GetName().FullName,
                            StringComparison.OrdinalIgnoreCase));
            if (f == null)
                return null;

            asmName = mi.DeclaringType.Assembly.GetName().Name;
            expandedName = !String.IsNullOrEmpty(asmName)
                ? mi.DeclaringType.FullName.Replace(String.Format("{0}", asmName), String.Empty)
                : mi.DeclaringType.FullName;
            if (!String.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                tokenName.Name = String.Format("{0}{1}{2}", expandedName, Constants.TypeMethodNameSplitOn, tokenName.Name);

            tokenName.OwnAsmIdx = f.IndexId;

            var mti = mi as MethodInfo;
            if (mti == null)
                return tokenName;

            var mtiParams = mti.GetParameters();
            if (mtiParams.Length <= 0)
            {
                tokenName.Name = String.Format("{0}()", tokenName.Name);
                return tokenName;
            }

            var paramNames = new List<string>();

            foreach (var param in mtiParams)
            {
                var workingName = param.ParameterType.FullName;
                if (!param.ParameterType.IsGenericType)
                {
                    paramNames.Add(workingName);
                    continue;
                }

                var paramsGen = param.ParameterType.GetGenericArguments();
                foreach (var genParam in paramsGen)
                {
                    var asmGenParamName = genParam.AssemblyQualifiedName;
                    if (string.IsNullOrWhiteSpace(asmGenParamName))
                        continue;
                    workingName = workingName.Replace("[" + asmGenParamName + "]", genParam.FullName);

                }
                paramNames.Add(workingName);
            }

            tokenName.Name = String.Format("{0}({1})", tokenName.Name,
                String.Join(",", paramNames));

            return tokenName;
        }

        /// <summary>
        /// Gets the method parameter type-names from the token name.
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public static string[] ParseArgsFromTokenName(string tokenName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;

            if (!tokenName.Contains("(") || !tokenName.Contains(")"))
                return null;

            tokenName = tokenName.Trim();
            var idxSplt = tokenName.IndexOf('(');
            if (idxSplt < 0)
                return null;

            var argNames = tokenName.Substring(tokenName.IndexOf('('));
            argNames = argNames.Replace(")", string.Empty);

            if (argNames.Length <= 0)
                return null;

            return argNames.Split(',').Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
        }

        /// <summary>
        /// Gets the full name (namespace plus type-name) part of the <see cref="MetadataTokenName.Name"/>
        /// </summary>
        /// <param name="tokenName"></param>
        /// <param name="owningAsmName"></param>
        /// <returns></returns>
        public static string ParseTypeNameFromTokenName(string tokenName, string owningAsmName)
        {
            if (String.IsNullOrWhiteSpace(tokenName))
                return null;

            var sep = Constants.DefaultTypeSeparator.ToString(CultureInfo.InvariantCulture);

            //assembly name and namespace being equal will have equal portion removed, add it back
            if (!string.IsNullOrWhiteSpace(owningAsmName) && tokenName.StartsWith(sep))
            {
                tokenName = String.Format("{0}{1}", owningAsmName, tokenName);
            }

            var ns = Util.NfTypeName.GetNamespaceWithoutTypeName(tokenName);
            var tn = Util.NfTypeName.GetTypeNameWithoutNamespace(tokenName);

            return String.Format("{0}{1}{2}", ns, sep, tn);
        }

        /// <summary>
        /// Gets just the method name of the <see cref="MetadataTokenName.Name"/>
        /// </summary>
        /// <param name="tokenName"></param>
        /// <returns></returns>
        public static string ParseMethodNameFromTokenName(string tokenName)
        {
            if (string.IsNullOrWhiteSpace(tokenName))
                return null;

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

        #endregion

        #region static analysis

        /// <summary>
        /// For the given assembly, gets the named being made from itself to itself.
        /// </summary>
        /// <param name="asmIdx"></param>
        /// <param name="tokenMap"></param>
        /// <param name="tokenNames"></param>
        /// <returns></returns>
        public static MetadataTokenName[] InternallyCalled(AsmIndicies asmIdx, TokenIds tokenMap, TokenNames tokenNames)
        {
            if(tokenNames == null || tokenNames.Names == null || tokenNames.Names.Length <= 0)
                return new List<MetadataTokenName>().ToArray();
            if (tokenMap == null || tokenMap.Tokens == null || tokenMap.Tokens.Length <= 0)
                return new List<MetadataTokenName>().ToArray();
            var typeTokens = tokenMap.Tokens;

            if (typeTokens.All(x => x.Items == null || x.Items.Length <= 0))
                return new List<MetadataTokenName>().ToArray();
            

            var memberTokens = typeTokens.SelectMany(x => x.Items).ToList();

            //these are all the token Ids one could call on this assembly
            var memberTokenIds = memberTokens.Select(x => x.Id).ToList();

            //get all the tokenIds, defined in this assembly and being called from this assembly
            var callsInMembers =
                memberTokens.SelectMany(x => x.Items)
                    .Select(x => x.Id)
                    .Where(x => memberTokenIds.Any(y => x == y))
                    .ToList();


            tokenNames.ApplyFullName(asmIdx);

            return tokenNames.Names.Where(x => callsInMembers.Any(y => y == x.Id)).ToArray();
        }

        /// <summary>
        /// Set operation for comparision of two <see cref="TokenNames"/> with
        /// thier respective <see cref="AsmIndicies"/>
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <param name="rightListTopLvlOnly">
        /// Set to true to have <see cref="rightListTopLvlOnly"/> 
        /// only those in <see cref="rightList"/> whose <see cref="MetadataTokenName.OwnAsmIdx"/> is '0'.
        /// </param>
        /// <returns></returns>
        public static MetadataTokenName[] RightSetDiff(Tuple<AsmIndicies, TokenNames> leftList,
            Tuple<AsmIndicies, TokenNames> rightList, bool rightListTopLvlOnly = false)
        {
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (leftList == null || rightList == null)
                return new List<MetadataTokenName>().ToArray();
            if (rightList.Item2 == null || rightList.Item2.Names == null || rightList.Item2.Names.Length <= 0)
                return new List<MetadataTokenName>().ToArray();
            if (leftList.Item2 == null || leftList.Item2.Names == null || leftList.Item2.Names.Length <= 0)
                return rightList.Item2.Names;

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var setOp = rightList.Item2.Names.Select(hashCode).Except(leftList.Item2.Names.Select(hashCode));

            var listOut = new List<MetadataTokenName>();
            foreach (var j in setOp)
            {
                var k = rightList.Item2.Names.FirstOrDefault(x => hashCode(x) == j);
                if (k == null || (rightListTopLvlOnly && k.OwnAsmIdx != 0) || !k.IsMethodName())
                    continue;
                listOut.Add(k);
            }
            
            return listOut.ToArray();
        }

        /// <summary>
        /// Set operation for the joining of two <see cref="TokenNames"/> with
        /// thier respective <see cref="AsmIndicies"/>
        /// </summary>
        /// <param name="leftList"></param>
        /// <param name="rightList"></param>
        /// <returns></returns>
        public static MetadataTokenName[] Union(Tuple<AsmIndicies, TokenNames> leftList,
            Tuple<AsmIndicies, TokenNames> rightList)
        {
            Func<MetadataTokenName, int> hashCode = x => x.GetNameHashCode();

            if (leftList == null || rightList == null)
                return new List<MetadataTokenName>().ToArray();
            if (rightList.Item2 == null)
                return leftList.Item2.Names;
            if (leftList.Item2 == null)
                return rightList.Item2.Names;

            //expand to full names
            leftList.Item2.ApplyFullName(leftList.Item1);
            rightList.Item2.ApplyFullName(rightList.Item1);

            var d = rightList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);
            var e = leftList.Item2.Names.Distinct(new MetadataTokenNameComparer()).ToDictionary(hashCode);

            foreach(var key in e.Keys.Where(k => !d.ContainsKey(k)))
                d.Add(key, e[key]);

            return d.Values.Where(x => x.IsMethodName()).ToArray();
        }

        /// <summary>
        /// Get the <see cref="System.Reflection.MetadataToken"/> for the <see cref="asmType"/>
        /// and for all its members.
        /// </summary>
        /// <param name="asmType"></param>
        /// <param name="asmIdx"></param>
        /// <returns></returns>
        public static MetadataTokenId GetMetadataToken(Type asmType, int asmIdx = 0)
        {
            if (asmType == null)
                return new MetadataTokenId { Items = new MetadataTokenId[0] };

            var token = new MetadataTokenId { Id = asmType.MetadataToken, RslvAsmIdx = asmIdx};
            token.Items =
                asmType.GetMembers(Constants.DefaultFlags).Select(x => GetMetadataToken(x, true, asmIdx)).Distinct().ToArray();
            return token;
        }

        /// <summary>
        /// Get the <see cref="System.Reflection.MetadataToken"/> for the <see cref="mi"/>
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="resolveManifold">True will cast the <see cref="mi"/> to 
        /// a <see cref="MethodBase"/> and find the tokens used within the 
        /// method's body. </param>
        /// <param name="asmIdx"></param>
        /// <returns></returns>
        public static MetadataTokenId GetMetadataToken(MemberInfo mi, bool resolveManifold, int asmIdx = 0)
        {
            if (mi == null)
                return new MetadataTokenId();
            var token = new MetadataTokenId { Id = mi.MetadataToken, RslvAsmIdx = asmIdx, Items = new MetadataTokenId[0] };
            var mti = mi as MethodBase;
            if (mti == null)
                return token;

            token.Items = resolveManifold
                ? Asm.GetCallsMetadataTokens(mti)
                    .Where(t => t != mi.MetadataToken)
                    .Select(t => new MetadataTokenId {Id = t, RslvAsmIdx = asmIdx})
                    .ToArray()
                : new MetadataTokenId[0];

            return token;
        }

        #endregion
    }
}
