using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Net.Http.Json;

namespace UpravlenieRC.Client.pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        
        static HttpClient httpClient = new HttpClient();
        public record AuthRequest(string Login, string Password);
        public AuthorizationPage()
        {
            
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5152/api/auth");
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("http://localhost:5152/api/auth", new AuthRequest(TB_Login.Text, TB_password.Password));
            MessageBox.Show(response.StatusCode.ToString());
        }

    }

    
}
