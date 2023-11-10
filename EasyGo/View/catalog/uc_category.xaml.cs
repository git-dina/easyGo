using netoaster;
using EasyGo.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using EasyGo.Classes.ApiClasses;
using EasyGo.View.windows;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Windows.Documents;
using Microsoft.Reporting.WinForms;
using EasyGo.View.sectionData;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace EasyGo.View.catalog
{
    /// <summary>
    /// Interaction logic for uc_category.xaml
    /// </summary>
    public partial class uc_category : UserControl
    {
        public uc_category()
        {
            InitializeComponent();
        }

        Category category = new Category();
        IEnumerable<Category> categorysQuery;
        IEnumerable<Category> categorys;
        string searchText = "";
        public static List<string> requiredControlList;
        SaveFileDialog saveFileDialog = new SaveFileDialog();

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
                requiredControlList = new List<string> { "Name", "Code" };

                translate();


                Keyboard.Focus(tb_Name);
                await Search();
               // await FillCombo.FillCategoriesWithDefault(cb_ParentId);

               await Clear();
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

            txt_title.Text = AppSettings.resourcemanager.GetString("trCategory");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Name, AppSettings.resourcemanager.GetString("trNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Code, AppSettings.resourcemanager.GetString("trCodeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_ParentId, AppSettings.resourcemanager.GetString("trParentCategorieHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Details, AppSettings.resourcemanager.GetString("trDetailsHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));



            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");

            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");


            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");
            tt_report.Content = AppSettings.resourcemanager.GetString("trPdf");
            tt_print.Content = AppSettings.resourcemanager.GetString("trPrint");
            tt_excel.Content = AppSettings.resourcemanager.GetString("trExcel");
            tt_preview.Content = AppSettings.resourcemanager.GetString("trPreview");
            tt_count.Content = AppSettings.resourcemanager.GetString("trCount");
        }
        #region Add - Update - Delete - Search - Tgl - Clear - DG_SelectionChanged - refresh
        private async void Btn_add_Click(object sender, RoutedEventArgs e)
        { //add
            try
            {
                HelpClass.StartAwait(grid_main);
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "add"))
                {

                    //chk password length

                    category = new Category();
                    if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                    {

                        category.Name = tb_Name.Text;
                        category.Code = tb_Code.Text;
                        if(cb_ParentId.SelectedIndex > 0)
                            category.ParentId = (int) cb_ParentId.SelectedValue;
                        category.Details = tb_Details.Text;
                        category.Notes = tb_Notes.Text;


                        var res = await category.Save(category);


                        if (res.Equals("failed"))
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);



                            await FillCombo.RefreshCategoriesList();
                            await Search();
                            Clear();

                            FillCombo.categoriesList = categorys.ToList();
                        }

                    }
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
        private async void Btn_update_Click(object sender, RoutedEventArgs e)
        {//update
            try
            {
                HelpClass.StartAwait(grid_main);
                if (category.CategoryId > 0)
                {
                    //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {
                            category.Name = tb_Name.Text;
                            category.Code = tb_Code.Text;
                            if (cb_ParentId.SelectedIndex > 0)
                                category.ParentId = (int)cb_ParentId.SelectedValue; ;
                            category.Details = tb_Details.Text;
                            category.Notes = tb_Notes.Text;
                            category.UpdateUserId = MainWindow.userLogin.UserId;

                            var res = await category.Save(category);


                            if (res.Equals("failed"))
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                           else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await FillCombo.RefreshCategoriesList();
                                await Search();
                                FillCombo.categoriesList = categorys.ToList();


                            }
                        }
                    }
                    //else
                    //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);
                }
                else
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectItemFirst"), animation: ToasterAnimation.FadeIn);

                HelpClass.EndAwait(grid_main);
            
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void Btn_delete_Click(object sender, RoutedEventArgs e)
        {//delete
            try
            {
                //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "delete"))
                {

                    HelpClass.StartAwait(grid_main);
                    if (category.CategoryId != 0)
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
                            var res = await category.Delete(category.CategoryId, MainWindow.userLogin.UserId);
                            if (res.Equals("failed"))
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await FillCombo.RefreshCategoriesList();
                                await Search();
                                Clear();
                              //  FillCombo.categoriesList = categorys.ToList();
                            }
                        }

                    }
                    HelpClass.EndAwait(grid_main);
     
                }
                //else
                //    Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trdontHavePermission"), animation: ToasterAnimation.FadeIn);

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
        private async void Dg_category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_category.SelectedIndex != -1)
                {
                    category = dg_category.SelectedItem as Category;
                    this.DataContext = category;
                    if (category != null)
                    {

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
            */
        }
        private async void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {//refresh

                HelpClass.StartAwait(grid_main);
                tb_search.Text = "";
                searchText = "";
                await FillCombo.RefreshCategoriesList();
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
            if (FillCombo.categoriesList is null)
                await FillCombo.RefreshCategoriesList();

            categorys = FillCombo.categoriesList.ToList();
            searchText = tb_search.Text.ToLower();
            categorysQuery = categorys.Where(s => s.Name.ToLower().Contains(searchText));
            RefresCategorysView();

        }

        void RefresCategorysView()
        {
            //dg_category.ItemsSource = categorysQuery;

            tv_categorys.Items.Clear();
            if (categorysQuery.Where(x => x.ParentId is null).Count() > 0)
            {
                buildTreeViewList(categorysQuery.Where(x => x.ParentId is null).ToList(), tv_categorys);

            }
            else
            {
                buildTreeViewList(categorysQuery.ToList(), tv_categorys);
            }

            txt_count.Text = categorysQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        async Task Clear()
        {
            category = new Category();
            this.DataContext = category;
            await FillCombo.FillCategoriesWithDefault(cb_ParentId);
            /*
            dg_category.SelectedIndex = -1;
            */
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
        private decimal _commissionValue = 0;
        private void ValidateEmpty_TextChange(object sender, TextChangedEventArgs e)
        {
            try
            {
                var txb = sender as TextBox;
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
                addpath = @"\Reports\Catalog\Ar\ArCategory.rdlc";
            }
            else
            {
                addpath = @"\Reports\Catalog\En\EnCategory.rdlc";
            }
            string searchval = "";
            //filter   
            paramarr.Add(new ReportParameter("trSearch", AppSettings.resourcemanagerreport.GetString("trSearch")));
            searchval = tb_search.Text;
            paramarr.Add(new ReportParameter("searchVal", searchval));
            //end filter
            string reppath = reportclass.PathUp(Directory.GetCurrentDirectory(), 2, addpath);

            ReportConfig.CategoryReport(categorysQuery, rep, reppath, paramarr);
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
        #region TreeView

        void buildTreeViewList(List<Category> _categories, TreeView treeViewItemParent)
        {
            
            foreach (var item in _categories)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.DataContext = item;
                treeViewItem.Tag = item.CategoryId.ToString();
                treeViewItem.Header = item.Name;
                treeViewItem.FontSize = 16;
                treeViewItem.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush; 
                treeViewItem.Selected += TreeViewItem_Selected;

                treeViewItemParent.Items.Add(treeViewItem);
                if (categorysQuery.Where(x => x.ParentId == item.CategoryId).ToList().Count() > 0)
                {
                    buildTreeViewList(categorysQuery.Where(x => x.ParentId == item.CategoryId).ToList(), treeViewItem);
                }


            }
            
        }
        void buildTreeViewList(List<Category> _categories, TreeViewItem treeViewItemParent)
        {
            foreach (var item in _categories)
            {
                TreeViewItem treeViewItem = new TreeViewItem();
                treeViewItem.DataContext = item;
                treeViewItem.Tag = item.CategoryId.ToString();
                treeViewItem.Header = item.Name;
                treeViewItem.FontSize = 16;
                treeViewItem.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush; ;
                treeViewItem.Selected += TreeViewItem_Selected;

                treeViewItemParent.Items.Add(treeViewItem);
                if (categorysQuery.Where(x => x.ParentId == item.CategoryId).ToList().Count() > 0)
                {
                    buildTreeViewList(categorysQuery.Where(x => x.ParentId == item.CategoryId).ToList(), treeViewItem);
                }
            }
        }
        private async void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {

            TreeViewItem treeViewItem = sender as TreeViewItem;
            if (treeViewItem.IsSelected)
            {
                unExpandTreeViewItem();
                setSelectedStyleTreeViewItem();
                category = treeViewItem.DataContext as Category;
                this.DataContext = category;

                await FillCombo.FillCategoriesWithDefault(cb_ParentId, category.CategoryId);
                //await FillCombo.fillCategorysWithDefault(cb_CategoryParentId, category.CategoryId);
            }
            treeViewItem.IsExpanded = true;
        }
        void unExpandTreeViewItem()
        {
            List<TreeViewItem> list = FindControls.FindVisualChildren<TreeViewItem>(this).ToList();
            foreach (var item in list)
            {
                if (!item.IsSelected)
                    item.IsExpanded = false;
            }
        }
        void setSelectedStyleTreeViewItem()
        {
            List<TreeViewItem> list = FindControls.FindVisualChildren<TreeViewItem>(this).ToList();
            foreach (var item in list)
            {
                if (!item.IsSelected)
                {
                    item.Foreground = Application.Current.Resources["textColor"] as SolidColorBrush;
                    item.FontWeight = FontWeights.Regular;
                }
                else
                {
                    item.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    item.FontWeight = FontWeights.SemiBold;
                }
            }
        }

        #endregion

    }
}
