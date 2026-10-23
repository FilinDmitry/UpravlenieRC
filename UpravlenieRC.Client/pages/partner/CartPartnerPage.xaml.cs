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
    /// Логика взаимодействия для CartPartnerPage.xaml
    /// </summary>
    public partial class CartPartnerPage : Page
    {
        private ObservableCollection<GoodsNeededInfo> _goodsList;
        public CartPartnerPage(ObservableCollection<GoodsNeededInfo> goods)
        {
            
            InitializeComponent();
            _goodsList = goods;
            LB_catalog.ItemsSource = new ObservableCollection<GoodsNeededInfo>(_goodsList.Where(i => i.AmountToSell > 0));
            Loaded += async (s, e) => await InilizeData();
        }
        private async Task InilizeData()
        {
            Task<List<string>> categories = GoodsNeededInfo.GetCategories();
            List<string> categories_lst = await categories;
            categories_lst.Insert(0, "Все категории");
            CB_Categories.ItemsSource = categories_lst;
            CB_Categories.SelectedIndex = 0;
        }
        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is GoodsNeededInfo item)
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
            if (item.AmountToSell == 0)
            {
                Filter();
            }
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
            List<GoodsNeededInfo> list = _goodsList.ToList();
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GoodsPartnerPage(_goodsList));
        }
        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                        "Продолжая вы обязуетесь поставить указанные товары. Нажимая далее вы заключаете договор о поставке. Продолжить?",
                        "Подтверждение",
                        MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                await Http.CreateArrivalOrder(_goodsList.Where(i => i.AmountToSell > 0).ToList());
                NavigationService.Navigate(new GoodsPartnerPage());
            }
        }
    }
}
