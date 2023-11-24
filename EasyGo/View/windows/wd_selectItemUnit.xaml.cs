using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
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
    /// Interaction logic for wd_selectItemUnit.xaml
    /// </summary>
    public partial class wd_selectItemUnit : Window
    {
        public wd_selectItemUnit()
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
        public long? itemUnitId { get; set; }
        public string unitName { get; set; }
        public List<ItemUnit> itemUnitsList { get; set; }

        public static List<string> requiredControlList = new List<string>();

        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                //HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { };


                translate();


                cb_itemUnit.ItemsSource = itemUnitsList;
                cb_itemUnit.SelectedValuePath = "ItemUnitId";
                cb_itemUnit.DisplayMemberPath = "UnitName";
                cb_itemUnit.SelectedValue = itemUnitId;

                //HelpClass.EndAwait(grid_main);
            }
            catch (Exception ex)
            {

                //HelpClass.EndAwait(grid_main);
                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        private void translate()
        {
            txt_title.Text = AppSettings.resourcemanager.GetString("trItemUnits");
            MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_itemUnit, AppSettings.resourcemanager.GetString("trUnitHint"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
        }

        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HelpClass.validate(requiredControlList, this))
                {
                    if (cb_itemUnit.SelectedValue != null)
                    {
                        itemUnitId = (long)cb_itemUnit.SelectedValue;
                        var unit = (ItemUnit)cb_itemUnit.SelectedItem;
                        unitName = unit.UnitName;
                    }
                    isOk = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
