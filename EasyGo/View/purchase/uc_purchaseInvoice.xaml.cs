using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using EasyGo.Template;
using EasyGo.View.windows;
using Microsoft.Reporting.WinForms;
using netoaster;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
        PurchaseInvoice invoiceModel = new PurchaseInvoice();
        ItemLocation itemLocation = new ItemLocation();
        CashTransfer cashTransfer = new CashTransfer();
        string _InvoiceType = "pd";
        public static bool isFromReport = false;
        #region barcode params
        DateTime _lastKeystroke = new DateTime(0);
        #endregion
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

            txt_CountTitle.Text = AppSettings.resourcemanager.GetString("trCount");
            txt_SupTotalTitle.Text = AppSettings.resourcemanager.GetString("trSum");
            txt_taxValueTitle.Text = AppSettings.resourcemanager.GetString("trTaxValue");
            txt_taxRateTitle.Text = AppSettings.resourcemanager.GetString("trTaxRate");
            txt_discountValueTitle.Text = AppSettings.resourcemanager.GetString("trDiscountValue");
            txt_discountRateTitle.Text = AppSettings.resourcemanager.GetString("trDiscountRate");
            txt_totalTitle.Text = AppSettings.resourcemanager.GetString("trTotal");

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
            invoice = new PurchaseInvoice();
            this.DataContext = invoice;
            invoiceDetailsList = new List<PurInvoiceItem>();
            listPayments = new List<CashTransfer>();
            refreshInvoiceDetails();
            _InvoiceType = "pd";
            isFromReport = false;
            inputEditable();

            ActiveButton(btn_supplier, false, AppSettings.resourcemanager.GetString("trSupplier"));
            ActiveButton(btn_discount, false);
            ActiveButton(btn_tax, false);
            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }

        private void inputEditable()
        {
            if (_InvoiceType == "pbw") // purchase invoice
            {
                col_delete.Visibility = Visibility.Visible; //make delete column visible
                col_price.IsReadOnly = false; //make price read only
                col_unit.IsReadOnly = false; //make unit read only
                col_quantity.IsReadOnly = false; //make count read only
                btn_supplier.IsEnabled = false;
                //tb_barcode.IsEnabled = false;
                //cb_branch.IsEnabled = false;
                btn_discount.IsEnabled = false;
                btn_save.IsEnabled = true;
                btn_tax.IsEnabled = false;

            }
            else if (_InvoiceType == "pbd") // return invoice
            {
                col_delete.Visibility = Visibility.Visible; //make delete column visible
                col_price.IsReadOnly = false; //make price read only
                col_unit.IsReadOnly = true; //make unit read only
                col_quantity.IsReadOnly = false; //make count read only
                btn_supplier.IsEnabled = false;
                //tb_barcode.IsEnabled = false;
                //cb_branch.IsEnabled = true;
                btn_discount.IsEnabled = false;
                btn_save.IsEnabled = true;
                btn_tax.IsEnabled = false;
            }
            else if (_InvoiceType == "pd") // purchase draft 
            {
                col_delete.Visibility = Visibility.Visible; //make delete column visible
                col_price.IsReadOnly = false;
                col_unit.IsReadOnly = false;
                col_quantity.IsReadOnly = false;
                btn_supplier.IsEnabled = true;
                //tb_barcode.IsEnabled = true;
                // cb_branch.IsEnabled = true;
                btn_discount.IsEnabled = true;
                btn_save.IsEnabled = true;
                btn_tax.IsEnabled = true;

            }
            //else if (_InvoiceType == "po") //  purchase order
            //{
            //    col_delete.Visibility = Visibility.Visible; //make delete column visible
            //    col_price.IsReadOnly = false;
            //    col_unit.IsReadOnly = false;
            //    col_quantity.IsReadOnly = false;
            //    btn_supplier.IsEnabled = false;
            //    //tb_barcode.IsEnabled = true;
            //    //cb_branch.IsEnabled = true;
            //    btn_discount.IsEnabled = true;
            //    btn_save.IsEnabled = true;
            //    btn_tax.IsEnabled = true;
            //}
            else if (_InvoiceType == "pw" || _InvoiceType == "p" || _InvoiceType == "pb")//|| archived)
            {
                col_delete.Visibility = Visibility.Collapsed; //make delete column unvisible
                col_price.IsReadOnly = true; //make price read only
                col_unit.IsReadOnly = true; //make unit read only
                col_quantity.IsReadOnly = true; //make count read only
                btn_supplier.IsEnabled = false;
                // tb_barcode.IsEnabled = false;
                // cb_branch.IsEnabled = false;
                btn_discount.IsEnabled = false;
                btn_save.IsEnabled = false;
                btn_tax.IsEnabled = false;


                if (_InvoiceType.Equals("pb") || _InvoiceType.Equals("p"))
                {
                    #region print - pdf - send email
                    btn_printInvoice.Visibility = Visibility.Visible;
                    btn_pdf.Visibility = Visibility.Visible;
                    //if (FillCombo.groupObject.HasPermissionAction(printCountPermission, FillCombo.groupObjects, "one"))
                    //{
                    //    btn_printCount.Visibility = Visibility.Visible;
                    //    bdr_printCount.Visibility = Visibility.Visible;
                    //}
                    //else
                    //{
                    //    btn_printCount.Visibility = Visibility.Collapsed;
                    //    bdr_printCount.Visibility = Visibility.Collapsed;
                    //}
                    //if (FillCombo.groupObject.HasPermissionAction(sendEmailPermission, FillCombo.groupObjects, "one"))
                    //{
                    //    btn_emailMessage.Visibility = Visibility.Visible;
                    //    bdr_emailMessage.Visibility = Visibility.Visible;
                    //}
                    //else
                    //{
                    //    btn_emailMessage.Visibility = Visibility.Collapsed;
                    //    bdr_emailMessage.Visibility = Visibility.Collapsed;
                    //}
                    #endregion
                }
                else
                {
                    #region print - pdf - send email
                    btn_printInvoice.Visibility = Visibility.Collapsed;
                    btn_pdf.Visibility = Visibility.Collapsed;
                    // btn_printCount.Visibility = Visibility.Collapsed;
                    // btn_emailMessage.Visibility = Visibility.Collapsed;
                    // bdr_emailMessage.Visibility = Visibility.Collapsed;
                    #endregion
                }
                if (!isFromReport)
                {
                    btn_next.Visibility = Visibility.Visible;
                    btn_previous.Visibility = Visibility.Visible;
                }

                //if ((_InvoiceType != "pd" && invoice.tax == 0) || _InvoiceType == "pbd")
                //{
                //    sp_tax.Visibility = Visibility.Collapsed;
                //    tb_taxValue.Text = "0";
                //}
                //else if (AppSettings.invoiceTax_bool == true || invoice.tax > 0)
                //    sp_tax.Visibility = Visibility.Visible;
            }
        }
        //bool menuState = false;

        #region validate - clearValidate - textChange - lostFocus - . . . . 

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
        List<Category> categoryPath = new List<Category>();
        List<Category> categoryItem = new List<Category>();
        List<Item> items = new List<Item>();
        Item selectedItem = new Item();
        private async void btn_allItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                // categoryPath
                categoryPath.Clear();
                buildCategoryPath(categoryPath);
                // categoryItem
                if (FillCombo.categoriesFirstLevelList is null)
                   await FillCombo.RefreshCategoriesList();
                categoryItem = FillCombo.categoriesFirstLevelList.ToList();
                buildCategoryItem(categoryItem);
                // itemsCard
                if (FillCombo.itemsHasUnitsList is null)
                    await FillCombo.RefreshItemsHasUnits();
                items =  FillCombo.itemsHasUnitsList.ToList();
                buildItemCards(items);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void buildCategoryPath(List<Category> categories)
        {
            sp_categoryPath.Children.Clear();
            int counter = 1;
            bool isLast = false;
            foreach (var item in categories)
            {
                if (counter == categories.Count)
                    isLast = true;
                else
                    isLast = false;

                #region borderMain
                Border borderMain = new Border();
                borderMain.Margin = new Thickness(0);
                borderMain.Padding = new Thickness(0);
                borderMain.MinWidth = 50;
                borderMain.Background = Application.Current.Resources["White"] as SolidColorBrush;
                #region buttonMain
                Button buttonMain = new Button();
                buttonMain.DataContext = item;
                buttonMain.BorderBrush = null;
                buttonMain.Background = null;
                buttonMain.Height = 40;
                buttonMain.Padding = new Thickness(0);

                buttonMain.Click += Btn_category_Click;
                #region textName
                
                TextBlock textName = new TextBlock();
                if (isLast)
                {
                    textName.Text = item.Name;
                    textName.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                }
                else
                {
                    textName.Text = item.Name + ">";
                    textName.Foreground = Application.Current.Resources["Grey"] as SolidColorBrush;
                }
                textName.Margin = new Thickness(5);
                buttonMain.Content = textName;
                #endregion
                borderMain.Child = buttonMain;
                #endregion
                sp_categoryPath.Children.Add(borderMain);
                #endregion
                counter++;
            }

        }
        void buildCategoryItem(List<Category> categories)
        {
            sp_categoryItem.Children.Clear();
            foreach (var item in categories)
            {

            
                #region buttonMain
                Button buttonMain = new Button();
                buttonMain.DataContext = item;
                buttonMain.Content = item.Name;
                buttonMain.Margin = new Thickness(5);
                MaterialDesignThemes.Wpf.ButtonAssist.SetCornerRadius(buttonMain,new CornerRadius(7));
                buttonMain.Background = Application.Current.Resources["White"] as SolidColorBrush;
                buttonMain.BorderBrush = Application.Current.Resources["MainColor"] as SolidColorBrush;
                buttonMain.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                buttonMain.BorderThickness = new Thickness(1);

                buttonMain.Click += Btn_category_Click;
                sp_categoryItem.Children.Add(buttonMain);
                #endregion
            }

        }
        private async void Btn_category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button.DataContext != null)
                {



                    var item = button.DataContext as Category;

                    // categoryItem
                    categoryItem = await FillCombo.category.GetCategoryChilds(item.CategoryId);
                        buildCategoryItem(categoryItem);
                        // categoryPath
                        categoryPath = await FillCombo.category.GetCategoryPath(item.CategoryId);
                        buildCategoryPath(categoryPath);
                    // itemsCard
                    items = await FillCombo.item.GetCategoryItems(item.CategoryId);
                    buildItemCards(items);

                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        
        #region ItemCards
        void buildItemCards(List<Item> list)
        {
            wp_itemsCard.Children.Clear();
            foreach (var item in list)
            {
                #region mco_itemCards
                uc_itemCards mco_itemCards = new uc_itemCards();
                mco_itemCards.item = item;
                mco_itemCards.Color = Application.Current.Resources["MainColor"] as SolidColorBrush;
                mco_itemCards.Click += Btn_itemCards_Click;
                wp_itemsCard.Children.Add(mco_itemCards);
                #endregion
            }
        }
        private void Btn_itemCards_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                var itemCards = sender as uc_itemCards;
                if (itemCards != null)
                {
                    selectedItem = itemCards.item;
                    AddItemToInvoice(selectedItem);

                }


                HelpClass.EndAwait(grid_main);
            }
            catch
            {
                HelpClass.EndAwait(grid_main);

            }
        }


        #endregion

        #endregion




        #region invoiceDetails
      
        private async void Dg_invoiceDetails_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
          
            try
            {

                TextBox t = e.EditingElement as TextBox;  // Assumes columns are all TextBoxes
                var columnName = e.Column.Header.ToString();

                PurInvoiceItem row = e.Row.Item as PurInvoiceItem;
                int index = invoiceDetailsList.IndexOf(invoiceDetailsList.Where(p => p.ItemUnitId == row.ItemUnitId ).FirstOrDefault());

                //TimeSpan elapsed = (DateTime.Now - _lastKeystroke);
                //if (elapsed.TotalMilliseconds < 100)
                //{
                //    if (columnName == AppSettings.resourcemanager.GetString("trAmount"))
                //        t.Text = invoiceDetailsList[index].Quantity.ToString();
                //    else if (columnName == AppSettings.resourcemanager.GetString("trPrice"))
                //        t.Text = HelpClass.DecTostring(invoiceDetailsList[index].Price);

                //}
                //else
                {
                    int oldCount = 0;
                    long newCount = 0;
                    decimal oldPrice = 0;
                    decimal newPrice = 0;

                    //"tb_amont"
                    if (columnName == AppSettings.resourcemanager.GetString("trAmount"))
                    {
                        if (!t.Text.Equals(""))
                            newCount = int.Parse(t.Text);
                        else
                            newCount = 0;
                        if (newCount < 0)
                        {
                            newCount = 0;
                            t.Text = "0";
                        }
                    }
                    else
                        newCount = row.Quantity;

                    oldCount = row.Quantity;
                    oldPrice = row.Price;

                    #region if return invoice
                    if (_InvoiceType == "pbd" || _InvoiceType == "pbw")
                    {
                        var selectedItemUnitId = row.ItemUnitId;

                        var itemUnitsIds = FillCombo.itemUnitList.Where(x => x.ItemId == row.ItemId).Select(x => x.ItemUnitId).ToList();

                        #region caculate available amount in this invoice 
                       int availableAmountInBranch = await itemLocation.getAmountInBranch((long)row.ItemUnitId, MainWindow.branchLogin.BranchId);
                        int amountInBill = await getAmountInBill(row.ItemId, (long)row.ItemUnitId, row.ID);
                        int availableAmount = availableAmountInBranch - amountInBill;
                        #endregion
                        #region calculate amount in purchase invoice
                        var items = invoice.InvoiceItems.ToList().Where(i => itemUnitsIds.Contains((long)i.ItemUnitId));
                        int purchasedAmount = 0;
                        foreach (var it in items)
                        {
                            if (selectedItemUnitId == (long)it.ItemUnitId)
                                purchasedAmount += (int)it.Quantity;
                            else
                                purchasedAmount +=  FillCombo.itemUnit.fromUnitToUnitQuantity((int)it.Quantity, row.ItemId, (long)it.ItemUnitId,(long) selectedItemUnitId);
                        }
                        #endregion
                        if (newCount > (purchasedAmount - amountInBill) || newCount > availableAmount)
                        {
                            // return old value 
                            t.Text = (purchasedAmount - amountInBill) > availableAmount ? availableAmount.ToString() : (purchasedAmount - amountInBill).ToString();

                            newCount = (purchasedAmount - amountInBill) > availableAmount ? availableAmount : (purchasedAmount - amountInBill);
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                    #endregion
                    if (columnName == AppSettings.resourcemanager.GetString("trPrice") && !t.Text.Equals(""))
                        newPrice = decimal.Parse(t.Text);
                    else
                        newPrice = row.Price;



                    // old total for changed item
                    decimal total = oldPrice * oldCount;

                    // new total for changed item
                    total = newCount * newPrice;


                    //refresh total cell
                    //TextBlock tb = dg_invoiceDetails.Columns[6].GetCellContent(dg_invoiceDetails.Items[index]) as TextBlock;
                    //tb.Text = HelpClass.DecTostring(total);

                    //  refresh sum and total text box
                    CalculateInvoiceValues();

                    // update item in invoiceDetails           
                    invoiceDetailsList[index].Quantity = (int)newCount;
                    invoiceDetailsList[index].Price = newPrice;
                    invoiceDetailsList[index].Total = total;
                }
                refreshInvoiceDetails();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
          
            try
            {
                await Task.Delay(0050);
                CalculateInvoiceValues();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async Task<int> getAmountInBill(long itemId, long itemUnitId, long ID)
        {
            int quantity = 0;
            var itemUnits = FillCombo.itemUnitList.Where(a => a.ItemId == itemId).ToList();

            var smallUnits =  FillCombo.itemUnit.getSmallItemUnits(itemId, itemUnitId);
            foreach (ItemUnit u in itemUnits)
            {
                var isInBill = invoiceDetailsList.ToList().Find(x => x.ItemUnitId == (long)u.ItemUnitId && x.ID != ID); // unit exist in invoice
                if (isInBill != null)
                {
                    var isSmall = smallUnits.Find(x => x.ItemUnitId == (long)u.ItemUnitId);
                    int unitValue = 0;

                    int index = invoiceDetailsList.IndexOf(invoiceDetailsList.Where(p => p.ItemUnitId == u.ItemUnitId).FirstOrDefault());
                    int count = invoiceDetailsList[index].Quantity;
                    if (itemUnitId == u.ItemUnitId)
                    {
                        quantity += count;
                    }
                    else if (isSmall != null) // from-unit is bigger than to-unit
                    {
                        unitValue =  FillCombo.itemUnit.largeToSmallUnitQuan(itemUnitId, (long)u.ItemUnitId, itemId);
                        quantity += count / unitValue;
                    }
                    else
                    {
                        unitValue =  FillCombo.itemUnit.smallToLargeUnit(itemUnitId, (long)u.ItemUnitId, itemId);

                        if (unitValue != 0)
                        {
                            quantity += count * unitValue;
                        }
                    }

                }
            }
            return quantity;
        }
        private void dg_invoiceDetails_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                CalculateInvoiceValues();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void unitRowFromInvoiceItems(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                    if (vis is DataGridRow)
                    {

                        PurInvoiceItem row = (PurInvoiceItem)dg_invoiceDetails.SelectedItems[0];
                        var item = FillCombo.itemsHasUnitsList.Where(x => x.ItemId == row.ItemId).FirstOrDefault();
                        wd_selectItemUnit w = new wd_selectItemUnit();
                        w.itemUnitsList = item.ItemUnits;
                        w.itemUnitId = row.ItemUnitId;
                        w.ShowDialog();
                        if (w.isOk)
                        {
                            row.ItemUnitId = w.itemUnitId;
                            row.UnitName = w.unitName;
                        }

                        refreshInvoiceDetails();
                    }

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
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

                        refreshInvoiceDetails();
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

        #region Invoice Operations

        private void refreshInvoiceDetails()
        {
            try
            {
                int index = 1;
                foreach (var item in invoiceDetailsList)
                {
                    item.Index = index;
                    index++;
                    item.ID = index;
                }
                dg_invoiceDetails.ItemsSource = invoiceDetailsList;
                dg_invoiceDetails.Items.Refresh();
            }
            catch { }
        }
        private void AddItemToInvoice(Item item)
        {
            long defaultUnitId = 0;

            var defaultUnit = item.ItemUnits.Where(x => x.IsDefaultPurchase == true).FirstOrDefault();
            if (defaultUnit != null)
            {
                defaultUnitId = defaultUnit.ItemUnitId;
                var itemInInvoice = invoiceDetailsList.Where(x => x.ItemUnitId == defaultUnitId).FirstOrDefault();
                if (itemInInvoice != null)
                {
                    itemInInvoice.Quantity++;
                    itemInInvoice.Total = itemInInvoice.Quantity * itemInInvoice.Price;
                }
                else
                {

                    invoiceDetailsList.Add(new PurInvoiceItem()
                    {
                        ItemName = item.Name,
                        ItemUnitId = defaultUnitId,
                        ItemId = item.ItemId,
                        UnitName = defaultUnit.UnitName,
                        Quantity = 1,
                        Price = (decimal)defaultUnit.PurchasePrice,
                        Total = (decimal)defaultUnit.PurchasePrice,
                        CreateUserId = MainWindow.userLogin.UserId,
                    });
                }
            }
            else
            {
                invoiceDetailsList.Add(new PurInvoiceItem()
                {
                    ItemName = item.Name,
                    Quantity = 1,
                    Price = 0,
                    Total = 0,
                });
            }

            refreshInvoiceDetails();
            CalculateInvoiceValues();
        }

        private void CalculateInvoiceValues()
        {
            decimal total = invoiceDetailsList.Select(x => x.Total).Sum();


            #region tax

           if(invoice.TaxType.Equals("rate"))
            {
                invoice.Tax = HelpClass.calcPercentage(total, (decimal)invoice.TaxPercentage);//tax value
            }
           else if(total != 0)
                invoice.TaxPercentage = (invoice.Tax * 100) / total; //tax rate

            #endregion
            decimal totalAfterTax = total + invoice.Tax;

            #region discount

            decimal manualDiscount = 0;
            decimal manualDiscountRate = 0;

            if (invoice.DiscountType == "rate" )
            {
                manualDiscount = HelpClass.calcPercentage(totalAfterTax, (decimal)invoice.DiscountPercentage);
                manualDiscountRate = (decimal)invoice.DiscountPercentage;
            }
            else
            {
                manualDiscount = invoice.DiscountValue;
                manualDiscountRate = (manualDiscount * 100) / totalAfterTax;
            }


            #endregion

            decimal totalNet = totalAfterTax - manualDiscount;


            //display
            invoice.Count = invoiceDetailsList.Select(x => x.Quantity).Sum();
            invoice.Total = invoiceDetailsList.Select(x => x.Total).Sum();

            invoice.DiscountValue = manualDiscount;
            invoice.DiscountPercentage = manualDiscountRate;
            invoice.TotalNet = totalNet;

            this.DataContext = null;
            this.DataContext = invoice;
        }

        #endregion

        #region search

        private void Btn_search_Click(object sender, RoutedEventArgs e)
        {
           

        }
        private void tb_search_KeyDown(object sender, KeyEventArgs e)
        {

        }

        #endregion


       
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

        #region Save
        List<CashTransfer> listPayments;
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

               // if (FillCombo.groupObject.HasPermissionAction(invoicePermission, FillCombo.groupObjects, "one"))
                {
                   // if (MainWindow.posLogin.BoxState == "o") // box is open
                    {
                        //check mandatory inputs
                        if (HelpClass.validate(requiredControlList, this))
                        {
                            bool multipleValid = true;
                            bool validate = validateInvoiceValues();
                            bool valid = validateItemUnits();
                           // TextBox tb = (TextBox)dp_desrvedDate.Template.FindName("PART_TextBox", dp_desrvedDate);
                            if (valid && validate)
                            {
                                Window.GetWindow(this).Opacity = 0.2;
                                wd_multiplePaymentPurchase w = new wd_multiplePaymentPurchase();
                                if ( invoice.SupplierId != null && invoice.SupplierId != 0)
                                    w.hasCredit = true;
                                else
                                    w.hasCredit = false;
                                w.invoice.InvType = _InvoiceType;
                                //w.invoice.TotalNet = decimal.Parse(tb_total.Text);
                                w.invoice.TotalNet = invoice.TotalNet;

                                w.ShowDialog();
                                Window.GetWindow(this).Opacity = 1;
                                multipleValid = w.isOk;
                                invoice.Remain = w.theRemine;
                                listPayments = w.listPayments;
                                if (multipleValid)
                                {
                                    switch (invoice.InvType)
                                    {
                                        case "pbd":
                                            await addInvoice("pbw"); // pbw means waiting purchase bounce
                                            break;

                                        default:
                                            foreach (var item in listPayments)
                                            {
                                                item.TransType = "p"; //pull
                                                item.PosId = MainWindow.posLogin.PosId;
                                                if (invoice.SupplierId != null )
                                                    item.AgentId = (long)invoice.SupplierId;

                                                item.TransNum = "pv";
                                                item.Side = "v"; // vendor
                                                item.CreateUserId = MainWindow.userLogin.UserId;
                                            }
                                           // if ((long)cb_branch.SelectedValue == MainWindow.branchLogin.branchId) // reciept invoice directly
                                            {
                                                await addInvoice("p");
                                            }
                                            //else
                                            //{
                                            //    await addInvoice("pw");
                                            //}
                                            break;
                                    };
                                }

               
                            }
                        }

                    }
                    //else //box is closed
                    //{
                    //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trBoxIsClosed"), animation: ToasterAnimation.FadeIn);
                    //}
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

     
        private async void btn_newInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                if (invoiceDetailsList.Count > 0  && (_InvoiceType == "pd" || _InvoiceType == "pbd"))
                {
                    bool valid = validateItemUnits();
                    if (valid)
                    {
                        #region Accept
                        MainWindow.mainWindow.Opacity = 0.2;
                        wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                        w.contentText = AppSettings.resourcemanager.GetString("trSaveInvoiceNotification");
                        w.ShowDialog();
                        MainWindow.mainWindow.Opacity = 1;
                        #endregion
                        if (w.isOk)
                        {
                            await addInvoice(_InvoiceType);
                        }
                        clearInvoice();
                        _InvoiceType = "pd";
                    }
                    else if (invoiceDetailsList.Count == 0)
                    {
                        clearInvoice();
                        _InvoiceType = "pd";
                    }
                }
                else
                    clearInvoice();

                //setNotifications();

                if (sender != null)
                    HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                if (sender != null)
                    HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private bool validateItemUnits()
        {
            bool valid = true;
            for (int i = 0; i < invoiceDetailsList.Count; i++)
            {
                if (invoiceDetailsList[i].ItemUnitId == 0)
                {
                    valid = false;
                    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trItemWithNoUnit"), animation: ToasterAnimation.FadeIn);

                    return valid;
                }
            }
            return valid;
        }
        private bool validateInvoiceValues()
        {
            if (decimal.Parse(txt_total.Text) == 0)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorTotalIsZeroToolTip"), animation: ToasterAnimation.FadeIn);
                return false;
            }

            return true;
        }
        private async Task addInvoice(string invType)
        {
            InvoiceResult invoiceResult = new InvoiceResult();

            #region invoice object
            if ((invoice.InvType == "p" || invoice.InvType == "pw") && (invType == "pbw" || invType == "pbd")) // invoice is purchase and will be bounce purchase  or purchase bounce draft , save another invoice in db
            {
                invoice.InvoiceMainId = invoice.InvoiceId;
                invoice.InvoiceId = 0;
                invoice.InvNumber = "pb";
                invoice.BranchCreatorId = MainWindow.branchLogin.BranchId;
                invoice.PosId = MainWindow.posLogin.PosId;
            }
            else if (invoice.InvType == "po")
            {
                invoice.InvNumber = "pi";
            }
            else if (invType == "pd" && invoice.InvoiceId == 0)
                invoice.InvNumber = "pd";

            if (invoice.BranchCreatorId == 0 || invoice.BranchCreatorId == null)
            {
                invoice.BranchCreatorId = MainWindow.branchLogin.BranchId;
                invoice.PosId = MainWindow.posLogin.PosId;
            }

            if (invoice.InvType != "pw" || invoice.InvoiceId == 0)
            {
                invoice.InvType = invType;
                //if (!tb_discount.Text.Equals(""))
                //    invoice.discountValue = decimal.Parse(tb_discount.Text);

                //if (!tb_shippingCost.Text.Equals(""))
                //    invoice.shippingCost = decimal.Parse(tb_shippingCost.Text);

                //if (cb_typeDiscount.SelectedIndex != -1)
                //    invoice.discountType = cb_typeDiscount.SelectedValue.ToString();


                //invoice.total = _Sum;
                //invoice.totalNet = decimal.Parse(tb_total.Text);
                //if (cb_vendor.SelectedValue != null && cb_vendor.SelectedValue.ToString() != "0")
                //    invoice.agentId = (int)cb_vendor.SelectedValue;
                //invoice.DeservedDate = dp_desrvedDate.SelectedDate;
                //invoice.VendorInvNum = tb_invoiceNumber.Text;
                //invoice.VendorInvDate = dp_invoiceDate.SelectedDate;
                //invoice.Notes = tb_note.Text;
                //invoice.TaxType = "";
                //if (tb_taxValue.Text != "" && AppSettings.invoiceTax_bool == true)
                //{
                //    invoice.tax = decimal.Parse(tb_taxValue.Text);
                //    invoice.taxValue = _TaxValue;
                //}
                //else
                //{
                //    invoice.tax = 0;
                //    invoice.taxValue = 0;
                //}

                invoice.Paid = 0;
                invoice.Deserved = invoice.TotalNet;

                //invoice.BranchId = (int)cb_branch.SelectedValue;
                invoice.BranchId = invoice.BranchCreatorId;
               

                invoice.CreateUserId = MainWindow.userLogin.UserId;
                invoice.UpdateUserId = MainWindow.userLogin.UserId;
 
                if (invType == "pw" || invType == "p")
                    invoice.InvNumber = "pi";

                invoice.ListPayments = listPayments;
                invoice.InvoiceItems = invoiceDetailsList;
                #endregion
            
                // save invoice in DB
                switch (invType)
                {
                    case "pbw":
                        #region notification Object
                        Notification not = new Notification()
                        {
                            Title = "trPurchaseReturnInvoiceAlertTilte",
                            Ncontent = "trPurchaseReturnInvoiceAlertContent",
                            MsgType = "alert",
                            ObjectName = "storageAlerts_ctreatePurchaseReturnInvoice",
                            BranchId = (int)invoice.BranchCreatorId,
                            Prefix = MainWindow.branchLogin.Name,
                            CreateUserId = MainWindow.userLogin.UserId,
                            UpdateUserId = MainWindow.userLogin.UserId,
                        };
                        #endregion
                        #region posCash posCash with type inv
                        var cashT = invoice.posCashTransfer(invoice, "pb");
                        #endregion
                        invoiceResult = await invoiceModel.savePurchaseBounce(invoice,  cashT, not, MainWindow.posLogin.PosId, MainWindow.branchLogin.BranchId);
                        break;
                    case "p":
                    case "pw":
                        #region notification Object
                        Notification amountNot = new Notification()
                        {
                            Title = "trExceedMaxLimitAlertTilte",
                            Ncontent = "trExceedMaxLimitAlertContent",
                            MsgType = "alert",
                            ObjectName = "storageAlerts_minMaxItem",
                            BranchId = MainWindow.branchLogin.BranchId,
                            CreateUserId = MainWindow.userLogin.UserId,
                            UpdateUserId = MainWindow.userLogin.UserId,
                        };
                        #endregion

                        #region purchase wait alert
                        //int branchId = 0;
                        //if ((int)cb_branch.SelectedValue != MainWindow.branchID)
                        //    branchId = (int)cb_branch.SelectedValue;
                        //Notification waitNot = new Notification()
                        //{
                        //    title = "trPurchaseInvoiceAlertTilte",
                        //    ncontent = "trPurchaseInvoiceAlertContent",
                        //    msgType = "alert",
                        //    objectName = "storageAlerts_ctreatePurchaseInvoice",
                        //    Prefix = MainWindow.loginBranch.name,
                        //    branchId = branchId,
                        //    createUserId = MainWindow.userID.Value,
                        //    updateUserId = MainWindow.userID.Value,
                        //};
                        #endregion

                        #region posCash
                        CashTransfer posCashTransfer = invoice.posCashTransfer(invoice, "pi");
                        #endregion
                        invoiceResult = await invoiceModel.savePurchaseInvoice(invoice, amountNot,  posCashTransfer, MainWindow.posLogin.PosId);

                        break;

                    default:
                        invoiceResult = await invoiceModel.savePurchaseDraft(invoice,  MainWindow.posLogin.PosId);
                        break;
                };

                if (invoiceResult.Result.Equals("lowBalance"))// رصيد pos غير كاف
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                //else if (invoiceResult.Result == -10) // كمية الخصائص المرجعة غير كافية
                //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("PropertiesNotAvailable"), animation: ToasterAnimation.FadeIn);
                else if (invoiceResult.Result.Equals("lowReturnQty"))// الكمية المرجعة أكبر من الكمية المشتراة
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountIncreaseToolTip"), animation: ToasterAnimation.FadeIn);
                else if (invoiceResult.Result.Equals("lowQty"))// الكمية في المخزن غير كافية في حالة الإرجاع
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorAmountNotAvailableFromToolTip") + " " + invoiceResult.Message, animation: ToasterAnimation.FadeIn);
                //else if (invoiceResult.Result == -4) // رصيد المورد غير كاف في حالة الإرجاع
                //    Toaster.ShowWarning(Window.GetWindow(this), message: MainWindow.resourcemanager.GetString("trErrorMaxDeservedExceeded"), animation: ToasterAnimation.FadeIn);
                else if(invoiceResult.Result.Equals("failed"))
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                else // success
                {
                    // prInvoiceId = invoiceResult.Result;
                    invoice.InvoiceId = long.Parse( invoiceResult.Result);
                    invoice.InvNumber = invoiceResult.Message;
                    invoice.UpdateDate = invoiceResult.UpdateDate;
                    TimeSpan ts;
                    TimeSpan.TryParse(invoiceResult.InvTime, out ts);
                    invoice.InvTime = ts;

                    AppSettings.PurchaseDraftCount = invoiceResult.PurchaseDraftCount;
                    // AppSettings.PosBalance = invoiceResult.PosBalance;
                    MainWindow.posLogin.Balance = invoiceResult.PosBalance;
                    MainWindow.setBalance();

                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                    #region print
                    ///////////////////////////////////////

                    if (invoice.InvType == "pw" || invoice.InvType == "p")
                    {
                        // if (AppSettings.print_on_save_pur == "1")
                        {
                            Thread t1 = new Thread(async () =>
                            {
                                string msg = "";
                                List<PayedInvclass> payedlist = new List<PayedInvclass>();
                                payedlist = await cashTransfer.PayedBycashlist(listPayments);
                                msg = await printPurInvoice(invoice, invoice.InvoiceItems, payedlist);
                                if (msg == "")
                                {

                                }
                                else
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(msg), animation: ToasterAnimation.FadeIn);
                                    });
                                }
                            });
                            t1.Start();
                        }
                        // if (AppSettings.email_on_save_pur == "1")
                        //{
                        //    Thread t2 = new Thread(async () =>
                        //    {
                        //        string msg = "";
                        //        List<PayedInvclass> payedlist = new List<PayedInvclass>();
                        //        payedlist = await cashTransfer.PayedBycashlist(listPayments);
                        //        if (invoice.InvDate == null)
                        //        {
                        //            invoice.InvDate = DateTime.Now;
                        //        }
                        //        msg = await sendPurEmail(invoice, invoice.InvoiceItems, payedlist);
                        //        this.Dispatcher.Invoke(() =>
                        //        {

                        //            if (msg == "")
                        //            {
                        //                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailNotSent"), animation: ToasterAnimation.FadeIn);
                        //            }
                        //            else if (msg == "trTheCustomerHasNoEmail")
                        //            {

                        //            }
                        //            else if (msg == "trMailSent")
                        //            {
                        //                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMailSent"), animation: ToasterAnimation.FadeIn);

                        //            }
                        //            else
                        //            {
                        //                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString(msg), animation: ToasterAnimation.FadeIn);

                        //            }
                        //        });
                        //    });
                        //    t2.Start();
                        //}

                    }
                    #endregion

                    //MainWindow.InvoiceGlobalItemUnitsList = await itemUnitModel.Getall();

                    clearInvoice();
                    // setNotifications();

                }
             
            }

        }
        #endregion

        #region Print
        LocalReport rep = new LocalReport();
        ReportCls reportclass = new ReportCls();
        public async Task<string> printPurInvoice(PurchaseInvoice prInvoice, List<PurInvoiceItem> invoiceItems, List<PayedInvclass> payedList)
        {
            string msg = "";
            try
            {
                if (prInvoice.InvoiceId > 0)
                {
                    if (prInvoice.InvType == "pd"  || prInvoice.InvType == "pbd")
                    {
                        msg = "trPrintDraftInvoice";
                    }
                    else
                    {
                        List<ReportParameter> paramarr = new List<ReportParameter>();


                        if (prInvoice.InvoiceId > 0)
                        {
                            //user
                            User employ = new User();

                            if (FillCombo.usersList is null)
                            { await FillCombo.RefreshUsers(); }
                            employ = FillCombo.usersList.Where(u => u.UserId == (long)prInvoice.UpdateUserId).FirstOrDefault();

                            prInvoice.UserName = employ.FirstName;
                            prInvoice.UserLastName = employ.LastName;
                            //supplier
                            if (prInvoice.SupplierId != null)
                            {
                                Supplier agentinv = new Supplier();
                                if (FillCombo.suppliersList is null)
                                { await FillCombo.RefreshSuppliers(); }
                                agentinv = FillCombo.suppliersList.Where(X => X.SupplierId == prInvoice.SupplierId).FirstOrDefault();

                                prInvoice.SupplierCode = agentinv.Code;
                                prInvoice.SupplierName = agentinv.Name;
                                prInvoice.SupplierCompany = agentinv.Company;
                                prInvoice.SupplierMobile = agentinv.Mobile;
                            }
                            else
                            {

                                prInvoice.SupplierCode = "-";
                                prInvoice.SupplierName = "-";
                                prInvoice.SupplierCompany = "-";
                                prInvoice.SupplierMobile = "-";
                            }

                            //branch
                            Branch branch = new Branch();
                            if (FillCombo.branchsList is null)
                            { await FillCombo.RefreshBranchs(); }
                            branch = FillCombo.branchsList.Where(b => b.BranchId == (int)prInvoice.BranchCreatorId).FirstOrDefault();

                            if (branch.BranchId > 0)
                            {
                                prInvoice.BranchCreatorName = branch.Name;
                            }
                            //branch reciver
                            if (prInvoice.BranchId != null)
                            {
                                if (prInvoice.BranchId > 0)
                                {
                                    if (FillCombo.branchsList is null)
                                    { await FillCombo.RefreshBranchs(); }
                                    branch = FillCombo.branchsList.Where(b => b.BranchId == (int)prInvoice.BranchId).FirstOrDefault();

                                    prInvoice.BranchName = branch.Name;
                                }
                                else
                                {
                                    prInvoice.BranchName = "-";
                                }

                            }
                            else
                            {
                                prInvoice.BranchName = "-";
                            }
                            // end branch reciever

                            ReportCls.checkInvLang();
                            foreach (var i in invoiceItems)
                            {
                                i.Price = decimal.Parse(HelpClass.DecTostring(i.Price));
                                i.Total = decimal.Parse(HelpClass.DecTostring(i.Total));
                            }
                            ReportSize repsize = new ReportSize();
                            int itemscount = 0;
                            ReportConfig.purchaseInvoiceReport(invoiceItems, rep, "");
                            itemscount = invoiceItems.Count();
                            //printer
                            ReportConfig clsrep = new ReportConfig();
                            ReportSize repsset = new ReportSize{
                                    paperSize = AppSettings.salePaperSize,
                                    printerName ="",};
                            repsize.paperSize = repsset.paperSize;
                            repsize = reportclass.GetpayInvoiceRdlcpath(prInvoice, itemscount, repsize.paperSize);
                            repsize.printerName = repsset.printerName;
                            //end 

                            string reppath = repsize.reppath;
                            rep.ReportPath = reppath;
                            ReportConfig.setInvoiceLanguage(paramarr);

                            ReportConfig.InvoiceHeader(paramarr);

                            multiplePaytable(paramarr, prInvoice);
                            paramarr = reportclass.fillPurInvReport(prInvoice, paramarr);

                            rep.SetParameters(paramarr);
                            rep.Refresh();


                            //copy count
                            if ( prInvoice.InvType == "p" || prInvoice.InvType == "pb" || prInvoice.InvType == "pw" || prInvoice.InvType == "pbw")
                            {

                                //paramarr.Add(new ReportParameter("isOrginal", prInvoice.isOrginal.ToString()));

                                rep.SetParameters(paramarr);
                                rep.Refresh();
                                if (repsize.printerName == "")
                                {
                                    if (AppSettings.sale_printer_name == "")
                                    {
                                        repsize.printerName = HelpClass.getdefaultPrinters();
                                    }
                                    else
                                    {
                                        repsize.printerName = AppSettings.sale_printer_name;
                                    }

                                }
                                // if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                                {
                                      
                                    if (repsize.paperSize == "A4")
                                    {
                                        LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, repsize.printerName, 1);
                                    }
                                    else
                                    {
                                        LocalReportExtensions.customPrintToPrinter(rep, repsize.printerName, 1, repsize.width, repsize.height);
                                    }

                                }

                            }
                            //else
                            //{


                            //    if (repsize.paperSize == "A4")
                            //    {

                            //        LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, repsize.printerName, short.Parse(AppSettings.pur_copy_count));

                            //    }
                            //    else
                            //    {
                            //        LocalReportExtensions.customPrintToPrinter(rep, repsize.printerName, short.Parse(AppSettings.pur_copy_count), repsize.width, repsize.height);

                            //    }

                            //}
                            // end copy count

                           
                        }
                    }

                    //
                }
            }
            catch
            {
                
                msg = "trNotCompleted";

            }
            return msg;
        }
        List<PayedInvclass> mailpayedList = new List<PayedInvclass>();
        public void multiplePaytable(List<ReportParameter> paramarr, PurchaseInvoice prInvoice)
        {

            CashTransfer cachModel = new CashTransfer();
            List<PayedInvclass> payedList = new List<PayedInvclass>();

            payedList = prInvoice.cachTrans;
            payedList = payedList == null ? new List<PayedInvclass>() : payedList;//
            mailpayedList = payedList;
            decimal sump = payedList.Sum(x => x.Cash);
            decimal deservd = (decimal)prInvoice.TotalNet - sump;
            prInvoice.Deserved = deservd;
            ReportConfig clsrep = new ReportConfig();
            List<PayedInvclass> repPayedList = clsrep.cashPayedinvoice(payedList, prInvoice);
            //convertter
            foreach (var p in payedList)
            {

                 p.Cash = decimal.Parse(HelpClass.DecTostring(p.Cash ));

            }
            paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

            paramarr.Add(new ReportParameter("sumP", HelpClass.DecTostring(sump)));
            paramarr.Add(new ReportParameter("deserved", HelpClass.DecTostring(deservd)));
            rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", repPayedList));


        }

        #endregion

        private void btn_supplier_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                HelpClass.StartAwait(MainWindow.mainWindow.grid_mainWindow);
                Window.GetWindow(this).Opacity = 0.2;
                wd_selectSupplier w = new wd_selectSupplier();
                w.supplierId = invoice.SupplierId;
                w.ShowDialog();
                if (w.isOk)
                {
                    invoice.SupplierId = w.supplierId;
                    invoice.SupplierName = w.supplierName;
                    //yasin
                    if (invoice.SupplierId != null)
                    {
                        ActiveButton(btn_supplier, true, invoice.SupplierName);
                    }
                    else
                    {
                        ActiveButton(btn_supplier,false, AppSettings.resourcemanager.GetString("trSupplier"));
                    }
                }
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btn_discount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(MainWindow.mainWindow.grid_mainWindow);
                Window.GetWindow(this).Opacity = 0.2;
                wd_selectDiscount w = new wd_selectDiscount();
                w.discountType = invoice.DiscountType;
                w.discountRate = invoice.DiscountPercentage;
                w.discountValue = invoice.DiscountValue;
                w.ShowDialog();
                if (w.isOk)
                {
                    invoice.DiscountValue = w.discountValue;
                    invoice.DiscountPercentage = w.discountRate;
                    invoice.DiscountType = w.discountType;

                    CalculateInvoiceValues();

                    //yasin
                    if (invoice.DiscountValue != 0)
                    {
                        ActiveButton(btn_discount, true);
                    }
                    else
                    {
                        ActiveButton(btn_discount, false);
                    }
                }
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btn_tax_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(MainWindow.mainWindow.grid_mainWindow);
                Window.GetWindow(this).Opacity = 0.2;
                wd_selectTax w = new wd_selectTax();
                w.taxType = invoice.TaxType;
                w.taxValue = invoice.Tax;
                w.taxRate = invoice.TaxPercentage;
                w.ShowDialog();
                if (w.isOk)
                {
                    invoice.TaxType = w.taxType;
                    invoice.Tax = w.taxValue;
                    invoice.TaxPercentage = w.taxRate;

                    CalculateInvoiceValues();

                    //yasin
                    if (invoice.Tax != 0)
                    {
                        ActiveButton(btn_tax, true);
                    }
                    else
                    {
                        ActiveButton(btn_tax, false);
                    }
                }
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        void ActiveButton(Button button,bool isActive, string text ="")
        {
            if(isActive)
            {
                button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                button.BorderBrush = Application.Current.Resources["Grey"] as SolidColorBrush;

                Path path = FindControls.FindVisualChildren<Path>(button).FirstOrDefault();
                if (path != null)
                    path.Fill = Application.Current.Resources["White"] as SolidColorBrush;

                TextBlock textBlock = FindControls.FindVisualChildren<TextBlock>(button).FirstOrDefault();
                if (textBlock != null)
                {
                    textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                    if (!string.IsNullOrWhiteSpace(text))
                        textBlock.Text = text;
                }


            }
            else
            {
                button.Background = Application.Current.Resources["White"] as SolidColorBrush;
                button.BorderBrush = Application.Current.Resources["MainColor"] as SolidColorBrush;

                Path path = FindControls.FindVisualChildren<Path>(button).FirstOrDefault();
                if (path != null)
                    path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;

                TextBlock textBlock = FindControls.FindVisualChildren<TextBlock>(button).FirstOrDefault();
                if (textBlock != null)
                {
                    textBlock.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    if (!string.IsNullOrWhiteSpace(text))
                        textBlock.Text = text;
                }
            }
        }
        private void btn_invoiceNumber_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(MainWindow.mainWindow.grid_mainWindow);
                Window.GetWindow(this).Opacity = 0.2;
                wd_purchaseInvoice w = new wd_purchaseInvoice();
                w.ShowDialog();
                if (w.isOk)
                {
                    invoice = w.purchaseInvoice;
                    invoice.Count = invoice.InvoiceItems.Sum(x => x.Quantity);
                    this.DataContext = invoice;
                    _InvoiceType = invoice.InvType;
                    invoiceDetailsList = invoice.InvoiceItems;

                    refreshInvoiceDetails();
                    inputEditable();
                }
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
