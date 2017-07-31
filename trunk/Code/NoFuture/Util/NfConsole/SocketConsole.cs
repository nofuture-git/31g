using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NoFuture.Shared;

namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// A Nf type to extend for console apps which listen on 
    /// one or more sockets.
    /// </summary>
    public abstract class SocketConsole : Program
    {
        protected SocketConsole(string[] args, bool isVisable)
            : base(args, isVisable)
        {
        }

        /// <summary>
        /// Resolve a port number from the config file's appSettings
        /// on <see cref="appKey"/>
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        protected internal int? ResolvePort(string appKey)
        {
            var cval = ConfigurationManager.AppSettings[appKey];
            return ResolveInt(cval);
        }

        /// <summary>
        /// The residence of the listening-socket thread, one for each of the <see cref="ICmd"/>
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdPort"></param>
        protected internal void HostCmd(ICmd cmd, int cmdPort)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                //this should NOT be reachable from any other machine
                var endPt = new IPEndPoint(IPAddress.Loopback, cmdPort);
                PrintToConsole($"Listening on port {cmdPort}");
                socket.Bind(endPt);
                socket.Listen(Constants.SOCKET_LISTEN_NUM);

                for (; ; )//ever
                {
                    try
                    {
                        var buffer = new List<byte>();

                        var client = socket.Accept();
                        var data = new byte[NfConfig.DefaultBlockSize];

                        //park for first data received
                        client.Receive(data, 0, data.Length, SocketFlags.None);
                        buffer.AddRange(data.Where(b => b != (byte)'\0'));
                        while (client.Available > 0)
                        {
                            if (client.Available < NfConfig.DefaultBlockSize)
                            {
                                data = new byte[client.Available];
                                client.Receive(data, 0, client.Available, SocketFlags.None);
                            }
                            else
                            {
                                data = new byte[NfConfig.DefaultBlockSize];
                                client.Receive(data, 0, (int)NfConfig.DefaultBlockSize, SocketFlags.None);
                            }
                            buffer.AddRange(data.Where(b => b != (byte)'\0'));
                        }

                        var output = cmd.Execute(buffer.ToArray());
                        client.Send(output);
                        client.Close();

                    }
                    catch (Exception ex)
                    {
                        PrintToConsole(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Opens all the sockets for all <see cref="ICmd"/>
        /// </summary>
        protected abstract void LaunchListeners();
    }
}
