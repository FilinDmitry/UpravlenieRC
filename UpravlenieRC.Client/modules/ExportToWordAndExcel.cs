using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace UpravlenieRC.Client.modules
{
    

    static class ExportToWordAndExcel
    {

        public static async Task<bool> Inventarization()
        {
            try {
                var response = await Http.httpClient.GetAsync(Http.Urls.InventarizationExcelUrl);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Ошибка сервера: {response.StatusCode}");
                    return false;
                }

                var content = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName ?? "test.xlsx";

                // Папка «Загрузки» текущего пользователя
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
