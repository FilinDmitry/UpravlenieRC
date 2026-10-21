using System;
using System.Collections.Generic;
using System.Linq;
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

namespace UpravlenieRC.Client.pages.skladWorker
{
    /// <summary>
    /// Логика взаимодействия для InventarizationPage.xaml
    /// </summary>
    public partial class InventarizationPage : Page
    {
        public InventarizationPage()
        {
            InitializeComponent();
        }

        private void Btn_Inventarization_Click(object sender, RoutedEventArgs e)
        {
            ExportToWordAndExcel.Inventarization();
        }
        
    }
}
