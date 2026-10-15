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
            InitializeComponent();
        }

        

        private void ListV_MouseEnter(object sender, MouseEventArgs e)
        {
            if (TgBtn.IsChecked == true)
            {
                //tt1.Visibility = Visibility.Collapsed;
            }
            else
            {
                //tt1.Visibility = Visibility.Visible;
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
                    MainFrame.NavigationService.Navigate(new SettingsPage());
                    break;
            }
        }

        private void TgBtn_Checked(object sender, RoutedEventArgs e)
        {
            // Раскрываем меню
            menu_column.Width = new GridLength(230);
            NavPanel.Width = 230;
            
            TgBtn.IsChecked = true;
            MainGrid.Margin = new Thickness(-165, 0, 0, 0);
        }

        private void TgBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            // Скрываем меню
            menu_column.Width = new GridLength(65);
            NavPanel.Width = 65;
            TgBtn.IsChecked = false;
            MainGrid.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}
