using EasyGo.Classes;
using EasyGo.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
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

namespace EasyGo.View.sectionData
{
    /// <summary>
    /// Interaction logic for uc_sectionData.xaml
    /// </summary>
    public partial class uc_sectionData : UserControl
    {
        public uc_sectionData()
        {
            InitializeComponent();
        }
        //private static uc_sectionData _instance;
        //public static uc_sectionData Instance
        //{
        //    get
        //    {
        //        if (_instance is null)
        //            _instance = new uc_sectionData();
        //        return _instance;
        //    }
        //    set
        //    {
        //        _instance = value;
        //    }
        //}
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //Instance = null;
            GC.Collect();
        }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                #region translate
                if (AppSettings.lang.Equals("en"))
                    grid_main.FlowDirection = FlowDirection.LeftToRight;
                else
                    grid_main.FlowDirection = FlowDirection.RightToLeft;
                await translate();
                #endregion
                permission();
                buildCards();



            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        void buildCards()
        {
            #region mco_user
            uc_mainCardsOnce mco_user = new uc_mainCardsOnce();
            mco_user.Title = "user";
            mco_user.Hint = "add, update, delete...";
            mco_user.ButtonText = "enter";
            mco_user.Icon = "user";
            mco_user.Color = Application.Current.Resources["dashboardColor1"] as SolidColorBrush;
            mco_user.Click += Btn_user_Click;
            wp_main.Children.Add(mco_user);
            #endregion

            #region mco_supplier
            uc_mainCardsOnce mco_supplier = new uc_mainCardsOnce();
            mco_supplier.Title = "supplier";
            mco_supplier.Hint = "add, update, delete...";
            mco_supplier.ButtonText = "enter";
            mco_supplier.Icon = "supplier";
            mco_supplier.Color = Application.Current.Resources["dashboardColor2"] as SolidColorBrush;
            mco_supplier.Click += Btn_supplier_Click;
            wp_main.Children.Add(mco_supplier);
            #endregion

            #region mco_customer
            uc_mainCardsOnce mco_customer = new uc_mainCardsOnce();
            mco_customer.Title = "customer";
            mco_customer.Hint = "add, update, delete...";
            mco_customer.ButtonText = "enter";
            mco_customer.Icon = "customer";
            mco_customer.Color = Application.Current.Resources["dashboardColor3"] as SolidColorBrush;
            mco_customer.Click += Btn_customer_Click;
            wp_main.Children.Add(mco_customer);
            #endregion

            #region mco_card
            uc_mainCardsOnce mco_card = new uc_mainCardsOnce();
            mco_card.Title = "card";
            mco_card.Hint = "add, update, delete...";
            mco_card.ButtonText = "enter";
            mco_card.Icon = "creditCard";
            mco_card.Color = Application.Current.Resources["dashboardColor4"] as SolidColorBrush;
            mco_card.Click += Btn_card_Click;
            wp_main.Children.Add(mco_card);
            #endregion


        }
        void permission()
        {
            /*
            bool loadWindow = false;
            int counter = 0;
            if (!loadWindow)
                if (!HelpClass.isAdminPermision())
                {
                    foreach (Border border in FindControls.FindVisualChildren<Border>(this))
                    {
                        if (border.Tag != null)
                            if (FillCombo.groupObject.HasPermission(border.Tag.ToString(), FillCombo.groupObjects))
                            {
                                border.Visibility = Visibility.Visible;
                                counter++;
                            }
                            else border.Visibility = Visibility.Collapsed;
                    }
                    if (counter == 1)
                    {
                        foreach (Button button in FindControls.FindVisualChildren<Button>(this))
                        {
                            if (button.Tag != null)
                                if (FillCombo.groupObject.HasPermission(button.Tag.ToString(), FillCombo.groupObjects))
                                {
                                    button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                                    loadWindow = true;
                                }
                        }
                    }
                }
            */
        }
        private async Task translate()
        {

        }
        private void Btn_user_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_user uc = new uc_user();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                Button button = sender as Button;
                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_supplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_supplier uc = new uc_supplier();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                Button button = sender as Button;
                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_customer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_customer uc = new uc_customer();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                Button button = sender as Button;
                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        private void Btn_card_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_card uc = new uc_card();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                Button button = sender as Button;
                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
