﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using NoFuture.Shared;

namespace NoFuture.Util.NfConsole
{
    public abstract class SocketConsole : Program
    {
        public const int LISTEN_NUM_REQUEST = 5;

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
                socket.Bind(endPt);
                socket.Listen(LISTEN_NUM_REQUEST);

                for (; ; )//ever
                {
                    try
                    {
                        var buffer = new List<byte>();

                        var client = socket.Accept();
                        PrintToConsole($"Connect from port {cmdPort}");
                        var data = new byte[Constants.DEFAULT_BLOCK_SIZE];

                        //park for first data received
                        client.Receive(data, 0, data.Length, SocketFlags.None);
                        buffer.AddRange(data.Where(b => b != (byte)'\0'));
                        while (client.Available > 0)
                        {
                            if (client.Available < Constants.DEFAULT_BLOCK_SIZE)
                            {
                                data = new byte[client.Available];
                                client.Receive(data, 0, client.Available, SocketFlags.None);
                            }
                            else
                            {
                                data = new byte[Constants.DEFAULT_BLOCK_SIZE];
                                client.Receive(data, 0, (int)Constants.DEFAULT_BLOCK_SIZE, SocketFlags.None);
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
