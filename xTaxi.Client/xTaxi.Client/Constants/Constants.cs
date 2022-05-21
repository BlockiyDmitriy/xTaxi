using System;
using System.Collections.Generic;
using System.Text;

namespace xTaxi.Client.Constants
{
    public static class Constants
    {
        public const string GoogleMapsApiKey = "AIzaSyAHj3-k5ZJSX3ZWOwUgpACDEqBSXe9oInQ";


        public static string BaseUrl = "https://10.0.2.2:44399";

        public static string AuthorizationControllerName = "/api/Account";

        public static string Register = AuthorizationControllerName + "/Register";
        public static string Login = "/Token";
        public static string GetToken = "/Token";
    }
}
