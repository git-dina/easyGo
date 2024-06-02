using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyGo.Template
{
    /// <summary>
    /// Interaction logic for uc_itemSalesInvoice.xaml
    /// </summary>
    public partial class uc_itemSalesInvoice : UserControl
    {
        public uc_itemSalesInvoice()
        {
            InitializeComponent();
        }
        public Func<SalesInvoiceItem, bool> funcDelete;
        public SalesInvoiceItem salesInvoiceItem;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = salesInvoiceItem;

        }
        void buttonPlus_Click(object sender, RoutedEventArgs e)
        {
                salesInvoiceItem.Quantity++;
        }
        void buttonMinus_Click(object sender, RoutedEventArgs e)
        {
            {
                if (salesInvoiceItem.Quantity > 1)
                {
                    salesInvoiceItem.Quantity--;
                }
            }
        }
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            bool state = funcDelete(salesInvoiceItem);
        }


        #region validate - clearValidate - textChange - lostFocus - . . . . 
        private void NumberInt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                //if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Spaces_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.Handled = e.Key == Key.Space;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion

    }
}
