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
using UpravlenieRC.Client.ViewModels;

namespace UpravlenieRC.Client.pages.manager
{
    /// <summary>
    /// Логика взаимодействия для GoodsManagerPage.xaml
    /// </summary>
    public partial class GoodsManagerPage : Page
    {
        public GoodsManagerPage()
        {
            InitializeComponent();
            InitializeData();
        }
        private async void InitializeData()
        {
            List<GoodsNeededInfo>? goodsNeededInfos = await GoodsNeededInfo.GetGoodsNeeded();
            LB_catalog.ItemsSource = goodsNeededInfos;
        }
        private void CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
