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
using UpravlenieRC.Client.modules;

namespace UpravlenieRC.Client.pages.admin
{
    /// <summary>
    /// Логика взаимодействия для CreateUserPage.xaml
    /// </summary>
    public partial class CreateUserPage : Page
    {
        public CreateUserPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeData();
        }
        private async Task InitializeData()
        {
            CB_Role.ItemsSource = await Http.AdminFunc.GetRoles();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
