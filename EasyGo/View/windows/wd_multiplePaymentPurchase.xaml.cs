using EasyGo.Classes.ApiClasses;
using EasyGo.Classes;
using netoaster;
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
using System.Windows.Resources;
using System.Windows.Shapes;
using WPFTabTip;

namespace EasyGo.View.windows
{
    /// <summary>
    /// Interaction logic for wd_multiplePaymentPurchase.xaml
    /// </summary>
    public partial class wd_multiplePaymentPurchase : Window
    {
        public wd_multiplePaymentPurchase()
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
        static private object _Sender;
        public bool isOk { get; set; }
        public bool hasCredit { get; set; }
        public bool hasDeliveryCompany { get; set; }
        ImageBrush brush = new ImageBrush();
        static private string _SelectedPaymentType = "cash";
        static private string _SelectedPaymentTypeText = "Cash";
        static private int _SelectedCard = -1;
        static private Card selectedCard = new Card();
        public List<CashTransfer> listPayments = new List<CashTransfer>();
        CashTransfer cashTrasnfer;
        Card cardModel = new Card();
        IEnumerable<Card> cards;
        List<Ellipse> cardEllipseList = new List<Ellipse>();

        public PurchaseInvoice invoice = new PurchaseInvoice();
        public Supplier supplier = new Supplier();
        bool amountIsValid = false;

