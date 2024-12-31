using LuminaryEmu.LoginPackets;
using PacketEnc;
using System;
using System.Net.Sockets;

namespace LuminaryEmu {
    public class LoginProtocol {
        private readonly Socket socket;
        private byte Key01;
        private byte Key02;
        private byte Key03;
        public LoginUser LoginUser;
        public LoginServer LoginServer;

        public LoginProtocol(Socket socket, LoginServer loginsserver) {
            this.socket = socket;
            LoginServer = loginsserver;


            SendWelcomePacket();

            byte[] buffer = new byte[2];
            if (socket.Connected) {
                _ = socket.BeginReceive(buffer, 0, 2, SocketFlags.None, new AsyncCallback(OnDataReceived), buffer);
            } else {
                socket.Close();
            }
        }

        public void OnDataReceived(IAsyncResult asyn) {
            if (socket.Connected) {
                try { _ = socket.EndReceive(asyn); } catch {
                    socket.Close();
                    return;
                }

                byte[] buffer = (byte[])asyn.AsyncState;
                int l = BitConverter.ToUInt16(buffer, 0);

                if (l <= 0) {
                    socket.Close();
                    return;
                }

                buffer = new byte[l];
                _ = socket.Receive(buffer, l, SocketFlags.None);
                Packet p = new Packet(buffer, buffer.Length);
                Key01++;
                Key02++;
                p.Decrypt(Key01, Key02);

                try {
                    ParsePacket(p);
                    GC.Collect();
                } catch (Exception e) {
                    Tools.Log(e.TargetSite.ToString());
                    Tools.Log(e.Source.ToString());
                    Tools.Log(e.ToString());
                    socket.Close();
                    return;
                }

                buffer = new byte[2];
                _ = socket.BeginReceive(buffer, 0, 2, SocketFlags.None, new AsyncCallback(OnDataReceived), buffer);
            } else {
                socket.Close();
            }
        }

        private void ParsePacket(Packet p) {

            RecvLSOps header = (RecvLSOps)p.GetUInt16();
            switch (header) {
                case RecvLSOps.UserLoginRequest:
                    RecvUserLogin(p);
                    break;
                case RecvLSOps.UserAcceptRequest:
                    SendUserAccept();
                    break;
                case RecvLSOps.UserServerInfoRequest:
                    SendServerInfo();
                    break;
                default:
                    Tools.Log(header.ToString());
                    break;
            }
        }

        private void SendServerInfo() {
            Packet p = new Packet(1000 * World.GameServersNum);

            p.AddByte(0);
            p.AddHeader((Packet.SendLSOps)SendLSOps.ServerInfo);
            p.AddInt16(World.GameServersNum);

            for (short i = 1; i < World.GameServersNum + 1; i++) {
                p.AddInt16(i);
                p.AddString(World.GameServers[i - 1].Name, 22);
                p.AddInt32(99 + i);
                p.AddString(World.WANIP, 16);
                p.AddInt16(World.GameServers[i - 1].Port);
                p.AddString("1111-11-11", 10);

                p.AddByteTimes(0x00, 72);

                p.AddInt32(World.GameServers[i - 1].Icon); //icon
                p.AddInt32(i); //dunno =[ can be any number
                p.AddString(World.GameServers[i - 1].GoonzuName, 20);

                p.AddString(World.GameServers[i - 1].InfoText, 256);
                p.AddInt32(World.GameServers[i - 1].Population);
                p.AddInt16(24113);
                p.AddInt16(World.GameServers[i - 1].Prices);
                p.AddInt32(1750);
                p.AddInt32(World.GameServers[i - 1].Bonus);
            }

            p.AddLenght();
            socket.Send(p.Raw, p.pos, SocketFlags.None);





            UserGoingToGameServer();
        }

        private void SendUserAccept() {
            Packet p = new Packet(30);
            p.AddBytes(0xCC, 0xE9, 0x03, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC);
            p.AddLenght();
            socket.Send(p.Raw, p.pos, SocketFlags.None);
        }

        private void SendUserInfoOK() {
            Packet packet = new Packet(200);
            packet.AddByte(0xCC);
            packet.AddHeader((Packet.SendLSOps)SendLSOps.UserInfoOK);
            packet.AddInt16(0);
            packet.AddInt32(0);
            packet.AddString(LoginUser.UserName, 11);
            packet.AddInt32(0);
            packet.AddInt32(0);
            packet.AddByteTimes(0x00, 81);
            packet.AddLenght();
            socket.Send(packet.Raw, packet.pos, SocketFlags.None);
        }

        private void SendWelcomePacket() {
            Key01 = 0x01;
            Key02 = 0x02;
            Key03 = 0xCC;

            Packet p = new Packet(10);
            p.AddBytes(0xCC, 0x30, 0x75, 0xCC, 0xCC);
            p.AddByte(Key01);
            p.AddByte(Key03);
            p.AddByte(Key02);
            p.AddLenght();
            socket.Send(p.Raw);
        }

        private void RecvUserLogin(Packet p) {
            LoginUser = new LoginUser {
                UserName = p.GetString(20).Split('\0')[0],
                Password = p.GetString(20).Split('\0')[0],
                IP = socket.RemoteEndPoint.ToString().Split(':')[0]
            };
            Tools.Log(LoginUser.ToString() + "Logging in.");

            SendUserInfoOK();
        }

        private void UserGoingToGameServer() {
            World.LoginUsers.Add(LoginUser);
        }


    }
}