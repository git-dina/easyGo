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

namespace EasyGo.View.sectionData
{
    /// <summary>
    /// Interaction logic for uc_user.xaml
    /// </summary>
    public partial class uc_user : UserControl
    {
        public uc_user()
        {
            InitializeComponent();
        }

        User user = new User();
        IEnumerable<User> usersQuery;
        IEnumerable<User> users;
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
                requiredControlList = new List<string> { "FirstName", "LastName", "Mobile","UserName" ,"Password"};

                translate();

              
                Keyboard.Focus(tb_FirstName);
                await Search();


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

            txt_title.Text = AppSettings.resourcemanager.GetString("trUser");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_search, AppSettings.resourcemanager.GetString("trSearchHint"));
            txt_baseInformation.Text = AppSettings.resourcemanager.GetString("trBaseInformation");

            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_FirstName, AppSettings.resourcemanager.GetString("trFirstNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_LastName, AppSettings.resourcemanager.GetString("trLastNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Mobile, AppSettings.resourcemanager.GetString("trMobileHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Email, AppSettings.resourcemanager.GetString("trEmailHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Address, AppSettings.resourcemanager.GetString("trAdressHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_Notes, AppSettings.resourcemanager.GetString("trNoteHint"));
            


            txt_loginInformation.Text = AppSettings.resourcemanager.GetString("trLoginInformation");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(tb_UserName, AppSettings.resourcemanager.GetString("trUserNameHint"));
            MaterialDesignThemes.Wpf.HintAssist.SetHint(pb_Password, AppSettings.resourcemanager.GetString("trPasswordHint"));
            txt_addButton.Text = AppSettings.resourcemanager.GetString("trAdd");
            txt_updateButton.Text = AppSettings.resourcemanager.GetString("trUpdate");
            txt_deleteButton.Text = AppSettings.resourcemanager.GetString("trDelete");
            tt_add_Button.Content = AppSettings.resourcemanager.GetString("trAdd");
            tt_update_Button.Content = AppSettings.resourcemanager.GetString("trUpdate");
            tt_delete_Button.Content = AppSettings.resourcemanager.GetString("trDelete");

            dg_user.Columns[0].Header = AppSettings.resourcemanager.GetString("trName");
            dg_user.Columns[1].Header = AppSettings.resourcemanager.GetString("trMobile");
            dg_user.Columns[2].Header = AppSettings.resourcemanager.GetString("trAddress");
            dg_user.Columns[3].Header = AppSettings.resourcemanager.GetString("trNotes");
            btn_clear.ToolTip = AppSettings.resourcemanager.GetString("trClear");


            //txt_branchButton.Text = AppSettings.resourcemanager.GetString("trBranch");
            //txt_storesButton.Text = AppSettings.resourcemanager.GetString("trStore");
            //txt_sliceButton.Text = AppSettings.resourcemanager.GetString("prices");

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
                    bool passLength = false;
                    passLength = chkPasswordLength(pb_Password.Password);

                    user = new User();
                    if (HelpClass.validate(requiredControlList, this) &&  passLength && HelpClass.IsValidEmail(this))
                    {

                        user.UserName = tb_UserName.Text;
                        user.Password = Md5Encription.MD5Hash("Inc-m" + pb_Password.Password);
                        user.FirstName = tb_FirstName.Text;
                        user.LastName = tb_LastName.Text;
                        user.Mobile =tb_Mobile.Text; ;
                        user.Email = tb_Email.Text;
                        user.Address = tb_Address.Text;
                        user.Notes = tb_Notes.Text;
                        user.CreateUserId = MainWindow.userLogin.UserId;

                        //user.driverIsAvailable = 0;

                        //user.hasCommission = (bool)tgl_hasCommission.IsChecked;
                        //try { user.commissionValue = decimal.Parse(tb_commissionValue.Text); }
                        //catch { user.commissionValue = 0; }
                        //try { user.commissionRatio = decimal.Parse(tb_commissionRatio.Text); }
                        //catch { user.commissionRatio = 0; }

                        //if (FillCombo.groupObject.HasPermissionAction(permissionPermission, FillCombo.groupObjects, "one"))
                        //{

                        //    if (cb_groupId.SelectedValue != null && (long)cb_groupId.SelectedValue != 0)
                        //        user.groupId = (long)cb_groupId.SelectedValue;
                        //    else
                        //        user.groupId = null;
                        //}


                        var res = await user.Save(user);


                        if (res.Equals("failed"))
                            Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                        else if (res.Equals("dUserName")) //user name already exist
                            Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trUserNameAlreadyExist"), animation: ToasterAnimation.FadeIn);

                        else if (res.Equals("dFullName")) //full name already exist
                            Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFullNameAlreadyExist"), animation: ToasterAnimation.FadeIn);

                        else
                        {
                            Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopAdd"), animation: ToasterAnimation.FadeIn);

                            if (openFileDialog.FileName != "")
                            {
                                long userId = long.Parse(res);
                                string b = await user.uploadImage(imgFileName,
                                    Md5Encription.MD5Hash("Inc-m" + userId.ToString()), userId);
                                user.Image = b;

                            }

                            Clear();
                            await RefreshUsersList();
                            await Search();
                            FillCombo.usersList = users.ToList();
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
                if (user.UserId > 0)
                {
                    //if (FillCombo.groupObject.HasPermissionAction(basicsPermission, FillCombo.groupObjects, "update"))
                    {
                        if (HelpClass.validate(requiredControlList, this) && HelpClass.IsValidEmail(this))
                        {
                            //user.UserName = tb_UserName.Text;
                            //user.Password = Md5Encription.MD5Hash("Inc-m" + pb_Password.Password);
                            user.FirstName = tb_FirstName.Text;
                            user.LastName = tb_LastName.Text;
                            user.Mobile = tb_Mobile.Text; ;
                            user.Email = tb_Email.Text;
                            user.Address = tb_Address.Text;
                            user.Notes = tb_Notes.Text;
                            user.UpdateUserId = MainWindow.userLogin.UserId;
                            //user.driverIsAvailable = 0;

                            //user.hasCommission = (bool)tgl_hasCommission.IsChecked;
                            //try { user.commissionValue = decimal.Parse(tb_commissionValue.Text); }
                            //catch { user.commissionValue = 0; }
                            //try { user.commissionRatio = decimal.Parse(tb_commissionRatio.Text); }
                            //catch { user.commissionRatio = 0; }

                            //if (FillCombo.groupObject.HasPermissionAction(permissionPermission, FillCombo.groupObjects, "one"))
                            //{

                            //    if (cb_groupId.SelectedValue != null && (long)cb_groupId.SelectedValue != 0)
                            //        user.groupId = (long)cb_groupId.SelectedValue;
                            //    else
                            //        user.groupId = null;
                            //}


                            var res = await user.Save(user);


                            if (res.Equals("failed"))
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);

                            else if (res.Equals("dUserName")) //user name already exist
                                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trUserNameAlreadyExist"), animation: ToasterAnimation.FadeIn);

                            else if (res.Equals("dFullName")) //full name already exist
                                Toaster.ShowInfo(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trFullNameAlreadyExist"), animation: ToasterAnimation.FadeIn);

                           else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopUpdate"), animation: ToasterAnimation.FadeIn);
                                await Search();
                                FillCombo.usersList = users.ToList();
                                long userId = long.Parse(res);
                                if (MainWindow.userLogin.UserId == userId)
                                    MainWindow.userLogin = user;
                                if (openFileDialog.FileName != "")
                                {
                                    string b = await user.uploadImage(imgFileName, Md5Encription.MD5Hash("Inc-m" + userId.ToString()), userId);
                                    user.Image = b;
                                    if (!b.Equals(""))
                                    {
                                        await getImg();
                                    }
                                    else
                                    {
                                        HelpClass.clearImg(btn_image);
                                    }
                                }
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
                    if (user.UserId != 0)
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
                            var res = await user.Delete(user.UserId, MainWindow.userLogin.UserId);
                            if (res.Equals("failed"))
                                Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopError"), animation: ToasterAnimation.FadeIn);
                            else
                            {
                                Toaster.ShowSuccess(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPopDelete"), animation: ToasterAnimation.FadeIn);

                                await RefreshUsersList();
                                await Search();
                                Clear();
                                FillCombo.usersList = users.ToList();
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
        private async void Dg_user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);
                //selection
                if (dg_user.SelectedIndex != -1)
                {
                    user = dg_user.SelectedItem as User;
                    this.DataContext = user;
                    if (user != null)
                    {
                        #region image
                        bool isModified = HelpClass.chkImgChng(user.Image, (DateTime)user.UpdateDate, Global.TMPUsersFolder);
                        if (isModified)
                            getImg();
                        else
                            HelpClass.getLocalImg("User", user.Image, btn_image);
                        #endregion

                        inputEditable();
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

                tb_search.Text = "";
                searchText = "";
                await RefreshUsersList();
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
            if (users is null)
                await RefreshUsersList();
            searchText = tb_search.Text.ToLower();
            usersQuery = users.Where(s => s.FirstName.ToLower().Contains(searchText) );
            RefresUsersView();
 
        }
        async Task<IEnumerable<User>> RefreshUsersList()
        {
            await FillCombo.RefreshUsers();
            users = FillCombo.usersList.ToList();

            users = users.Where(x => x.IsAdmin != true);

            return users;
        }
        void RefresUsersView()
        {
            dg_user.ItemsSource = usersQuery;
            txt_count.Text = usersQuery.Count().ToString();
        }
        #endregion
        #region validate - clearValidate - textChange - lostFocus - . . . . 
        void Clear()
        {
            user = new User();
            this.DataContext = user;

            dg_user.SelectedIndex = -1;
            #region image
            HelpClass.clearImg(btn_image);
            #endregion
            inputEditable();
            // last 
            HelpClass.clearValidate(requiredControlList, this);
            p_error_Email.Visibility = Visibility.Collapsed;
          
        }
        private void inputEditable()
        {
            tb_UserName.IsEnabled = user.UserId.Equals(0) ? true : false;
            pb_Password.IsEnabled = user.UserId.Equals(0) ? true : false;

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

   
          #region Image
          string imgFileName = "pic/no-image-icon-125x125.png";

          OpenFileDialog openFileDialog = new OpenFileDialog();
          SaveFileDialog saveFileDialog = new SaveFileDialog();
         
          private async Task getImg()
          {
              try
              {
                  HelpClass.StartAwait(grid_image, "forImage");
                  if (string.IsNullOrEmpty(user.Image))
                  {
                      HelpClass.clearImg(btn_image);
                  }
                  else
                  {
                      byte[] imageBuffer = await user.DownloadImage(user.Image); // read this as BLOB from your DB

                      var bitmapImage = new BitmapImage();
                      if (imageBuffer != null)
                      {
                          using (var memoryStream = new MemoryStream(imageBuffer))
                          {
                              bitmapImage.BeginInit();
                              bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                              bitmapImage.StreamSource = memoryStream;
                              bitmapImage.EndInit();
                          }

                          btn_image.Background = new ImageBrush(bitmapImage);
                          // configure trmporary path
                          string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                          string tmpPath = System.IO.Path.Combine(dir, Global.TMPUsersFolder);
                          tmpPath = System.IO.Path.Combine(tmpPath, user.Image);
                          openFileDialog.FileName = tmpPath;
                      }
                      else
                          HelpClass.clearImg(btn_image);
                  }
                  HelpClass.EndAwait(grid_image, "forImage");
              }
              catch
              {
                  HelpClass.EndAwait(grid_image, "forImage");
              }
          }

        private void Btn_uploadPic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.StartAwait(grid_main);

                openFileDialog.Filter = "Images|*.png;*.jpg;*.bmp;*.jpeg;*.jfif";
                if (openFileDialog.ShowDialog() == true)
                {
                    HelpClass.imageBrush = new ImageBrush();
                    HelpClass.imageBrush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.Relative));
                    btn_image.Background = HelpClass.imageBrush;
                    imgFileName = openFileDialog.FileName;
                }
                else
                { }
                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_deletePic_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.clearImg(btn_image);
                openFileDialog.FileName = "";
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion




        #region Password
        private void ValidateEmpty_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                HelpClass.validate(requiredControlList, this);
                p_error_password.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void P_showPassword_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                tb_Password.Text = pb_Password.Password;
                tb_Password.Visibility = Visibility.Visible;
                pb_Password.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        private void P_showPassword_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                tb_Password.Visibility = Visibility.Collapsed;
                pb_Password.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            { HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name); }
        }
        private bool chkPasswordLength(string password)
        {
            bool isValid = true;

            if (password.Length < 6)
                isValid = false;

            if (!isValid)
            {
                p_error_password.Visibility = Visibility.Visible;
                #region Tooltip
                ToolTip toolTip = new ToolTip();
                toolTip.Content = AppSettings.resourcemanager.GetString("trErrorPasswordLengthToolTip");
                toolTip.Style = Application.Current.Resources["ToolTipError"] as Style;
                p_error_password.ToolTip = toolTip;
                #endregion
            }

            return isValid;
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
                addpath = @"\Reports\SectionData\Persons\Ar\ArUsers.rdlc";
            }
            else
            {
                addpath = @"\Reports\SectionData\Persons\En\EnUsers.rdlc";
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

            ReportConfig.UserReport(usersQuery, rep, reppath, paramarr);
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
        
        
    }
}
