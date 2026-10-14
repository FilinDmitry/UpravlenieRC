using ReCaptcha.Desktop.WPF.Client;
using ReCaptcha.Desktop.WPF.Client.Interfaces;
using ReCaptcha.Desktop.WPF.Configuration;
using ReCaptcha.Desktop.WPF.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
namespace UpravlenieRC.Client.pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        private int failedAttempts = 0;
        static HttpClient httpClient = new HttpClient();
        public record AuthRequest(string Login, string Password);
        public AuthorizationPage()
        {
            
            InitializeComponent();
            
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegestrationPage(failedAttempts));
        }


        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Captcha_displayer.Text == TB_Captcha.Text)
            {
                 bool result = await TryAuthorizationAsync();
                

            }
            else
            {
                MessageBox.Show("Введен некоректный текст капчи", "Ошибка проверки антиспама", MessageBoxButton.OK, MessageBoxImage.Error);
                CaptchaChange();
            }
        }
        public async Task<bool> TryAuthorizationAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5152/api/auth");
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5152/api/auth", new AuthRequest(TB_Login.Text, TB_password.Password));
            MessageBox.Show(response.StatusCode.ToString());
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                failedAttempts++;
                if (failedAttempts >= 3)
                {
                    CaptchaChange();
                    SP_Captcha.Visibility = Visibility.Visible;
                    return false;
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
            }
            return true;
        }
        public void CaptchaChange() // TODO: Сделать нормальную капчу

        {

            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            Captcha_displayer.Text = pwd;
        }

    }

    
}
