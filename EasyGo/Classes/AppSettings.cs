using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Deployment.Application;
using System.Reflection;

namespace EasyGo.Classes
{
    public class AppSettings
    {

        public static ResourceManager resourcemanager;
        public static ResourceManager resourcemanagerreport;
        public static ResourceManager resourcemanagerAr;
        public static ResourceManager resourcemanagerEn;
        // app version
        static public string CurrentVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                       ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                       : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public static string lang = "en";
        public static string Reportlang = "en";
        public static string invoice_lang = "en";
        public static bool menuState;

        internal static string dateFormat = "ShortDatePattern";
        internal static string timeFormat = "ShortTimePattern";
        internal static string accuracy;

        public static string Currency = "SYP";
        public static long CurrencyId;

        #region company info
        public static string logoImage;
        public static string companyName;

        public static string com_name_ar="";
        public static string com_address_ar="";
        public static string Email="";
        public static string Fax ="";
        public static string Mobile ="";
        public static string Address="";
        public static string Phone ="";

        //social
        public static string com_website="";
        public static string com_website_icon;
        public static string com_social="";
        public static string com_social_icon;
        #endregion

        #region print
        public static string show_header;
        public static string rep_printer_name;
        public static string rep_print_count;
        public static string sale_printer_name;
        public static string salePaperSize = "5.7cm";
        #endregion

        public static decimal PosBalance;
        // invoices count for logged user
        #region purchase invoice
        public static int PurchaseDraftCount;
        public static int duration = 1;
        #endregion
    }
}
