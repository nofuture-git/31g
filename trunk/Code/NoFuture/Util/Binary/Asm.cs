using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Reflection.Emit;
using NoFuture.Exceptions;
using NoFuture.Shared;

namespace NoFuture.Util.Binary
{
    public static class Asm
    {
        #region constants
        public const string DEFAULT_ASM_LOG_FILE_NAME = "ResolveAssemblyEventHandler.log";
        #endregion

        #region fields
        private static readonly Dictionary<int, OpCode> _opCodes2Value = new Dictionary<int, OpCode>();
        private static string _resolveAsmLog;
        private static readonly object _resolveAsmLogLock = new object();
        #endregion

        #region asm logging
        /// <summary>
        /// A log file used to record assembly load events.
        /// </summary>
        public static string ResolveAsmLog
        {
            get
            {
                if (!String.IsNullOrWhiteSpace(_resolveAsmLog))
                    return _resolveAsmLog;
                var logDir = String.IsNullOrWhiteSpace(TempDirectories.Debug) || !Directory.Exists(TempDirectories.Debug)
                    ? TempDirectories.AppData
                    : TempDirectories.Debug;
                _resolveAsmLog = Path.Combine(logDir, DEFAULT_ASM_LOG_FILE_NAME);
                return _resolveAsmLog;
            }
            set { _resolveAsmLog = value; }
        }

