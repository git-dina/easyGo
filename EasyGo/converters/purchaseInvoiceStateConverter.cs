using EasyGo.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EasyGo.converters
{
    public class purchaseInvoiceStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                switch (value)
                {
                    //منتهية 
                    case "p":
                        value = AppSettings.resourcemanager.GetString("done");
                        break;
                    //مسودة
                    case "pd":
                        value = AppSettings.resourcemanager.GetString("draft");
                        break;
                    //منتهية
                    case "pb":
                        value = AppSettings.resourcemanager.GetString("done");
                        break;
                    //مسودة
                    case "pbd":
                        value = AppSettings.resourcemanager.GetString("draft");
                        break;

                    default: break;
                }

                return value;
            }
            catch
            {
                return "";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
