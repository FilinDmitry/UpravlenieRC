using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using UpravlenieRC.Client.modules;
using UpravlenieRC.Client.ViewModels;


namespace UpravlenieRC.Client.pages.manager
{
    /// <summary>
    /// Логика взаимодействия для ArrivalOrdersStatistikPage.xaml
    /// </summary>
    public partial class ArrivalOrdersStatistikPage : Page
    {
        private readonly OrdersViewModel _viewModel;
        public ArrivalOrdersStatistikPage()
        {

            InitializeComponent();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Http.Urls.ChartOrders) // твой API
            };
            _viewModel = new OrdersViewModel();
            DataContext = _viewModel;

            Loaded += async (s, e) => await _viewModel.LoadChartAsync();
        }
    }
    
}
