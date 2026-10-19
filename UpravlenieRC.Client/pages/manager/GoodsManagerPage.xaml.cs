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

            _goods = await GoodsNeededInfo.GetGoodsNeeded();
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
            ObservableCollection<GoodsNeededInfo> list = _goods;
            if (Genre.SelectedItem != null)
            {
                list = (ObservableCollection<GoodsNeededInfo>)list.Where(i => i.Category == Genre.SelectedItem);
            }

            switch (Sort.SelectedIndex)
            {
                case (0):
                    list = (ObservableCollection<GoodsNeededInfo>)list.OrderByDescending(i => i.Name);
                    break;
                case (1):
                    list = (ObservableCollection<GoodsNeededInfo>)list.OrderBy(i => i.Name);
                    break;
            }
            LB_catalog.ItemsSource = null;
            LB_catalog.ItemsSource = list;
        }

        private void Change_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}

