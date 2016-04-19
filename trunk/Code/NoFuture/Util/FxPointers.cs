﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography;
using NoFuture.Util.Binary;

namespace NoFuture.Util
{
    public class FxPointers
    {
        #region fields
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static readonly object AppDomainSearchLock = new object();
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static bool _addedReflectionOnlyAssemblyHandler = false;
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static bool _addedAssemblyHandler = false;
        #endregion

        #region properties
        public static bool IsReflectionOnlyAssemblyResolve { get { return _addedReflectionOnlyAssemblyHandler; } }
        public static bool IsAssemblyResolve { get { return _addedAssemblyHandler; } }
        #endregion

        #region methods
        /// <summary>
        /// Adds an event handler to <see cref="AppDomain.ReflectionOnlyAssemblyResolve"/>
        /// when the global variable <see cref="NoFuture.Shared.Constants.UseReflectionOnlyLoad"/> is True.  
        /// Otherwise add and event handler to <see cref="AppDomain.AssemblyResolve"/>.
        /// </summary>
        /// <remarks>
        /// Regarding PowerShell the Register-ObjectEvent cmdlet will not work since 
        /// the <see cref="System.AppDomain.ReflectionOnlyAssemblyResolve"/>
        /// handler is not a 'void' return type.
        /// </remarks>
        public static void AddResolveAsmEventHandlerToDomain()
        {
            if (Shared.Constants.UseReflectionOnlyLoad)
            {
                if (_addedAssemblyHandler)
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= ResolveAssembly;
                    _addedAssemblyHandler = false;
                }
                if (!_addedReflectionOnlyAssemblyHandler)
                {
                    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveReflectionOnlyAssembly;
                    _addedReflectionOnlyAssemblyHandler = true;
                }
            }
            else
            {
                if (_addedReflectionOnlyAssemblyHandler)
                {
                    AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= ResolveReflectionOnlyAssembly;
                    _addedReflectionOnlyAssemblyHandler = false;
                }
                if (!_addedAssemblyHandler)
                {
                    AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
                    _addedAssemblyHandler = true;
                }
            }
        }

        /// <summary>
        /// Assign the <see cref="System.Net.ServicePointManager.ServerCertificateValidationCallback"/>
        /// to accept all certs.
        /// </summary>
        public static void AddAcceptAllCerts()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = AcceptAllCertificates;
        }

        /// <summary>
        /// Adds the NF implementation of SHA512 <see cref="Shared.RSAPKCS1SHA512SigDesc"/> to
        /// the <see cref="CryptoConfig"/>
        /// </summary>
        public static void AddSHA512ToCryptoConfig()
        {
            CryptoConfig.AddAlgorithm(typeof (Shared.RSAPKCS1SHA512SigDesc), Shared.RSAPKCS1SHA512SigDesc.XML_NS_SIG);
        }

        /// <summary>
        /// Event handler to be added to the <see cref="System.AppDomain.ReflectionOnlyAssemblyResolve"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <remarks>
        /// To enable the Fusion Log add a REG_DWORD entry to the registry key at 
        /// HKLM\SOFTWARE\Microsoft\Fusion named 'EnableLog' with a value of '1' - 
        /// the Fusion Log is typically found on the FileNotFoundException and LoaderException 
        /// as a property of the same name.
        /// </remarks>
        public static Assembly ResolveReflectionOnlyAssembly(object sender, ResolveEventArgs args)
        {
            var rqstAsmName = args.Name;

            //no obvious way to resolve the dependency so first check around the assembly making the request
            var asmInNeedOfDependency = args.RequestingAssembly;
            var locationOfAsmInNeed = asmInNeedOfDependency.Location;
            lock (AppDomainSearchLock)
            {
                return Asm.SearchAppDomainForAssembly(rqstAsmName, locationOfAsmInNeed, true);
            }
        }

        /// <summary>
        /// Event handler to be added to the <see cref="System.AppDomain.AssemblyResolve"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <remarks>
        /// To enable the Fusion Log add a REG_DWORD entry to the registry key at 
        /// HKLM\SOFTWARE\Microsoft\Fusion named 'EnableLog' with a value of '1' - 
        /// the Fusion Log is typically found on the FileNotFoundException and LoaderException 
        /// as a property of the same name.
        /// </remarks>
        public static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var rqstAsmName = args.Name;

            //no obvious way to resolve the dependency so first check around the assembly making the request
            var asmInNeedOfDependency = args.RequestingAssembly;
            var locationOfAsmInNeed = asmInNeedOfDependency.Location;
            lock (AppDomainSearchLock)
            {
                return Asm.SearchAppDomainForAssembly(rqstAsmName, locationOfAsmInNeed, false);
            }
        }

        public static bool AcceptAllCertificates(object o,
            System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public static bool RejectAllCertificates(object o,
            System.Security.Cryptography.X509Certificates.X509Certificate certification,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return false;
        }

        #endregion
    }
}
