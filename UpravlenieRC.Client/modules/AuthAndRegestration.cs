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
        private record AuthRequest(string Login, string Password);
        public static async Task<HttpStatusCode> TryAuthorizationAsync(string login, string password)
        {
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.AuthUrl, new AuthRequest(login, password));
            return response.StatusCode;
        }
    }
}
