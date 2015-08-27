using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using NoFuture.Exceptions;
using NoFuture.Shared;
using NoFuture.Util.Binary;
using NoFuture.Util.Gia.Args;
using Newtonsoft.Json;

namespace NoFuture.Util.Gia
{
    /// <summary>
    /// Acts a a wrapper around the independet process of similar name.
    /// </summary>
    public class AssemblyAnalysis
    {
        #region constants
        public const string GET_TOKEN_IDS_PORT_CMD_SWITCH = "nfDumpAsmTokensPort";
        public const string GET_TOKEN_NAMES_PORT_CMD_SWITCH = "nfResolveTokensPort";
        public const string GET_ASM_INDICES_PORT_CMD_SWITCH = "nfGetAsmIndicies";
        public const string RESOLVE_GAC_ASM_SWITCH = "nfResolveGacAsm";
        public const int DF_START_PORT = 5059;
        #endregion

        #region fields
        private readonly InvokeDumpMetadataKey _myProcessPorts;
        private readonly AsmIndicies _asmIndices;
        #endregion

        #region inner types
        /// <summary>
        /// Data concerning the launch of <see cref="StartNewDumpMetadaTokenProcess"/>
        /// </summary>
        public class InvokeDumpMetadataKey
        {
            private readonly int _pid;
            public InvokeDumpMetadataKey(int pid)
            {
                _pid = pid;
            }

            public string AssemblyPath { get; set; }
            public int GetAsmIndiciesPort { get; set; }
            public int GetTokenIdsPort { get; set; }
            public int GetTokenNamesPort { get; set; }
            public int Pid { get { return _pid; } }

            public bool PortsAreValid
            {
                get
                {
                    return Net.IsValidPortNumber(GetAsmIndiciesPort) && Net.IsValidPortNumber(GetTokenIdsPort) &&
                           Net.IsValidPortNumber(GetTokenNamesPort);
                }
            }

            public bool ProcessIsRunning
            {
                get
                {
                    if (_pid == 0)
                        return false;
                    var proc = Process.GetProcessById(_pid);
                    return !proc.HasExited;
                }
            }
            
        }
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
        public AssemblyAnalysis(string assemblyPath, bool resolveGacAsmNames, params int[] ports)
        {
            if (String.IsNullOrWhiteSpace(assemblyPath) || !File.Exists(assemblyPath))
                throw new ItsDeadJim("This isn't a valid assembly path");

            if (String.IsNullOrWhiteSpace(CustomTools.InvokeAssemblyAnalysis) || !File.Exists(CustomTools.InvokeAssemblyAnalysis))
                throw new ItsDeadJim("Don't know where to locate the InvokeDumpMetadataTokens.exe, assign " +
                                     "the global variable at NoFuture.CustomTools.InvokeDumpMetadataTokens.");

            var invoke = StartNewDumpMetadaTokenProcess(assemblyPath, resolveGacAsmNames, ports);
            var myProcess = invoke.Item2;
            _myProcessPorts = invoke.Item1;

            if (_myProcessPorts == null || !_myProcessPorts.PortsAreValid)
                throw new ItsDeadJim(
                    String.Format("Could resolve the ports to used for connecting to process by pid [{0}]", myProcess.Id));

            //connect to the process on a socket 
            var asmIndicesBuffer = CallInvokeDumpMetadataToken(Encoding.UTF8.GetBytes(assemblyPath),
                _myProcessPorts.GetAsmIndiciesPort);
            _asmIndices =
                JsonConvert.DeserializeObject<AsmIndicies>(Encoding.UTF8.GetString(asmIndicesBuffer));
        }
        #endregion

