using EasyGo.Classes.ApiClasses;
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

namespace EasyGo.Template
{
    /// <summary>
    /// Interaction logic for uc_itemUnitCards.xaml
    /// </summary>
    public partial class uc_itemUnitCards : UserControl
    {
        public uc_itemUnitCards()
        {
            InitializeComponent();
        }
        public ItemUnit itemUnit { get; set; }
        public SolidColorBrush Color { get; set; }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }
        public event RoutedEventHandler Click;
        void onButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }
    }
}
