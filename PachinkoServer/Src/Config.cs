using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PachinkoServer
{
    public class Config
    {
        public string PORT = "8080";
    }

    public class URL
    {
        public static string PATH_YDU = "/ydu";
        public static string PATH_YDU_GET_INPUT = "/ydu/getinput";
        public static string PATH_SWITCH = "/control/switch";
        public static string PATH_CHANGE_LEVEL = "/control/changelv";

        public static string PARAM_YDU_TYPE_OPEN = "1";
        public static string PARAM_YDU_TYPE_CLOSE = "0";

        public static string PARAM_SWITCH_TYPE_ON = "1";
        public static string PARAM_SWITCH_TYPE_OFF = "0";
    }
}
