using EasyGo.Classes;
using EasyGo.Template;
using EasyGo.View.sectionData;
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

namespace EasyGo.View.catalog
{
    /// <summary>
    /// Interaction logic for uc_catalog.xaml
    /// </summary>
    public partial class uc_catalog : UserControl
    {
        public uc_catalog()
        {
            InitializeComponent();
        }
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
            #region mco_category
            uc_mainCardsOnce mco_category = new uc_mainCardsOnce();
            mco_category.Title = "category";
            mco_category.Hint = "add, update, delete...";
            mco_category.ButtonText = "enter";
            mco_category.Icon = "categoryTree";
            mco_category.Color = Application.Current.Resources["dashboardColor1"] as SolidColorBrush;
            mco_category.Click += Btn_category_Click;
            wp_main.Children.Add(mco_category);
            #endregion

            #region mco_item
            uc_mainCardsOnce mco_item = new uc_mainCardsOnce();
            mco_item.Title = "item";
            mco_item.Hint = "add, update, delete...";
            mco_item.ButtonText = "enter";
            mco_item.Icon = "items";
            mco_item.Color = Application.Current.Resources["dashboardColor2"] as SolidColorBrush;
            mco_item.Click += Btn_item_Click;
            wp_main.Children.Add(mco_item);
            #endregion

            #region mco_unit
            uc_mainCardsOnce mco_unit = new uc_mainCardsOnce();
            mco_unit.Title = "unit";
            mco_unit.Hint = "add, update, delete...";
            mco_unit.ButtonText = "enter";
            mco_unit.Icon = "units";
            mco_unit.Color = Application.Current.Resources["dashboardColor3"] as SolidColorBrush;
            mco_unit.Click += Btn_unit_Click;
            wp_main.Children.Add(mco_unit);
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
        private void Btn_category_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_category uc = new uc_category();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                Button button = sender as Button;
                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_item_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_item uc = new uc_item();
                MainWindow.mainWindow.grid_main.Children.Add(uc);

                Button button = sender as Button;
                //MainWindow.mainWindow.initializationMainTrack(button.Tag.ToString());
            }
            catch (Exception ex)
            {
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void Btn_unit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_unit uc = new uc_unit();
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
