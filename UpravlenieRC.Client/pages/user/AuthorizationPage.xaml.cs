
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
using UpravlenieRC.Client.pages.admin;
using UpravlenieRC.Client.pages.manager;
using UpravlenieRC.Client.pages.partner;
using UpravlenieRC.Client.pages.skladWorker;
using UpravlenieRC.Client.pages.user;
namespace UpravlenieRC.Client.pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        
        
        public record AuthRequest(string Login, string Password);
        public AuthorizationPage()
        {
            
            InitializeComponent();
            if (Captcha.CaptchaRequered())
            {
                SP_Captcha.Visibility = Visibility.Visible;
                Captcha_displayer.Text = Captcha.CaptchaCreate();
            }
            
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegestrationPage());
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Captcha.CheckCaptcha(TB_Captcha.Text))
            {
                 bool result = await AuthorizationAsync();
            }
            else
            {
                MessageBox.Show("Введен некоректный текст капчи", "Ошибка проверки антиспама", MessageBoxButton.OK, MessageBoxImage.Error);
                Captcha_displayer.Text = Captcha.CaptchaCreate();
            }
        }
        public async Task<bool> AuthorizationAsync()
        {
            HttpStatusCode responseCode = await AuthAndRegestration.TryAuthorizationAsync(TB_Login.Text, TB_password.Password); 
            if (responseCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (Captcha.CanBeSpam())
                {
                    Captcha_displayer.Text = Captcha.CaptchaCreate();
                    SP_Captcha.Visibility = Visibility.Visible;
                    return false;
                }
            }
            else if (responseCode == System.Net.HttpStatusCode.OK)
            {
                
                NavigationService.Navigate(GetStartPageForRole(AuthAndRegestration.Role));
            }
            return true;
        }
        
        private Page GetStartPageForRole(UserRoles role)
        {
            switch (role)
            {
                case UserRoles.Admin:
                    return new GoodsManagerPage();
                case UserRoles.Partner:
                    return new GoodsPartnerPage();
                case UserRoles.Manager:
                    return new GoodsManagerPage();
                case UserRoles.skladWorker:
                    return new InventarizationPage();
                //case UserRoles.Driver:
                //return new GoodsAdminPage();
                //case UserRoles.Logist:
                //    return new GoodsAdminPage();
                //case UserRoles.arrivalOutbound:
                //    return new GoodsAdminPage();
                default:
                    return new ProfilePage();
            }
        }

    }

    
}
