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
        private string invType = "pd,pbd,p , pw , pb, pbw";
        public long posId { get; set; }
        public long branchId { get; set; }
        public long branchCreatorId { get; set; }
        public long userId { get; set; }
        public long purchaseInvoiceId { get; set; }
        public bool isOk { get; set; }
      
        public string icon { get; set; }
        public string purchaseInvoiceType { get; set; }

        public string title { get; set; }
        
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                purchaseInvoice = dg_PurchaseInvoice.SelectedItem as PurchaseInvoice;
                DialogResult = true;
                isOk = true;
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

        #region search
        private void Txb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                search();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void cb_invType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cb_invType.SelectedIndex > 0)
                invType = cb_invType.SelectedValue.ToString();
            else
                invType = "pd,pbd,p , pw , pb, pbw";
            search();
        }

        private void search()
        {
            try
            {
                purchaseInvoices = FillCombo.purchaseInvoices.Where(x => x.InvNumber.ToLower().Contains(txb_search.Text.ToLower())).ToList();
                if (cb_invType.SelectedIndex > 0)
                    purchaseInvoices = purchaseInvoices.Where(x => x.InvType.Equals(invType));
                dg_PurchaseInvoice.ItemsSource = purchaseInvoices;
                txt_count.Text = dg_PurchaseInvoice.Items.Count.ToString();

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #endregion
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_ucPurchaseInvoice);

                await refreshInvoices();
               search();

                FillCombo.fillPurchaseTypes(cb_invType);

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
            txt_PurchaseInvoices.Text = AppSettings.resourcemanager.GetString("trPurchaseInvoices");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(txb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_invType, AppSettings.resourcemanager.GetString("trTypeHint"));

            col_num.Header = AppSettings.resourcemanager.GetString("trCharp");
            //col_branch.Header = AppSettings.resourcemanager.GetString("trBranch");
            //col_user.Header = AppSettings.resourcemanager.GetString("trUser");
            col_count.Header = AppSettings.resourcemanager.GetString("trCount");
            col_total.Header = AppSettings.resourcemanager.GetString("trTotal");
            col_type.Header = AppSettings.resourcemanager.GetString("trType");


            txt_countTitle.Text = AppSettings.resourcemanager.GetString("trCount") + ":";

            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");

          
        }

        private async Task refreshInvoices()
        {

            FillCombo.purchaseInvoices = await purchaseInvoice.GetInvoicesByCreator(invType, MainWindow.userLogin.UserId, AppSettings.duration);
           
        }
       
       
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
