using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using UpravlenieRC.Client.ViewModels;
namespace UpravlenieRC.Client.modules
{
    
    static internal class Http
    {
        public static HttpClient httpClient = new HttpClient();
        public static string? JWTBearer { get; set; }
        public static class AdminFunc
        {
            public static async Task<List<RegRequestViewModel>> GetRegestrationrequests()
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Urls.RegUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JWTBearer);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                return await response.Content.ReadFromJsonAsync<List<RegRequestViewModel>>();
            }
            public static async Task<HttpStatusCode> AcceptRegAsync(int id)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Urls.AcceptRegUrl(id));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JWTBearer);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                return response.StatusCode;
            }
            public static async Task<HttpStatusCode> DenyRegAsync(int id)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Urls.DenyRegUrl(id));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JWTBearer);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                return response.StatusCode;
            }
            public static async Task<List<string>> GetRoles()
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Urls.GetRolesUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JWTBearer);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                return (await response.Content.ReadFromJsonAsync<List<string>>());

            }
        }
        public static class Urls
        {
            private const string BaseUrl = "http://localhost:5152/api/";
            public const string AuthUrl = BaseUrl + "auth";
            public const string RegUrl = BaseUrl + "reg";
            public const string ChangePasswordUrl = BaseUrl + "userUpdateProfile/password";
            public const string ChangePhoneUrl = BaseUrl + "userUpdateProfile/phone";
            public const string ChangeEmailUrl = BaseUrl + "userUpdateProfile/email";
            public const string GoodsNeededUrl = BaseUrl + "Goods/needed";
            public const string InventarizationExcelUrl = BaseUrl + "Inventarization";
            public const string GetRolesUrl = RegUrl + "/GetRoles";
            public static string AcceptRegUrl(int id) => RegUrl + "/" + id + "/accept";
            public static string DenyRegUrl(int id) => RegUrl + "/" + id + "/decline";

            public static string GoodsNeededChangeUrl(int id) => BaseUrl + "Goods/needed/" + id;
            public const string GoodsCategoryUrl = BaseUrl + "Goods/Categories";
        }
        public static async Task<ObservableCollection<GoodsNeededInfo>?> GetGoodsNeeded()
        {

            HttpResponseMessage response = await httpClient.GetAsync(Urls.GoodsNeededUrl);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<ObservableCollection<GoodsNeededInfo>>());
            }

            return null;
        }
        public static async Task<List<string>> GetCategories()
        {

            HttpResponseMessage response = await httpClient.GetAsync(Urls.GoodsCategoryUrl);
            if (response.IsSuccessStatusCode)
            {
                return (await response.Content.ReadFromJsonAsync<List<string>>());
            }

            return new();
        }
        
        

    }
    
}
