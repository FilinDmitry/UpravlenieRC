using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace UpravlenieRC.Client.modules
{
    static internal class AuthAndRegestration
    {
        private static UserRoles _role  = UserRoles.None;

        public static UserRoles Role
        {
            get => _role;
            set { _role = value; OnRoleChanged(); }
        }
        private static MainWindow? MainWindow { get; set; } // для изменеия панели навигации
        public static bool isAuthorized => !string.IsNullOrEmpty(Http.JWTBearer);
        private record JWTToken(string access_token, string token_type);
        private record AuthRequest(string Login, string Password);
        public record ChangePasswordRequest(string OldPassword, string NewPassword);
        public record ChangeEmailRequest(string Password, string NewEmail);
        public record ChangePhoneRequest(string Password, string NewPhone);
        private record RegRequest(string Login, string Password, string INN, string message);
        public static async Task<HttpStatusCode> TryAuthorizationAsync(string login, string password)
        {
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.AuthUrl, new AuthRequest(login, password));
            if (response.IsSuccessStatusCode)
            {
                SetToken((await response.Content.ReadFromJsonAsync<JWTToken>())?.access_token);
            }
            else 
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
                return response.StatusCode;
        }
        public static void SetWindow(MainWindow mw)
        {
            MainWindow = mw;
        }
        private static void OnRoleChanged()
        {
            
            if (MainWindow != null)
            {
                MainWindow.ChangeMenuItemsVisibility(Role);
            }
        }
        public static void Quit()
        {
            Http.JWTBearer = null;
            Role = UserRoles.None;
        }
        public static void SetToken(string? token)
        {
            Http.JWTBearer = token;
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            string? role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            switch (role)
            {
                case "Администратор":
                    Role = UserRoles.Admin;
                    break;
                case "Партнер":
                    Role = UserRoles.Partner;
                    break;
                case "Менеджер закупок":
                    Role = UserRoles.Manager;
                    break;
                case "Приемщик/отгрузчик":
                    Role = UserRoles.arrivalOutbound;
                    break;
                case "Сотрудник склада":
                    Role = UserRoles.skladWorker;
                    break;
                case "Водитель":
                    Role = UserRoles.Driver;
                    break;
                case "Логист":
                    Role = UserRoles.Logist;
                    break;
                default:
                    Role = UserRoles.None;
                    break;
            }
        }
        public static async Task<HttpStatusCode> TryRegestrationAsync(string login, string password, string INN, string message)
        {
            
            HttpResponseMessage response = await Http.httpClient.PostAsJsonAsync(Http.Urls.RegUrl, new RegRequest(login, password, INN, message));
            if (response.StatusCode == HttpStatusCode.Created)
            {
                MessageBox.Show("Заявка успешно создана");
            }
            else
            {
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> ChangeEmailAsync(string email, string password)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, Http.Urls.ChangeEmailUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Http.JWTBearer);
            request.Content = new StringContent(JsonSerializer.Serialize(new ChangeEmailRequest(password, email)), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Http.httpClient.SendAsync(request);
            MessageBox.Show(await response.Content.ReadAsStringAsync());
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> ChangePhoneAsync(string phone, string password)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, Http.Urls.ChangePhoneUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Http.JWTBearer);
            request.Content = new StringContent(JsonSerializer.Serialize(new ChangePhoneRequest(password, phone)), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Http.httpClient.SendAsync(request);
            MessageBox.Show(await response.Content.ReadAsStringAsync());
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch, Http.Urls.ChangePasswordUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Http.JWTBearer);
            request.Content = new StringContent(JsonSerializer.Serialize(new ChangePasswordRequest(oldPassword, newPassword)), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Http.httpClient.SendAsync(request);
            MessageBox.Show(await response.Content.ReadAsStringAsync());
            return response.StatusCode;
        }
    }
    public enum UserRoles
    {
        None,
        Admin,
        Manager,
        Partner,
        skladWorker,
        Driver,
        Logist,
        arrivalOutbound
    }
    
}