        #region instance methods
        /// <summary>
        /// Gets the manifold of Metadata Token Ids for the assembly at <see cref="asmIdx"/>
        /// </summary>
        /// <param name="asmIdx"></param>
        /// <returns></returns>
        /// <remarks>
        /// Use the <see cref="AsmIndicies"/> property to get the index.
        /// </remarks>
        public TokenIds GetTokenIds(int asmIdx)
        {
            if (_asmIndices.Asms.All(x => x.IndexId != asmIdx))
                throw new RahRowRagee(string.Format("No matching index found for {0}", asmIdx));
            if(!_myProcessPorts.ProcessIsRunning)
                throw new RahRowRagee(string.Format("The process by id [{0}] has exited", _myProcessPorts.Pid));
            var asmName = _asmIndices.Asms.First(x => x.IndexId == asmIdx).AssemblyName;

            var bufferIn = Encoding.UTF8.GetBytes(asmName);
            var bufferOut = CallInvokeDumpMetadataToken(bufferIn, _myProcessPorts.GetTokenIdsPort);

            return JsonConvert.DeserializeObject<TokenIds>(Encoding.UTF8.GetString(bufferOut));
        }

        /// <summary>
        /// Resolves the given Metadata Token Ids to thier names & types.
        /// </summary>
        /// <param name="metadataTokenIds"></param>
        /// <returns></returns>
        public TokenNames ResolveTokenNames(int[] metadataTokenIds)
        {
            if (metadataTokenIds == null)
                throw new ArgumentNullException("metadataTokenIds");
            if (!_myProcessPorts.ProcessIsRunning)
                throw new RahRowRagee(string.Format("The process by id [{0}] has exited", _myProcessPorts.Pid));

            var bufferIn = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(metadataTokenIds));

            var bufferOut = CallInvokeDumpMetadataToken(bufferIn, _myProcessPorts.GetTokenNamesPort);

            return JsonConvert.DeserializeObject<TokenNames>(Encoding.UTF8.GetString(bufferOut));
        }
        #endregion

        #region metadata token statics
        /// <summary>
        /// Starts the NoFuture.Util.Gia.InvokeAssemblyAnalysis.exe process.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="ports"></param>
        /// <param name="resolveGacAsmNames"></param>
        /// <returns></returns>
        public static Tuple<InvokeDumpMetadataKey, Process> StartNewDumpMetadaTokenProcess(string assemblyPath, bool resolveGacAsmNames, params int[] ports)
        {

            var port00 = ports != null && ports.Length >= 1 ? ports[0] : DF_START_PORT;
            var port01 = ports != null && ports.Length >= 2 ? ports[1] : DF_START_PORT + 1;
            var port02 = ports != null && ports.Length >= 3 ? ports[2] : DF_START_PORT + 2;

            var args = String.Join(" ",
                new[]
                    {
                        ConsoleCmd.ConstructCmdLineArgs(GET_ASM_INDICES_PORT_CMD_SWITCH, port00.ToString(CultureInfo.InvariantCulture)),
                        ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_IDS_PORT_CMD_SWITCH, port01.ToString(CultureInfo.InvariantCulture)),
                        ConsoleCmd.ConstructCmdLineArgs(GET_TOKEN_NAMES_PORT_CMD_SWITCH, port02.ToString(CultureInfo.InvariantCulture)),
                        ConsoleCmd.ConstructCmdLineArgs(RESOLVE_GAC_ASM_SWITCH, resolveGacAsmNames.ToString())
                    });

            var proc = new Process
            {
                StartInfo =
                    new ProcessStartInfo(CustomTools.InvokeAssemblyAnalysis, args)
                    {
                        CreateNoWindow = false,
                        UseShellExecute = true,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false
                    }
            };

            proc.Start();
            Thread.Sleep(Constants.ThreadSleepTime);
            var key = new InvokeDumpMetadataKey(proc.Id)
            {
                AssemblyPath = assemblyPath,
                GetAsmIndiciesPort = port00,
                GetTokenIdsPort = port01,
                GetTokenNamesPort = port02
            };

