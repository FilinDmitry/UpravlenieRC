using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
        private ObservableCollection<GoodsNeededInfo> _goods = new();
        public GoodsManagerPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeData();
        }

        private async Task InitializeData()
        {
            Task<List<string>> categories = GoodsNeededInfo.GetCategories();
            var goods = GoodsNeededInfo.GetGoodsNeeded();
            List<String> categories_lst = await categories;
            categories_lst.Insert(0, "Все категории");
            CB_Categories.ItemsSource = categories_lst;
            CB_Categories.SelectedIndex = 0;
            _goods = await goods;
            LB_catalog.ItemsSource = _goods;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is GoodsNeededInfo item)
            {
                item.Amount += 1;
                item.isChanged = true;
            }
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            GoodsNeededInfo item = btn.DataContext as GoodsNeededInfo;
            if (item.Amount > 0)
            {
                item.Amount -= 1;
                item.isChanged = true;
            }
        }
        

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var changed = _goods.Where(g => g.isChanged).ToList();


            
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

