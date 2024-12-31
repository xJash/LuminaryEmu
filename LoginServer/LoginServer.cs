using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LuminaryEmu {
    public class LoginServer {
        private readonly Socket mainSocket;
        public List<LoginProtocol> UserList;
        public LoginServer() {
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mainSocket.Bind(new IPEndPoint(IPAddress.Parse(World.LocalIP), 5700));
            mainSocket.Listen(8);
            _ = mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
            Tools.Log("LoginServer is up and running.");
            UserList = new List<LoginProtocol>();
            _ = ThreadPool.SetMinThreads(1000, 1000);
        }

        protected void OnClientConnect(IAsyncResult asyn) {
            UserList.Add(new LoginProtocol(mainSocket.EndAccept(asyn), this));
            Tools.Log("A user has entered the server.");
            _ = mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        public void ShutDown() {
            mainSocket.Close();
            Tools.Log("LoginServer Shut Down.");
        }
    }
}