            return new Tuple<InvokeDumpMetadataKey, Process>(key, proc);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static byte[] CallInvokeDumpMetadataToken(byte[] mdt, int port)
        {
            var buffer = new List<byte>();
            using (var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP))
            {

                server.Connect(new IPEndPoint(IPAddress.Loopback, port));
                server.Send(mdt);
                var data = new byte[Constants.DEFAULT_BLOCK_SIZE];

                //waits for response
                server.Receive(data, 0, data.Length, SocketFlags.None);
                buffer.AddRange(data.Where(b => b != (byte)'\0'));
                while (server.Available > 0)
                {

                    if (server.Available < Constants.DEFAULT_BLOCK_SIZE)
                    {
                        data = new byte[server.Available];
                        server.Receive(data, 0, server.Available, SocketFlags.None);
                    }
                    else
                    {
                        data = new byte[Constants.DEFAULT_BLOCK_SIZE];
                        server.Receive(data, 0, (int)Constants.DEFAULT_BLOCK_SIZE, SocketFlags.None);
                    }

                    buffer.AddRange(data.Where(b => b != (byte)'\0'));
                }
                server.Close();
            }
            return buffer.ToArray();
        }
        /// <summary>
        /// Get the <see cref="System.Reflection.MetadataToken"/> for the <see cref="asmType"/>
        /// and for all its members.
        /// </summary>
        /// <param name="asmType"></param>
        /// <returns></returns>
        public static MetadataTokenId GetMetadataToken(Type asmType)
        {
            if (asmType == null)
                return new MetadataTokenId() { Items = new MetadataTokenId[0] };

            var token = new MetadataTokenId { Id = asmType.MetadataToken };
            var manifold =
                asmType.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic |
                                   BindingFlags.Public | BindingFlags.Static).Select(x => GetMetadataToken(x, true)).ToList();
            token.Items = manifold.ToArray();
            return token;
        }

        /// <summary>
        /// Get the <see cref="System.Reflection.MetadataToken"/> for the <see cref="mi"/>
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="resolveManifold">True will cast the <see cref="mi"/> to 
        /// a <see cref="MethodBase"/> and find the tokens used within the 
        /// method's body. </param>
        /// <returns></returns>
        public static MetadataTokenId GetMetadataToken(MemberInfo mi, bool resolveManifold)
        {
            if (mi == null)
                return new MetadataTokenId();
            var token = new MetadataTokenId { Id = mi.MetadataToken, Items = new MetadataTokenId[0] };
            var mti = mi as MethodBase;
            if (mti == null)
                return token;

            token.Items = resolveManifold
                ? Binary.Asm.GetCallsMetadataTokens(mti).Select(t => new MetadataTokenId { Id = t }).ToArray()
                : new MetadataTokenId[0];

            return token;
        }
        #endregion

        #region static analysis
        /// <summary>
        /// Flattens a type and breaks each word on Pascel or camel-case
        /// then gets a count of that word.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <param name="flattenMaxDepth"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetAllPropertyWholeWordsByCount(Assembly assembly, string typeFullName, int flattenMaxDepth)
        {
            var flattenTypeArg = new FlattenTypeArgs()
            {
                Assembly = assembly,
                Depth = flattenMaxDepth,
                Separator = FlattenTypeArgs.DEFAULT_SEPARATOR,
                TypeFullName = typeFullName
            };
            var flattenedType = (Flatten.FlattenType(flattenTypeArg)).PrintLines();
            var allWords = new List<string>();

            foreach (var v in flattenedType)
            {
                if (!v.Contains(" "))
                    continue;
                var parts = v.Split((char)0x20);
                if (parts.Length < 2)
                    continue;
                var line = parts[1];
                if (String.IsNullOrWhiteSpace(line))
                    continue;

                var newline = Etc.TransformCamelCaseToSeparator(line, '-');
                allWords.AddRange(newline.Split('-'));
            }

            var allWholeWords = allWords.Distinct().ToList();
            var wordsByCount = new Dictionary<string, int>();

            foreach (var word in allWholeWords)
            {
                var wordCount = allWords.Count(x => x == word);
                if (wordCount <= 0)
                    continue;
                if (wordsByCount.ContainsKey(word))
                    wordsByCount[word] += wordCount;
                else
                    wordsByCount.Add(word, wordCount);
            }

            return wordsByCount;
        }

        /// <summary>
        /// Same as its counterpart <see cref="GetAllPropertyWholeWordsByCount"/> except doing so on every property on 
        /// every type within the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="flattenMaxDepth"></param>
        /// <returns></returns>
        public static Dictionary<string, int> GetAssemblyPropertyWholeWordsByCount(Assembly assembly,
            int flattenMaxDepth)
        {
            var startDictionary = new Dictionary<string, int>();
            foreach (var t in assembly.NfGetTypes())
            {
                var tPropWords = GetAllPropertyWholeWordsByCount(assembly, t.FullName, flattenMaxDepth);
                foreach (var k in tPropWords.Keys)
                {
                    if (startDictionary.ContainsKey(k))
                        startDictionary[k] += tPropWords[k];
                    else
                    {
                        startDictionary.Add(k, tPropWords[k]);
                    }
                }
            }
            return startDictionary;
        }

        /// <summary>
        /// Gets the words which appear on the left or right of 
        /// the target word <see cref="Args.AssemblyLeftRightArgs.TargetWord"/>, with optional limit 
        /// on the primitive type <see cref="Args.AssemblyLeftRightArgs.LimitOnThisType"/>
        /// having flattened the assembly.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static Tuple<FlattenedLine, FlattenedLine> MapAssemblyWordLeftAndRight(AssemblyLeftRightArgs arg)
        {
            if (arg == null)
                throw new ArgumentNullException("arg");

            var targetWord = arg.TargetWord;
            var assembly = arg.Assembly;
            var flattenMaxDepth = arg.MaxDepth;
            var justOnValueTypes = arg.JustOnValueTypes;
            var limitOnType = arg.LimitOnThisType;
            var separator = arg.Separator;
            var useTypeNames = arg.UseTypeNames;

            var allTypeNames = assembly.NfGetTypes().Select(x => x.FullName).ToList();
            var allLines = new List<FlattenedLine>();
            foreach (var t in allTypeNames)
            {
                var flattenArgs = new FlattenTypeArgs()
                {
                    Assembly = assembly,
                    TypeFullName = t,
                    Depth = flattenMaxDepth,
                    Separator = separator,
                    UseTypeNames = useTypeNames,
                    LimitOnThisType = limitOnType
                };
                var flattenedType = Flatten.FlattenType(flattenArgs);
                foreach (var line in flattenedType.Lines.Where(x => !String.IsNullOrWhiteSpace(x.ValueType)))
                {
                    if (!String.IsNullOrWhiteSpace(limitOnType) && !String.IsNullOrWhiteSpace(line.ValueType))
                        if (!String.Equals(limitOnType, line.ValueType, StringComparison.OrdinalIgnoreCase) &&
                            !String.Equals(limitOnType, TypeName.GetLastTypeNameFromArrayAndGeneric(line.ValueType), StringComparison.OrdinalIgnoreCase))
                            continue;

                    if (line == null)
                        continue;
                    if (allLines.Any(x => x.Equals(line)))
                        continue;
                    if (!line.Contains(targetWord))
                        continue;
                    if (justOnValueTypes && !line.LastItemContains(targetWord))
                        continue;

                    allLines.Add(line);
                }
            }

            Debug.WriteLine(String.Format("Refactored count of lines {0}",allLines.Count));

            if (allLines.Count <= 0)
                return null;

            var toTheLeft = new List<FlattenedItem>();
            var toTheRight = new List<FlattenedItem>();

            foreach (var line in allLines)
            {
                for (var i = 0; i < line.Items.Count; i++)
                {
                    if (line.UseTypeNames
                        ? !line.Items[i].SimpleTypeName.Contains(targetWord)
                        : !line.Items[i].FlName.Contains(targetWord))
                        continue;

                    var targetIndex = i;

                    if (targetIndex > 0)
                    {
                        var leftWord = line.Items[targetIndex - 1];
                        if (toTheLeft.Any(x => x.Equals(leftWord)))
                            continue;

                        toTheLeft.Add(leftWord);
                    }
                    if (targetIndex < line.Items.Count - 1)
                    {
                        var rightWord = line.Items[targetIndex + 1];
                        if (toTheRight.Any(x => x.Equals(rightWord)))
                            continue;

                        toTheRight.Add(rightWord);
                    }
                }
            }

            var flLeft = new FlattenedLine(toTheLeft)
            {
                Separator = arg.Separator,
                UseTypeNames = arg.UseTypeNames
            };
            var flRight = new FlattenedLine(toTheRight)
            {
                Separator = arg.Separator,
                UseTypeNames = arg.UseTypeNames
            };

            return new Tuple<FlattenedLine, FlattenedLine>(flLeft, flRight);
        }
        #endregion
    }
}
