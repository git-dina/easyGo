using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGo
{
    class Global
    {
        public static string APIUri = "http://localhost:4464/api/";
        //public static string APIUri = "http://192.168.1.101:4464/api/";


        public const string TMPFolder = "Thumb";
        public const string TMPUsersFolder = "Thumb/users"; // folder to save users photos locally 
        public const string TMPCustomersFolder = "Thumb/customers"; // folder to save customers photos locally 
        public const string TMPSuppliersFolder = "Thumb/suppliers"; // folder to save Suppliers photos locally 
        public const string TMPCatFolder = "Thumb/categories"; // folder to save Categories photos locally 
        public const string TMPItemFolder = "Thumb/items"; // folder to save Categories photos locally 

    }
}
