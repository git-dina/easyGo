﻿using EasyGo.Classes;
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
        //public Tables table;
        //List<Tables> tables = new List<Tables>();
        public bool isOk { get; set; }


        public static List<string> requiredControlList = new List<string>();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                HelpClass.StartAwait(grid_main);
                requiredControlList = new List<string> { "discount" };


                translate();


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
            //txt_title.Text = AppSettings.resourcemanager.GetString("discount");
            //MaterialDesignThemes.Wpf.HintAssist.SetHint(cb_discount, AppSettings.resourcemanager.GetString("discount"));
            btn_select.Content = AppSettings.resourcemanager.GetString("trSelect");
        }

        private void Btn_select_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HelpClass.validate(requiredControlList, this))
                {



                }
            }
            catch (Exception ex)
            {

                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}