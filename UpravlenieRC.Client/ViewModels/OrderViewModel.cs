using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UpravlenieRC.Client.modules;

namespace UpravlenieRC.Client.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<OrdersByDateDto> _ordersByDate = new();

        [ObservableProperty]
        private bool _isLoading;



        public async Task LoadChartAsync()
        {
            IsLoading = true;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, Http.Urls.ChartOrders);
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", Http.JWTBearer);

                var response = await Http.httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                MessageBox.Show(response.StatusCode.ToString());
                using var contentStream = await response.Content.ReadAsStreamAsync();
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var data = await System.Text.Json.JsonSerializer.DeserializeAsync<List<OrdersByDateDto>>(contentStream, options);

                OrdersByDate = data ?? new();
            }
            catch (Exception ex)
            {
                // Тут можно показать сообщение пользователю (через событие/команду)
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
        public record OrdersByDateDto(DateTime Date, int Count);
    }
}
