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
    /// Interaction logic for uc_mainCardsOnce.xaml
    /// </summary>
    public partial class uc_mainCardsOnce : UserControl
    {
        public uc_mainCardsOnce()
        {
            InitializeComponent();
        }
        public string Title { get; set; }
        public string Hint { get; set; }
        public string Icon { get; set; }
        public string ButtonText { get; set; }
        public SolidColorBrush Color { get; set; }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            path_icon.Data = App.Current.Resources[Icon] as Geometry;
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
