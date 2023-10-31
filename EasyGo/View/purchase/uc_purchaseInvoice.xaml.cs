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

namespace EasyGo.View.purchase
{
    /// <summary>
    /// Interaction logic for uc_purchaseInvoice.xaml
    /// </summary>
    public partial class uc_purchaseInvoice : UserControl
    {
        public uc_purchaseInvoice()
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
       


        public static List<string> requiredControlList;
        PurchaseInvoice invoice = new PurchaseInvoice();
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //Instance = null;
            GC.Collect();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);

               

                translate();
                requiredControlList = new List<string> { "" };

                btn_allItems_Click(btn_allItems, null);



                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void translate()
        {
            txt_invoiceTitle.Text = AppSettings.resourcemanager.GetString("trInvoice"); 

            dg_invoiceDetails.Columns[1].Header = AppSettings.resourcemanager.GetString("trNo.");
            dg_invoiceDetails.Columns[2].Header = AppSettings.resourcemanager.GetString("trItem");
            dg_invoiceDetails.Columns[3].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_invoiceDetails.Columns[4].Header = AppSettings.resourcemanager.GetString("trAmount");
            dg_invoiceDetails.Columns[5].Header = AppSettings.resourcemanager.GetString("trPrice");
            dg_invoiceDetails.Columns[6].Header = AppSettings.resourcemanager.GetString("trTotal");

            txt_supplier.Text = AppSettings.resourcemanager.GetString("trSupplier");
            txt_discount.Text = AppSettings.resourcemanager.GetString("trDiscount");
            txt_tax.Text = AppSettings.resourcemanager.GetString("trTax");

            txt_Count.Text = AppSettings.resourcemanager.GetString("trCount");
            txt_SupTotalTitle.Text = AppSettings.resourcemanager.GetString("trSum");
            txt_taxValueTitle.Text = AppSettings.resourcemanager.GetString("trTaxPercentage");
            txt_total.Text = AppSettings.resourcemanager.GetString("trTotal");

            //txt_payInvoice.Text = AppSettings.resourcemanager.GetString("trPurchaseBill");
            //txt_store.Text = AppSettings.resourcemanager.GetString("trStore/Branch");
            //txt_vendor.Text = AppSettings.resourcemanager.GetString("trVendor");
            //txt_vendorIvoiceDetails.Text = AppSettings.resourcemanager.GetString("trVendorDetails");

            //txt_printInvoice.Text = AppSettings.resourcemanager.GetString("trPrint");
            //txt_preview.Text = AppSettings.resourcemanager.GetString("trPreview");
            //txt_invoiceImages.Text = AppSettings.resourcemanager.GetString("trImages");
            //txt_items.Text = AppSettings.resourcemanager.GetString("trItems");
            //txt_drafts.Text = AppSettings.resourcemanager.GetString("trDrafts");
            //txt_newDraft.Text = AppSettings.resourcemanager.GetString("trNew");
            //txt_payments.Text = AppSettings.resourcemanager.GetString("trPayments");
            //txt_returnInvoice.Text = AppSettings.resourcemanager.GetString("trReturn");
            //txt_invoices.Text = AppSettings.resourcemanager.GetString("trInvoices");
            //txt_purchaseOrder.Text = AppSettings.resourcemanager.GetString("trOrders");
            //txt_totalDescount.Text = AppSettings.resourcemanager.GetString("trDiscount");
            //txt_shippingCost.Text = AppSettings.resourcemanager.GetString("shippingAmount");

            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discount, AppSettings.resourcemanager.GetString("trDiscountHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_typeDiscount, AppSettings.resourcemanager.GetString("trDiscountTypeHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_branch, AppSettings.resourcemanager.GetString("trStore/BranchHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_vendor, AppSettings.resourcemanager.GetString("trVendorHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_desrvedDate, AppSettings.resourcemanager.GetString("trDeservedDateHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_invoiceNumber, AppSettings.resourcemanager.GetString("trInvoiceNumberHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(dp_invoiceDate, AppSettings.resourcemanager.GetString("trInvoiceDateHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_shippingCost, AppSettings.resourcemanager.GetString("shippingAmount"));

            //tt_error_previous.Content = AppSettings.resourcemanager.GetString("trPrevious");
            //tt_error_next.Content = AppSettings.resourcemanager.GetString("trNext");

            btn_save.Content = AppSettings.resourcemanager.GetString("trBuy");
        }
      
       
        private void clearInvoice()
        {
          
        }
       
        //bool menuState = false;

        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {

            //this.DataContext = new Receipt();


            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }
        string input;
        decimal _decimal = 0;
        private void Number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {


                //only  digits
                TextBox textBox = sender as TextBox;
                HelpClass.InputJustNumber(ref textBox);
                if (textBox.Tag.ToString() == "int")
                {
                    Regex regex = new Regex("[^0-9]");
                    e.Handled = regex.IsMatch(e.Text);
                }
                else if (textBox.Tag.ToString() == "decimal")
                {
                    input = e.Text;
                    e.Handled = !decimal.TryParse(textBox.Text + input, out _decimal);

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                //only english and digits
                Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
                if (!regex.IsMatch(e.Text))
                    e.Handled = true;
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
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void validateEmpty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }










        #endregion
    

    

       
        #region category
        private async void btn_allItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             
                /*
                // categoryPath
                categoryPath.Clear();
                buildCategoryPath(categoryPath);
                */
               
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /*
        List<CategoryModel> categoryPath = new List<CategoryModel>();
        void buildCategoryPath(List<CategoryModel> categories)
        {
            sp_categoryPath.Children.Clear();
            foreach (var item in categories)
            {

                #region borderMain
                Border borderMain = new Border();

                borderMain.Margin = new Thickness(0);
                borderMain.Padding = new Thickness(0);
                borderMain.MinWidth = 50;
                borderMain.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                #region buttonMain
                Button buttonMain = new Button();
                buttonMain.Tag = item.id;
                buttonMain.DataContext = item;
                buttonMain.Padding = new Thickness(0);
                buttonMain.BorderBrush = null;
                buttonMain.Background = null;
                buttonMain.Height = 50;

                buttonMain.Click += btn_categoryPath_Click;
                #region textName
                TextBlock textName = new TextBlock();
                textName.Text = ">" + item.name;
                textName.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                textName.Margin = new Thickness(5);
                buttonMain.Content = textName;
                #endregion
                borderMain.Child = buttonMain;
                #endregion
                sp_categoryPath.Children.Add(borderMain);
                #endregion
            }

        }
        private async void btn_categoryPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button.DataContext != null)
                {

                    switchGrid1_1("mainItemsCatalog");


                    var item = button.DataContext as CategoryModel;
                    // categoryPath
                    //categoryPath.Add(item);

                    categoryPath = _itemService.getCategoryPath(item.id).ToList();
                    buildCategoryPath(categoryPath);

                    // itemsCard
                    items = _itemService.getCatItems(item.id);
                    await buildItemsCard(items);

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */
        #endregion




        #region invoiceDetails
        private void Dg_invoiceDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void unitRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {

        }
        private void deleteRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {

            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        PurInvoiceItem row = (PurInvoiceItem)dg_invoiceDetails.SelectedItems[0];
                        invoiceDetailsList.Remove(row);

                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        #endregion
        List<PurInvoiceItem> invoiceDetailsList = new List<PurInvoiceItem>();


        #region search
       
        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
           

        }
        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion

        private void btn_discount_Click(object sender, RoutedEventArgs e)
        {

        }

       
        private void btn_printInvoice_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_invoiceImage_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
