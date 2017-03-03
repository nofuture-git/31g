using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NoFuture.Shared;

namespace NoFuture.Host
{
    /// <summary>
    /// Intended for an existing console app for which there is 
    /// no source code.
    /// Allows the cli to run as a process with sockets used 
    /// for stdin and stdout.
    /// <example>
    /// Simple example
    /// <code>
    /// <![CDATA[
    /// public static void Main(string[] args)
    /// {
    ///        var testing = new NoFuture.Host.Proc();
    ///
    ///        try
    ///     {
    ///         testing.ExePath = @"C:\Projects\WindowsPowerShell\bin\ConsoleRepeatInOut.exe";
    ///         testing.Port = 780;
    ///         testing.Address = System.Net.IPAddress.Loopback;
    ///         testing.InitProc(new System.Diagnostics.DataReceivedEventHandler(StandardOutReceived));
    ///         System.Net.Sockets.Socket loopSocket;
    ///         System.Net.IPEndPoint myEndPoint;
    ///
    ///         testing.BeginProc();
    ///         for (var i = 0; i<3; i++ )
    ///         {
    ///             loopSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.IP);
    ///             myEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 780);
    ///             loopSocket.Connect(myEndPoint);
    ///             loopSocket.Send(System.Text.Encoding.UTF8.GetBytes("my string"));
    ///             loopSocket.Close();
    ///             System.Threading.Thread.Sleep(2000);
    ///         }
    ///
    ///         loopSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.IP);
    ///         myEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Loopback, 780);
    ///            loopSocket.Connect(myEndPoint);
    ///         //System.Threading.Thread.Sleep(2000);
    ///         loopSocket.Send(System.Text.Encoding.UTF8.GetBytes(NoFuture.Host.Proc.STOP_COMMAND));
    ///         loopSocket.Close();
    ///
    ///     }
    ///     catch (Exception ex)
    ///     {
    ///         Console.WriteLine(ex.Message);
    ///         Console.WriteLine(ex.StackTrace);
    ///         
    ///        }
    ///     finally
    ///     {
    ///         if (testing.Exe != null && testing.Exe.HasExited == false)
    ///         {
    ///             testing.Exe.Kill();
    ///         }
    ///     }
    ///     Console.ReadKey();
    /// 
    /// }
    /// private static void StandardOutReceived(object sendingProcess, System.Diagnostics.DataReceivedEventArgs outtext)
    /// {
    ///     Console.WriteLine(outtext.Data);
    /// }
    /// ]]>
    /// </code>
    /// </example>
    /// </summary>
    public class Proc
    {
        #region private fields

        private Process _proc;

        private Socket _commLink;

        private static readonly object LOCK = new object();

        private static readonly List<byte> STD_OUT = new List<byte>();

        #endregion

        #region public properties

        public string ExePath { get; set; }

        public int Port { get; set; }

        public IPAddress Address { get; set; }

        public string Log { get; set; }

        public Process Exe
        {
            get { return _proc; }
            set { _proc = value; }
        }

        public int WaitCount { get; set; }

        public bool SendStdOutToSocket { get; set; }

        #endregion

        #region constants

        public static int DefaultPort = NfConfig.NfDefaultPorts.HostProc;
        public const int BUFFSIZE = 512;
        public const int DEFAULT_THREAD_WAIT_MS = 50;
        public const int DEFAULT_WAIT_COUNT = 40; //roughly 2 seconds @ 50ms each
        public const string KEY_CONFIG_IP_ADDR = "NoFuture.Host.Proc.IpAddress";
        public const string KEY_CONFIG_PORT = "NoFuture.Host.Proc.Port";
        public const string STOP_COMMAND = "exit";

        #endregion

        public Proc()
        {
            Log = string.Format("{0:yyyyMMdd}_nofuture.txt", DateTime.Now);
        }

        /// <summary>
        /// Instantiates the target process as a non-shell launched, hidden window, reditected
        /// stdin and stdout.  The calling assembly will still need to add an event listener to the 
        /// <see cref="System.Diagnostics.DataReceivedEventHandler"/> via the <see cref="Exe"/> property.
        /// </summary>
        public void InitProc()
        {
            try
            {
                WriteLogEntry("now begin init proc" );
                if (string.IsNullOrEmpty(ExePath))
                {
                    throw new ArgumentException("set the ExePath prior to calling start.");
                }
                _proc = new Process
                                 {
                                     StartInfo =
                                         {
                                             FileName = ExePath,
                                             UseShellExecute = false,
                                             RedirectStandardOutput = true,
                                             RedirectStandardInput = true,
                                             RedirectStandardError = true,
                                             CreateNoWindow = true,
                                             StandardOutputEncoding = Encoding.UTF8,
                                             StandardErrorEncoding = Encoding.UTF8
                                         }
                                 };
                _proc.OutputDataReceived += new DataReceivedEventHandler(StandardOutReceived);
                _proc.ErrorDataReceived += new DataReceivedEventHandler(StandardErrorReceived);
                WriteLogEntry("now end init proc");
            }
            catch (Exception ex)
            {
                WriteLogEntry(ex);
            }
        }

        /// <summary>
        /// Calls the likewise latter overloaded function and additionally passes in the 
        /// event handler for the targets process's Standard Output handler.
        /// </summary>
        /// <param name="standardOutHandler"></param>
        public void InitProc(DataReceivedEventHandler standardOutHandler)
        {
            InitProc();
            try
            {
                _proc.OutputDataReceived += new DataReceivedEventHandler(standardOutHandler);
            }
            catch (Exception ex)
            {
                WriteLogEntry(ex);
            }
        }

