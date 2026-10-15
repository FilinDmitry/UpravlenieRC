using ReCaptcha.Desktop.WPF.Client;
using ReCaptcha.Desktop.WPF.Client.Interfaces;
using ReCaptcha.Desktop.WPF.Configuration;
using ReCaptcha.Desktop.WPF.UI;
using System;
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
                MessageBox.Show("Ок");
            }
            return true;
        }
        

    }

    
}
