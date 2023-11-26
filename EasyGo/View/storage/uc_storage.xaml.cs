using EasyGo.Classes;
using EasyGo.Template;
using EasyGo.View.catalog;
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

namespace EasyGo.View.storage
{
    /// <summary>
    /// Interaction logic for uc_storage.xaml
    /// </summary>
    public partial class uc_storage : UserControl
    {
        public uc_storage()
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
            #region mco_itemsStorage
            uc_mainCardsOnce mco_itemsStorage = new uc_mainCardsOnce();
            mco_itemsStorage.Title = "itemsStorage";
            mco_itemsStorage.Hint = "items quantity";
            mco_itemsStorage.ButtonText = "enter";
            mco_itemsStorage.Icon = "itemsStorage";
            mco_itemsStorage.Color = Application.Current.Resources["dashboardColor1"] as SolidColorBrush;
            mco_itemsStorage.Click += Btn_itemsStorage_Click;
            wp_main.Children.Add(mco_itemsStorage);
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
        private void Btn_itemsStorage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.mainWindow.grid_main.Children.Clear();
                uc_itemsStorage uc = new uc_itemsStorage();
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
