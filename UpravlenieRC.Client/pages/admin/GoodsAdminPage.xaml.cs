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
using UpravlenieRC.Client.ViewModels;

namespace UpravlenieRC.Client.pages.admin
{
    /// <summary>
    /// Логика взаимодействия для GoodsAdminPage.xaml
    /// </summary>
    public partial class GoodsAdminPage : Page
    {
        public GoodsAdminPage(GoodsNeededInfo good)
        {
            InitializeComponent();
        }
        public GoodsAdminPage()
        {
            InitializeComponent();
        }

        

    }
}
