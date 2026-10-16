using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace UpravlenieRC.Client.modules
{
    static internal class AuthAndRegestration
    {
        public static bool isAuthorized = false;
        private record JWTToken(string access_token, string token_type);
        private record AuthRequest(string Login, string Password);
        public record ChangePasswordRequest(string OldPassword, string NewPassword);
        public record ChangeEmailRequest(string Password, string NewEmail);
        public record ChangePhoneRequest(string Password, string NewPhone);
        private record RegRequest(string Login, string Password, string INN);
        public static async Task<HttpStatusCode> TryAuthorizationAsync(string login, string password)
        {
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.AuthUrl, new AuthRequest(login, password));
            if (response.IsSuccessStatusCode)
            {
                isAuthorized = true;
                Http.JWTBearer = (await response.Content.ReadFromJsonAsync<JWTToken>())?.access_token;
            }
            
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> TryRegestrationAsync(string login, string password, string INN)
        {
            
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.RegUrl, new RegRequest(login, password, INN));
            MessageBox.Show(response.StatusCode.ToString());
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> ChangeEmailAsync(string email, string password)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, Http.Urls.ChangeEmailUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Http.JWTBearer);
            request.Content = new StringContent(JsonSerializer.Serialize(new ChangeEmailRequest(password, email)), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Http.httpClient.SendAsync(request);
            MessageBox.Show(response.StatusCode.ToString());
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> ChangePhoneAsync(string phone, string password)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, Http.Urls.ChangeEmailUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Http.JWTBearer);
            request.Content = new StringContent(JsonSerializer.Serialize(new ChangePhoneRequest(password, phone)), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Http.httpClient.SendAsync(request);
            MessageBox.Show(response.StatusCode.ToString());
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, Http.Urls.ChangeEmailUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Http.JWTBearer);
            request.Content = new StringContent(JsonSerializer.Serialize(new ChangePasswordRequest(oldPassword, newPassword)), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Http.httpClient.SendAsync(request);
            MessageBox.Show(response.StatusCode.ToString());
            return response.StatusCode;
        }
    }
}
