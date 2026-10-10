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
                tt1.Visibility = Visibility.Collapsed;
            }
            else
            {
                tt1.Visibility = Visibility.Visible;
            }
        }

        private void ListV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (Auth.cur_user == null)
            {
                //MessageBox.Show("Сначала необходимо авторизироваться");
                //return;
            }
            ListView lv = sender as ListView;
            ListViewItem lvi = lv.SelectedItem as ListViewItem;
            switch (lvi.Name)
            {
                case ("El1"):
              //      MainFrame.NavigationService.Navigate(new ProfilePage());
                    break;
                case ("El2"):
                //    MainFrame.NavigationService.Navigate(new BookCatalog());
                    break;
                case ("El3"):
                  //  MainFrame.NavigationService.Navigate(new BookListPage());
                    break;
                case ("El4"):
                    //MainFrame.NavigationService.Navigate(new AuthorPage());
                    break;
                case ("El5"):
                    //MainFrame.NavigationService.Navigate(new AdminPage());
                    break;
                case ("El6"):
                    //MainFrame.NavigationService.Navigate(new FreezePage());
                    break;
            }
        }

        private void TgBtn_Checked(object sender, RoutedEventArgs e)
        {
            // Раскрываем меню
            menu_column.Width = new GridLength(230);
            NavPanel.Width = 230;
            
            TgBtn.IsChecked = true;
        }

        private void TgBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            // Скрываем меню
            menu_column.Width = new GridLength(65);
            NavPanel.Width = 65;
            TgBtn.IsChecked = false;
        }
    }
}
