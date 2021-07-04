﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Qserver.Util;
using System.Collections.Generic;

namespace Qserver.GameServer.Network
{
    public class QpangServer
    {
        public bool ListenServerSocket = true;
        private TcpListener _listener;
        private int _port;
        private Dictionary<uint, ConnServer> _connections;

        public QpangServer(int port)
        {
            this._port = port;
            //Start();
        }
        public bool Start()
        {
            try
            {
#if DEBUG
                _listener = new TcpListener(IPAddress.Parse(Settings.SERVER_IP), this._port);
#else
                _authListener = new TcpListener(Util.Util.GetLocalIPAddress(), Settings.SERVER_PORT_AUTH);
#endif
                _listener.Start();

                return true;
            }
            catch (Exception e)
            {
                Log.Message(LogType.ERROR, "{0}", e.Message);
                return false;
            }
        }

        public void StartConnectionThreads()
        {
            new Thread(AcceptConnection).Start();
        }

        protected void AcceptConnection()
        {
            try
            {
                while (ListenServerSocket)
                {
                    Thread.Sleep(1);
                    if (_listener.Pending())
                    {
                        AuthServer Server = new AuthServer();
                        Server.Socket = _listener.AcceptSocket();

                        Thread NewThread = new Thread(Server.RecieveAuth);
                        NewThread.Start();
                    }
                }
            }
            catch (Exception e)
            {
                // We do not want our server to crash due to a invalid packet beeing sent
                Console.WriteLine(e.ToString());
            }
            
        }
        protected void Dispose()
        {
            ListenServerSocket = false;
            _listener.Stop();
        }
    }
}
