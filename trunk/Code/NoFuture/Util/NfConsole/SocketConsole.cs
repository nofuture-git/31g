﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NoFuture.Shared.Cfg;

namespace NoFuture.Util.NfConsole
{
    /// <summary>
    /// A Nf type to extend for console apps which listen on 
    /// one or more sockets.
    /// </summary>
    public abstract class SocketConsole : Program
    {
        public const int SOCKET_LISTEN_NUM = 5;

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
            var cval = SysCfg.GetAppCfgSetting(appKey);
            return ResolveInt(cval);
        }

        public virtual IPAddress IpAddress { get; set; } = IPAddress.Loopback;

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
                var endPt = new IPEndPoint(IpAddress, cmdPort);
                PrintToConsole($"Listening on port {cmdPort}");
                socket.Bind(endPt);
                socket.Listen(SOCKET_LISTEN_NUM);

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
                            var avail = client.Available;
                            data = avail < NfConfig.DefaultBlockSize
                                ? new byte[avail]
                                : new byte[NfConfig.DefaultBlockSize];
                            client.Receive(data, 0, data.Length, SocketFlags.None);
                            buffer.AddRange(data.Where(b => b != (byte)'\0'));
                            if(client.Available <= 0)
                                Thread.Sleep(NfConfig.ThreadSleepTime);//give it a moment
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
