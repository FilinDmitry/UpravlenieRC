using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UpravlenieRC.Client.modules;
using UpravlenieRC.Client.ViewModels;

namespace UpravlenieRC.Client.pages.partner
{
    /// <summary>
    /// Логика взаимодействия для GoodsPartnerPage.xaml
    /// </summary>
    public partial class GoodsPartnerPage : Page
    {
        private ObservableCollection<GoodsNeededInfo> _goods = new();
        public GoodsPartnerPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeDataAsync();
        }
        public GoodsPartnerPage(ObservableCollection<GoodsNeededInfo> goods)
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeDataFromCartAsync(goods);
        }
        private async Task InitializeDataFromCartAsync(ObservableCollection<GoodsNeededInfo> goods)
        {
            Task<List<NameWithIdDTO>> categories = GoodsNeededInfo.GetCategories();
            List<string> categories_lst = (await categories).Select(i => i.name).ToList();
            categories_lst.Insert(0, "Все категории");
            CB_Categories.ItemsSource = categories_lst;
            CB_Categories.SelectedIndex = 0;
            _goods = goods;
            LB_catalog.ItemsSource = _goods;
        }

        private async Task InitializeDataAsync()
        {
            Task<List<NameWithIdDTO>> categories = GoodsNeededInfo.GetCategories();
            var goods = GoodsNeededInfo.GetGoodsNeeded();
            List<string> categories_lst = (await categories).Select(i => i.name).ToList();
            categories_lst.Insert(0, "Все категории");
            CB_Categories.ItemsSource = categories_lst;
            CB_Categories.SelectedIndex = 0;
            _goods = await goods;
            LB_catalog.ItemsSource = _goods;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            GoodsNeededInfo item = btn.DataContext as GoodsNeededInfo;
            if (item.AmountToSell < item.Amount)
            {
                item.AmountToSell += 1;
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            GoodsNeededInfo item = btn.DataContext as GoodsNeededInfo;
            if (item.AmountToSell > 0)
            {
                item.AmountToSell -= 1;
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CartPartnerPage(_goods));
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }
        private void CB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
        private void Filter()
        {
            List<GoodsNeededInfo> list = _goods.ToList();
            if (CB_Categories.SelectedValue != null)
            {
                if (CB_Categories.SelectedIndex != 0)
                    list = list.Where(i => i.Category == CB_Categories.SelectedItem.ToString()).ToList();
            }

            switch (Sort.SelectedIndex)
            {
                case (0):
                    list = list.OrderByDescending(i => i.Name).ToList();
                    break;
                case (1):
                    list = list.OrderBy(i => i.Name).ToList();
                    break;
                case (2):
                    list = list.OrderByDescending(i => i.Price).ToList();
                    break;
            }
            if (!string.IsNullOrEmpty(Search.Text))
            {
                list = list.Where(i => i.Name.Contains(Search.Text)).ToList();
            }
            LB_catalog.ItemsSource = null;
            LB_catalog.ItemsSource = new ObservableCollection<GoodsNeededInfo>(list);
        }

        private void Change_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }
    }
}
