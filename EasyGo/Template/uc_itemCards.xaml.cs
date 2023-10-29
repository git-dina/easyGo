using EasyGo.Classes;
using EasyGo.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.IO;
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
        public Item item { get; set; }
        public SolidColorBrush Color { get; set; }
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DataContext = this;
                await getItemImage();
            }
            catch
            {

            }
        }
        public event RoutedEventHandler Click;
        void onButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        private async Task getItemImage()
        {
            byte[] imageBuffer;

            bool isModified = HelpClass.chkImgChng(item.Image, (DateTime)item.UpdateDate, Global.TMPItemFolder);
            if (isModified && item.Image != "")
                imageBuffer = await FillCombo.item.DownloadImage(item.Image); // read this as BLOB from your DB
            else
               imageBuffer =HelpClass.readLocalImage(item.Image, Global.TMPItemFolder);

            var bitmapImage = new BitmapImage();
            using (var memoryStream = new System.IO.MemoryStream(imageBuffer))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            

            img_image.Source = bitmapImage;
            //else
            //    HelpClass.getLocalImg("Item", item.Image, img_image);
        }

      
    }
}
