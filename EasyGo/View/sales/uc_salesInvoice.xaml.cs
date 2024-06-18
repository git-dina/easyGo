using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using EasyGo.converters;
using EasyGo.Template;
using EasyGo.View.windows;
using MaterialDesignThemes.Wpf;
using netoaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace EasyGo.View.sales
{
    /// <summary>
    /// Interaction logic for uc_salesInvoice.xaml
    /// </summary>
     public partial class uc_salesInvoice : UserControl
    {
        public uc_salesInvoice()
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
        SalesInvoice invoice = new SalesInvoice();
        SalesInvoice invoiceModel = new SalesInvoice();
        ItemLocation itemLocation = new ItemLocation();
        CashTransfer cashTransfer = new CashTransfer();
        string _InvoiceType = "sd";
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

                Clear();


              

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
            
        }


        private void Clear()
        {
            invoice = new SalesInvoice();
            this.DataContext = invoice;
            invoiceDetailsList = new List<SalesInvoiceItem>();
            listPayments = new List<CashTransfer>();
            refreshInvoiceDetails();
            _InvoiceType = "sd";
            isFromReport = false;

            /*
            inputEditable();
            */
            btn_save.Content = AppSettings.resourcemanager.GetString("trBuy");

            ActiveButton(btn_supplier, false, AppSettings.resourcemanager.GetString("trCustomer"));
            ActiveButton(btn_discount, false);
            ActiveButton(btn_tax, false);
            // last 
            HelpClass.clearValidate(requiredControlList, this);
        }

       

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
                items = FillCombo.itemsHasUnitsList.ToList();
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
                MaterialDesignThemes.Wpf.ButtonAssist.SetCornerRadius(buttonMain, new CornerRadius(7));
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
                uc_itemCardWithPrice itemCard = new uc_itemCardWithPrice();
                itemCard.item = item;
                itemCard.Color = Application.Current.Resources["MainColor"] as SolidColorBrush;
                itemCard.Click += Btn_itemCards_Click;
                wp_itemsCard.Children.Add(itemCard);
                #endregion
            }
        }
        private void Btn_itemCards_Click(object sender, RoutedEventArgs e)
        {
            // dina
            Item _item = (sender as uc_itemCardWithPrice).item;
            MessageBox.Show($"Id: {_item.ItemId}, Name: {_item.Name}.");

            MessageBox.Show("add item to invoice details here");

            // this example 
            invoiceDetailsList.Add(new SalesInvoiceItem() { ItemName = "Item1", Quantity = 1, Total = 1972 });
            BuildInvoiceDetails(invoiceDetailsList);

        }


        #endregion

        #endregion




        #region invoiceDetails
        List<SalesInvoiceItem> invoiceDetailsList = new List<SalesInvoiceItem>();
        int _SequenceNum = 1;

        void BuildInvoiceDetails(List<SalesInvoiceItem> itemsList)
        {
            sv_invoiceDetails.Content = new Grid();
            if (itemsList.Count > 0)
            {
                #region Grid Container
                Grid gridContainer = new Grid();
                gridContainer.Margin = new Thickness(5);
                //int rowCount = billDetailsList.Count();
                int rowCount = itemsList.Count;
                RowDefinition[] rd = new RowDefinition[rowCount];
                for (int i = 0; i < rowCount; i++)
                {
                    rd[i] = new RowDefinition();
                    rd[i].Height = new GridLength(1, GridUnitType.Auto);
                }
                for (int i = 0; i < rowCount; i++)
                {
                    gridContainer.RowDefinitions.Add(rd[i]);
                }
                /////////////////////////////////////////////////////

                #endregion
                _SequenceNum = 1;
                foreach (var item in itemsList)
                {
                    var it = items.Where(x => x.ItemId == item.ItemId).FirstOrDefault();
                    item.index = _SequenceNum;
                    uc_itemSalesInvoice itemSalesInvoiceTemp = new uc_itemSalesInvoice();
                    itemSalesInvoiceTemp.salesInvoiceItem = item;
                    itemSalesInvoiceTemp.funcDelete = DeleteInvoiceDetails;

                    Grid.SetRow(itemSalesInvoiceTemp, item.index-1);
                    gridContainer.Children.Add(itemSalesInvoiceTemp);                  
                    

                    _SequenceNum++;
                }
                sv_invoiceDetails.Content = gridContainer;
            }
        }
        bool DeleteInvoiceDetails(SalesInvoiceItem _salesInvoiceItem)
        {
            invoiceDetailsList.Remove(_salesInvoiceItem);

            int counter = 1;
            foreach (var item in invoiceDetailsList)
            {
                item.index = counter;
                counter++;
            }
            BuildInvoiceDetails(invoiceDetailsList);
            return true;
        }
        
        #endregion

        #region Invoice Operations

        private void refreshInvoiceDetails()
        {
            /*
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
            */
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

        #region return
        private async void btn_returnInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(returnPermission, FillCombo.groupObjects, "one"))
                {
                    bool showReturn = false;
                    if (_InvoiceType == "p")
                    {
                        /*
                        invoice = await invoiceModel.GetInvoiceToReturn(invoice.InvoiceId);
                        if (invoice.InvoiceItems.Select(x => x.Quantity).Sum() > 0)
                            showReturn = true;
                        else
                        {
                            showReturn = false;
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAllQuantityReturned"), animation: ToasterAnimation.FadeIn);
                        }
                        */
                    }
                    else
                    {
                        await saveBeforeExit();
                        Window.GetWindow(this).Opacity = 0.2;
                        wd_selectInvoiceSaleByNumber w = new wd_selectInvoiceSaleByNumber();
                        //w.page = "purchase";
                        //w.userId = MainWindow.userLogin.UserId;
                        //w.invoiceType = "p";
                        w.ShowDialog();
                        if (w.isOk == true)
                        {

                            invoice = w.salesInvoice;
                            showReturn = true;

                        }

                        Window.GetWindow(this).Opacity = 1;
                    }
                    if (showReturn)
                    {
                        invoice.InvType = "pbd";
                        invoice.InvNumber = "#000000";
                        invoice.InvoiceMainId = invoice.InvoiceId;
                        invoice.InvoiceId = 0;

                        _InvoiceType = "pbd";

                        // isFromReport = true;

                        viewInvoice();
                        btn_save.Content = AppSettings.resourcemanager.GetString("trReturn");
                        //  setNotifications();

                    }
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
        #endregion
        #region Save
        List<CashTransfer> listPayments;
        private async void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                invoiceDetailsList[0].Quantity++;
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async Task saveBeforeExit()
        {
            /*
            if (invoiceDetailsList.Count > 0 && (_InvoiceType == "pd" || _InvoiceType == "pbd"))
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
                    Clear();
                    //_InvoiceType = "pd";
                }
                else if (invoiceDetailsList.Count == 0)
                {
                    Clear();
                    // _InvoiceType = "pd";
                }
            }
      */
        }
        private async void btn_newInvoice_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender != null)
                    HelpClass.StartAwait(grid_main);

                if (invoiceDetailsList.Count > 0 && (_InvoiceType == "pd" || _InvoiceType == "pbd"))
                {
                    await saveBeforeExit();
                }
                else
                    Clear();

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
        /*
        private async Task addInvoice(string invType)
        {
            InvoiceResult invoiceResult = new InvoiceResult();

            #region invoice object
            if ((invoice.InvType == "p" || invoice.InvType == "pw") && (invType == "pb" || invType == "pbw" || invType == "pbd")) // invoice is purchase and will be bounce purchase  or purchase bounce draft , save another invoice in db
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
                    //case "pbw":
                    case "pb":
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
                        #region posCash  with type inv
                        var cashT = invoice.posCashTransfer(invoice, "pb");
                        #endregion
                        invoiceResult = await invoiceModel.savePurchaseBounce(invoice, cashT, not, MainWindow.posLogin.PosId, MainWindow.branchLogin.BranchId);
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
                        //    title = "trSalesInvoiceAlertTilte",
                        //    ncontent = "trSalesInvoiceAlertContent",
                        //    msgType = "alert",
                        //    objectName = "storageAlerts_ctreateSalesInvoice",
                        //    Prefix = MainWindow.loginBranch.name,
                        //    branchId = branchId,
                        //    createUserId = MainWindow.userID.Value,
                        //    updateUserId = MainWindow.userID.Value,
                        //};
                        #endregion

                        #region posCash
                        CashTransfer posCashTransfer = invoice.posCashTransfer(invoice, "pi");
                        #endregion
                        invoiceResult = await invoiceModel.saveSalesInvoice(invoice, amountNot, posCashTransfer, MainWindow.posLogin.PosId);

                        break;

                    default:
                        invoiceResult = await invoiceModel.savePurchaseDraft(invoice, MainWindow.posLogin.PosId);
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
                else if (invoiceResult.Result.Equals("failed"))
                    Toaster.ShowError(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                else // success
                {
                    // prInvoiceId = invoiceResult.Result;
                    invoice.InvoiceId = long.Parse(invoiceResult.Result);
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

                    Clear();
                    // setNotifications();

                }

            }

        }
        */
        #endregion

        #region Print
        /*
        LocalReport rep = new LocalReport();
        ReportCls reportclass = new ReportCls();
        public async Task<string> printPurInvoice(SalesInvoice prInvoice, List<PurInvoiceItem> invoiceItems, List<PayedInvclass> payedList)
        {
            string msg = "";
            try
            {
                if (prInvoice.InvoiceId > 0)
                {
                    if (prInvoice.InvType == "pd" || prInvoice.InvType == "pbd")
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
                            ReportConfig.salesInvoiceReport(invoiceItems, rep, "");
                            itemscount = invoiceItems.Count();
                            //printer
                            ReportConfig clsrep = new ReportConfig();
                            ReportSize repsset = new ReportSize
                            {
                                paperSize = AppSettings.salePaperSize,
                                printerName = "",
                            };
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
                            if (prInvoice.InvType == "p" || prInvoice.InvType == "pb" || prInvoice.InvType == "pw" || prInvoice.InvType == "pbw")
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
        public void multiplePaytable(List<ReportParameter> paramarr, SalesInvoice prInvoice)
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

                p.Cash = decimal.Parse(HelpClass.DecTostring(p.Cash));

            }
            paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

            paramarr.Add(new ReportParameter("sumP", HelpClass.DecTostring(sump)));
            paramarr.Add(new ReportParameter("deserved", HelpClass.DecTostring(deservd)));
            rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", repPayedList));


        }
        */
        #endregion

        private void btn_discount_Click(object sender, RoutedEventArgs e)
        {
            /*
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
            */
        }

        private void btn_tax_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                HelpClass.StartAwait(MainWindow.mainWindow.grid_mainWindow);
                Window.GetWindow(this).Opacity = 0.2;
                wd_selectTax w = new wd_selectTax();
                w.taxType = invoice.TaxType;
                w.taxValue = invoice.Tax.Value;
                w.taxRate = invoice.TaxPercentage;
                w.ShowDialog();
                if (w.isOk)
                {
                    invoice.TaxType = w.taxType;
                    invoice.Tax = w.taxValue;
                    invoice.TaxPercentage = w.taxRate;

                    CalculateInvoiceValues();

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
            */
        }

        void ActiveButton(Button button, bool isActive, string text = "")
        {
            if (isActive)
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
                /*
                HelpClass.StartAwait(MainWindow.mainWindow.grid_mainWindow);
                Window.GetWindow(this).Opacity = 0.2;
                wd_salesInvoice w = new wd_salesInvoice();
                w.ShowDialog();
                if (w.isOk)
                {
                    invoice = w.salesInvoice;
                    _InvoiceType = invoice.InvType;
                    viewInvoice();

                }
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
                */
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(MainWindow.mainWindow.grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void viewInvoice()
        {
            /*
            invoice.Count = invoice.InvoiceItems.Sum(x => x.Quantity);
            this.DataContext = invoice;
            invoiceDetailsList = invoice.InvoiceItems;

            refreshInvoiceDetails();
            inputEditable();
            */
        }
        #region navigation
        private async void btn_next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                HelpClass.StartAwait(grid_main);

                invoice = await invoiceModel.GetNextInvoice(invoice.InvoiceId, invoice.InvType, MainWindow.userLogin.UserId, AppSettings.duration);
                viewInvoice();

                HelpClass.EndAwait(grid_main);
                */
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async void btn_previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*

                HelpClass.StartAwait(grid_main);

                invoice = await invoiceModel.GetPreviousInvoice(invoice.InvoiceId, invoice.InvType, MainWindow.userLogin.UserId, AppSettings.duration);
                viewInvoice();

                HelpClass.EndAwait(grid_main);
                */
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion


    }
}
