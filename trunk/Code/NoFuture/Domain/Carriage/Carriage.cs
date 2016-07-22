using System;
using System.Text;
using NoFuture.Exceptions;
using NoFuture.Util.NfConsole;

namespace NoFuture.Domain
{
    public class Carriage : Program
    {
        #region inner types
        public struct DomainParameters
        {
            public string AppDomain { get; set; }
            public string AspListenerPrefix { get; set; }
        }
        #endregion

        #region ctors
        public Carriage(string[] args) : base(args, true) { }
        #endregion

        #region properties
        public string ConfigFileAppDomain
            => System.Configuration.ConfigurationManager.AppSettings[Engine.APP_DOMAIN_SWITCH];

        public string ConfigFileAspHttp
            => System.Configuration.ConfigurationManager.AppSettings[Engine.ASP_LISTENER_SWITCH];

        public DomainParameters MyParameters { get; private set; }

        #endregion

        public static void Main(string[] args)
        {

            var p = new Carriage(args);
            try
            {
                p.StartConsole();

                if (p.PrintHelp()) return;

                p.ParseProgramArgs();

                p.PrintParameters(p.MyParameters);

                //AppDomain
                var myApp = (Engine)
                    System.Web.Hosting.ApplicationHost.CreateApplicationHost(typeof(Engine), Engine.V_DIR,
                        p.MyParameters.AppDomain);
                p.PrintToConsole(
                    $"Application domain created at '{p.MyParameters.AppDomain}'");

                //asp fitting 
                myApp.AspFitting(new[] {p.MyParameters.AspListenerPrefix}, Engine.V_DIR, p.MyParameters.AppDomain);

                //start the engine
                myApp.LaunchHost();

            }
            catch (Exception ex) //exceptions move up the entire stack to the main, get caught and the exe exits.
            {
                p.PrintToConsole(ex);
            }
            Console.Out.WriteLine("press any key to exit...");
            Console.ReadKey();
        }

        #region methods
        public void PrintParameters(DomainParameters cmdL)
        {
            var i = 0;
            PrintToConsole("Arguements :: ");
            PrintToConsole($"{++i:00} {"App Domain",-15} = {cmdL.AppDomain,-40}");
            PrintToConsole($"{++i:00} {"Http Prefix",-15} = {cmdL.AspListenerPrefix,-40}");
            PrintToConsole("");

        }

        protected override string MyName => "NoFuture.Domain.Carriage";

        protected override string GetHelpText()
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
            help.AppendLine($" -{Engine.APP_DOMAIN_SWITCH}=[location]  Must be a valid path,");
            help.AppendLine("                        must contain a \bin folder");
            help.AppendLine("                        with this exe in it.");
            help.AppendLine("");
            help.AppendLine($" -{Engine.ASP_LISTENER_SWITCH}=[prefix]      Only one. ");
            help.AppendLine("                        Example 'http://+:8081/' ");
            help.AppendLine("                        for all request on port 8081.");
            help.AppendLine("");
 
            return help.ToString();
        }

        protected override void ParseProgramArgs()
        {
            var argHash = ConsoleCmd.ArgHash(_args);
            MyParameters = new DomainParameters
            {
                AppDomain = argHash.ContainsKey(Engine.APP_DOMAIN_SWITCH) ? $"{argHash[Engine.APP_DOMAIN_SWITCH]}"
                    : ConfigFileAppDomain,
                AspListenerPrefix = argHash.ContainsKey(Engine.ASP_LISTENER_SWITCH) ?
                    $"{argHash[Engine.ASP_LISTENER_SWITCH]}"
                    : ConfigFileAspHttp
            };
            if (string.IsNullOrWhiteSpace(MyParameters.AppDomain) || !System.IO.Directory.Exists(MyParameters.AppDomain))
                throw new ItsDeadJim($"The '{Engine.APP_DOMAIN_SWITCH}' value is missing or invalid.");

            if (string.IsNullOrWhiteSpace(MyParameters.AspListenerPrefix))
                throw new ItsDeadJim($"The '{Engine.ASP_LISTENER_SWITCH}' value is missing or invalid");
        }

        #endregion
    }
}