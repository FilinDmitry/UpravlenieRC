using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using UpravlenieRC.Client.pages;
using UpravlenieRC.Client.pages.admin;
using UpravlenieRC.Client.pages.manager;
using UpravlenieRC.Client.pages.partner;
using UpravlenieRC.Client.pages.skladWorker;
using UpravlenieRC.Client.pages.user;
namespace UpravlenieRC.Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Cache.LoadCache();
            AuthAndRegestration.SetWindow(this);
            InitializeComponent();
            ChangeMenuItemsVisibility(AuthAndRegestration.Role);
        }

        

        

        private void ChangeTooltipVisibility(Visibility mode)
        {
            foreach (ListViewItem item in ListV.Items)
            {
                ToolTip? tt = item.ToolTip as ToolTip;
                if (tt != null)
                {
                    tt.Visibility = mode;
                }
            }
        }
        private void ListV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            ListView lv = sender as ListView;
            ListViewItem? lvi = lv.SelectedItem as ListViewItem;
            if (lvi is null)
            { return; }
            switch (lvi.Name)
            {
                case ("Menu"):
                    break;
                case ("Profile"):
                    if (!AuthAndRegestration.isAuthorized)
                    {
                        MainFrame.NavigationService.Navigate(new AuthorizationPage());
                    }
                    else
                    {
                        MainFrame.NavigationService.Navigate(new ProfilePage());
                    }
                    break;
                case ("Settings"):
                    Cache.SaveCache();
                    
                    MainFrame.NavigationService.Navigate(new SettingsPage());
                    break;
                case ("Orders"):
                    MainFrame.NavigationService.Navigate(OrdersClick(AuthAndRegestration.Role));
                    break;
                case ("Statistic"):
                    MainFrame.NavigationService.Navigate(StatisticClick(AuthAndRegestration.Role));
                    break;
                case ("Administration"):
                    MainFrame.NavigationService.Navigate(new AcceptOrDenyUserRegPage());
                    break;
            }
            ListV.SelectedIndex = -1;
        }
        private Page OrdersClick(UserRoles role)
        {
            switch (role)
            {
                case UserRoles.Admin:
                    return new GoodsManagerPage();
                case UserRoles.Manager:
                    return new GoodsManagerPage();
                case UserRoles.Partner:
                    return new GoodsPartnerPage();
                default:
                    return new ProfilePage();
            }
        }
        private Page StatisticClick(UserRoles role)
        {
            switch (role)
            {
                case UserRoles.Admin:
                    return new InventarizationPage();
                case UserRoles.skladWorker:
                    return new InventarizationPage();
                default:
                    return new ProfilePage();
            }
        }
        private void TgBtn_Checked(object sender, RoutedEventArgs e)
        {
            ChangeTooltipVisibility(Visibility.Hidden);
            menu_column.Width = new GridLength(230);
            NavPanel.Width = 230;
            
            TgBtn.IsChecked = true;
            MainGrid.Margin = new Thickness(-165, 0, 0, 0);
        }

        private void TgBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeTooltipVisibility(Visibility.Hidden);
            menu_column.Width = new GridLength(65);
            NavPanel.Width = 65;
            TgBtn.IsChecked = false;
            MainGrid.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Message", MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
                e.Cancel = true;
            else
            {
                Cache.SaveCache();
                Application.Current.Shutdown();
                e.Cancel = false;
            }
        }
        public void ChangeMenuItemsVisibility(UserRoles role)
        {
            switch (role)
            {
                case UserRoles.Admin:
                    AdminMenuVisinility();
                    break;
                case UserRoles.Partner:
                    PartnerMenuVisinility();
                    break;
                case UserRoles.Manager:
                    ManagerMenuVisinility();
                    break;
                case UserRoles.skladWorker:
                    SkladWorkerMenuVisinility();
                    break;
                case UserRoles.Driver:
                    DriverMenuVisinility(); 
                    break;
                case UserRoles.Logist:
                    LogistMenuVisinility();
                    break;
                case UserRoles.arrivalOutbound:
                    ArrivalOutboundMenuVisinility();
                    break;
                default:
                    DefaultMenuVisinility();
                    break;
            }
        }
        private void AdminMenuVisinility()
        {
            Orders.Visibility = Visibility.Visible;
            Statistic.Visibility = Visibility.Visible;
            Administration.Visibility = Visibility.Visible;
        }
        private void ArrivalOutboundMenuVisinility()
        {
            Orders.Visibility = Visibility.Collapsed;
            Statistic.Visibility = Visibility.Collapsed;
            Administration.Visibility = Visibility.Collapsed;
        }
        private void DefaultMenuVisinility()
        {
            Orders.Visibility = Visibility.Collapsed;
            Statistic.Visibility = Visibility.Collapsed;
            Administration.Visibility = Visibility.Collapsed;
        }
        private void PartnerMenuVisinility()
        {
            Orders.Visibility = Visibility.Visible;
            Statistic.Visibility = Visibility.Collapsed;
            Administration.Visibility = Visibility.Collapsed;
        }
        private void ManagerMenuVisinility()
        {
            Orders.Visibility = Visibility.Visible;
            Statistic.Visibility = Visibility.Collapsed;
            Administration.Visibility = Visibility.Collapsed;
        }
        private void SkladWorkerMenuVisinility()
        {
            Orders.Visibility = Visibility.Collapsed;
            Statistic.Visibility = Visibility.Visible;
            Administration.Visibility = Visibility.Collapsed;
        }
        private void DriverMenuVisinility()
        {
            Orders.Visibility = Visibility.Collapsed;
            Statistic.Visibility = Visibility.Collapsed;
            Administration.Visibility = Visibility.Collapsed;
        }
        private void LogistMenuVisinility()
        {
            Orders.Visibility = Visibility.Collapsed;
            Statistic.Visibility = Visibility.Collapsed;
            Administration.Visibility = Visibility.Collapsed;
        }

    }
}
