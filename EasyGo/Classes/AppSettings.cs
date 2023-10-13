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
        public static bool menuState;


    }
}
