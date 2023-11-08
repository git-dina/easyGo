using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;
using System.Reflection;
using System.Resources;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using WPFTabTip;
using EasyGo.Classes;
using EasyGo.View.sectionData;
using EasyGo.Classes.ApiClasses;
using EasyGo.View.catalog;
using EasyGo.View.purchase;

namespace EasyGo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal static UserLog userLog;
        internal static User userLogin = new User();
        internal static Pos posLogin = new Pos();
        internal static Branch branchLogin = new Branch();

        public static Boolean go_out = false;

        public static DispatcherTimer timer;

        static public MainWindow mainWindow;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                mainWindow = this;
                windowFlowDirection();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name, false);
            }
        }
        void windowFlowDirection()
        {
            #region translate
            if (AppSettings.lang.Equals("en"))
            {
                AppSettings.resourcemanager = new ResourceManager("EasyGo.en_file", Assembly.GetExecutingAssembly());
                grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
            }
            else
            {
                AppSettings.resourcemanager = new ResourceManager("EasyGo.ar_file", Assembly.GetExecutingAssembly());

                grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
            }
            #endregion
        }
        bool firstLoad = true;
        public static List<string> menuList;

        public async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_mainWindow, "mainWindow_loaded");
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();

                menuList = new List<string> { "home", "catalog", "purchase",
                   "sectionData",""};

                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("EasyGo.en_file", Assembly.GetExecutingAssembly());
                    grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
                    //txt_lang.Text = "AR";

                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("EasyGo.ar_file", Assembly.GetExecutingAssembly());
                    grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
                    //txt_lang.Text = "EN";

                }
                translate();

                setHeaderValues();
               



                #region Permision
                permission();
                #endregion

                Btn_home_Click(btn_home, null);

                //SelectAllText
                EventManager.RegisterClassHandler(typeof(System.Windows.Controls.TextBox), System.Windows.Controls.TextBox.GotKeyboardFocusEvent, new RoutedEventHandler(SelectAllText));
                //txt_rightReserved.Text = DateTime.Now.Date.Year + " © All Right Reserved for ";
                firstLoad = false;
                HelpClass.EndAwait(grid_mainWindow, "mainWindow_loaded");
            }
            catch (Exception ex)
            {
                HelpClass.EndAwait(grid_mainWindow, "mainWindow_loaded");
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (timer != null)
                timer.Stop();
        }

        void SelectAllText(object sender, RoutedEventArgs e)
        {
            try
            {
                var textBox = sender as System.Windows.Controls.TextBox;
                if (textBox != null)
                    if (!textBox.IsReadOnly)
                        textBox.SelectAll();
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void setHeaderValues()
        {
            //txt_posNameTitle.Text = Translate.getResource("746");


            //txt_userName.Text = AppSettings.userName;
            //txt_posName.Text = GeneralInfoService.cashBoxes.Where(x => x.BoxId == AppSettings.cashBoxId).Select(x => x.Name).FirstOrDefault();
            //txt_licenseNumber.Text = GeneralInfoService.GeneralInfo.MainOp.MySno;
        }
        void permission()
        {
            /*
            bool loadWindow = false;
            //loadWindow = loadingDefaultPath(AppSettings.defaultPath);
            if (!HelpClass.isAdminPermision())
                foreach (Button button in FindControls.FindVisualChildren<Button>(this))
                {
                    if (button.Tag != null)
                        if (FillCombo.groupObject.HasPermission(button.Tag.ToString(), FillCombo.groupObjects))
                        {
                            button.Visibility = Visibility.Visible;
                            if (!loadWindow)
                            {
                                button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                loadWindow = true;
                            }
                        }
                        else button.Visibility = Visibility.Collapsed;
                }
            else
            if (!loadWindow)
                Btn_home_Click(btn_home, null);
            */
        }
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {

                txtTime.Text = DateTime.Now.ToShortTimeString();
                txtDate.Text = DateTime.Now.ToShortDateString();


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private async void BTN_Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HelpClass.StartAwait(grid_mainWindow);

                {
                    //await close();

                    //HelpClass.deleteDirectoryFiles(Global.TMPFolder);

                    Application.Current.Shutdown();
                }


                HelpClass.EndAwait(grid_mainWindow);
            }
            catch (Exception ex)
            {

                HelpClass.EndAwait(grid_mainWindow);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void BTN_Minimize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Minimized;
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /*
        void colorTextRefreash(TextBlock txt)
        {
            txt_home.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_catalog.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_storage.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_purchases.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_sales.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_kitchen.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_delivery.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_accounts.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_reports.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_sectiondata.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            txt_settings.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));

            txt.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        }
        void fn_ColorIconRefreash(Path p)
        {
            path_iconSettings.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconSectionData.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconReports.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconAccounts.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconSales.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconKitchen.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconDelivery.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconPurchases.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconStorage.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconCatalog.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));
            path_iconHome.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FEDFB7"));

            p.Fill = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFFFF"));
        }
        */
        public void translate()
        {
           

        }
        /*
        private void Btn_home_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var control in FindControls.FindVisualChildren<Expander>(this))
                {

                    var expander = control as Expander;
                    if (expander.Tag != null)
                        expander.IsExpanded = false;
                }
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        */
        private void Btn_userImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.GetWindow(this).Opacity = 0.2;
                //wd_userInfo w = new wd_userInfo();
                //w.ShowDialog();
                Window.GetWindow(this).Opacity = 1;
            }
            catch (Exception ex)
            {
                Window.GetWindow(this).Opacity = 1;
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }



        /*
         private void Btn_vendorsData_Click(object sender, RoutedEventArgs e)
         {

             try
             {
                 grid_main.Children.Clear();
                 grid_main.Children.Add(uc_vendorsData.Instance);
                 Button button = sender as Button;
                 secondMenuTitleActivate(button.Tag.ToString());
             }
             catch (Exception ex)
             {
                 HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
             }


         }
         */

        private void Btn_lang_Click(object sender, RoutedEventArgs e)
        {
            if (AppSettings.lang.Equals("en"))
                AppSettings.lang = "ar";
            else
                AppSettings.lang = "en";


            //update languge in main window
            MainWindow parentWindow = Window.GetWindow(this) as MainWindow;

            if (parentWindow != null)
            {
                //access property of the MainWindow class that exposes the access rights...
                if (AppSettings.lang.Equals("en"))
                {
                    AppSettings.resourcemanager = new ResourceManager("Hesabate_POS.en_file", Assembly.GetExecutingAssembly());
                    parentWindow.grid_mainWindow.FlowDirection = FlowDirection.LeftToRight;
                    //txt_lang.Text = "AR";

                }
                else
                {
                    AppSettings.resourcemanager = new ResourceManager("Hesabate_POS.ar_file", Assembly.GetExecutingAssembly());
                    parentWindow.grid_mainWindow.FlowDirection = FlowDirection.RightToLeft;
                    //txt_lang.Text = "EN";
                }
                parentWindow.translate();

            }
        }

  

        private void Btn_lockApp_Click(object sender, RoutedEventArgs e)
        {
            /*
            try
            {
                HelpClass.StartAwait(grid_main);
                Window.GetWindow(this).Visibility = Visibility.Collapsed;
                wd_pauseScreen w = new wd_pauseScreen();
                w.ShowDialog();
                Window.GetWindow(this).Visibility = Visibility.Visible;

                HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                Window.GetWindow(this).Visibility = Visibility.Visible;
                HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            */
        }
        private void Btn_Keyboard_Click(object sender, RoutedEventArgs e)
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
        #region grid0_0
        private void btn_menu_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!AppSettings.menuState)
                {
                    Storyboard sb = this.FindResource("Storyboard1") as Storyboard;
                    sb.Begin();
                    AppSettings.menuState = true;
                }
                else
                {
                    Storyboard sb = this.FindResource("Storyboard2") as Storyboard;
                    sb.Begin();
                    AppSettings.menuState = false;
                }
                //if (!firstLoad)
                //{
                //    Properties.Settings.Default.menuState = AppSettings.menuState;
                //    Properties.Settings.Default.Save();
                //}


            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void ColorButtonRefresh(string str)
        {
            foreach (Button button in FindControls.FindVisualChildren<Button>(this))
            {
                if (button.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == button.Tag.ToString())
                        {
                            if (item == str)
                                button.Background = Application.Current.Resources["MainColor"] as SolidColorBrush;
                            else
                                button.Background = Application.Current.Resources["White"] as SolidColorBrush;

                        }
                    }
                }
            }

            foreach (TextBlock textBlock in FindControls.FindVisualChildren<TextBlock>(this))
            {
                if (textBlock.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == textBlock.Tag.ToString())
                        {
                            if (item == str)
                                textBlock.Foreground = Application.Current.Resources["White"] as SolidColorBrush;
                            else
                                textBlock.Foreground = Application.Current.Resources["MainColor"] as SolidColorBrush;

                        }
                    }
                }
            }

            foreach (Path path in FindControls.FindVisualChildren<Path>(this))
            {
                if (path.Tag != null)
                {
                    foreach (var item in menuList)
                    {
                        if (item == path.Tag.ToString())
                        {
                            if (item == str)
                                path.Fill = Application.Current.Resources["White"] as SolidColorBrush;
                            else
                                path.Fill = Application.Current.Resources["MainColor"] as SolidColorBrush;

                        }
                    }
                }
            }
        }
            private void Btn_home_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                ColorButtonRefresh(button.Tag.ToString());
                MainWindow.mainWindow.grid_main.Children.Clear();
                //uc_sectionData uc = new uc_sectionData();
                //MainWindow.mainWindow.grid_main.Children.Add(uc);

                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_catalog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                ColorButtonRefresh(button.Tag.ToString());
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_catalog uc = new uc_catalog();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
         private void Btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                ColorButtonRefresh(button.Tag.ToString());
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_purchaseInvoice uc = new uc_purchaseInvoice();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_sectionData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                ColorButtonRefresh(button.Tag.ToString());
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_sectionData uc = new uc_sectionData();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }





        #endregion


        #region pos Balance
        public static void setBalance()
        {
            mainWindow.txt_cashValue.Text = HelpClass.DecTostring(posLogin.Balance);
            mainWindow.txt_cashSympol.Text = AppSettings.Currency;
        }
        #endregion
    }
}
