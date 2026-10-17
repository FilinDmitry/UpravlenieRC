using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                Captcha.CaptchaCreate();
            }
        }
        public async Task<bool> RegestrationAsync()
        {
            HttpStatusCode responseCode = await AuthAndRegestration.TryRegestrationAsync(TB_Login.Text, TB_password.Password, TB_INN.Text, TB_Message.Text);
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


        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
