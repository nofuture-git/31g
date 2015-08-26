using System;
using System.Collections;
using System.Text;
using NoFuture.Exceptions;

namespace NoFuture.Domain
{
    public class Carriage
    {
        public struct DomainParameters
        {
            public string AppDomain { get; set; }
            public string AspListenerPrefix { get; set; }
        }

        public static string ConfigFileAppDomain
        {
            get { return System.Configuration.ConfigurationManager.AppSettings[Engine.APP_DOMAIN_SWITCH]; }
        }

        public static string ConfigFileAspHttp
        {
            get { return System.Configuration.ConfigurationManager.AppSettings[Engine.ASP_LISTENER_SWITCH]; }
        }

        public static void Main(string[] args)
        {
            Console.Title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            try
            {
                Util.ConsoleCmd.SetConsoleAsTransparent();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WindowWidth = 160;
                Console.CursorVisible = false;

                Console.Out.WriteLine("{0:yyyyMMdd HH:mm:ss.fffff} - Starting application domain...", DateTime.Now);
                //check for 'help' switch or no-arg
                if (args.Length > 0 && (args[0] == "-h" || args[0] == "-help" || args[0] == "/?"))
                {
                    Console.Out.WriteLine(Help());
                    return;
                }

                var argHash = Util.ConsoleCmd.ArgHash(args);

                var cmdL = AssignArgsToDomainParameters(argHash);

                if(string.IsNullOrWhiteSpace(cmdL.AppDomain) || !System.IO.Directory.Exists(cmdL.AppDomain))
                    throw new ItsDeadJim(string.Format("The '{0}' value is missing or invalid.", Engine.APP_DOMAIN_SWITCH));

                if(string.IsNullOrWhiteSpace(cmdL.AspListenerPrefix))
                    throw new ItsDeadJim(string.Format("The '{0}' value is missing or invalid", Engine.ASP_LISTENER_SWITCH));

                PrintParameters(cmdL);

                //AppDomain
                var myApp = (Engine)
                    System.Web.Hosting.ApplicationHost.CreateApplicationHost(typeof(Engine), Engine.V_DIR, cmdL.AppDomain);
                Console.Out.WriteLine("{0:yyyyMMdd HH:mm:ss.fffff} - Application domain created at '{1}'",
                                             DateTime.Now,
                                             cmdL.AppDomain);

                //asp fitting 
                myApp.AspFitting(new[] { cmdL.AspListenerPrefix }, Engine.V_DIR, cmdL.AppDomain);

                //start the engine
                myApp.LaunchHost();

            }
            catch (Exception ex) //exceptions move up the entire stack to the main, get caught and the exe exits.
            {
                Console.Out.WriteLine("Exception in Main statement");
                Console.Out.WriteLine(ex.Message);
                Console.Out.WriteLine(ex.StackTrace);

                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
            }
            Console.Out.WriteLine("press any key to exit...");
            Console.ReadKey();
        }

        #region command args parse

        public static DomainParameters AssignArgsToDomainParameters(Hashtable argHash)
        {
            var cmdLineProperties = new DomainParameters
            {
                AppDomain = argHash.ContainsKey(Engine.APP_DOMAIN_SWITCH) ? string.Format("{0}", argHash[Engine.APP_DOMAIN_SWITCH]) : ConfigFileAppDomain,
                AspListenerPrefix = argHash.ContainsKey(Engine.ASP_LISTENER_SWITCH) ? string.Format("{0}", argHash[Engine.ASP_LISTENER_SWITCH]) : ConfigFileAspHttp
            };

            return cmdLineProperties;
        }
        #endregion

        #region Prints
        public static void PrintParameters(DomainParameters cmdL)
        {
            var i = 0;
            Console.Out.WriteLine("Arguements :: ");
            Console.Out.WriteLine("{0:00} {1,-15} = {2,-40}", ++i, "App Domain", cmdL.AppDomain);
            Console.Out.WriteLine("{0:00} {1,-15} = {2,-40}", ++i, "Http Prefix", cmdL.AspListenerPrefix);
            Console.Out.WriteLine("");

        }

        public static String Help()
        {
            var help = new StringBuilder();
            help.AppendLine("Usage: [options] ");
            help.AppendLine(" These values may be set from the applications");
            help.AppendLine(" configuration file or passed in as command line");
            help.AppendLine(" args.");
            help.AppendLine("");
            help.AppendLine("Options:");
            help.AppendLine(" -h | -help             Will print this help.");
            help.AppendLine("");
            help.AppendLine(string.Format(" -{0}=[location]  Must be a valid path,",Engine.APP_DOMAIN_SWITCH));
            help.AppendLine("                        must contain a \bin folder");
            help.AppendLine("                        with this exe in it.");
            help.AppendLine("");
            help.AppendLine(string.Format(" -{0}=[prefix]      Only one. ", Engine.ASP_LISTENER_SWITCH));
            help.AppendLine("                        Example 'http://+:8081/' ");
            help.AppendLine("                        for all request on port 8081.");
            help.AppendLine("");
 
            return help.ToString();
        }
        #endregion
    }
}