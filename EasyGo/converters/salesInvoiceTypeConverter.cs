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
    public class salesInvoiceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                switch (value)
                {
                    //مبيعات 
                    case "s":
                        value = AppSettings.resourcemanager.GetString("trSalesInvoice");
                        break;
                    //مسودة مبيعات
                    case "sd":
                        value = AppSettings.resourcemanager.GetString("trSalesDraft");
                        break;

                    //مرتجع مبيعات
                    case "sb":
                        value = AppSettings.resourcemanager.GetString("trSalesReturnInvoice");
                        break;
                    // مسودة مرتجع مبيعات
                    case "sbd":
                        value = AppSettings.resourcemanager.GetString("trSalesReturnDraft");
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
