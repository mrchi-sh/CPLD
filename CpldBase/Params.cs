using System.Collections;

namespace CpldBase
{
    public class Params
    {
        public static string ServerHost = "192.168.100.200";
        public static int ServerPort = 2016;

        public static string DbName = "tds_data";
        public static string DbHost = "192.168.100.100";
        public static string DbUser = "root";
        public static string DbPsasword = "asdfghjkl";

        public static int DbPort = 3306;
    }

    public class BartendParams
    {
        public static string BartendPath = "";
    }

    public class Settings
    {
        public static bool IsDelayScan = false;
        public static bool IsShareSerial = false;
        public static int DelayTime = 1000;
    }
   
    public class CustomizeVersion
    {
        public static bool Debug = false;
    }
    
    public abstract class ThreadFlags
    {
        public static bool ConnStatCheck = true;
    }

    public class TimeoutConfig
    {
        public static Hashtable TimeoutHt = new Hashtable();

        static TimeoutConfig()
        {
            TimeoutHt.Add("32", 25);        //17.5
            TimeoutHt.Add("31", 24);        //16.1
            TimeoutHt.Add("30", 23);        //15.1
            TimeoutHt.Add("29", 23);        //15.7
            TimeoutHt.Add("28", 22);        //15
            TimeoutHt.Add("27", 22);        //14
            TimeoutHt.Add("26", 22);        //14
            TimeoutHt.Add("25", 22);        //14
            TimeoutHt.Add("24", 22);        //14
            TimeoutHt.Add("23", 21);        //13
            TimeoutHt.Add("22", 21);        //13
            TimeoutHt.Add("21", 20);        //12
            TimeoutHt.Add("20", 20);        //12
            TimeoutHt.Add("19", 19);        //11
            TimeoutHt.Add("18", 18);        //10
            TimeoutHt.Add("17", 18);        //10
            TimeoutHt.Add("16", 17);        //9
            TimeoutHt.Add("15", 16);        //8
            TimeoutHt.Add("14", 15);        //7
            TimeoutHt.Add("13", 15);        //7
            TimeoutHt.Add("12", 14);        //6
            TimeoutHt.Add("11", 13);        //5
            TimeoutHt.Add("10", 12);        //4
            TimeoutHt.Add("9", 11);         //4
            TimeoutHt.Add("8", 8);          //3
            TimeoutHt.Add("7", 7);          //2
            TimeoutHt.Add("6", 7);          //2
            TimeoutHt.Add("5", 7);          //2
            TimeoutHt.Add("4", 6);          //1
            TimeoutHt.Add("3", 6);          //1
            TimeoutHt.Add("2", 5);          //1
            TimeoutHt.Add("1", 5);          //1
        }
    }
}