        public string windowOfSourceName = "";
        public decimal theRemine = 0;
        async Task refreshCards()
        {
            try
            {
                if (FillCombo.cardsList is null)
                    await FillCombo.RefreshCards();
                cards = FillCombo.cardsList;
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
                #region translate

                if (AppSettings.lang.Equals("en"))
                {
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                }
                else
                {
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                }

                translate();
                #endregion


                await refreshCards();
                configurProcessType();

                // get it from invoice
                loading_fillCardCombo();

                //////////////////////////
                //invoice.SupplierId
                //////////////////////////
                tb_moneySympol1.Text =
                    tb_moneySympol2.Text =
                    tb_moneySympol3.Text = AppSettings.Currency;

                invoice.Paid = 0;
                tb_cash.Text = tb_total.Text = invoice.TotalNet.ToString();

               
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
       
        private void translate()
        {

            txt_title.Text = AppSettings.resourcemanager.GetString("trMultiplePayment");
            txt_theRemineTitle.Text = AppSettings.resourcemanager.GetString("trTheRemine");
            txt_total.Text = AppSettings.resourcemanager.GetString("trTotal");
            txt_sumTitle.Text = AppSettings.resourcemanager.GetString("trCashPaid");

            chk_cash.Content = AppSettings.resourcemanager.GetString("trCash");
            chk_card.Content = AppSettings.resourcemanager.GetString("trAnotherPaymentMethods");
            //chk_admin.Content = AppSettings.resourcemanager.GetString("trAdministrative");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_cash, AppSettings.resourcemanager.GetString("trCash"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_processNum, AppSettings.resourcemanager.GetString("trProcessNumTooltip"));

            btn_save.Content = AppSettings.resourcemanager.GetString("trSave");
        }
        private void configurProcessType()
        {

            //chk_cash.Visibility = Visibility.Visible;
            //chk_card.Visibility = Visibility.Visible;
            chk_cash.IsChecked = true;
        }
        private void Btn_colse_Click(object sender, RoutedEventArgs e)
        {
            isOk = false;
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
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Return)
                {
                    Btn_save_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //decimal remain = getCusAvailableBlnc(agent);
                if (hasCredit == true || invoice.Paid >= invoice.TotalNet)
                {
                    if (listPayments.Where(x => x.ProcessType == "cash").Count() > 0 &&
                        listPayments.Where(x => x.ProcessType == "cash").FirstOrDefault().Cash > MainWindow.posLogin.Balance
                        && invoice.InvType != "pbd")
                    {
                        isOk = false;
                        Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopNotEnoughBalance"), animation: ToasterAnimation.FadeIn);
                    }
                    else
                    {
                        if (invoice.TotalNet - invoice.Paid > 0)
                        {
                            cashTrasnfer = new CashTransfer();

                            ///////////////////////////////////////////////////
                            cashTrasnfer.AgentId = invoice.SupplierId;
                            cashTrasnfer.InvId = invoice.InvoiceId;
                            cashTrasnfer.ProcessType = "balance";
                            cashTrasnfer.Cash = invoice.TotalNet.Value - invoice.Paid.Value;
                            cashTrasnfer.IsInvPurpose = true;
                            listPayments.Add(cashTrasnfer);
                        }
                        //lst_payments.Items.Add(s);
                        ///////////////////////////////////////////////////
                        isOk = true;
                        this.Close();
                    }
                }
                else
                {
                    isOk = false;
                    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trAmountPaidEqualInvoiceValue"), animation: ToasterAnimation.FadeIn);
                }

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void input_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                string name = sender.GetType().Name;
                if (name == "TextBox")
                {
                    if ((sender as TextBox).Name == "tb_processNum")
                        HelpClass.validateEmptyTextBox((TextBox)sender, p_errorProcessNum, tt_errorProcessNum, "trEmptyProcessNumToolTip");
                }
                if (name == "ComboBox")
                {

                    if ((sender as ComboBox).Name == "cb_paymentProcessType")
                        HelpClass.validateEmptyComboBox((ComboBox)sender, p_errorpaymentProcessType, tt_errorpaymentProcessType, "trErrorEmptyPaymentTypeToolTip");

                }
                if (name == "TextBlock")
                {

                    if ((sender as TextBlock).Name == "txt_card")
                        HelpClass.validateEmptyTextBlock((TextBlock)sender, p_errorCard, tt_errorCard, "trSelectCreditCard");
                }
                */

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void paymentProcessType_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                if (checkBox.Name == "chk_cash")
                {
                    chk_card.IsChecked = false;
                    //chk_admin.IsChecked = false;
                }
                else if (checkBox.Name == "chk_card")
                {
                    chk_cash.IsChecked = false;
                    //chk_admin.IsChecked = false;
                }
                //else if (checkBox.Name == "chk_admin")
                //{
                //    chk_cash.IsChecked = false;
                //    chk_card.IsChecked = false;
                //}




                if (chk_cash.IsChecked.Value)
                {
                    //case 0://cash
                    _SelectedPaymentType = "cash";
                    _SelectedPaymentTypeText = chk_cash.Content.ToString();
                    gd_card.Visibility = Visibility.Collapsed;
                    tb_processNum.Clear();
                    _SelectedCard = -1;
                    txt_card.Text = "";
                    brd_processNum.Visibility = Visibility.Collapsed;
                }
                else if (chk_card.IsChecked.Value)
                {
                    //case 1://card
                    _SelectedPaymentType = "card";
                    _SelectedPaymentTypeText = chk_card.Content.ToString();
                    gd_card.Visibility = Visibility.Visible;
                }
                //else if (chk_admin.IsChecked.Value)
                //{
                //    //case 2://admin
                //    _SelectedPaymentType = "admin";
                //    _SelectedPaymentTypeText = chk_admin.Content.ToString();
                //    gd_card.Visibility = Visibility.Collapsed;
                //    tb_processNum.Clear();
                //    _SelectedCard = -1;
                //    txt_card.Text = "";
                //    brd_processNum.Visibility = Visibility.Collapsed;
                //}
                foreach (var el in cardEllipseList)
                {
                    el.Stroke = Application.Current.Resources["LightGrey"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void paymentProcessType_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox cb = sender as CheckBox;
                if (cb.IsFocused)
                {
                    if (cb.Name == "chk_cash")
                        chk_cash.IsChecked = true;
                    else if (cb.Name == "chk_card")
                        chk_card.IsChecked = true;
                    //else if (cb.Name == "chk_admin")
                    //    chk_admin.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        void InitializeCardsPic(IEnumerable<Card> cards)
        {
            #region cardImageLoad
            dkp_cards.Children.Clear();
            int userCount = 0;
            foreach (var item in cards)
            {
                #region Button
                Button button = new Button();
                //button.DataContext = item.name;
                button.DataContext = item;
                button.Tag = item.CardId;
                button.Padding = new Thickness(0, 0, 0, 0);
                button.Margin = new Thickness(2.5, 5, 2.5, 5);
                button.Background = null;
                button.BorderBrush = null;
                button.Height = 35;
                button.Width = 35;
                button.Click += card_Click;
                //Grid.SetColumn(button, 4);
                #region grid
                Grid grid = new Grid();
                #region 
                Ellipse ellipse = new Ellipse();
                //ellipse.Margin = new Thickness(-5, 0, -5, 0);
                ellipse.StrokeThickness = 1;
                ellipse.Stroke = Application.Current.Resources["LightGrey"] as SolidColorBrush;
                ellipse.Height = 35;
                ellipse.Width = 35;
                ellipse.FlowDirection = FlowDirection.LeftToRight;
                ellipse.ToolTip = item.Name;
                ellipse.Tag = item.CardId;
                //userImageLoad(ellipse, item.image);
                cardImageLoad(ellipse, item.Image, (DateTime)item.UpdateDate);
                Grid.SetColumn(ellipse, userCount);
                grid.Children.Add(ellipse);
                cardEllipseList.Add(ellipse);
                #endregion
                #endregion
                button.Content = grid;
                #endregion
                dkp_cards.Children.Add(button);

            }
            #endregion
        }
        void card_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var button = sender as Button;
                _SelectedCard = int.Parse(button.Tag.ToString());
                //txt_card.Text = button.DataContext.ToString();
                Card card = button.DataContext as Card;
                txt_card.Text = card.Name;
                if (card.HasProcessNum.Value)
                {
                    brd_processNum.Visibility = Visibility.Visible;
                    tb_processNum.Visibility = Visibility.Visible;
                }
                else
                {
                    brd_processNum.Visibility = Visibility.Collapsed;
                    tb_processNum.Visibility = Visibility.Collapsed;
                }
                //set border color
                foreach (var el in cardEllipseList)
                {
                    if ((int)el.Tag == (int)button.Tag)
                        el.Stroke = Application.Current.Resources["MainColor"] as SolidColorBrush;
                    else
                        el.Stroke = Application.Current.Resources["LightGrey"] as SolidColorBrush;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void loading_fillCardCombo()
        {
            try
            {
                InitializeCardsPic(cards);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }
        //ImageBrush brush = new ImageBrush();
        async void cardImageLoad(Ellipse ellipse, string image, DateTime updateDate)
        {
            try
            {
                if (!string.IsNullOrEmpty(image))
                {
                    // clearImg(ellipse);
                    bool isModified = HelpClass.chkImgChng(image, updateDate, Global.TMPCardsFolder);
                    if (!isModified)
                        HelpClass.ellipsLocalImg("Card", image, ellipse);
                    else
                    {
                        byte[] imageBuffer = await cardModel.downloadImage(image); // read this as BLOB from your DB
                        var bitmapImage = new BitmapImage();
                        using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();
                        }
                        ellipse.Fill = new ImageBrush(bitmapImage);
                    }
                }
                else
                {
                    clearImg(ellipse);
                }
            }
            catch
            {
                clearImg(ellipse);
            }
        }
        private void clearImg(Ellipse ellipse)
        {
            Uri resourceUri = new Uri("pic/no-image-icon-90x90.png", UriKind.Relative);
            StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);
            BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
            brush.ImageSource = temp;
            ellipse.Fill = brush;
        }
        private void PreventSpaces(object sender, KeyEventArgs e)
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
        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                    e.Handled = false;

                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Tb_EnglishDigit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only english and digits
            Regex regex = new Regex("^[a-zA-Z0-9. -_?]*$");
            if (!regex.IsMatch(e.Text))
                e.Handled = true;
        }
        private void Tb_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                _Sender = sender;
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
                var txb = sender as TextBox;
                if ((sender as TextBox).Name == "tb_cash")
                {
                    HelpClass.InputJustNumber(ref txb);
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Tb_validateEmptyLostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = sender.GetType().Name;
                validateEmpty(name, sender);
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void validateEmpty(string name, object sender)
        {
            amountIsValid = true;
            /*
            if (name == "TextBox")
            {
                if ((sender as TextBox).Name == "tb_cash")
                    amountIsValid = HelpClass.validateEmptyTextBox((TextBox)sender, p_errorCash, tt_errorCash, "trEmptyCashToolTip");
            }
            */
        }
        private void Tb_cash_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//only decimal
            var regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (regex.IsMatch(e.Text) && !(e.Text == "." && ((TextBox)sender).Text.Contains(e.Text)))
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void Btn_enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.clearValidate(p_error_cash);
                HelpClass.clearValidate(p_error_processNum);
                //tb_cash.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f8f8f8"));
                //p_error_cash.Visibility = Visibility.Collapsed;
                //listPayments
                string s = "";
                cashTrasnfer = new CashTransfer();
                try
                {
                    cashTrasnfer.Cash = decimal.Parse(tb_cash.Text);
                }
                catch
                {
                    cashTrasnfer.Cash = 0;
                }
                if (cashTrasnfer.Cash > 0)
                {
                    decimal totalCashList = (listPayments.Where(x => x.ProcessType == "cash").Count() > 0) ?
                        listPayments.Where(x => x.ProcessType == "cash").FirstOrDefault().Cash
                        : 0;



                    if (cashTrasnfer.Cash + invoice.Paid <= invoice.TotalNet
                        || !(
                        (_SelectedPaymentType.Equals("card")
                        //|| _SelectedPaymentType.Equals("admin")
                        )
                        && cashTrasnfer.Cash - (invoice.TotalNet - invoice.Paid) > totalCashList)
                        )
                    {
                        cashTrasnfer.AgentId = invoice.SupplierId;
                        cashTrasnfer.InvId = invoice.InvoiceId;
                        cashTrasnfer.ProcessType = _SelectedPaymentType;
                        if (_SelectedPaymentType.Equals("cash"))
                        {
                            //s = cb_paymentProcessType.Text + " : " + cashTrasnfer.cash;
                            s = validateDuplicate(cashTrasnfer.Cash);
                        }
                        else if (_SelectedPaymentType.Equals("card"))
                        {
                            if (_SelectedCard == -1)
                            {
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trSelectCreditCard"), animation: ToasterAnimation.FadeIn);
                                return;
                            }
                            else if (tb_processNum.Visibility == Visibility.Visible && string.IsNullOrEmpty(tb_processNum.Text))
                            {
                                HelpClass.SetValidate(p_error_processNum, "trIsRequired");
                                return;
                            }
                            else
                            {

                                cashTrasnfer.CardId = _SelectedCard;
                                cashTrasnfer.DocNum = tb_processNum.Text;
                                s = txt_card.Text + " : " + cashTrasnfer.Cash;
                            }
                        }
                        //else if (_SelectedPaymentType.Equals("admin"))
                        //{
                        //    s = validateDuplicate(cashTrasnfer.Cash);
                        //}

                        lst_payments.Items.Add(s);
                        cashTrasnfer.IsInvPurpose = true;
                        listPayments.Add(cashTrasnfer);
                        invoice.Paid += cashTrasnfer.Cash;

                        #region test if we have remain
                        if (invoice.Paid > invoice.TotalNet)
                        {

                            int index = 0;
                            foreach (var item in lst_payments.Items)
                            {
                                if (item.ToString().Contains(AppSettings.resourcemanager.GetString("trCash")))
                                {
                                    //List<string> str = item.ToString().Split(':').ToList<string>();
                                    //str[1] = str[1].Replace(" ", "");
                                    //dec += decimal.Parse(str[1]);
                                    //invoice.Paid -= decimal.Parse(str[1]);
                                    //hasDuplicate = true;
                                    break;
                                }
                                index++;
                            }
                            //if (hasDuplicate)
                            {

                                decimal difference = invoice.Paid.Value - invoice.TotalNet.Value;
                                listPayments[index].Cash -= difference;
                                lst_payments.Items[index] = AppSettings.resourcemanager.GetString("trCash") + " : " + listPayments[index].Cash;
                                invoice.Paid -= difference;
                                theRemine += difference;

                                if (listPayments[index].Cash == 0)
                                {
                                    listPayments.Remove(listPayments[index]);
                                    lst_payments.Items.Remove(lst_payments.Items[index]);
                                }
                            }
                        }

                        txt_sum.Text = (invoice.Paid + theRemine).ToString();
                        txt_theRemine.Text = theRemine.ToString();


                        if (invoice.Paid >= invoice.TotalNet)
                            txt_sum.Foreground = Application.Current.Resources["Green"] as SolidColorBrush;
                        else
                            txt_sum.Foreground = Application.Current.Resources["mediumRed"] as SolidColorBrush;

                        tb_cash.Text = (invoice.TotalNet - invoice.Paid).ToString();

                        #endregion
                    }
                    else
                    {
                        HelpClass.SetValidate(p_error_cash, "trAmountGreaterInvoiceValue");
                    }
                }
                else
                {
                    HelpClass.SetValidate(p_error_cash, "trZeroAmmount");
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        string validateDuplicate(decimal dec)
        {
            try
            {
                string s = "";
                //decimal dec = 0;
                //List<string> str1 = s.ToString().Split(':').ToList<string>();
                //str1[0] = str1[0].Replace(" ", "");
                //dec = Decimal.Parse(str1[0]);
                bool hasDuplicate = false;
                int index = 0;
                foreach (var item in lst_payments.Items)
                {
                    if (item.ToString().Contains(_SelectedPaymentTypeText))
                    {
                        List<string> str = item.ToString().Split(':').ToList<string>();
                        str[1] = str[1].Replace(" ", "");
                        dec += decimal.Parse(str[1]);
                        invoice.Paid -= decimal.Parse(str[1]);
                        hasDuplicate = true;
                        break;
                    }
                    index++;
                }
                if (hasDuplicate)
                {
                    listPayments.Remove(listPayments[index]);
                    lst_payments.Items.Remove(lst_payments.Items[index]);
                }

                cashTrasnfer.Cash = dec;
                s = _SelectedPaymentTypeText + " : " + cashTrasnfer.Cash;
                return s;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }
        private void Btn_clearSerial_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lst_payments.Items.Clear();
                listPayments.Clear();
                invoice.Paid = 0;
                tb_cash.Text =
                    txt_sum.Text =
                    txt_theRemine.Text = "0";

            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Lst_payments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (lst_payments.SelectedItem != null)
                {


                    List<string> str = lst_payments.SelectedItem.ToString().Split(':').ToList<string>();
                    str[1] = str[1].Replace(" ", "");
                    invoice.Paid -= decimal.Parse(str[1]);
                    txt_sum.Text = (invoice.Paid + theRemine).ToString();

                    listPayments.Remove(listPayments[lst_payments.SelectedIndex]);
                    lst_payments.Items.Remove(lst_payments.SelectedItem);

                    if (invoice.Paid >= invoice.TotalNet)
                        txt_sum.Foreground = Application.Current.Resources["mediumGreen"] as SolidColorBrush;
                    else
                        txt_sum.Foreground = Application.Current.Resources["mediumRed"] as SolidColorBrush;

                    tb_cash.Text = (invoice.TotalNet - invoice.Paid).ToString();

                    #region test remain
                    if (theRemine > 0)
                    {
                        try
                        {
                            string s = "";
                            cashTrasnfer = new CashTransfer();
                            try
                            {
                                cashTrasnfer.Cash = theRemine;
                            }
                            catch
                            {
                                cashTrasnfer.Cash = 0;
                            }
                            if (cashTrasnfer.Cash > 0)
                            {
                                decimal totalCashList = (listPayments.Where(x => x.ProcessType == "cash").Count() > 0) ?
                                    listPayments.Where(x => x.ProcessType == "cash").FirstOrDefault().Cash
                                    : 0;



                                //if (cashTrasnfer.cash + invoice.Paid <= invoice.TotalNet || !(cb_paymentProcessType.SelectedValue.ToString().Equals("card") && cashTrasnfer.cash > totalCashList))
                                {
                                    cashTrasnfer.AgentId = invoice.SupplierId;
                                    cashTrasnfer.InvId = invoice.InvoiceId;
                                    _SelectedPaymentType = "cash";
                                    cashTrasnfer.ProcessType = _SelectedPaymentType;
                                    //if (cb_paymentProcessType.SelectedValue.ToString().Equals("cash"))
                                    {
                                        s = validateDuplicate(cashTrasnfer.Cash);
                                    }


                                    lst_payments.Items.Add(s);
                                    cashTrasnfer.IsInvPurpose = true;
                                    listPayments.Add(cashTrasnfer);
                                    invoice.Paid += cashTrasnfer.Cash;
                                    theRemine = 0;

                                    #region test if we have remain
                                    if (invoice.Paid > invoice.TotalNet)
                                    {

                                        int index = 0;
                                        foreach (var item in lst_payments.Items)
                                        {
                                            if (item.ToString().Contains(AppSettings.resourcemanager.GetString("trCash")))
                                            {
                                                break;
                                            }
                                            index++;
                                        }
                                        {

                                            decimal difference = invoice.Paid.Value - invoice.TotalNet.Value;
                                            listPayments[index].Cash -= difference;
                                            lst_payments.Items[index] = AppSettings.resourcemanager.GetString("trCash") + " : " + listPayments[index].Cash;
                                            invoice.Paid -= difference;
                                            theRemine += difference;

                                            if (listPayments[index].Cash == 0)
                                            {
                                                listPayments.Remove(listPayments[index]);
                                                lst_payments.Items.Remove(lst_payments.Items[index]);
                                            }
                                        }
                                    }

                                    txt_sum.Text = (invoice.Paid + theRemine).ToString();
                                    txt_theRemine.Text = theRemine.ToString();


                                    if (invoice.Paid >= invoice.TotalNet)
                                        txt_sum.Foreground = Application.Current.Resources["Green"] as SolidColorBrush;
                                    else
                                        txt_sum.Foreground = Application.Current.Resources["mediumRed"] as SolidColorBrush;

                                    tb_cash.Text = (invoice.TotalNet - invoice.Paid).ToString();

                                    #endregion
                                }
                                //else
                                //{
                                //    HelpClass.SetValidate(p_error_cash, "trAmountGreaterInvoiceValue");
                                //}
                            }
                            else
                            {
                                HelpClass.SetValidate(p_error_cash, "trZeroAmmount");
                            }
                        }
                        catch (Exception ex)
                        {
                            HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
                        }
                    }
                    #endregion




                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        #region check balance
        #endregion

        private void btn_Keyboard_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (TabTip.Close())
                {
#pragma warning disable CS0436 // Type conflicts with imported type
                    TabTip.OpenUndockedAndStartPoolingForClosedEvent();
#pragma warning restore CS0436 // Type conflicts with imported type
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
    }
}
