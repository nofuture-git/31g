using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.Binary;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenRank;
using NoFuture.Util.DotNetMeta.TokenType;
using NoFuture.Util.NfConsole;

namespace NoFuture.Util.DotNetMeta
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
        public const string GET_TOKEN_PAGE_RANK_PORT_CMD_SWITCH = "nfGetTokenPageRank";
        public const string GET_TOKEN_TYPES_PORT_CMD_SWITCH = "nfGetTokenTypePort";
        public const string REASSIGN_TOKEN_NAMES_PORT_CMD_SWITCH = "nfReassignTokensPort";
        public const string RESOLVE_GAC_ASM_SWITCH = "nfResolveGacAsm";
        public static int DefaultPort = NfConfig.NfDefaultPorts.AssemblyAnalysis;
        internal const string UNKNOWN_NAME_SUB = "T";
        #endregion

        #region fields
        private AsmIndexResponse _asmIndices;
        private readonly InvokeGetAsmIndicies _getAsmIndiciesCmd;
        private readonly InvokeGetTokenIds _getTokenIdsCmd;
        private readonly InvokeGetTokenNames _getTokenNamesCmd;
        private readonly InvokeGetTokenPageRank _getTokenPageRankCmd;
        private readonly InvokeGetTokenTypes _getTokenTypesCmd;
        private readonly InvokeReassignTokenNames _reassignTokenNamesCmd;

        #endregion

        #region properties
        /// <summary>
        /// A mapping of index ids to assembly names.
        /// </summary>
        public AsmIndexResponse AsmIndicies => _asmIndices;

        #endregion

        #region ctors
        /// <summary>
        /// Launches the NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe and calls 
        /// GetAsmIndicies command.
        /// </summary>
        /// <param name="resolveGacAsmNames"></param>
        /// <param name="ports">
        /// Optional, allows caller to specify the ports used - will use defaults 
        /// otherwise.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// The ports used are <see cref="DefaultPort"/> to <see cref="DefaultPort"/> + 5.
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
        /// (2) new instance of NoFuture.Util.DotNetMeta.AssemblyAnalysis
        /// (3) new process NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis.exe
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
        ///  # will launch a console
        ///  $myAsmAly = New-Object NoFuture.Util.DotNetMeta.AssemblyAnalysis($false)
        ///  
        ///  # this is assembly-name-to-index used to reduce the size of socket payloads
        ///  $myAsmIndices = $myAsmAly.GetAsmIndices($AssemblyPath)
        ///  
        ///  # this is all the types in a similar form 
        ///  $myTokenTypes = $myAsmAly.GetTokenTypes("NoFuture.*")
        /// 
        ///  # this will represent the call-stack-tree in terms of just metadata token ids
        ///  $myTokensIds = $myAsmAly.GetTokenIds(0, "NoFuture.*")
        /// 
        ///  # translates the metadata token into a name (assembly, type, method, etc.)
        ///  $myTokenNames = $myAsmAly.GetTokenNames($myTokensIds.GetAsRoot().SelectDistinct(), $true)
        /// ]]>
        /// </example>
        public AssemblyAnalysis(bool resolveGacAsmNames, params int[] ports)
        {
            if (string.IsNullOrWhiteSpace(NfConfig.CustomTools.InvokeAssemblyAnalysis) || !File.Exists(NfConfig.CustomTools.InvokeAssemblyAnalysis))
                throw new ItsDeadJim("Don't know where to locate the NoFuture.Util.DotNetMeta.InvokeAssemblyAnalysis, assign " +
                                     "the global variable at NoFuture.CustomTools.InvokeAssemblyAnalysis.");

            var np = DefaultPort;
            var usePorts = new int[6];
            for (var i = 0; i < usePorts.Length; i++)
            {
                usePorts[i] = ports != null && ports.Length >= i + 1 ? ports[i] : np + i;
            }
            var args = string.Join(" ",
                ConsoleCmd.ConstructCmdLineArgs(GET_ASM_INDICES_PORT_CMD_SWITCH,
                    usePorts[0].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_IDS_PORT_CMD_SWITCH,
                    usePorts[1].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_NAMES_PORT_CMD_SWITCH,
                    usePorts[2].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_PAGE_RANK_PORT_CMD_SWITCH,
                    usePorts[3].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_TYPES_PORT_CMD_SWITCH,
                    usePorts[4].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(REASSIGN_TOKEN_NAMES_PORT_CMD_SWITCH,
                    usePorts[5].ToString(CultureInfo.InvariantCulture)),
                ConsoleCmd.ConstructCmdLineArgs(RESOLVE_GAC_ASM_SWITCH, resolveGacAsmNames.ToString()));

            MyProcess = StartRemoteProcess(NfConfig.CustomTools.InvokeAssemblyAnalysis, args);

            _getAsmIndiciesCmd = new InvokeGetAsmIndicies
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[0]
            };

            _getTokenIdsCmd = new InvokeGetTokenIds
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[1]
            };

            _getTokenNamesCmd = new InvokeGetTokenNames
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[2]
            };

            _getTokenPageRankCmd = new InvokeGetTokenPageRank
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[3]
            };

            _getTokenTypesCmd = new InvokeGetTokenTypes
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[4]
            };
            _reassignTokenNamesCmd = new InvokeReassignTokenNames
            {
                ProcessId = MyProcess.Id,
                SocketPort = usePorts[5]
            };
        }
        #endregion

        #region instance methods

        /// <summary>
        /// Gets the fully qualified assembly names to an index 
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public AsmIndexResponse GetAsmIndices(string assemblyPath)
        {
            if (string.IsNullOrWhiteSpace(assemblyPath) || !File.Exists(assemblyPath))
                throw new ItsDeadJim("This isn't a valid assembly path");

            _asmIndices = _getAsmIndiciesCmd.Receive(assemblyPath);
            return _asmIndices;
        }

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
        public TokenIdResponse GetTokenIds(int asmIdx, string recurseAnyAsmNamedLike = null)
        {
            _getTokenIdsCmd.RecurseAnyAsmNamedLike = recurseAnyAsmNamedLike;
            _getTokenIdsCmd.AsmIndices = _asmIndices;
            return _getTokenIdsCmd.Receive(asmIdx);
        }

        /// <summary>
        /// Resolves the given Metadata Token Ids to thier names & types.
        /// </summary>
        /// <param name="metadataTokenIds"></param>
        /// <param name="mapFullCallStack">
        /// Optional switch which directs the Assembly Analysis process to fully resolve all names as 
        /// a call stack tree.
        /// The remote proc will need to have the token id response and token type response in memory 
        /// from previous calls thereof.
        /// </param>
        /// <returns></returns>
        public TokenNameResponse GetTokenNames(MetadataTokenId[] metadataTokenIds, bool mapFullCallStack = false)
        {
            _getTokenNamesCmd.MapFullCallStack = mapFullCallStack;
            return _getTokenNamesCmd.Receive(metadataTokenIds);
        }

        public TokenPageRankResponse GetTokenPageRank(TokenIdResponse tokenIdResponse)
        {
            return _getTokenPageRankCmd.Receive(tokenIdResponse);
        }

        public TokenTypeResponse GetTokenTypes(string recurseAnyAsmNamedLike = null)
        {
            _getTokenTypesCmd.RecurseAnyAsmNamedLike = recurseAnyAsmNamedLike;
            return _getTokenTypesCmd.Receive(null);
        }

        /// <summary>
        /// An optional invocation, the caller could just invoke the ReassignAllInterfaceTokens
        /// on the <see cref="subjectNames"/>; however, for some assemblies this takes a long time.
        /// This option does the same thing only on a remote process with a progress indicator.
        /// </summary>
        /// <param name="subjectNames"></param>
        /// <param name="foreignNames"></param>
        /// <param name="foreignTokenTypes"></param>
        /// <returns></returns>
        public TokenReassignResponse ReassignTokenNames(MetadataTokenName subjectNames, MetadataTokenName foreignNames,
            MetadataTokenType foreignTokenTypes)
        {
            _reassignTokenNamesCmd.Request = new TokenReassignRequest
            {
                SubjectTokenNames = subjectNames,
                ForeignTokenNames = foreignNames,
                ForeignTokenTypes = foreignTokenTypes
            };
            return _reassignTokenNamesCmd.Receive(null);
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
        public static MetadataTokenName ConvertToMetadataTokenName(MemberInfo mi, AsmIndexResponse indicies, Func<string, bool> isIgnore)
        {
            if (mi == null)
                return null;

            var localIsIgnore = isIgnore ?? (s => false);
            var localIndicies = indicies ?? new AsmIndexResponse { Asms = new MetadataTokenAsm[0] };

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
                expandedName = !string.IsNullOrEmpty(asmName)
                    ? (type.FullName ?? UNKNOWN_NAME_SUB).Replace($"{asmName}", string.Empty)
                    : type.FullName;
                if (!string.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
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
                        string.Equals(x.AssemblyName, mi.DeclaringType.Assembly.GetName().FullName,
                            StringComparison.OrdinalIgnoreCase));
            if (f == null)
                return null;

            asmName = mi.DeclaringType.Assembly.GetName().Name;
            expandedName = !string.IsNullOrEmpty(asmName)
                ? (mi.DeclaringType.FullName ?? UNKNOWN_NAME_SUB).Replace($"{asmName}", string.Empty)
                : mi.DeclaringType.FullName;
            if (!string.Equals(expandedName, tokenName.Name, StringComparison.OrdinalIgnoreCase))
                tokenName.Name = $"{expandedName}{Constants.TYPE_METHOD_NAME_SPLIT_ON}{tokenName.Name}";

            tokenName.OwnAsmIdx = f.IndexId;

            var mti = mi as MethodInfo;
            if (mti == null)
                return tokenName;

            var mtiParams = mti.GetParameters();
            if (mtiParams.Length <= 0)
            {
                tokenName.Name = $"{tokenName.Name}()";
                return tokenName;
            }

            var paramNames = new List<string>();

            foreach (var param in mtiParams)
            {
                var workingName = param.ParameterType.FullName ?? UNKNOWN_NAME_SUB;
                
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

            tokenName.Name = $"{tokenName.Name}({string.Join(",", paramNames)})";

            return tokenName;
        }

        #endregion

        #region static analysis

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

            var token = new MetadataTokenId
            {
                Id = asmType.MetadataToken,
                RslvAsmIdx = asmIdx,
                Items =
                    asmType.GetMembers(NfSettings.DefaultFlags)
                        .Select(x => GetMetadataToken(x, true, asmIdx))
                        .Distinct()
                        .ToArray()
            };
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
            var token = new MetadataTokenId
            {
                Id = mi.MetadataToken, RslvAsmIdx = asmIdx, Items = new MetadataTokenId[0]
            };

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
