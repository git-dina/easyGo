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
    public class purchaseInvoiceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                /*
                switch (value)
                {
                    //مشتريات 
                    case "p":
                        value = AppSettings.resourcemanager.GetString("trPurchaseInvoice");
                        break;
                   
                    default: break;
                }
                */
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
