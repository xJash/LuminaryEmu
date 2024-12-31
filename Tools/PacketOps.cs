namespace LuminaryEmu.LoginPackets {
    public enum SendLSOps {
        // Decimal OPs
        // Don't ask why.
        UserInfoOK = 1000,
        UserInfoAccept = 1001,
        ServerInfo = 1002
    }

    public enum RecvLSOps {
        // Hex OPs
        UserLoginRequest = 0x15, // 21
        UserAcceptRequest = 0x02,
        UserServerInfoRequest = 0x03

    }
}
namespace LuminaryEmu.GameServerPackets {
    public enum SendGSOps {


    }

    public enum RecvGSOps {


    }
}


