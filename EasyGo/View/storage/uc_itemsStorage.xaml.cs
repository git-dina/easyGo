using EasyGo.Classes.ApiClasses;
using EasyGo.Classes;
using EasyGo.View.windows;
using Microsoft.Reporting.WinForms;
using netoaster;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
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
using Microsoft.Win32;

namespace EasyGo.View.storage
{
    /// <summary>
    /// Interaction logic for uc_itemsStorage.xaml
    /// </summary>
    public partial class uc_itemsStorage : UserControl
    {
        public uc_itemsStorage()
        {
            InitializeComponent();
        }

        ItemLocation itemLocation = new ItemLocation();
        IEnumerable<ItemLocation> itemLocationsQuery;
        IEnumerable<ItemLocation> itemLocations;

        string searchText = "";
        public static List<string> requiredControlList;
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
                //requiredControlList = new List<string> { "itemName", "quantity", "sectionId", "locationId" };
                requiredControlList = new List<string> { "" };
                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }
                translate();

                //await FillCombo.FillComboSections(cb_sectionId);

                btn_transfer.IsEnabled = false;

                Keyboard.Focus(tb_quantity);

                await refreshGrids();
                Search();
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

           
            ////////////////////////////////----invoice----/////////////////////////////////
            col_itemUnit.Header = AppSettings.resourcemanager.GetString("trItemUnit");
           // dg_itemsStorage.Columns[1].Header = AppSettings.resourcemanager.GetString("trSectionLocation");
           col_quantity.Header = AppSettings.resourcemanager.GetString("trQTR");
           col_startDate.Header = AppSettings.resourcemanager.GetString("trStartDate");
            col_endDate.Header = AppSettings.resourcemanager.GetString("trEndDate");
           col_notes.Header = AppSettings.resourcemanager.GetString("trNote");
            //dg_itemsStorage.Columns[6].Header = AppSettings.resourcemanager.GetString("trOrderNum");

