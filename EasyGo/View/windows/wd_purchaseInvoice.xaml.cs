using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using PdfSharp.Drawing.BarCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyGo.View.windows
{
    /// <summary>
    /// Interaction logic for wd_purchaseInvoice.xaml
    /// </summary>
    public partial class wd_purchaseInvoice : Window
    {
        public wd_purchaseInvoice()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public PurchaseInvoice purchaseInvoice = new PurchaseInvoice();
        IEnumerable<PurchaseInvoice> purchaseInvoices;
        public long posId { get; set; }
        public long branchId { get; set; }
        public long branchCreatorId { get; set; }
        public long userId { get; set; }
        public long purchaseInvoiceId { get; set; }
        public bool isOk { get; set; }
      
        public string icon { get; set; }
        public string purchaseInvoiceType { get; set; }

        List<string> invTypeL;

        public string title { get; set; }
        
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                purchaseInvoice = dg_PurchaseInvoice.SelectedItem as PurchaseInvoice;
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_select_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                /*
                dg_PurchaseInvoice.ItemsSource = FillCombo.purchaseInvoices.Where(x => x.invNumber.ToLower().Contains(txb_search.Text.ToLower())).ToList();
                txt_count.Text = dg_PurchaseInvoice.Items.Count.ToString();
                */
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_ucPurchaseInvoice);
                /*
                invTypeL = purchaseInvoiceType.Split(',').ToList();

                #region translate
                if (AppSettings.lang.Equals("en"))
                {
                    grid_ucPurchaseInvoice.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_ucPurchaseInvoice.FlowDirection = FlowDirection.RightToLeft;
                }
                txt_PurchaseInvoices.Text = title;
                translat();
                #endregion
                dg_PurchaseInvoice.Columns[0].Visibility = Visibility.Collapsed;

                hidDisplayColumns();
                await refreshPurchaseInvoices();
                Txb_search_TextChanged(null, null);

                */
                HelpClass.EndAwait(grid_ucPurchaseInvoice);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_ucPurchaseInvoice);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void translat()
        {
            /*
            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            col_num.Header = AppSettings.resourcemanager.GetString("trCharp");
            col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            col_user.Header = AppSettings.resourcemanager.GetString("trUser");
            col_count.Header = AppSettings.resourcemanager.GetString("trCount");
            col_total.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_type.Header = AppSettings.resourcemanager.GetString("trType");

            #region translate agent column
            string[] invTypeArray = new string[] { "tsd", "ssd" };
            var invTypes = invTypeArray.ToList();
            var inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_agent.Header = AppSettings.resourcemanager.GetString("trCustomer");
            else
                col_agent.Header = AppSettings.resourcemanager.GetString("trVendor");

            #endregion

            txt_countTitle.Text = AppSettings.resourcemanager.GetString("trCount") + ":";

            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");

            if (page == "storageMov" && icon == "orders") // import
                col_branch.Header = AppSettings.resourcemanager.GetString("trToBranch");
            else if (page == "storageMov" && icon == "ordersWait") // export
                col_branch.Header = AppSettings.resourcemanager.GetString("trFromBranch");
            */
        }
        /*
        private void hidDisplayColumns()
        {
            #region hide Total column in grid if purchaseInvoice is import/export order/purchase order/ spending request order/Food Beverages Consumption

            string[] invTypeArray = new string[] { "imd", "exd", "im", "ex", "exw", "pod", "po", "srd", "sr", "srw", "src", "fbc" };
            var invTypes = invTypeArray.ToList();
            var inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_total.Visibility = Visibility.Collapsed; //make total column unvisible
            #endregion

            #region display branch & user columns in grid if purchaseInvoice is sales order and purchase orders
            invTypeArray = new string[] { "or" };
            invTypes = invTypeArray.ToList();
            invTypeL = purchaseInvoiceType.Split(',').ToList();
            inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
            {
                col_agent.Header = AppSettings.resourcemanager.GetString("trCustomer");
                col_agent.Visibility = Visibility.Visible;
                if (fromOrder == false)
                {
                    col_branch.Visibility = Visibility.Visible; //make branch column visible
                    col_user.Visibility = Visibility.Visible; //make user column visible
                }
                //dg_PurchaseInvoice.Columns[7].Visibility = Visibility.Visible; //make user column visible
            }
            #endregion

            #region display branch, vendor & user columns in grid if purchaseInvoice is  purchase orders
            if (purchaseInvoiceType == "po" && fromOrder == false)
            {
                col_agent.Header = AppSettings.resourcemanager.GetString("trVendor");
                col_branch.Visibility = Visibility.Visible; //make branch column visible
                col_user.Visibility = Visibility.Visible; //make user column visible
                col_agent.Visibility = Visibility.Visible;
            }
            #endregion

            #region display branch if purchaseInvoice is export or import process
            invTypeArray = new string[] { "exw", "im", "ex" };
            invTypes = invTypeArray.ToList();
            inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_branch.Visibility = Visibility.Visible; //make branch column unvisible
            #endregion

            #region display customer if purchaseInvoice is take away or self-service
            invTypeArray = new string[] { "tsd", "ssd" };
            invTypes = invTypeArray.ToList();
            inCommen = invTypeL.Any(s => invTypes.Contains(s));
            if (inCommen)
                col_agent.Visibility = Visibility.Visible; //make branch column unvisible
            #endregion
        }
        private async Task refreshPurchaseInvoices()
        {
          

        }
        */
        private void Dg_PurchaseInvoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                purchaseInvoice = dg_PurchaseInvoice.SelectedItem as PurchaseInvoice;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Dg_PurchaseInvoice_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Btn_select_Click(null, null);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
    }
}