        /// <summary>
        /// Intended for logging exception which are thrown when calling <see cref="Assembly.GetTypes"/>
        /// </summary>
        /// <param name="attemptedResolveType">
        /// Optional parameter for more detail about which type we were attempting to resolve at the time of exception.
        /// </param>
        /// <param name="ex">The exception being caught</param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        public static void AddLoaderExceptionToLog(Tuple<Type, MemberInfo> attemptedResolveType, Exception ex, string logFile = "")
        {
            if (ex == null)
                return;
            var logFilePath = string.IsNullOrWhiteSpace(logFile) ||
                              string.IsNullOrWhiteSpace(Path.GetDirectoryName(logFile)) ||
                              !Directory.Exists(Path.GetDirectoryName(logFile))
                ? ResolveAsmLog
                : logFile;

            lock (_resolveAsmLogLock)
            {
                try
                {
                    var logEntry = GetExceptionStrBldr(attemptedResolveType, ex);
                    File.AppendAllText(logFilePath,
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                    var fnfe = ex as FileNotFoundException;
                    var tle = ex as TypeLoadException;
                    var fle = ex as FileLoadException;
                    var bife = ex as BadImageFormatException;
                    var rftl = ex as ReflectionTypeLoadException;

                    if (rftl != null && rftl.LoaderExceptions != null && rftl.LoaderExceptions.Length > 0)
                    {
                        logEntry.AppendLine("LoaderExceptions:");
                        for (var i = 0; i < rftl.LoaderExceptions.Length; i++)
                        {
                            var loaderText = GetExceptionStrBldr(null, rftl.LoaderExceptions[i]);
                            logEntry.Append($"({i}) {loaderText}");
                        }
                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (fnfe != null)
                    {
                        logEntry.AppendFormat("FusionLog:{0}", fnfe.FusionLog);
                        logEntry.AppendLine();
                        logEntry.AppendFormat("FileName:{0}", fnfe.FileName);

                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (tle != null)
                    {
                        logEntry.AppendFormat("TypeLoaderException causing type: '{0}'", tle.TypeName);
                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (fle != null)
                    {
                        logEntry.AppendFormat("FusionLog:{0}", fle.FusionLog);
                        logEntry.AppendLine();
                        logEntry.AppendFormat("FileName:{0}", fle.FileName);

                        File.AppendAllText(logFilePath,
                            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                        return;
                    }

                    if (bife == null) return;

                    logEntry.AppendFormat("FusionLog:{0}", bife.FusionLog);
                    logEntry.AppendLine();
                    logEntry.AppendFormat("FileName:{0}", bife.FileName);

                    File.AppendAllText(logFilePath,
                        $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] {logEntry}\n");

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    //Yuron Yurown
                }
            }
        }
        #endregion

        /// <summary>
        /// The actual search functionality used by the 
        /// <see cref="FxPointers.ResolveReflectionOnlyAssembly"/> - in itself reuseable.
        /// </summary>
        /// <param name="asmFullName">This is expected as the fully qualified assembly name.</param>
        /// <param name="searchDirectory"></param>
        /// <param name="reflectionOnly">
        /// Branches search to <see cref="AppDomain.ReflectionOnlyGetAssemblies"/>
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// see
        /// [https://msdn.microsoft.com/en-us/library/k8xx4k69(v=vs.110).aspx]
        /// NOTE: attempting to send both the ReflectionOnlyAssemblyResolve and the AssemblyResolve
        /// to this same method has caused the entire process to crash.
        /// </remarks>
        public static Assembly SearchAppDomainForAssembly(string asmFullName, string searchDirectory = "",
            bool reflectionOnly = true)
        {
            Assembly foundOne = null;
            //an assembly'esque name is critical to this effort
            if (String.IsNullOrWhiteSpace(asmFullName))
                throw new ItsDeadJim(
                    $"There is no way to resolve a assembly dependency with a name of '{asmFullName}'");


            File.AppendAllText(ResolveAsmLog,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] Starting search for the assembly named '{asmFullName}' with reflection only set to '{reflectionOnly}'. \n");

            //if its already in the appdomain then just return it, why are we here in such a case?
            var itsAlreadyHere = AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                .FirstOrDefault(
                    asm => AssemblyName.ReferenceMatchesDefinition(asm.GetName(), new AssemblyName(asmFullName))) ??
                                      AppDomain.CurrentDomain.GetAssemblies()
                                          .FirstOrDefault(
                                              asm => AssemblyName.ReferenceMatchesDefinition(asm.GetName(), new AssemblyName(asmFullName)));
            if (itsAlreadyHere != null)
                return itsAlreadyHere;

            //if requesting assembly has a location then start there
            if (!String.IsNullOrWhiteSpace(searchDirectory))
            {
                //expect dependecies from the GAC to be resolved using the GAC
                if (searchDirectory.Contains("\\GAC_MSIL\\") || searchDirectory.Contains("\\GAC_64\\") ||
                    searchDirectory.StartsWith(@"C:\Windows\Microsoft.NET\Framework"))
                    return reflectionOnly ? Assembly.ReflectionOnlyLoad(asmFullName) : Assembly.Load(asmFullName);

                if (SearchDirectoriesForAssembly(Path.GetDirectoryName(searchDirectory), asmFullName, out foundOne,
                    reflectionOnly))
                    return foundOne;
            }

            //search the directory explicity set
            if (NfConfig.AssemblySearchPaths.Any())
            {
                if (
                    NfConfig.AssemblySearchPaths.Distinct()
                        .Any(sd => SearchDirectoriesForAssembly(sd, asmFullName, out foundOne,
                            reflectionOnly)))
                {
                    return foundOne;
                }
            }

            //check in NoFuture's bin
            if (!String.IsNullOrWhiteSpace(BinDirectories.Root))
            {
                if (SearchDirectoriesForAssembly(BinDirectories.Root, asmFullName, out foundOne, reflectionOnly))
                    return foundOne;
            }

            //search this domain's working directory
            if (SearchDirectoriesForAssembly(AppDomain.CurrentDomain.BaseDirectory, asmFullName, out foundOne,
                reflectionOnly))
                return foundOne;

            //have next app domain handler give it a go
            return null;
        }

        #region metadata tokens
        /// <summary>
        /// Gets the length of a <see cref="OpCode.OperandType"/>
        /// as the number of bytes.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://msdn.microsoft.com/en-us/library/system.reflection.emit.operandtype(v=vs.110).aspx
        /// </remarks>
        public static int GetOpCodeOperandByteSize(OpCode op)
        {
            switch (op.OperandType)
            {
                case OperandType.InlineI8:
                case OperandType.InlineR:
                    return 8;
                case OperandType.InlineVar:
                    return 2;
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineVar:
                case OperandType.ShortInlineI:
                    return 1;
                case OperandType.InlineNone:
                    return 0;
                default:
                    return 4;
            }
        }

        /// <summary>
        /// Gets the <see cref="OpCode"/> whose <see cref="OpCode.Value"/> equals <see cref="opCodeValue"/>
        /// </summary>
        /// <param name="opCodeValue"></param>
        /// <returns></returns>
        /// <remarks>
        /// partially generated using the following PowerShell:
        /// <![CDATA[
        ///[System.Reflection.Assembly]::LoadWithPartialName("System.Reflection.Emit")
        ///
        ///$refToOpCodes = ([System.Reflection.Emit.OpCodes])
        ///
        ///([System.Reflection.Emit.OpCodes]).GetFields() | ? {$_.FieldType.Name -eq  "OpCode"} | % { 
        ///    $opCodeName = $_.Name
        ///    $opCodeRef = $refToOpCodes::$opCodeName
        ///    $opCodeValue = $opCodeRef.Value
        ///
        ///@"
        ///                _opCodes2Value.Add($opCodeValue, OpCodes.$opCodeName);
        ///"@    
        ///}
        /// ]]>
        /// </remarks>
        public static OpCode GetOpCodeByValue(int opCodeValue)
        {
            if (_opCodes2Value.Count <= 0)
            {
                _opCodes2Value.Add(0, OpCodes.Nop);
                _opCodes2Value.Add(1, OpCodes.Break);
                _opCodes2Value.Add(2, OpCodes.Ldarg_0);
                _opCodes2Value.Add(3, OpCodes.Ldarg_1);
                _opCodes2Value.Add(4, OpCodes.Ldarg_2);
                _opCodes2Value.Add(5, OpCodes.Ldarg_3);
                _opCodes2Value.Add(6, OpCodes.Ldloc_0);
                _opCodes2Value.Add(7, OpCodes.Ldloc_1);
                _opCodes2Value.Add(8, OpCodes.Ldloc_2);
                _opCodes2Value.Add(9, OpCodes.Ldloc_3);
                _opCodes2Value.Add(10, OpCodes.Stloc_0);
                _opCodes2Value.Add(11, OpCodes.Stloc_1);
                _opCodes2Value.Add(12, OpCodes.Stloc_2);
                _opCodes2Value.Add(13, OpCodes.Stloc_3);
                _opCodes2Value.Add(14, OpCodes.Ldarg_S);
                _opCodes2Value.Add(15, OpCodes.Ldarga_S);
                _opCodes2Value.Add(16, OpCodes.Starg_S);
                _opCodes2Value.Add(17, OpCodes.Ldloc_S);
                _opCodes2Value.Add(18, OpCodes.Ldloca_S);
                _opCodes2Value.Add(19, OpCodes.Stloc_S);
                _opCodes2Value.Add(20, OpCodes.Ldnull);
                _opCodes2Value.Add(21, OpCodes.Ldc_I4_M1);
                _opCodes2Value.Add(22, OpCodes.Ldc_I4_0);
                _opCodes2Value.Add(23, OpCodes.Ldc_I4_1);
                _opCodes2Value.Add(24, OpCodes.Ldc_I4_2);
                _opCodes2Value.Add(25, OpCodes.Ldc_I4_3);
                _opCodes2Value.Add(26, OpCodes.Ldc_I4_4);
                _opCodes2Value.Add(27, OpCodes.Ldc_I4_5);
                _opCodes2Value.Add(28, OpCodes.Ldc_I4_6);
                _opCodes2Value.Add(29, OpCodes.Ldc_I4_7);
                _opCodes2Value.Add(30, OpCodes.Ldc_I4_8);
                _opCodes2Value.Add(31, OpCodes.Ldc_I4_S);
                _opCodes2Value.Add(32, OpCodes.Ldc_I4);
                _opCodes2Value.Add(33, OpCodes.Ldc_I8);
                _opCodes2Value.Add(34, OpCodes.Ldc_R4);
                _opCodes2Value.Add(35, OpCodes.Ldc_R8);
                _opCodes2Value.Add(37, OpCodes.Dup);
                _opCodes2Value.Add(38, OpCodes.Pop);
                _opCodes2Value.Add(39, OpCodes.Jmp);
                _opCodes2Value.Add(40, OpCodes.Call);
                _opCodes2Value.Add(41, OpCodes.Calli);
                _opCodes2Value.Add(42, OpCodes.Ret);
                _opCodes2Value.Add(43, OpCodes.Br_S);
                _opCodes2Value.Add(44, OpCodes.Brfalse_S);
                _opCodes2Value.Add(45, OpCodes.Brtrue_S);
                _opCodes2Value.Add(46, OpCodes.Beq_S);
                _opCodes2Value.Add(47, OpCodes.Bge_S);
                _opCodes2Value.Add(48, OpCodes.Bgt_S);
                _opCodes2Value.Add(49, OpCodes.Ble_S);
                _opCodes2Value.Add(50, OpCodes.Blt_S);
                _opCodes2Value.Add(51, OpCodes.Bne_Un_S);
                _opCodes2Value.Add(52, OpCodes.Bge_Un_S);
                _opCodes2Value.Add(53, OpCodes.Bgt_Un_S);
                _opCodes2Value.Add(54, OpCodes.Ble_Un_S);
                _opCodes2Value.Add(55, OpCodes.Blt_Un_S);
                _opCodes2Value.Add(56, OpCodes.Br);
                _opCodes2Value.Add(57, OpCodes.Brfalse);
                _opCodes2Value.Add(58, OpCodes.Brtrue);
                _opCodes2Value.Add(59, OpCodes.Beq);
                _opCodes2Value.Add(60, OpCodes.Bge);
                _opCodes2Value.Add(61, OpCodes.Bgt);
                _opCodes2Value.Add(62, OpCodes.Ble);
                _opCodes2Value.Add(63, OpCodes.Blt);
                _opCodes2Value.Add(64, OpCodes.Bne_Un);
                _opCodes2Value.Add(65, OpCodes.Bge_Un);
                _opCodes2Value.Add(66, OpCodes.Bgt_Un);
                _opCodes2Value.Add(67, OpCodes.Ble_Un);
                _opCodes2Value.Add(68, OpCodes.Blt_Un);
                _opCodes2Value.Add(69, OpCodes.Switch);
                _opCodes2Value.Add(70, OpCodes.Ldind_I1);
                _opCodes2Value.Add(71, OpCodes.Ldind_U1);
                _opCodes2Value.Add(72, OpCodes.Ldind_I2);
                _opCodes2Value.Add(73, OpCodes.Ldind_U2);
                _opCodes2Value.Add(74, OpCodes.Ldind_I4);
                _opCodes2Value.Add(75, OpCodes.Ldind_U4);
                _opCodes2Value.Add(76, OpCodes.Ldind_I8);
                _opCodes2Value.Add(77, OpCodes.Ldind_I);
                _opCodes2Value.Add(78, OpCodes.Ldind_R4);
                _opCodes2Value.Add(79, OpCodes.Ldind_R8);
                _opCodes2Value.Add(80, OpCodes.Ldind_Ref);
                _opCodes2Value.Add(81, OpCodes.Stind_Ref);
                _opCodes2Value.Add(82, OpCodes.Stind_I1);
                _opCodes2Value.Add(83, OpCodes.Stind_I2);
                _opCodes2Value.Add(84, OpCodes.Stind_I4);
                _opCodes2Value.Add(85, OpCodes.Stind_I8);
                _opCodes2Value.Add(86, OpCodes.Stind_R4);
                _opCodes2Value.Add(87, OpCodes.Stind_R8);
                _opCodes2Value.Add(88, OpCodes.Add);
                _opCodes2Value.Add(89, OpCodes.Sub);
                _opCodes2Value.Add(90, OpCodes.Mul);
                _opCodes2Value.Add(91, OpCodes.Div);
                _opCodes2Value.Add(92, OpCodes.Div_Un);
                _opCodes2Value.Add(93, OpCodes.Rem);
                _opCodes2Value.Add(94, OpCodes.Rem_Un);
                _opCodes2Value.Add(95, OpCodes.And);
                _opCodes2Value.Add(96, OpCodes.Or);
                _opCodes2Value.Add(97, OpCodes.Xor);
                _opCodes2Value.Add(98, OpCodes.Shl);
                _opCodes2Value.Add(99, OpCodes.Shr);
                _opCodes2Value.Add(100, OpCodes.Shr_Un);
                _opCodes2Value.Add(101, OpCodes.Neg);
                _opCodes2Value.Add(102, OpCodes.Not);
                _opCodes2Value.Add(103, OpCodes.Conv_I1);
                _opCodes2Value.Add(104, OpCodes.Conv_I2);
                _opCodes2Value.Add(105, OpCodes.Conv_I4);
                _opCodes2Value.Add(106, OpCodes.Conv_I8);
                _opCodes2Value.Add(107, OpCodes.Conv_R4);
                _opCodes2Value.Add(108, OpCodes.Conv_R8);
                _opCodes2Value.Add(109, OpCodes.Conv_U4);
                _opCodes2Value.Add(110, OpCodes.Conv_U8);
                _opCodes2Value.Add(111, OpCodes.Callvirt);
                _opCodes2Value.Add(112, OpCodes.Cpobj);
                _opCodes2Value.Add(113, OpCodes.Ldobj);
                _opCodes2Value.Add(114, OpCodes.Ldstr);
                _opCodes2Value.Add(115, OpCodes.Newobj);
                _opCodes2Value.Add(116, OpCodes.Castclass);
                _opCodes2Value.Add(117, OpCodes.Isinst);
                _opCodes2Value.Add(118, OpCodes.Conv_R_Un);
                _opCodes2Value.Add(121, OpCodes.Unbox);
                _opCodes2Value.Add(122, OpCodes.Throw);
                _opCodes2Value.Add(123, OpCodes.Ldfld);
                _opCodes2Value.Add(124, OpCodes.Ldflda);
                _opCodes2Value.Add(125, OpCodes.Stfld);
                _opCodes2Value.Add(126, OpCodes.Ldsfld);
                _opCodes2Value.Add(127, OpCodes.Ldsflda);
                _opCodes2Value.Add(128, OpCodes.Stsfld);
                _opCodes2Value.Add(129, OpCodes.Stobj);
                _opCodes2Value.Add(130, OpCodes.Conv_Ovf_I1_Un);
                _opCodes2Value.Add(131, OpCodes.Conv_Ovf_I2_Un);
                _opCodes2Value.Add(132, OpCodes.Conv_Ovf_I4_Un);
                _opCodes2Value.Add(133, OpCodes.Conv_Ovf_I8_Un);
                _opCodes2Value.Add(134, OpCodes.Conv_Ovf_U1_Un);
                _opCodes2Value.Add(135, OpCodes.Conv_Ovf_U2_Un);
                _opCodes2Value.Add(136, OpCodes.Conv_Ovf_U4_Un);
                _opCodes2Value.Add(137, OpCodes.Conv_Ovf_U8_Un);
                _opCodes2Value.Add(138, OpCodes.Conv_Ovf_I_Un);
                _opCodes2Value.Add(139, OpCodes.Conv_Ovf_U_Un);
                _opCodes2Value.Add(140, OpCodes.Box);
                _opCodes2Value.Add(141, OpCodes.Newarr);
                _opCodes2Value.Add(142, OpCodes.Ldlen);
                _opCodes2Value.Add(143, OpCodes.Ldelema);
                _opCodes2Value.Add(144, OpCodes.Ldelem_I1);
                _opCodes2Value.Add(145, OpCodes.Ldelem_U1);
                _opCodes2Value.Add(146, OpCodes.Ldelem_I2);
                _opCodes2Value.Add(147, OpCodes.Ldelem_U2);
                _opCodes2Value.Add(148, OpCodes.Ldelem_I4);
                _opCodes2Value.Add(149, OpCodes.Ldelem_U4);
                _opCodes2Value.Add(150, OpCodes.Ldelem_I8);
                _opCodes2Value.Add(151, OpCodes.Ldelem_I);
                _opCodes2Value.Add(152, OpCodes.Ldelem_R4);
                _opCodes2Value.Add(153, OpCodes.Ldelem_R8);
                _opCodes2Value.Add(154, OpCodes.Ldelem_Ref);
                _opCodes2Value.Add(155, OpCodes.Stelem_I);
                _opCodes2Value.Add(156, OpCodes.Stelem_I1);
                _opCodes2Value.Add(157, OpCodes.Stelem_I2);
                _opCodes2Value.Add(158, OpCodes.Stelem_I4);
                _opCodes2Value.Add(159, OpCodes.Stelem_I8);
                _opCodes2Value.Add(160, OpCodes.Stelem_R4);
                _opCodes2Value.Add(161, OpCodes.Stelem_R8);
                _opCodes2Value.Add(162, OpCodes.Stelem_Ref);
                _opCodes2Value.Add(163, OpCodes.Ldelem);
                _opCodes2Value.Add(164, OpCodes.Stelem);
                _opCodes2Value.Add(165, OpCodes.Unbox_Any);
                _opCodes2Value.Add(179, OpCodes.Conv_Ovf_I1);
                _opCodes2Value.Add(180, OpCodes.Conv_Ovf_U1);
                _opCodes2Value.Add(181, OpCodes.Conv_Ovf_I2);
                _opCodes2Value.Add(182, OpCodes.Conv_Ovf_U2);
                _opCodes2Value.Add(183, OpCodes.Conv_Ovf_I4);
                _opCodes2Value.Add(184, OpCodes.Conv_Ovf_U4);
                _opCodes2Value.Add(185, OpCodes.Conv_Ovf_I8);
                _opCodes2Value.Add(186, OpCodes.Conv_Ovf_U8);
                _opCodes2Value.Add(194, OpCodes.Refanyval);
                _opCodes2Value.Add(195, OpCodes.Ckfinite);
                _opCodes2Value.Add(198, OpCodes.Mkrefany);
                _opCodes2Value.Add(208, OpCodes.Ldtoken);
                _opCodes2Value.Add(209, OpCodes.Conv_U2);
                _opCodes2Value.Add(210, OpCodes.Conv_U1);
                _opCodes2Value.Add(211, OpCodes.Conv_I);
                _opCodes2Value.Add(212, OpCodes.Conv_Ovf_I);
                _opCodes2Value.Add(213, OpCodes.Conv_Ovf_U);
                _opCodes2Value.Add(214, OpCodes.Add_Ovf);
                _opCodes2Value.Add(215, OpCodes.Add_Ovf_Un);
                _opCodes2Value.Add(216, OpCodes.Mul_Ovf);
                _opCodes2Value.Add(217, OpCodes.Mul_Ovf_Un);
                _opCodes2Value.Add(218, OpCodes.Sub_Ovf);
                _opCodes2Value.Add(219, OpCodes.Sub_Ovf_Un);
                _opCodes2Value.Add(220, OpCodes.Endfinally);
                _opCodes2Value.Add(221, OpCodes.Leave);
                _opCodes2Value.Add(222, OpCodes.Leave_S);
                _opCodes2Value.Add(223, OpCodes.Stind_I);
                _opCodes2Value.Add(224, OpCodes.Conv_U);
                _opCodes2Value.Add(248, OpCodes.Prefix7);
                _opCodes2Value.Add(249, OpCodes.Prefix6);
                _opCodes2Value.Add(250, OpCodes.Prefix5);
                _opCodes2Value.Add(251, OpCodes.Prefix4);
                _opCodes2Value.Add(252, OpCodes.Prefix3);
                _opCodes2Value.Add(253, OpCodes.Prefix2);
                _opCodes2Value.Add(254, OpCodes.Prefix1);
                _opCodes2Value.Add(255, OpCodes.Prefixref);
                _opCodes2Value.Add(-512, OpCodes.Arglist);
                _opCodes2Value.Add(-511, OpCodes.Ceq);
                _opCodes2Value.Add(-510, OpCodes.Cgt);
                _opCodes2Value.Add(-509, OpCodes.Cgt_Un);
                _opCodes2Value.Add(-508, OpCodes.Clt);
                _opCodes2Value.Add(-507, OpCodes.Clt_Un);
                _opCodes2Value.Add(-506, OpCodes.Ldftn);
                _opCodes2Value.Add(-505, OpCodes.Ldvirtftn);
                _opCodes2Value.Add(-503, OpCodes.Ldarg);
                _opCodes2Value.Add(-502, OpCodes.Ldarga);
                _opCodes2Value.Add(-501, OpCodes.Starg);
                _opCodes2Value.Add(-500, OpCodes.Ldloc);
                _opCodes2Value.Add(-499, OpCodes.Ldloca);
                _opCodes2Value.Add(-498, OpCodes.Stloc);
                _opCodes2Value.Add(-497, OpCodes.Localloc);
                _opCodes2Value.Add(-495, OpCodes.Endfilter);
                _opCodes2Value.Add(-494, OpCodes.Unaligned);
                _opCodes2Value.Add(-493, OpCodes.Volatile);
                _opCodes2Value.Add(-492, OpCodes.Tailcall);
                _opCodes2Value.Add(-491, OpCodes.Initobj);
                _opCodes2Value.Add(-490, OpCodes.Constrained);
                _opCodes2Value.Add(-489, OpCodes.Cpblk);
                _opCodes2Value.Add(-488, OpCodes.Initblk);
                _opCodes2Value.Add(-486, OpCodes.Rethrow);
                _opCodes2Value.Add(-484, OpCodes.Sizeof);
                _opCodes2Value.Add(-483, OpCodes.Refanytype);
                _opCodes2Value.Add(-482, OpCodes.Readonly);
            }
            if (_opCodes2Value.Any(x => x.Key == opCodeValue))
                return _opCodes2Value.First(x => x.Key == opCodeValue).Value;
            return OpCodes.Nop;
        }

        /// <summary>
        /// Given the <see cref="rawIlByteStream"/> the occurance of 
        /// </summary>
        /// <param name="rawIlByteStream"></param>
        /// <param name="opCodes"></param>
        /// <returns></returns>
        /// <remarks>
        /// Pass the metadata token into an assembly's manifest module's <see cref="System.Reflection.Module.ResolveMember(int)"/>
        /// to get the <see cref="MethodInfo"/>
        /// </remarks>
        public static int[] GetOpCodesArgs(byte[] rawIlByteStream, OpCode[] opCodes)
        {
            if (rawIlByteStream == null)
                return null;

            var metadataTokens = new List<int>();

            //expecting first byte to always be an OpCode, never an arg thereof
            for (var i = 0; i < rawIlByteStream.Length; i++)
            {
                var opCodei = GetOpCodeByValue(rawIlByteStream[i]);
                var moveBuffer = GetOpCodeOperandByteSize(opCodei);
                if (moveBuffer == 0 || moveBuffer > 4 ||//don't handle IA64 since its not resolvable as a token
                    (rawIlByteStream.Length <= i + moveBuffer) ||
                    (opCodes.All(x => opCodei != x)))
                {
                    i = i + moveBuffer;
                    continue;
                }

                var metadataToken = 0;
                switch (moveBuffer)
                {
                    case 1:
                        metadataToken = rawIlByteStream[i + 1];
                        break;
                    case 2:
                        metadataToken = ByteArray.ToWord(rawIlByteStream[i + 2], rawIlByteStream[i + 1]);
                        break;
                    case 4:
                        metadataToken = ByteArray.ToDWord(rawIlByteStream[i + 4], rawIlByteStream[i + 3],
                            rawIlByteStream[i + 2], rawIlByteStream[i + 1]);
                        break;
                }

                //get only distinct entries
                if (metadataToken != 0 && metadataTokens.All(x => x != metadataToken))
                    metadataTokens.Add(metadataToken);

                i = i + moveBuffer;
            }

            return metadataTokens.ToArray();
        }

        /// <summary>
        /// Gets a method's body as a list of IL <see cref="OpCode"/> 
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static OpCode[] GetOpCodesList(MethodBase mb)
        {
            var codesOut = new List<OpCode>();
            if (mb == null)
                return codesOut.ToArray();
            var il = GetMethodBody(mb);
            if (il == null || !il.Any())
                return codesOut.ToArray();
            for (var i = 0; i < il.Length; i++)
            {
                var opCodei = GetOpCodeByValue(il[i]);
                codesOut.Add(opCodei);
                var moveBuffer = GetOpCodeOperandByteSize(opCodei);
                if (il.Length <= i + moveBuffer)
                {
                    return codesOut.ToArray();
                }
                i = i + moveBuffer;
            }
            return codesOut.ToArray();
        }

        /// <summary>
        /// Gets the IL byte array of the method.
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static byte[] GetMethodBody(MethodBase mb)
        {
            var emptyArray = new byte[0];
            if (mb == null)
                return emptyArray;
            var body = mb.GetMethodBody();
            return body == null ? emptyArray : body.GetILAsByteArray();
        }

        /// <summary>
        /// Helper method for its overload checking 
        /// for nulls down to the <see cref="System.Reflection.MethodBody"/>
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="opCodes"></param>
        /// <returns></returns>
        public static int[] GetOpCodesArgs(MethodBase mb, OpCode[] opCodes)
        {
            var il = GetMethodBody(mb);
            return il == null || !il.Any() ? new int[0] : GetOpCodesArgs(il, opCodes);
        }

        /// <summary>
        /// Uses the <see cref="OpCodes.Callvirt"/> and <see cref="OpCodes.Call"/>
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static int[] GetCallsMetadataTokens(MethodBase mb)
        {
            return GetOpCodesArgs(mb, new[] { OpCodes.Callvirt, OpCodes.Call });
        }
        #endregion

        #region reflection api wrappers
        /// <summary>
        /// Intended when an assembly is only recognizable by its location and therefore must 
        /// be loaded as such.  The reflection only load-from assemblies are 
        /// loaded from <see cref="TempDirectories.Binary"/>.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static Assembly NfReflectionOnlyLoadFrom(string assemblyPath)
        {
            if(String.IsNullOrWhiteSpace(assemblyPath))
                throw new ItsDeadJim($"There is no assembly at the path '{assemblyPath}'");

            //check our domain for this assembly having been loaded once before
            var asm =
                AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .FirstOrDefault(x => x.Location == assemblyPath);
            if (asm != null)
                return asm;

            asm = Assembly.ReflectionOnlyLoadFrom(assemblyPath);
            if (asm != null &&
                NfConfig.AssemblySearchPaths.Any(
                    x => !string.Equals(x, Path.GetDirectoryName(assemblyPath), StringComparison.OrdinalIgnoreCase)))
                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(assemblyPath));

            return asm;
        }

        /// <summary>
        /// Intended when an assembly is only recognizable by its location and therefore must 
        /// be loaded as such.  The reflection only load-from assemblies are 
        /// loaded from <see cref="TempDirectories.Binary"/>.
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static Assembly NfLoadFrom(string assemblyPath)
        {
            if (String.IsNullOrWhiteSpace(assemblyPath))
                throw new ItsDeadJim($"There is no assembly at the path '{assemblyPath}'");

            //check our domain for this assembly having been loaded once before
            var asm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => !x.IsDynamic)
                    .FirstOrDefault(x => x.Location == assemblyPath);
            if (asm != null)
                return asm;

            asm = Assembly.LoadFrom(assemblyPath);
            if (asm != null &&
                NfConfig.AssemblySearchPaths.Any(
                    x => !string.Equals(x, Path.GetDirectoryName(assemblyPath), StringComparison.OrdinalIgnoreCase)))
                NfConfig.AssemblySearchPaths.Add(Path.GetDirectoryName(assemblyPath));

            return asm;

        }

        /// <summary>
        /// Simply wraps the call to <see cref="Assembly.GetTypes"/> with a try-catch
        /// that adds logging.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type[] NfGetTypes(this Assembly assembly, bool rethrow = true, string logFile = "")
        {
            if (assembly == null)
                return null;
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Simply wraps the call to <see cref="Assembly.GetExportedTypes"/> with a try-catch
        /// that adds logging.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type[] NfGetExportedTypes(this Assembly assembly, bool rethrow = true, string logFile = "")
        {
            if (assembly == null)
                return null;
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Assembly.GetType(string)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFullName"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfGetType(this Assembly assembly, string typeFullName, bool rethrow = true, string logFile = "")
        {
            if (assembly == null || String.IsNullOrWhiteSpace(typeFullName))
                return null;
            try
            {
                return assembly.GetType(typeFullName);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if(rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethod(name);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String, BindingFlags)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, BindingFlags bindingAttr, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethod(name, bindingAttr);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;            
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String, Type[])"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, Type[] types, bool rethrow = true,
            string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethod(name, types);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;             
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethod(System.String,BindingFlags,Binder,Type[],ParameterModifier[])"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="binder"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetMethod(this Type t, string name, BindingFlags bindingAttr, Binder binder,
            Type[] types, ParameterModifier[] modifiers, bool rethrow = true,
            string logFile = "")
        {
            if (t == null || types == null)
                return null;
            try
            {
                return t.GetMethod(name, bindingAttr, binder, types, modifiers);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="Type.GetMethods(BindingFlags)"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo[] NfGetMethods(this Type t, BindingFlags bindingAttr, bool rethrow = true,
            string logFile = "")
        {
            if (t == null)
                return null;
            try
            {
                return t.GetMethods(bindingAttr);
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;             
        }

        /// <summary>
        /// Wraps the call to <see cref="PropertyInfo.PropertyType"/> adding logging 
        /// of typical loader exceptions - the exception is still thrown.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfPropertyType(this PropertyInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;
            try
            {
                return pi.PropertyType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="FieldInfo.FieldType"/> adding logging 
        /// of typical loader exceptions - the exception is still thrown.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfFieldType(this FieldInfo fi, bool rethrow = true, string logFile = "")
        {
            if (fi == null)
                return null;

            try
            {
                return fi.FieldType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, fi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="MethodInfo.ReturnType"/> adding logging 
        /// of typical loader exceptions - the exception is still thrown.
        /// </summary>
        /// <param name="mi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfReturnType(this MethodInfo mi, bool rethrow = true, string logFile = "")
        {
            if (mi == null)
                return null;
            try
            {
                return mi.ReturnType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, mi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="ParameterInfo.ParameterType"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfParameterType(this ParameterInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;

            try
            {
                return pi.ParameterType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(null, rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(null, fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(null, tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(null, fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(null, bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="EventInfo.EventHandlerType"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="ei"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfEventHandlerType(this EventInfo ei, bool rethrow = true, string logFile = "")
        {
            if (ei == null)
                return null;

            try
            {
                return ei.EventHandlerType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, ei), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }


        /// <summary>
        /// Wraps the call to <see cref="Type.BaseType"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static Type NfBaseType(this Type t, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;

            try
            {
                return t.BaseType;
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="PropertyInfo.GetGetMethod()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetGetMethod(this PropertyInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;

            try
            {
                return pi.GetGetMethod();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }

        /// <summary>
        /// Wraps the call to <see cref="PropertyInfo.GetSetMethod()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="rethrow">
        /// optional ability to suppress the exception form being rethrown - default is true.
        /// </param>
        /// <param name="logFile">optional override of the default log file at <see cref="ResolveAsmLog"/> </param>
        /// <returns></returns>
        public static MethodInfo NfGetSetMethod(this PropertyInfo pi, bool rethrow = true, string logFile = "")
        {
            if (pi == null)
                return null;

            try
            {
                return pi.GetSetMethod();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), fle, logFile);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(null, pi), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;
        }


        /// <summary>
        /// Wraps the call to <see cref="Type.GetFields()"/> adding logging 
        /// of typical loader exceptions.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rethrow"></param>
        /// <param name="logFile"></param>
        /// <returns></returns>
        public static FieldInfo[] NfGetFields(this Type t, bool rethrow = true, string logFile = "")
        {
            if (t == null)
                return null;

            try
            {
                return t.GetFields();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), rtle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileNotFoundException fnfe)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fnfe, logFile);
                if (rethrow)
                    throw;
            }
            catch (TypeLoadException tle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), tle, logFile);
                if (rethrow)
                    throw;
            }
            catch (FileLoadException fle)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), fle);
                if (rethrow)
                    throw;
            }
            catch (BadImageFormatException bife)
            {
                AddLoaderExceptionToLog(new Tuple<Type, MemberInfo>(t, null), bife, logFile);
                if (rethrow)
                    throw;
            }

            return null;

        }
        #endregion

        #region internal api

        /// <summary>
        /// Search from <see cref="directoryLocation"/> recursively for all files 
        /// ending in *.dll extension and checks that <see cref="rqstAsmFullName"/>
        /// equals the full assembly name of the given dll file.
        /// </summary>
        /// <param name="directoryLocation"></param>
        /// <param name="rqstAsmFullName"></param>
        /// <param name="foundOne"></param>
        /// <param name="reflectionOnly"></param>
        /// <returns></returns>
        internal static bool SearchDirectoriesForAssembly(string directoryLocation, string rqstAsmFullName,
            out Assembly foundOne, bool reflectionOnly = true)
        {
            foundOne = null;
            if (String.IsNullOrWhiteSpace(directoryLocation) || !Directory.Exists(directoryLocation))
                return false;
            //this helps - a little
            var logDir = ResolveAsmLog;

            File.AppendAllText(logDir,
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffff}] Searching for '{rqstAsmFullName}' " +
                $"in '{directoryLocation}' \n");

            var di = new DirectoryInfo(directoryLocation);

            //search all the files down from the requesting assembly's location
            foreach (var d in di.EnumerateFileSystemInfos("*.dll", SearchOption.AllDirectories))
            {
                var asmName = AssemblyName.GetAssemblyName(d.FullName);
                if (asmName == null)
                    continue;

                if (!AssemblyName.ReferenceMatchesDefinition(asmName, new AssemblyName(rqstAsmFullName)))
                    continue;
                foundOne = reflectionOnly
                    ? Assembly.ReflectionOnlyLoad(File.ReadAllBytes(d.FullName))
                    : Assembly.Load(File.ReadAllBytes(d.FullName));
                return true;
            }

            return false;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        private static StringBuilder GetExceptionStrBldr(Tuple<Type, MemberInfo> attemptedResolveType, Exception ex)
        {
            var missingTypeMsg = new StringBuilder();
            if (ex == null)
                return missingTypeMsg;

            missingTypeMsg.AppendFormat("Exception Name: '{0}'", ex.GetType().Name);
            missingTypeMsg.AppendLine();
            missingTypeMsg.Append(ex.Message);
            missingTypeMsg.AppendLine();
            if (!String.IsNullOrWhiteSpace(ex.StackTrace))
            {
                missingTypeMsg.Append(ex.StackTrace);
                missingTypeMsg.AppendLine();
            }

            if (attemptedResolveType == null)
                return missingTypeMsg;

            missingTypeMsg.Append("The following type could not be resolved.");
            missingTypeMsg.AppendLine();
            if (attemptedResolveType.Item1 != null)
            {
                missingTypeMsg.AppendFormat("Type's full name: '{0}'", attemptedResolveType.Item1.FullName);
                missingTypeMsg.AppendLine();
            }
            if (attemptedResolveType.Item2 != null)
            {
                missingTypeMsg.AppendFormat("Type's particular member's name: '{0}'", attemptedResolveType.Item2.Name);
                missingTypeMsg.AppendLine();
            }

            return missingTypeMsg;
        }
 

        #endregion
    }
}
