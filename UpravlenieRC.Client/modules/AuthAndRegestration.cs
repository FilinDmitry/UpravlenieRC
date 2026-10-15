using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpravlenieRC.Client.modules
{
    static internal class AuthAndRegestration
    {
        public static bool isAuthorized = false; 
        private record AuthRequest(string Login, string Password);
        private record RegRequest(string Login, string Password, string INN);
        public static async Task<HttpStatusCode> TryAuthorizationAsync(string login, string password)
        {
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.AuthUrl, new AuthRequest(login, password));
            if (response.IsSuccessStatusCode)
            {
                isAuthorized = true;
            }
            
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> TryRegestrationAsync(string login, string password, string INN)
        {
            
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.RegUrl, new RegRequest(login, password, INN));
            MessageBox.Show(response.StatusCode.ToString());
            return response.StatusCode;
        }
    }
}
