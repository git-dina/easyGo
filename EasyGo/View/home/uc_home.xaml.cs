using EasyGo.Classes;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace EasyGo.View.home
{
    /// <summary>
    /// Interaction logic for uc_home.xaml
    /// </summary>
    public partial class uc_home : UserControl
    {
        public uc_home()
        {
            InitializeComponent();
        }

        public static DispatcherTimer timer;
        List<keyValueString> list = new List<keyValueString>();
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {//load
            try
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();

                txtVersion.Text = $"{Assembly.GetExecutingAssembly().GetName().Version.Major}." +
                                 $"{Assembly.GetExecutingAssembly().GetName().Version.Minor}";

                AmountMonthlySalPur();

                #region cards

                list.Add(new keyValueString(){ key = "ullamcorper malesuada", value= "12.59" });
                list.Add(new keyValueString(){ key = "nulla facilisi cras", value= "96.88" });
                list.Add(new keyValueString(){ key = "cras adipiscing enim", value= "358.1" });
                list.Add(new keyValueString(){ key = "elit pellentesque habitant", value= "361.5" });
                list.Add(new keyValueString(){ key = "porttitor lacus luctus", value = "12.59" });
                list.Add(new keyValueString(){ key = "viverra orci sagittis", value = "96.88" });
                list.Add(new keyValueString() { key = "elit pellentesque habitant", value = "361.5" });
                list.Add(new keyValueString() { key = "porttitor lacus luctus", value = "12.59" });
                list.Add(new keyValueString() { key = "viverra orci sagittis", value = "96.88" });
                buildConsList(list);
                #endregion
            }
            catch (Exception ex)
            {
                                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (timer != null)
                    timer.Stop();

                //Instance = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                                HelpClass.ExceptionMessage(ex, this, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
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

        #region cards
        void buildConsList(List<keyValueString> list)
        {
            grid_cards.Children.Clear();
            int index = 0;
            int indexColor = 0;
            foreach (var item in list)
            {
                if (indexColor < 8)
                    indexColor++;
                else
                    indexColor = 1;

                #region mainBorder
                Border mainBorder = new Border();
                mainBorder.Margin = new Thickness(10);
                mainBorder.CornerRadius = new CornerRadius(7);
                mainBorder.Background = Application.Current.Resources["White"] as SolidColorBrush;
                mainBorder.BorderBrush = Application.Current.Resources["dashboardColor" + indexColor] as SolidColorBrush;
                mainBorder.BorderThickness = new Thickness(1);
                //if (brd_date.ActualWidth > 0)
                //    mainBorder.Width = brd_date.ActualWidth - 5;
                //mainBorder.Height = brd_date.ActualHeight;

                Grid.SetRow(mainBorder, index / 3);
                Grid.SetColumn(mainBorder, index % 3);
                #region gridMain
                Grid gridMain = new Grid();
                #region gridSettings
                /////////////////////////////////////////////////////
                int columnCount = 2;
                ColumnDefinition[] cd = new ColumnDefinition[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    cd[i] = new ColumnDefinition();
                }
                cd[0].Width = new GridLength(1, GridUnitType.Auto);
                cd[1].Width = new GridLength(1, GridUnitType.Star);
                for (int i = 0; i < columnCount; i++)
                {
                    gridMain.ColumnDefinitions.Add(cd[i]);
                }
                #endregion
                #region mainPath
                Path mainPath = new Path();
                mainPath.Fill = Application.Current.Resources["dashboardColor" + indexColor] as SolidColorBrush;
                mainPath.Data = App.Current.Resources["invoices"] as Geometry;
                mainPath.Stretch = Stretch.Fill;
                mainPath.FlowDirection = System.Windows.FlowDirection.LeftToRight;
                mainPath.Width =
                mainPath.Height = 35;
                mainPath.Margin = new Thickness(10);
                gridMain.Children.Add(mainPath);
                #endregion
                #region mainStackPanel
                StackPanel mainStackPanel = new StackPanel();
                mainStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                mainStackPanel.VerticalAlignment = VerticalAlignment.Center;
                Grid.SetColumn(mainStackPanel, 1);
                #region titleTextBlock
                TextBlock titleTextBlock = new TextBlock();
                titleTextBlock.Text = $"{item.key}";
                titleTextBlock.Margin = new Thickness(10, 5, 10, 0);
                titleTextBlock.FontSize = 16;
                titleTextBlock.FontWeight = FontWeights.SemiBold;
                titleTextBlock.Foreground = Application.Current.Resources["dashboardColor" + indexColor] as SolidColorBrush;
                titleTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                titleTextBlock.VerticalAlignment = VerticalAlignment.Center;
                mainStackPanel.Children.Add(titleTextBlock);
                #endregion
                #region valueTextBlock
                TextBlock valueTextBlock = new TextBlock();
                valueTextBlock.Text = $"{item.value}";
                valueTextBlock.Margin = new Thickness(10, 0, 10, 5);
                valueTextBlock.FontSize = 16;
                valueTextBlock.Foreground = Application.Current.Resources["dashboardColor" + indexColor] as SolidColorBrush;
                valueTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                valueTextBlock.VerticalAlignment = VerticalAlignment.Center;
                mainStackPanel.Children.Add(valueTextBlock);
                #endregion
                gridMain.Children.Add(mainStackPanel);
                #endregion
                mainBorder.Child = gridMain;
                #endregion
                #endregion

                grid_cards.Children.Add(mainBorder);
                index++;
            }
        }
        #endregion
        #region AmountMonthlySalPur
        public SeriesCollection seriesCollection { get; set; }
        public string[] labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        void AmountMonthlySalPur()
        {
            try
            {
                Random random = new Random();
                int NumberDaysInMonth = 30;
                double[] ArrayS = new double[NumberDaysInMonth];
                double[] ArrayP = new double[NumberDaysInMonth];
                string[] ArrayCount = new string[NumberDaysInMonth];
                for (int i = 0; i < NumberDaysInMonth; i++)
                {
                    ArrayS[i] = random.NextDouble();
                    ArrayP[i] = random.NextDouble();
                    ArrayCount[i] = (i + 1).ToString();
                }
                seriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Sales" ,
                    Values = ArrayS.AsChartValues()
                },
                 new LineSeries
                {
                    Title = "Purchases",
                    Values = ArrayP.AsChartValues()
                }
            };

                axs_AxisY.Title = "Total";
                axs_AxisX.Title = "Days";
                labels = ArrayCount;
                YFormatter = value => value.ToString("C");
                DataContext = this;
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
