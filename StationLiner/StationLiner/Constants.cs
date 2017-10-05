using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StationLinerMVC
{
    public static class Constants
    {
        public static string AdminRole
        {
            get { return "SystemAdmin"; }
        }

        public static string AdminMode
        {
            get { return "AdminMode"; }
        }

        public static string ChannelMode
        {
            get { return "ChannelMode"; }
        }
        public static string Purchase { get { return "Purchase"; } }
        public static string Sale { get { return "Sale"; } }
        public static string FuelRecieption { get { return "FuelReception"; } }
        public static string ShiftOpening { get { return "ShiftOpening"; } }
        public static string ShiftClosing { get { return "ShiftClosing"; } }
        public static string Fuel { get { return "Fuel"; } }
    }
}