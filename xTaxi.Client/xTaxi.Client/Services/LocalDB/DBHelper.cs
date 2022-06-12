using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace xTaxi.Client.Services.LoaclDB
{
    public static class DBHelper
    {
        public static string DBName = "MyDB";

        public static string DBPath = Path.Combine(Environment.GetFolderPath(
            Device.RuntimePlatform == Device.Android ?
                Environment.SpecialFolder.LocalApplicationData :
                Environment.SpecialFolder.Resources), DBName);
        public static string CreditCardCollection = "xtaxipayment";
        public static string CreditCardPaymentMethodCollection = "xtaxipaymentmethod";
        public static string ActivityDateId = "activityDate";

        public static string LoginCollection = "loginData";
    }
}