        /// <summary>
        /// Launches the target process async on a thread under the control of this 
        /// app domain.
        /// </summary>
        public void BeginProc()
        {
            try
            {
                var launcher = new System.Threading.Tasks.TaskFactory();
                var hostProc = launcher.StartNew(() => StartProc());
                
            }
            catch (Exception ex)
            {
                WriteLogEntry(ex);
            }
        }

        /// <summary>
        /// Starts the given process.  This function should be called async 
        /// since it will cause  the thread to park until the <see cref="STOP_COMMAND"/> 
        /// is issued.
        /// </summary>
        public void StartProc()//this should be called as a task by the calling assembly
        {
            try
            {
                WriteLogEntry("now starting process");
                if (_proc == null)
                {
                    throw new Exception("the Process object is null, call InitProc first.");
                }

                //start the process
                _proc.Start();

                //start sending output to registered listeners
                _proc.BeginOutputReadLine();
                _proc.BeginErrorReadLine();

                //start the comm link to the process
                if (Port == 0)
                {
                    Port = DefaultPort;
                }
                if (WaitCount == 0)
                {
                    WaitCount = DEFAULT_WAIT_COUNT;
                }
                if (Address == null)
                {
                    Address = IPAddress.Loopback;
                }
                //process sits in a endless listen-loop on given socket until STOP command is sent
                StartCommLink(Port,Address);
                WriteLogEntry("now ending process");
                if(!_proc.HasExited)
                {
                    _proc.Kill();    
                }
            }
            catch (Exception ex)
            {
                WriteLogEntry(ex);
            }
        }

        /// <summary>
        /// Starts the listening on the given socket defined on the loopback address and 
        /// specified <see cref="port"/>  and <see cref="address"/> and contiunes to listen 
        /// until the <see cref="STOP_COMMAND"/> is issued on the Standard Input.
        /// </summary>
        /// <param name="port">Any open port number.</param>
        /// <param name="address">Any ip address.</param>
        public void StartCommLink(int port, IPAddress address)
        {
            WriteLogEntry("communication link started");
            _commLink = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            var myEndPoint = new IPEndPoint(address, port);

            //start the tcp listener
            _commLink.Bind(myEndPoint);
            _commLink.Listen(5);
            for (; ; )//ever
            {
                try
                {
                    var received = new List<byte>();

                    //async thread parks here - waits
                    WriteLogEntry("begin waiting for client connection");
                    Socket client = _commLink.Accept();
                    WriteLogEntry("client connected");

                    //receive data in segments having first byte as operation-type
                    int bytesReceived = 0;
                    while (client.Available > 0)
                    {
                        byte[] bytes;
                        if (client.Available < BUFFSIZE)
                        {
                            bytes = new byte[client.Available];
                            bytesReceived += client.Receive(bytes, 0, client.Available, 0);
                        }
                        else
                        {
                            bytes = new byte[BUFFSIZE];
                            bytesReceived += client.Receive(bytes, 0, BUFFSIZE, 0);
                        }

                        received.AddRange(bytes);
                    }

                    var utf8msg = Encoding.UTF8.GetString(received.ToArray());

                    if(utf8msg == STOP_COMMAND)
                    {
                        WriteLogEntry("STOP_COMMAND received");
                        client.Close();
                        return;
                    }

                    WriteLogEntry(string.Format("text command '{0}'",utf8msg));

                    //clear the std out buffer
                    STD_OUT.Clear();

                    //send message to process
                    _proc.StandardInput.WriteLine(utf8msg);
                    var waitCounter = 0;

                    if (!SendStdOutToSocket)
                        return;

                    //expect to receive response async so wait
                    while (STD_OUT.Count == 0)
                    {
                        Thread.Sleep(DEFAULT_THREAD_WAIT_MS);

                        //allow for configurable breakout time
                        if (waitCounter == WaitCount)
                            break;
                        waitCounter += 1;
                    }
                    client.Send(STD_OUT.ToArray());
                }
                catch (Exception ex)
                {
                    WriteLogEntry(ex);
                }
            }//end for

        }//end StartCommLink

        #region private methods

        private void StandardOutReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            WriteLogEntry(string.Format("output received '{0}'",outLine.Data));
            var outData = Encoding.UTF8.GetBytes(outLine.Data);
            STD_OUT.AddRange(outData);
        }
        private void StandardErrorReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            WriteLogEntry(string.Format("error received '{0}'", outLine.Data));
        }

        private void WriteLogEntry(string msg)
        {
            lock (LOCK)
            {
                System.IO.File.AppendAllText(Log,
                                             string.Format("{0:yyyyMMdd-HHmmss.fffff} {1}\n", DateTime.Now, msg));
            }
            
        }
        private void WriteLogEntry(Exception ex)
        {
            lock (LOCK)
            {
                System.IO.File.AppendAllText(Log,
                                             string.Format("{0:yyyyMMdd-HHmmss.fffff} EXCEPTION {1}\n\t\t{2}",
                                                           DateTime.Now,
                                                           ex.Message,
                                                           ex.StackTrace));
                if(ex.InnerException != null)
                {
                    System.IO.File.AppendAllText(Log,
                                                 string.Format(
                                                               "{0:yyyyMMdd-HHmmss.fffff} INNER EXCEPTION {1}\n\t\t{2}",
                                                               DateTime.Now,
                                                               ex.InnerException.Message,
                                                               ex.InnerException.StackTrace));
                }

            }

        }

        #endregion

    }//end Proc

}//end NoFuture

