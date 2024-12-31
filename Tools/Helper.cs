using System;

namespace LuminaryEmu {
    public class Tools {
        public static void Log(string msg) {
            string date = "[";
            int hour = System.DateTime.Now.Hour;
            int minute = System.DateTime.Now.Minute;
            int second = System.DateTime.Now.Second;

            if (hour > 9) {
                date += hour.ToString() + ":";
            } else {
                date += "0" + hour.ToString() + ":";
            }

            if (minute > 9) {
                date += minute.ToString() + ":";
            } else {
                date += "0" + minute.ToString() + ":";
            }

            if (second > 9) {
                date += second.ToString() + "] ";
            } else {
                date += "0" + second.ToString() + "] ";
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(date);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
        }
    }
}
