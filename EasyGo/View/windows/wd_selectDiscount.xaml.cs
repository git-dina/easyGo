using EasyGo.Classes;
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
using System.Windows.Shapes;

namespace EasyGo.View.windows
{
    /// <summary>
    /// Interaction logic for wd_selectDiscount.xaml
    /// </summary>
     public partial class wd_selectDiscount : Window
    {
        public wd_selectDiscount()
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
            isOk = false;
            this.Close();
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_select_Click(btn_select, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public bool isOk { get; set; }
        public decimal discountValue { get; set; }
        public decimal discountRate { get; set; }
        public string discountType { get; set; }

        public static List<string> requiredControlList = new List<string>();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "discount" };


                translate();
                FillCombo.fillValueTypes(cb_discountType);
                setDiscountValue();
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
            txt_title.Text = AppSettings.resourcemanager.GetString("trDiscount");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_discountType, AppSettings.resourcemanager.GetString("trTypeHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_discount, AppSettings.resourcemanager.GetString("trDiscount"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
        }
        private void setDiscountValue()
        {
            cb_discountType.SelectedValue = discountType;

            tb_discount.Text = discountType == "rate" ? HelpClass.DecTostring(discountRate) : HelpClass.DecTostring(discountValue);
        }
        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (cb_discountType.SelectedValue.ToString().Equals("rate"))
                        discountRate = decimal.Parse(tb_discount.Text);
                    else
                        discountValue = decimal.Parse(tb_discount.Text);

                    discountType = cb_discountType.SelectedValue.ToString();
                    isOk = true;
                    this.Close();


                }
            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
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
