using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LuminaryEmu {
    internal class Program {
        private static void Main(string[] args) {
            Tools.Log("LuminaryEmu For Luminary Version 357!");
            new Program().StartServer();
        }

        public void StartServer() {
            new World().Start();
            _ = Console.ReadLine();
        }

    }

    public class World {
        public static string LocalIP;
        public static string WANIP;
        public static short GameServersNum;

        public static List<GameServer> GameServers;
        public static IniFile iniReader;
        public static List<LoginUser> LoginUsers;
        public static LoginServer LoginServer;

        public void Start() {
            if (LoadIniFileData()) {
                LoginUsers = new List<LoginUser>();
                GameServers = new List<GameServer>();
                LoginServer = new LoginServer();
                LoadGameServer();
            }
        }

        private void LoadGameServer() {
            string number = iniReader.IniReadValue("Config", "GameServerNumber");
            if (number != "") {
                GameServersNum = Convert.ToInt16(number);
                for (int i = 0; i < GameServersNum; i++) {
                    GameServer gs = new GameServer
                    (
                     iniReader.IniReadValue("World:" + i, "Port"),
                     iniReader.IniReadValue("World:" + i, "Name"),
                     iniReader.IniReadValue("World:" + i, "GoonzuName"),
                     iniReader.IniReadValue("World:" + i, "InfoText"),
                     iniReader.IniReadValue("World:" + i, "Prices"),
                     iniReader.IniReadValue("World:" + i, "Population"),
                     iniReader.IniReadValue("World:" + i, "Bonus"),
                     iniReader.IniReadValue("World:" + i, "Icon")
                    );

                    if (gs.Allow == false) {
                        Tools.Log("Config file is wrong, GameServer:" + i + " not going to be initialzed.");
                    } else {
                        GameServers.Add(gs);
                    }
                }
            } else {
                Tools.Log("Config file is wrong, GameServers not going to be initialzed.");
            }
        }

        private bool LoadIniFileData() {
            string appPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            FileInfo ConfigFile = new FileInfo(appPath + @"\Config.ini");
            if (ConfigFile.Exists) {
                iniReader = new IniFile(ConfigFile.FullName);
                LocalIP = iniReader.IniReadValue("Config", "LocalIP");
                WANIP = iniReader.IniReadValue("Config", "WANIP");
                if ((LocalIP == "") || (WANIP == "")) {
                    Tools.Log("Config file is wrong, shuting down\r\nPress any key to exit the program.");
                    return false;
                }
                return true;
            }
            Tools.Log("Config file isnt exists, shuting down\r\nPress any key to exit the program.");
            return false;
        }
    }
}
