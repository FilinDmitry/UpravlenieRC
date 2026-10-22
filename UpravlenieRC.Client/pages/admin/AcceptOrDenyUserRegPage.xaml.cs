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
using UpravlenieRC.Client.ViewModels;

namespace UpravlenieRC.Client.pages.admin
{
    /// <summary>
    /// Логика взаимодействия для AcceptOrDenyUserRegPage.xaml
    /// </summary>
    public partial class AcceptOrDenyUserRegPage : Page
    {
        List<RegRequestViewModel> regRequestsList; 
        public AcceptOrDenyUserRegPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeData();
        }
        private async Task InitializeData()
        {
            regRequestsList = await Http.AdminFunc.GetRegestrationrequests();
            LB_catalog.ItemsSource = regRequestsList;
        }

        private async void Btn_Reg_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            RegRequestViewModel? regRequest = btn.DataContext as RegRequestViewModel;
            if (regRequest != null )
            {
                switch (btn.Name)
                {
                    case "Deny":
                        switch ((int)await Http.AdminFunc.DenyRegAsync(regRequest.Id))
                        {
                            case 200:
                                MessageBox.Show("Заявка успешно отклонена");
                                break;
                            default:
                                MessageBox.Show("Произошла непредиденная ошибка");
                                break;
                        }
                        break;
                    case "Accept":
                        switch ((int)await Http.AdminFunc.AcceptRegAsync(regRequest.Id))
                        {
                            case 201:
                                MessageBox.Show("Пользователь успешно создан");
                                break;
                            default:
                                MessageBox.Show("Произошла непредиденная ошибка");
                                break;
                        }
                        break;
                }
            }
            regRequestsList.Remove(regRequest);
            Search.Text = null;
            Search.Text = "";
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<RegRequestViewModel> list = regRequestsList.Where(i => i.Login.Contains(Search.Text) || i.PartnerName.Contains(Search.Text)).ToList();
            LB_catalog.ItemsSource = null;
            LB_catalog.ItemsSource = list;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateUserPage());
        }
    }
}
