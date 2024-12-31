using System.Net.Sockets;

namespace LuminaryEmu {
    public class GameProtocol {
        private readonly Socket socket;
        public GameProtocol(Socket socket) {
            this.socket = socket ?? throw new System.ArgumentNullException(nameof(socket));
        }

        private readonly GameServer gameServer;

        public GameProtocol(GameServer gameServer) {
            this.gameServer = gameServer;
        }

        private readonly LoginUser loginUser;

        public GameProtocol(LoginUser loginUser) {
            this.loginUser = loginUser;
        }

        public GameProtocol(Socket socket, GameServer gameserver, LoginUser loginuser) {
            this.socket = socket;
            gameServer = gameserver;
            loginUser = loginuser;
        }
    }
}
