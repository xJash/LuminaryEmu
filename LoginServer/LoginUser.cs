namespace LuminaryEmu {
    public class LoginUser {
        public string UserName;
        public string Password;
        public int ID;
        public string IP;

        public override string ToString() {
            return ID != 0
                ? "ID:" + ID + " Info{" + UserName + ":" + Password + "}[" + IP + "] "
                : "Info{" + UserName + ":" + Password + "}[" + IP + "] ";
        }
    }
}
