using System;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;

namespace NoFuture.Domain
{
    /// <summary>
    /// MySimpleAppDomain is a basic implementation of an Application Domain to display ASP.NET pages outside of IIS
    /// </summary>
    public class Engine : MarshalByRefObject
    {
        #region Properties
        /// <summary>
        /// Get or set the virtual directory of the app domain.
        /// Defaults to '\'.
        /// </summary>
        public string VirtualDir { get; set; }

        /// <summary>
        /// Get or set the physical location, on disk, of the app domain.
        /// </summary>
        public string PhysicalDir { get; set; }

        public string[] AspPrefixes { get; set; }

        public const int BREAK_ON_EXCEPTION_COUNT = 3;
        public const int DEFAULT_HTTP_PORT = 1138;
        public const string APP_DOMAIN_SWITCH = "appdomain";
        public const string ASP_LISTENER_SWITCH = "asphttp";
        public const string V_DIR = "/";

        #endregion

        /// <summary>
        /// Sets up the app domain instance.  
        /// NOTE: this does not create an app domain, the calling assembly must 
        /// either run this exe as a process or call the .NET factory 
        /// ApplicationHost.CreateApplicationHost.  Host address is hard-coded to 127.0.0.1.
        /// </summary>
        /// <param name="aspPrefixes">See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspPrefixes.aspx .</param>
        /// <param name="virtualDir">Pass in virtual directory - if null is passed in then it defaults to '\'.</param>
        /// <param name="physicalDir">A valid physical directory.</param>
        public void AspFitting(string[] aspPrefixes, string virtualDir, string physicalDir)
        {
            if(aspPrefixes == null || aspPrefixes.Length <= 0)
                throw new ArgumentNullException("aspPrefixes");

            //check v-dir is not null or empty
            if (virtualDir == null || String.IsNullOrEmpty(virtualDir))
                VirtualDir = @"\";
            else
                VirtualDir = virtualDir;

            //save the v-dir and p-dir for the http worker processes
            PhysicalDir = physicalDir;

            var logPath = Path.Combine(PhysicalDir, "log");
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            Log.RootPath = logPath;
            AspPrefixes = aspPrefixes;
        }

        /// <summary>
        /// Creates and releases two task, one to handle http request, one to handle byte-stream tcp request.
        /// </summary>
        public void LaunchHost()
        {
            try
            {
                var currentId = WindowsIdentity.GetCurrent();
                if(currentId != null)
                {
                    Log.Write(String.Format("{0,-32}{1}", "Current Windows Identity:", currentId.Name));
                    var currentPrincipal = new WindowsPrincipal(currentId);
                    var isAdmin = currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                    Log.Write(String.Format("{0,-32}{1}", "Is Administrator:", isAdmin));
                }
                    
                Log.Write(String.Format("{0,-32}{1}", "Base Directory:",AppDomain.CurrentDomain.BaseDirectory));
                Log.Write(String.Format("{0,-32}{1}", "Dynamic Directory:", AppDomain.CurrentDomain.DynamicDirectory));
                Log.Write(String.Format("{0,-32}{1}", "Is Fully Trusted:", AppDomain.CurrentDomain.IsFullyTrusted));
                Log.Write(String.Format("{0,-32}{1}", "Relative Search Path:", AppDomain.CurrentDomain.RelativeSearchPath));
                Log.Write(String.Format("{0,-32}{1}", "Configuration File:",
                    AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
                AspCam(AspPrefixes, VirtualDir, PhysicalDir);
            }
            catch (Exception ex)
            {
                Log.Write("Exception in LaunchHost()");
                Log.ExceptionLogging(ex);
            }
        }

        #region internal helpers
        internal static void SetPrefixes(string[] prefixes, HttpListener listener)
        {
            foreach (var prefix in prefixes)
                if (Regex.IsMatch(prefix, "^(https|http)\x3A\x2F\x2F.*"))
                    listener.Prefixes.Add(prefix);
        }

        internal static void AspCam(string[] aspPrefixes, string vDir, string pDir)
        {
            var exCount = 0;
            for (; ; )//ever
            {
                var httpListener = new HttpListener();
                try
                {
                    SetPrefixes(aspPrefixes, httpListener);
                    httpListener.Start();
                    
                    for (;;) //and ever
                    {
                        //get the context from the listener - async will rest here while waiting
                        var myContext = httpListener.GetContext();

                        Log.Write("=======================begin AspCam()");

                        //create our custom worker process
                        var myWorkerRequest = new AspTappet(myContext, vDir, pDir);

                        //using the HttpRuntime's ProcessRequest move the request into the pipe
                        HttpRuntime.ProcessRequest(myWorkerRequest);

                        Log.Write("=========================end AspCam()");
                        
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Exception at AspCam()");
                    Log.ExceptionLogging(ex);
                    CloseHttpListener(httpListener);
                    if (exCount > BREAK_ON_EXCEPTION_COUNT)
                    {
                        Log.Write(String.Format("Have exceeded the maximum number of errors - exiting"));
                        return;
                    }
                    exCount += 1;
                }
            }
        }

        internal static void CloseHttpListener(HttpListener l)
        {
            if (l == null)
                return;
            try
            {
                l.Close();
            }
            catch(Exception ex)
            {
                Log.Write("Exception at CloseHttpListener");
                Log.ExceptionLogging(ex);
            }
        }
        #endregion
    }//end Engine
}//end NoFuture.Domain



