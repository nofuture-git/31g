using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Util.Core;
using NoFuture.Util.NfType.InvokeCmds;

namespace NoFuture.Util.NfType
{
    /// <summary>
    /// A type and and a tool box of functions for operating and 
    /// proposing facts concerning assembly reflected types.
    /// These names are as they appear in IL and not one of 
    /// the particular .NET languages.
    /// </summary>
    public class NfTypeName
    {
        #region Fields
        private readonly string _ctorString;
        private readonly string _publicKeyToken;
        private readonly string _namespace;
        private readonly string _className;
        private readonly AssemblyName _asmName;
        private readonly List<NfTypeName> _genericArgs = new List<NfTypeName>();
        private static NfTypeNameProcess _myProcess;
        #endregion



        #region ReadOnly Properties

        /// <summary>
        /// This is a fully qualified type name having the namespace,class name, assembly name
        /// e.g. ('NoFuture.MyDatabase.Dbo.AccountExecutives, NoFuture.MyDatabase, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null')
        /// </summary>
        public virtual string AssemblyQualifiedName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(_asmName?.FullName))
                    return null;
                var myparts = new[] {FullName, _asmName?.FullName};
                return string.Join(", ", myparts.Where(n => !string.IsNullOrWhiteSpace(n)));
            }
        }

        /// <summary>
        /// This the namespace and class name
        /// e.g. ('NoFuture.MyDatabase.Dbo.AccountExecutives')
        /// </summary>
        public virtual string FullName
            => string.Join(".", new[] { Namespace, ClassName}.Where(x => !string.IsNullOrWhiteSpace(x)));

        /// <summary>
        /// This is just the namespace
        /// e.g. ('NoFuture.MyDatabase.Dbo')
        /// </summary>
        public virtual string Namespace => $"{_namespace}";

        /// <summary>
        /// This is just the class name, no namespace, no assembly
        /// e.g. ('AccountExecutives')
        /// </summary>
        public virtual string ClassName => $"{_className}";

        /// <summary>
        /// This is just the name part of an assembly's name, no Version, Culture nor PublicKeyToken
        /// e.g. ('NoFuture.MyDatabase') 
        /// </summary>
        public virtual string AssemblySimpleName => _asmName?.Name ?? FullName;

        /// <summary>
        /// This is the typical fully qualified assembly name having the Version, Culture PublicKeyToken and 
        /// sometimes ProcessorArchitecture.
        /// e.g. ('log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL')
        /// </summary>
        public virtual string AssemblyFullName => _asmName?.ToString();

        /// <summary>
        /// This is just the version part of an assembly name (both key and value)
        /// e.g. ('Version=0.0.0.0')
        /// </summary>
        public virtual string Version => _asmName?.Version == null ? "Version=0.0.0.0" :  $"Version={_asmName.Version}";

        /// <summary>
        /// This is just the culture part of an assembly name (both key and value)
        /// e.g. ('Culture=neutral')
        /// </summary>
        public virtual string Culture => string.IsNullOrWhiteSpace(_asmName?.CultureName) ? "Culture=neutral" : $"Culture={_asmName.CultureName}";

        /// <summary>
        /// This is just the PublicKeyToken part of an assembly's name (both key and value)
        /// e.g. ('PublicKeyToken=669e0ddf0bb1aa2a')
        /// </summary>
        public virtual string PublicKeyToken => string.IsNullOrWhiteSpace(_publicKeyToken) ? "PublicKeyToken=null" : $"PublicKeyToken={_publicKeyToken}";

        /// <summary>
        /// This is simply the <see cref="AssemblySimpleName"/> with a .dll 
        /// extension added to the end.
        /// </summary>
        public virtual string AssemblyFileName => NfReflect.DraftCscDllName(AssemblySimpleName);

        /// <summary>
        /// This is the original string passed to the ctor.
        /// </summary>
        public string RawString => $"{_ctorString}";

        /// <summary>
        /// Gets the version number as a strongly type .NET <see cref="Version"/>
        /// </summary>
        public Version VersionValue => _asmName?.Version;

        /// <summary>
        /// Gets just the value of the <see cref="PublicKeyToken"/>
        /// </summary>
        public string PublicKeyTokenValue => _publicKeyToken ?? "null";

        public IEnumerable<NfTypeName> GenericArgs => _genericArgs;

        #endregion

        public NfTypeName(string name)
        {
            if(string.IsNullOrWhiteSpace(name))
                return;

            _ctorString = name;

            if(_myProcess == null || !_myProcess.IsMyProcessRunning)
                _myProcess = new NfTypeNameProcess(null);

            var parseItem = _myProcess.GetNfTypeName(name);
            if (parseItem == null)
                return;
            _className = NfReflect.GetTypeNameWithoutNamespace(parseItem.FullName);
            _namespace = NfReflect.GetNamespaceWithoutTypeName(parseItem.FullName);
            _publicKeyToken = parseItem.PublicKeyTokenValue;
            if(!string.IsNullOrWhiteSpace(parseItem.AssemblyFullName))
                _asmName = new AssemblyName(parseItem.AssemblyFullName);

            if (parseItem.GenericItems == null || !parseItem.GenericItems.Any())
                return;

            foreach (var gi in parseItem.GenericItems)
            {
                _genericArgs.Add(new NfTypeName(gi));
            }
        }

        internal NfTypeName(NfTypeNameParseItem parseItem)
        {

            if (parseItem == null)
                return;
            _className = NfReflect.GetTypeNameWithoutNamespace(parseItem.FullName);
            _namespace = NfReflect.GetNamespaceWithoutTypeName(parseItem.FullName);
            _publicKeyToken = parseItem.PublicKeyTokenValue;
            if (!string.IsNullOrWhiteSpace(parseItem.AssemblyFullName))
                _asmName = new AssemblyName(parseItem.AssemblyFullName);

            if (parseItem.GenericItems == null || !parseItem.GenericItems.Any())
                return;

            foreach (var gi in parseItem.GenericItems)
            {
                _genericArgs.Add(new NfTypeName(gi));
            }
        }

        /// <summary>
        /// Factory method to get a <see cref="NfTypeName"/> from an assembly file on disk.
        /// </summary>
        /// <param name="filePath">Path to a .NET assembly file</param>
        /// <param name="nfTn"></param>
        /// <returns></returns>
        public static bool TryGetByAsmFile(string filePath, out NfTypeName nfTn)
        {
            nfTn = null;
            if (string.IsNullOrWhiteSpace(filePath))
                return false;
            if (!File.Exists(filePath))
                return false;
            try
            {
                var asmName = AssemblyName.GetAssemblyName(filePath);
                if (asmName == null)
                    return false;
                nfTn = new NfTypeName(asmName.FullName);
            }
            catch (BadImageFormatException)
            {
                return false;
            }
            catch (FileLoadException)
            {
                return false;
            }
            return true;
        }
    }
}

namespace NoFuture.Util.Core
{
}
