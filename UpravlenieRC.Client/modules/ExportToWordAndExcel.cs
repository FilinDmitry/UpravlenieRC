using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using static UpravlenieRC.Client.modules.Http;

namespace UpravlenieRC.Client.modules
{
    

    static class ExportToWordAndExcel
    {

        public static async Task<bool> Inventarization()
        {
            try {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Urls.InventarizationExcelUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", JWTBearer);
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка сервера: {response.StatusCode}");
                    return false;
                }

                var content = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName ?? "test.xlsx";

                var downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
                var fullPath = Path.Combine(downloadsPath, fileName);

                File.WriteAllBytes(fullPath, content);
                
                Process.Start(new ProcessStartInfo
                {
                    FileName = downloadsPath,
                    UseShellExecute = true
                });
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось скачать отчёт: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
