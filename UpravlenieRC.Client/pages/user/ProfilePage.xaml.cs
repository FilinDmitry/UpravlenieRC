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

namespace UpravlenieRC.Client.pages.user
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void ChangePasswordClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChangeUserProfilePage(ChangeProfileType.Password));
        }

        private void ChangeEmailClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new ChangeUserProfilePage(ChangeProfileType.Email));
        }

        private void ChangePhoneClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new ChangeUserProfilePage(ChangeProfileType.Phone));
        }
    }
}
