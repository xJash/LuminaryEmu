using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LuminaryEmu {
    public class GameServer {
        private Socket mainSocket;

        public List<GameProtocol> UserList;

        public string Name;
        public string GoonzuName;
        public string InfoText;
        public short Prices;
        public int Population;
        public int Bonus;
        public int Icon;

        public LoginUser FindIp(string ip) {
            for (int i = 0; i < World.LoginUsers.Count; i++) {
                if (ip == World.LoginUsers[i].IP) {
                    return World.LoginUsers[i];
                }
            }
            return null;
        }


        public short Port;
        public bool Allow = true;

        public GameServer(string port, string Name, string GoonzuName, string InfoText, string Prices, string Population, string Bonus, string Icon) {
            try {
                Port = Convert.ToInt16(port);
                this.Name = Name;
                this.GoonzuName = GoonzuName;
                this.Prices = Convert.ToInt16(Prices);
                this.Population = Convert.ToInt32(Population);
                this.Bonus = Convert.ToInt32(Bonus);
                this.InfoText = InfoText;
                this.Icon = Convert.ToInt32(Icon);
            } catch {
                Allow = false;
            }

            if (Allow) {
                Start();
            }
        }

        public void Start() {
            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mainSocket.Bind(new IPEndPoint(IPAddress.Parse(World.LocalIP), Port));
            mainSocket.Listen(8);
            _ = mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);

            Tools.Log("GameServer '" + Name + "' up and running.");

            UserList = new List<GameProtocol>();

            _ = ThreadPool.SetMinThreads(1000, 1000);
        }

        protected void OnClientConnect(IAsyncResult asyn) {
            Socket socket = mainSocket.EndAccept(asyn);
            LoginUser user = FindIp(socket.RemoteEndPoint.ToString().Split(':')[0]);
            if (user != null) {
                Tools.Log("A user is in the GameServer.");
                _ = World.LoginUsers.Remove(user);
                UserList.Add(new GameProtocol(socket, this, user));
            } else {
                socket.Close();
            }

            _ = mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), null);
        }

        internal void ShutDown() {
            mainSocket.Close();
            Tools.Log("GameServer " + Name + " shutting down.");
        }
    }
}
