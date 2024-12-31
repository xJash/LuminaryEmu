﻿using System.Net.Sockets;

namespace LuminaryEmu {
    public class GameProtocol {
        private readonly Socket socket;
        private readonly GameServer gameServer;
        private readonly LoginUser loginUser;

        public GameProtocol(Socket socket, GameServer gameserver, LoginUser loginuser) {
            this.socket = socket;
            gameServer = gameserver;
            loginUser = loginuser;
        }
    }
}