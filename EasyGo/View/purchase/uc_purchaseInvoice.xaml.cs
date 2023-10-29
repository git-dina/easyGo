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
