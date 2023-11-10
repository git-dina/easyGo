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
    class totalConverter : IMultiValueConverter
    {
        //accuracyConverter
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values != null && values[0] != null && values[1] != null)
                {
                    decimal quantity = decimal.Parse(values[0].ToString());
                    decimal price = decimal.Parse(values[1].ToString());
                    decimal total = quantity * price;
                    return HelpClass.DecTostring(total);
                }
                else return "";
            }
            catch
            {
                return "";
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
