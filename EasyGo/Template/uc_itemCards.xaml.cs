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
    /// Interaction logic for uc_itemCards.xaml
    /// </summary>
    public partial class uc_itemCards : UserControl
    {
        public uc_itemCards()
        {
            InitializeComponent();
        }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemPrice { get; set; }
        public ImageSource ItemImage { get; set; }
        public SolidColorBrush Color { get; set; }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            img_image.Source = ItemImage;
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
