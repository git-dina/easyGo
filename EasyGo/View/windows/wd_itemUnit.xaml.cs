using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using netoaster;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace EasyGo.View.windows
{
    /// <summary>
    /// Interaction logic for wd_itemUnit.xaml
    /// </summary>
    public partial class wd_itemUnit : Window
    {
        public wd_itemUnit()
        {
            InitializeComponent();
        }
        public bool isOpend = false;
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public Item item;
        ItemUnit itemUnit = new ItemUnit();
        IEnumerable<ItemUnit> itemUnitsQuery;
        IEnumerable<ItemUnit> itemUnits;
        public List<Unit> units;
        string searchText = "";
        public static List<string> requiredControlList;

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "UnitId", "UnitValue", "SubUnitId", "Barcode","Price","PurchasePrice" ,"Cost"};

                translate();

                await FillCombo.FillUnitsWithDefault(cb_UnitId);
                await RefreshItemUnitsList();
                await Search();

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
            txt_title.Text = AppSettings.resourcemanager.GetString("trUnits");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");
            btn_add.Content = AppSettings.resourcemanager.GetString("trAdd");
            btn_update.Content = AppSettings.resourcemanager.GetString("trUpdate");
            btn_delete.Content = AppSettings.resourcemanager.GetString("trDelete");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            ///////////////////////////Barcode
            dg_itemUnit.Columns[0].Header = AppSettings.resourcemanager.GetString("trUnit");
            dg_itemUnit.Columns[1].Header = AppSettings.resourcemanager.GetString("trCountUnit");
            dg_itemUnit.Columns[2].Header = AppSettings.resourcemanager.GetString("trPurchasePrice");
            dg_itemUnit.Columns[3].Header = AppSettings.resourcemanager.GetString("trPrice");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_UnitId, AppSettings.resourcemanager.GetString("trSelectUnitHint"));
            txt_IsDefaultPurchase.Text = AppSettings.resourcemanager.GetString("trIsDefaultPurchases");
            txt_IsDefaultSale.Text = AppSettings.resourcemanager.GetString("trIsDefaultSales");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_UnitValue, AppSettings.resourcemanager.GetString("trCountHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_PurchasePrice, AppSettings.resourcemanager.GetString("trPurchasePriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_SubUnitId, AppSettings.resourcemanager.GetString("trUnitHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Price, AppSettings.resourcemanager.GetString("trPriceHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Barcode, AppSettings.resourcemanager.GetString("trBarcodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));
        }

        #region barcode
        static private int _InternalCounter = 0;
        private Boolean CheckBarcodeValidity(string barcode)
        {
            if (FillCombo.itemUnitList != null)
            {
                var exist = FillCombo.itemUnitList.Where(x => x.Barcode == barcode && x.ItemUnitId != itemUnit.ItemUnitId).FirstOrDefault();
                if (exist != null)
                    return false;
                else
                    return true;
            }
            return true;
        }
        private void Tb_Barcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                string barCode = tb_Barcode.Text;
                if (e.Key == Key.Return && barCode.Length == 13)
                {
                    if (isBarcodeCorrect(barCode) == false)
                    {
                        itemUnit.Barcode = "";
                        this.DataContext = itemUnit;
                    }
                    //else
                    //    drawBarcode(barCode);
                }
                //else if (barCode.Length == 13 || barCode.Length == 12)
                //    drawBarcode(barCode);
                //else
                //    drawBarcode("");
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private bool isBarcodeCorrect(string barCode)
        {
            char checkDigit;
            char[] barcodeData;

            char cd = barCode[0];
            barCode = barCode.Substring(1);
            barcodeData = barCode.ToCharArray();
            checkDigit = Mod10CheckDigit(barcodeData);

            if (checkDigit != cd)
            {
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trErrorBarcodeToolTip"), animation: ToasterAnimation.FadeIn);
                return false;
            }
            else
                return true;
        }
        public static char Mod10CheckDigit(char[] data)
        {
            // Start the checksum calculation from the right most position.
            int factor = 3;
            int weight = 0;
            int length = data.Length;

            for (int i = 0; i <= length - 1; i++)
            {
                weight += (data[i] - '0') * factor;
                factor = (factor == 3) ? 1 : 3;
            }

            return (char)(((10 - (weight % 10)) % 10) + '0');

        }
        //private void drawBarcode(string barcodeStr)
        //{

        //    // configur check sum metrics
        //    BarcodeSymbology s = BarcodeSymbology.CodeEan13;

        //    BarcodeDraw drawObject = BarcodeDrawFactory.GetSymbology(s);

        //    BarcodeMetrics barcodeMetrics = drawObject.GetDefaultMetrics(60);
        //    barcodeMetrics.Scale = 2;

        //    if (barcodeStr != "")
        //    {
        //        if (barcodeStr.Length == 13)
        //            barcodeStr = barcodeStr.Substring(1);//remove check sum from barcode string
        //        var barcodeImage = drawObject.Draw(barcodeStr, barcodeMetrics);

        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        //            byte[] imageBytes = ms.ToArray();

        //           // img_barcode.Source = ImageProcess.ByteToImage(imageBytes);
        //        }
        //    }
        //    //else
        //    //    img_barcode.Source = null;

        //}
        static public string generateRandomBarcode()
        {
            var now = DateTime.Now;

            var days = (int)(now - new DateTime(2000, 1, 1)).TotalDays;
            var seconds = (int)(now - DateTime.Today).TotalSeconds;

            var counter = _InternalCounter++ % 100;
            string randomBarcode = days.ToString("00000") + seconds.ToString("00000") + counter.ToString("00");
            char[] barcodeData = randomBarcode.ToCharArray();
            char checkDigit = Mod10CheckDigit(barcodeData);
            return checkDigit + randomBarcode;

        }
        private async void generateBarcode()
        {
            string barcodeString = "";
            barcodeString = generateRandomBarcode();
            if (FillCombo.itemUnitList != null)
            {
                if (!CheckBarcodeValidity(barcodeString))
                    barcodeString = generateRandomBarcode();
            }
            tb_Barcode.Text = barcodeString;
            HelpClass.validateEmpty("trErrorEmptyBarcodeToolTip", p_error_Barcode);
            //drawBarcode(barcodeString);
        }

        #endregion
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        {//add
            try
            {
                HelpClass.StartAwait(grid_main);

                itemUnit = new ItemUnit();
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (tb_Barcode.Text.Length == 12 || tb_Barcode.Text.Length == 13)
                    {
                        bool valid = ValidateValues();
                        if (valid == true)
                        {
                            // check barcode value if assigned to any item
                            if (!CheckBarcodeValidity(tb_Barcode.Text))
                            {
                                #region Tooltip_code
                                p_error_Barcode.Visibility = Visibility.Visible;
                                ToolTip toolTip_barcode = new ToolTip();
                                toolTip_barcode.Content = AppSettings.resourcemanager.GetString("trErrorDuplicateBarcodeToolTip");
                                toolTip_barcode.Style = Application.Current.Resources["ToolTipError"] as Style;
                                p_error_Barcode.ToolTip = toolTip_barcode;
                                #endregion
                            }
                            else //barcode is available
                            {

                                //unit
                                Nullable<int> unitId = null;
                                if (cb_UnitId.SelectedIndex != -1)
                                    unitId = (int)cb_UnitId.SelectedValue;

                                //count
                                int unitValue = int.Parse(tb_UnitValue.Text);
                                //smallUnitId
                                Nullable<int> smallUnitId = (int)cb_SubUnitId.SelectedValue;

                               
                                /////////////////////////////////////
                                itemUnit.ItemUnitId = 0;
                                itemUnit.ItemId = item.ItemId;
                                itemUnit.UnitId = unitId;
                                itemUnit.UnitValue = unitValue;
                                itemUnit.SubUnitId = smallUnitId;
                                itemUnit.IsDefaultPurchase = (bool)chb_IsDefaultPurchase.IsChecked ? true: false;
                                itemUnit.IsDefaultSale = (bool)chb_IsDefaultSale.IsChecked ? true : false;
                                itemUnit.PurchasePrice = decimal.Parse(tb_PurchasePrice.Text);
                                itemUnit.Price = decimal.Parse(tb_Price.Text);
                                itemUnit.Barcode = tb_Barcode.Text;
                                itemUnit.Notes = tb_Notes.Text;
                                itemUnit.CreateUserId = MainWindow.userLogin.UserId;
                                itemUnit.UpdateUserId = MainWindow.userLogin.UserId;

                                var res = await itemUnit.Save(itemUnit);
                                if (res.Equals("failed"))
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                   // await FillCombo.RefreshPurchaseItems();
                                    Clear();
                                    await RefreshItemUnitsList();
                                    await Search();
                                }
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopErrorBarcodeLength"), animation: ToasterAnimation.FadeIn);

                    HelpClass.EndAwait(grid_main);
                }
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private bool ValidateValues()
        {
            bool valid = true;
            char[] barcodeData;
            char checkDigit;
            if (tb_Barcode.Text.Length == 12)// generate checksum didit
            {
                barcodeData = tb_Barcode.Text.ToCharArray();
                checkDigit = Mod10CheckDigit(barcodeData);
                itemUnit.Barcode = checkDigit + tb_Barcode.Text;
                this.DataContext = itemUnit;
            }
            else if (tb_Barcode.Text.Length == 13)
            {
                valid = isBarcodeCorrect(tb_Barcode.Text);
               
            }
            if (tb_UnitValue.Text.Equals("0"))
            {
                valid = false;
                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trMustBeMoreThanZero"), animation: ToasterAnimation.FadeIn);
            }
            return valid;
        }

        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                HelpClass.StartAwait(grid_main);

                if (HelpClass.validate(requiredControlList, this))
                {
                    if (tb_Barcode.Text.Length == 12 || tb_Barcode.Text.Length == 13)
                    {
                        bool valid = ValidateValues();
                        if (valid == true)
                        {

                            // check barcode value if assigned to any item
                            if (!CheckBarcodeValidity(tb_Barcode.Text) && itemUnit.Barcode != tb_Barcode.Text)
                            {
                                #region Tooltip_code
                                p_error_Barcode.Visibility = Visibility.Visible;
                                ToolTip toolTip_barcode = new ToolTip();
                                toolTip_barcode.Content = AppSettings.resourcemanager.GetString("trErrorDuplicateBarcodeToolTip");
                                toolTip_barcode.Style = Application.Current.Resources["ToolTipError"] as Style;
                                p_error_Barcode.ToolTip = toolTip_barcode;
                                #endregion
                            }
                            else //barcode is available
                            {
                                //unit
                                Nullable<int> unitId = null;
                                if (cb_UnitId.SelectedIndex != -1)
                                    unitId = (int)cb_UnitId.SelectedValue;

                                //count
                                int unitValue = int.Parse(tb_UnitValue.Text);
                                //smallUnitId
                                Nullable<int> smallUnitId = (int)cb_SubUnitId.SelectedValue;

                                /////////////////////////////////////
                                itemUnit.ItemId = item.ItemId;
                                itemUnit.UnitId = unitId;
                                itemUnit.UnitValue = unitValue;
                                itemUnit.SubUnitId = smallUnitId;
                                itemUnit.IsDefaultPurchase = (bool)chb_IsDefaultPurchase.IsChecked ? true : false;
                                itemUnit.IsDefaultSale = (bool)chb_IsDefaultSale.IsChecked ? true : false;
                                itemUnit.PurchasePrice = decimal.Parse(tb_PurchasePrice.Text);
                                itemUnit.Price = decimal.Parse(tb_Price.Text);
                                itemUnit.Barcode = tb_Barcode.Text;
                                itemUnit.Notes = tb_Notes.Text;
                                itemUnit.UpdateUserId = MainWindow.userLogin.UserId;

                                var res = await itemUnit.Save(itemUnit);
                                if (res.Equals("failed"))
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                                else
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                    //await FillCombo.RefreshPurchaseItems();
                                    Clear();
                                    await RefreshItemUnitsList();
                                    await Search();
                                }
                            }
                        }
                    }
                    else
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopErrorBarcodeLength"), animation: ToasterAnimation.FadeIn);
                }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {//delete
                HelpClass.StartAwait(grid_main);

                if (itemUnit.ItemUnitId != 0)
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;
                    wd_acceptCancelPopup w = new wd_acceptCancelPopup();
                    w.contentText = AppSettings.resourcemanager.GetString("trMessageBoxDelete");

                    w.ShowDialog();
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
                    if (w.isOk)
                    {
                        var res = await FillCombo.itemUnit.Delete(itemUnit.ItemUnitId, MainWindow.userLogin.UserId);
                        if (res.Equals("failed"))
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                            await RefreshItemUnitsList();
                            await Search();
                            Clear();
                        }
                    }
                }
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
        #region events
        private async void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
      
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    
        private async void Dg_itemUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_itemUnit.SelectedIndex != -1)
                {
                    itemUnit = dg_itemUnit.SelectedItem as ItemUnit;
                    this.DataContext = itemUnit;
                }
                HelpClass.clearValidate(requiredControlList, this);
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                await RefreshItemUnitsList();
                await Search();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        
        #endregion
        #region Refresh & Search
        async Task Search()
        {
            //search
            if (itemUnits is null)
                await RefreshItemUnitsList();
            RefreshItemUnitsView();
        }
        async Task<IEnumerable<ItemUnit>> RefreshItemUnitsList()
        {
            if (FillCombo.itemUnitList is null)
                itemUnits = await FillCombo.RefreshItemUnit();
            var itemUnitLst = await FillCombo.itemUnit.GetItemUnit(item.ItemId);
            item.ItemUnits = itemUnitLst; //refresh item units
            //await FillCombo.RefreshItemUnit();
            //itemUnits = FillCombo.itemUnitList;
            //itemUnits = itemUnits.Where(x => x.ItemId == item.ItemId);
            var lstToReplace = FillCombo.itemUnitList.Where(x => x.ItemId == item.ItemId).ToList();
            foreach(var itemUnit in lstToReplace)
            {
                FillCombo.itemUnitList.Remove(itemUnit);
            }
            FillCombo.itemUnitList.AddRange(itemUnitLst) ;
            itemUnits = FillCombo.itemUnitList;
            return itemUnits;
        }
        void RefreshItemUnitsView()
        {
            itemUnitsQuery = itemUnits;
            dg_itemUnit.ItemsSource = itemUnitsQuery;
            dg_itemUnit.Items.Refresh();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            itemUnit = new ItemUnit();
            this.DataContext = itemUnit;
            dg_itemUnit.SelectedIndex = -1;
           // tb_UnitValue.Text = "";
            cb_SubUnitId.SelectedIndex = -1;
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
        #region report
        ReportCls reportclass = new ReportCls();
        LocalReport rep = new LocalReport();
        public void BuildReport()
        {

            List<ReportParameter> paramarr = new List<ReportParameter>();

            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\SectionData\Persons\Ar\ArCategorys.rdlc";
            }
            else
            {
                addpath = @"\Reports\SectionData\Persons\En\EnCategorys.rdlc";
            }
            string searchval = "";
            //filter   
            // paramarr.Add(new ReportParameter("stateval", stateval));
            // paramarr.Add(new ReportParameter("trActiveState", AppSettings.resourcemanagerreport.GetString("trState")));
            paramarr.Add(new ReportParameter("trSearch", AppSettings.resourcemanagerreport.GetString("trSearch")));
            searchval = tb_search.Text;
            paramarr.Add(new ReportParameter("searchVal", searchval));
            //end filter
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            //ReportConfig.CategoryReport(categorysQuery, rep, reppath, paramarr);
            ReportConfig.setReportLanguage(paramarr);
            ReportConfig.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();

        }
        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            //try
            {

                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();

                    saveFileDialog.Filter = "PDF|*.pdf;";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filepath = saveFileDialog.FileName;
                        LocalReportExtensions.ExportToPDF(rep, filepath);
                    }
                    #endregion
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            }
            //catch (Exception ex)
            //{

            //    HelpClass.EndAwait(grid_main);
            //    HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            //}
        }

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {

                    #region
                    BuildReport();
                    LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
                    #endregion
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_exportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                // if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    BuildReport();
                    this.Dispatcher.Invoke(() =>
                    {
                        saveFileDialog.Filter = "EXCEL|*.xls;";
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filepath = saveFileDialog.FileName;
                            LocalReportExtensions.ExportToExcel(rep, filepath);
                        }
                    });
                    #endregion
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);


                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);

                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_preview_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "report"))
                {
                    #region
                    Window.GetWindow(this).Opacity = 0.2;

                    string pdfpath = "";

                    pdfpath = @"\Thumb\report\temp.pdf";
                    pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                    BuildReport();

                    LocalReportExtensions.ExportToPDF(rep, pdfpath);
                    //wd_previewPdf w = new wd_previewPdf();
                    //w.pdfPath = pdfpath;
                    //if (!string.IsNullOrEmpty(w.pdfPath))
                    //{
                    //    w.ShowDialog();
                    //    w.wb_pdfWebViewer.Dispose();


                    //}
                    Window.GetWindow(this).Opacity = 1;
                    #endregion
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

        private void Cb_SubUnitId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_UnitId.SelectedIndex != -1 && cb_SubUnitId.SelectedIndex != -1)
                {
                    if ((int)cb_UnitId.SelectedValue == (int)cb_SubUnitId.SelectedValue)
                        itemUnit.UnitValue = 1;
                        //tb_UnitValue.Text = "1";
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private async void Cb_UnitId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_UnitId.SelectedValue != null)
                {
                    await fillSmallUnits(item.ItemId, (int)cb_UnitId.SelectedValue);
                    cb_SubUnitId.SelectedValue = itemUnit.SubUnitId;
                    if (itemUnit.ItemUnitId == 0)
                        generateBarcode();

                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async Task fillSmallUnits(long itemId, int unitId)
        {
            List<Unit> units = await FillCombo.unit.getSmallUnits(itemId, unitId);

            if (itemUnits.Count().Equals(0)
                || (itemUnit != null && itemUnit.ItemUnitId != 0 && itemUnit.UnitId.Equals(itemUnit.SubUnitId)))
            {
                cb_SubUnitId.ItemsSource = units.Where(x => x.UnitId == unitId).ToList();
            }
            else
            {
                if (itemUnit != null && itemUnit.ItemUnitId != 0)
                    units.Remove(units.Where(x => x.UnitId == itemUnit.UnitId).FirstOrDefault());

                cb_SubUnitId.ItemsSource = units.Where(x => x.Name != "package"
                                                        && itemUnits.Select(s => s.UnitId).ToList().Contains(x.UnitId)).ToList();

            }
            cb_SubUnitId.SelectedValuePath = "UnitId";
            cb_SubUnitId.DisplayMemberPath = "Name";

        }

        private void Tb_UnitValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (cb_UnitId.SelectedIndex != -1 && cb_SubUnitId.SelectedIndex != -1 && (int)cb_UnitId.SelectedValue == (int)cb_SubUnitId.SelectedValue)
                {
                    itemUnit.UnitValue = 1;
                }
            }
            catch
            {

            }
        }
    }
}