            txt_Location.Text = AppSettings.resourcemanager.GetString("trLocationt");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_itemName, AppSettings.resourcemanager.GetString("trItemNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_quantity, AppSettings.resourcemanager.GetString("trQuantityHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_sectionId, AppSettings.resourcemanager.GetString("trSectionHint"));
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_locationId, AppSettings.resourcemanager.GetString("trLocationHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));

            //chk_stored.Content = AppSettings.resourcemanager.GetString("trStored");
            //chk_freezone.Content = AppSettings.resourcemanager.GetString("trFreeZone");
            //chk_kitchen.Content = AppSettings.resourcemanager.GetString("trKitchen");
            //btn_transfer.Content = AppSettings.resourcemanager.GetString("trTransfer");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_transfer_Click(object sender, RoutedEventArgs e)
        {
            // transfer
            try
            {
                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(transferPermission, FillCombo.groupObjects, "one"))
                //{
                /*
                    if (dg_itemsStorage.SelectedIndex != -1)
                    {
                        if (tb_quantity.Text != "" && int.Parse(tb_quantity.Text) == 0)
                            HelpClass.SetValidate(p_error_quantity, "trErrorQuantIsZeroToolTip");
                        else if (HelpClass.validate(requiredControlList, this))
                        {
                            long oldLocationId = (long)itemLocation.locationId;
                            long newLocationId = (long)cb_locationId.SelectedValue;
                            if (oldLocationId != newLocationId)
                            {
                                int quantity = int.Parse(tb_quantity.Text);
                                ItemLocation newLocation = new ItemLocation();
                                newLocation.itemUnitId = itemLocation.itemUnitId;
                                newLocation.invoiceId = itemLocation.invoiceId;
                                newLocation.locationId = newLocationId;
                                newLocation.quantity = quantity;
                                newLocation.startDate = dp_startDate.SelectedDate;
                                newLocation.endDate = dp_endDate.SelectedDate;
                                newLocation.notes = tb_notes.Text;
                                newLocation.updateUserId = MainWindow.userLogin.userId;
                                newLocation.createUserId = MainWindow.userLogin.userId;

                                int res = await itemLocation.trasnferItem(itemLocation.itemsLocId, newLocation);
                                if (res > 0)
                                {
                                    Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                                }
                                else
                                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                                // await RefreshItemLocationsList();
                                await refreshGrids();
                                Search();
                                Clear();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trTranseToSameLocation"), animation: ToasterAnimation.FadeIn);

                        }
                    }
                    */
                //}
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
        #endregion
        #region events
        int _int = 0;
        /*
        private async void Cb_sectionId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cb_sectionId.SelectedValue != null)
                {
                    if (int.TryParse(cb_sectionId.SelectedValue.ToString(), out _int))
                    {
                        await FillCombo.FillComboLocationsBySection(cb_locationId, (long)cb_sectionId.SelectedValue);
                        cb_locationId.SelectedValue = itemLocation.locationId;
                    }
                }
                else
                {
                    await FillCombo.FillComboLocationsBySection(cb_locationId, -1);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
      */
        /*
        private async void search_Checking(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_stored")
                    {
                        chk_freezone.IsChecked = false;
                        chk_kitchen.IsChecked = false;


                        sp_inputsData.IsEnabled = true;
                        btn_transfer.IsEnabled = true;
                    }
                    else if (cb.Name == "chk_freezone")
                    {
                        chk_stored.IsChecked = false;
                        chk_kitchen.IsChecked = false;


                        sp_inputsData.IsEnabled = true;
                        btn_transfer.IsEnabled = true;
                    }
                    else if (cb.Name == "chk_kitchen")
                    {
                        chk_stored.IsChecked = false;
                        chk_freezone.IsChecked = false;


                        sp_inputsData.IsEnabled = false;
                        btn_transfer.IsEnabled = false;
                    }

                }
                HelpClass.StartAwait(grid_main);
                // await RefreshItemLocationsList();
                Search();
                Clear();
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void chk_uncheck(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_stored")
                        chk_stored.IsChecked = true;
                    else if (cb.Name == "chk_freezone")
                        chk_freezone.IsChecked = true;
                    else if (cb.Name == "chk_kitchen")
                        chk_kitchen.IsChecked = true;
                    //else
                    //    chk_locked.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        */
        private void Tb_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                Search();
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
                Clear();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Dg_itemsStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_itemsStorage.SelectedIndex != -1)
                {
                    //Clear();
                    itemLocation = dg_itemsStorage.SelectedItem as ItemLocation;
                    this.DataContext = itemLocation;
                    if (itemLocation != null)
                    {
                        if (itemLocation.IsExpired)
                        {
                            gd_date.Visibility = Visibility.Visible;
                            requiredControlList = new List<string> { "itemName", "quantity", "sectionId", "locationId", "startDate", "endDate" };
                        }
                        else
                        {
                            gd_date.Visibility = Visibility.Collapsed;
                            requiredControlList = new List<string> { "itemName", "quantity", "sectionId", "locationId" };
                        }
                        btn_transfer.IsEnabled = true;

                    }
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
                await refreshGrids();
                Search();
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
        IEnumerable<ItemLocation> itemLocationList;
        IEnumerable<ItemLocation> itemLocationListQuery;
        void Search()
        {
            searchText = tb_search.Text.ToLower();
            //if (chk_stored.IsChecked == true)
            //{
            //    itemLocationList = itemLocations.ToList();
            //}
            //else if (chk_freezone.IsChecked == true)
            {
                itemLocationList = freeZoneItems.ToList();
            }
            //else if (chk_kitchen.IsChecked == true)
            //{
            //    itemLocationList = kitchenItems.ToList();
            //}
            Clear();

            if (itemLocationList != null)
            {
                itemLocationListQuery = itemLocationList.Where(s => (s.ItemName.ToLower().Contains(searchText) ||
                s.UnitName.ToLower().Contains(searchText) ||
                s.SectionName.ToLower().Contains(searchText) ||
                s.LocationName.ToLower().Contains(searchText)));

                dg_itemsStorage.ItemsSource = itemLocationListQuery;
                txt_count.Text = itemLocationListQuery.Count().ToString();
            }

        }

        private async Task refreshGrids()
        {
            //await refreshStoredItemsLocations();
            await refreshFreeZoneItemsLocations();
            //await refreshKitchenItemsLocations();

        }
        /*
        private async Task refreshStoredItemsLocations()
        {
            itemLocations = await itemLocation.get(MainWindow.branchLogin.branchId);
        }
        */
        List<ItemLocation> freeZoneItems = new List<ItemLocation>();
        private async Task refreshFreeZoneItemsLocations()
        {
            freeZoneItems = await itemLocation.GetFreeZoneItems(MainWindow.branchLogin.BranchId);
        }
        /*
        List<ItemLocation> kitchenItems = new List<ItemLocation>();
        private async Task refreshKitchenItemsLocations()
        {
            kitchenItems = await itemLocation.GetKitchenItems(MainWindow.branchLogin.branchId);
        }
        */
        #endregion
        #region validate - Clear - textChange - lostFocus - . . . . 
        void Clear()
        {
            this.DataContext = new ItemLocation();
            dg_itemsStorage.SelectedIndex = -1;
            // last 
            HelpClass.clearValidate(requiredControlList, this);
            btn_transfer.IsEnabled = false;
            //btn_locked.IsEnabled = false;
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
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private void BuildReport()
        {
            /*
            List<ReportParameter> paramarr = new List<ReportParameter>();
            string searchval = "";
            string stateval = "";
            string subtitle = "";
            string title = "";
            string addpath;
            bool isArabic = ReportCls.checkLang();
            if (isArabic)
            {
                addpath = @"\Reports\Storage\storageOperations\Ar\ArItemsStorage.rdlc";
            }
            else
            {
                addpath = @"\Reports\Storage\storageOperations\En\EnItemsStorage.rdlc";
            }
            //if ((bool)chk_freezone.IsChecked)
            //{
            //    subtitle = AppSettings.resourcemanagerreport.GetString("trFreeZone");
            //}
            //else
            //{
                subtitle = AppSettings.resourcemanagerreport.GetString("trStored");
            //}

            title = AppSettings.resourcemanagerreport.GetString("trLocations") + "/" + subtitle;
            paramarr.Add(new ReportParameter("trTitle", title));
            //filter   
            //stateval = tgl_isActive.IsChecked == true ? AppSettings.resourcemanagerreport.GetString("trActive_")
            //  : AppSettings.resourcemanagerreport.GetString("trNotActive");
            //paramarr.Add(new ReportParameter("stateval", stateval));
            //paramarr.Add(new ReportParameter("trActiveState", AppSettings.resourcemanagerreport.GetString("trState")));
            paramarr.Add(new ReportParameter("trSearch", AppSettings.resourcemanagerreport.GetString("trSearch")));
            searchval = tb_search.Text;
            paramarr.Add(new ReportParameter("searchVal", searchval));
            //end filter
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);
            //D:\myproj\posproject5\Restaurant\Restaurant\View\storage\storageOperations\uc_ItemsStorage.xaml.cs
            ReportCls.checkLang();

            clsReports.ItemsStorage(itemLocationListQuery, rep, reppath, paramarr);
            clsReports.setReportLanguage(paramarr);
            clsReports.Header(paramarr);

            rep.SetParameters(paramarr);

            rep.Refresh();
            */
        }
        private void PrintRep()
        {
            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rep, AppSettings.rep_printer_name, AppSettings.rep_print_count == null ? short.Parse("1") : short.Parse(AppSettings.rep_print_count));
            });
        }
        private void PdfRep()
        {

            BuildReport();

            this.Dispatcher.Invoke(() =>
            {
                saveFileDialog.Filter = "PDF|*.pdf;";

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filepath = saveFileDialog.FileName;
                    LocalReportExtensions.ExportToPDF(rep, filepath);
                }
            });

        }

        private void Btn_pdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                //{
                    if (itemLocationListQuery != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            PdfRep();
                        });
                        t1.Start();
                    }
                //}
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

        private void Btn_print_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_main);

                //if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                //{

                    if (itemLocationListQuery != null)
                    {
                        Thread t1 = new Thread(() =>
                        {
                            PrintRep();
                        });
                        t1.Start();
                    }

                //}
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

                //if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one") || HelpClass.isAdminPermision())
                //{
                    #region
                    if (itemLocationListQuery != null)
                    {
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
                    }
                    #endregion
                //}
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

                //if (FillCombo.groupObject.HasPermissionAction(reportsPermission, FillCombo.groupObjects, "one"))
                //{
                    #region
                    if (itemLocationListQuery != null)
                    {
                        Window.GetWindow(this).Opacity = 0.2;
                        /////////////////////
                        string pdfpath = "";
                        pdfpath = @"\Thumb\report\temp.pdf";
                        pdfpath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, pdfpath);

                        BuildReport();
                        LocalReportExtensions.ExportToPDF(rep, pdfpath);
                        ///////////////////
                        wd_previewPdf w = new wd_previewPdf();
                        w.pdfPath = pdfpath;
                        if (!string.IsNullOrEmpty(w.pdfPath))
                        {
                            w.ShowDialog();
                            w.wb_pdfWebViewer.Dispose();
                        }
                        Window.GetWindow(this).Opacity = 1;
                    }
                    //////////////////////////////////////
                    #endregion
                //}
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

        private async void Btn_saveDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);


                //if (MainWindow.groupObject.HasPermissionAction(transferPermission, FillCombo.groupObjects, "one"))
                //{
                    if (dg_itemsStorage.SelectedIndex != -1)
                    {
                        if (HelpClass.validateEmpty(dp_startDate.Text, p_error_startDate) && HelpClass.validateEmpty(dp_endDate.Text, p_error_endDate))
                        {

                            var startDate = dp_startDate.SelectedDate;
                            var endDate = dp_endDate.SelectedDate;
                            int res = (int)await itemLocation.changeUnitExpireDate(itemLocation.ItemLocId, (DateTime)startDate, (DateTime)endDate, MainWindow.userLogin.UserId);
                            if (res > 0)
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);
                                await refreshGrids();
                                Search();
                            }
                            else
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                        }
                    }
                //}

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
