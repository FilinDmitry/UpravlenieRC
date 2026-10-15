using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    /// Логика взаимодействия для RegestrationPage.xaml
    /// </summary>
    public partial class RegestrationPage : Page
    {
        public RegestrationPage()
        {
            InitializeComponent();
            Captcha_displayer.Text = Captcha.CaptchaCreate();
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            if (Captcha.CheckCaptcha(TB_Captcha.Text))
            {
                if (TB_password.Password == TB_passwordRepeat.Password)
                {
                    bool result = await RegestrationAsync();
                }
                else
                {
                    MessageBox.Show("Пароли не совпадют, проверьте ввод и повторите отправку формы", "Введены разные пароли", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введен некоректный текст капчи", "Ошибка проверки антиспама", MessageBoxButton.OK, MessageBoxImage.Error);
                CaptchaChange();
            }
        }
        public async Task<bool> RegestrationAsync()
        {
            HttpStatusCode responseCode = await AuthAndRegestration.TryRegestrationAsync(TB_Login.Text, TB_password.Password, TB_INN.Text);
            if (responseCode == System.Net.HttpStatusCode.Conflict)
            {
                if (Captcha.CanBeSpam())
                {
                    Captcha_displayer.Text = Captcha.CaptchaCreate();
                    SP_Captcha.Visibility = Visibility.Visible;
                    return false;
                }
            }
            else if (responseCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("Компания с таким ИНН не найдена");
            }
            else if (responseCode == System.Net.HttpStatusCode.Created)
            {
                MessageBox.Show("Запрос на создание успешно отправлен");
            }
            else
            {
                MessageBox.Show("Непредвиденная ошибка");
            }
                return true;
        }
        public void CaptchaChange() // TODO: Сделать нормальную капчу

        {

            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,m,n,o,p,q,r,s,t,u,v,w,y,z";
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

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
