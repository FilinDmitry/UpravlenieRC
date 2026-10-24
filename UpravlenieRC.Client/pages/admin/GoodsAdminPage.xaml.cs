using System;
using System.Collections.Generic;
using System.Globalization;
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
using UpravlenieRC.Client.ViewModels;

namespace UpravlenieRC.Client.pages.admin
{
    /// <summary>
    /// Логика взаимодействия для GoodsAdminPage.xaml
    /// </summary>
    public partial class GoodsAdminPage : Page
    {
        GoodsViewModel? _good;
        public GoodsAdminPage(GoodsNeededInfo good)
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeData(good);
        }
        public GoodsAdminPage()
        {
            InitializeComponent();
            Loaded += async (s, e) => await InitializeData(null);
        }
        private async Task InitializeData(GoodsNeededInfo good)
        {
            var goodTask = Http.AdminFunc.GetGood(good);
            var manTask = Http.GetManufacturers();
            var catTask = Http.GetCategories();
            var UnitsTask = Http.GetUnits();
            _good = await goodTask;
            CB_TovarCategory.ItemsSource = await catTask;
            CB_TovarManufact.ItemsSource = await manTask;
            CB_TovarUnits.ItemsSource = await UnitsTask;
            if (_good != null)
            {
                TB_MaxTemp.Text = _good.Maxtemperature.ToString();
                TB_TovarName.Text = _good.Name;
                TB_MinTemp.Text = _good.Mintemperature.ToString();
                TB_Price.Text = _good.Price.ToString();
                TB_Volume.Text = _good.VolumeInLitr.ToString();
                TB_Weight.Text = _good.WeightForOneUnit.ToString();
                CB_TovarCategory.SelectedValue = _good.CategoryID;
                CB_TovarManufact.SelectedValue = _good.ManufacturerID;
                CB_TovarUnits.SelectedValue = _good.UnitsID;
            }
            
        }

        private async void Change_Click(object sender, RoutedEventArgs e)
        {
            if (CB_TovarCategory.SelectedIndex == -1 || CB_TovarManufact.SelectedIndex == -1 || CB_TovarUnits.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все категориальные значения");
                return;
            }
            if (TB_TovarName.Text.Length <= 5)
            {
                MessageBox.Show("Название товара слишком короткое");
                return;
            }
            double volume = 0, weight = 0, price = 0;

            if (!double.TryParse(TB_Price.Text, out price) || !double.TryParse(TB_Volume.Text, out volume) || !double.TryParse(TB_Weight.Text, out weight))
            {
                MessageBox.Show("Заполните все обязательные поля");
                return;
            }
            double? minTemp = null, maxTemp = null;
            double _maxTemp = 500;
            double _minTemp = 500;
            if ((TB_MinTemp.Text.Length != 0 && !double.TryParse(TB_MinTemp.Text, out  _minTemp)) 
                ||
                (TB_MaxTemp.Text.Length != 0 && !double.TryParse(TB_MaxTemp.Text, out _maxTemp)))
            {
                MessageBox.Show("Поля температуры заполнены не верно");
                return;
            }
            if (price < 0)
            {
                MessageBox.Show("Цена не может быть меньше 0");
            }
            if (_minTemp != 500)
            {
                minTemp = _minTemp;
            }
            if (_maxTemp != 500)
            {
                maxTemp = _maxTemp;
            }
            if (_good == null)
            {
                _good = new GoodsViewModel();
                _good.Id = 0;
                _good.Category = "";
                _good.Manufacturer = "";
                _good.Units = "";
                _good.IsInStopList = false;
            }
            
                _good.Maxtemperature = maxTemp;
                _good.Mintemperature = minTemp;
                _good.Price = price;
                _good.Name = TB_TovarName.Text;
                _good.VolumeInLitr = volume;
                _good.WeightForOneUnit = weight;
                _good.CategoryID = (int)CB_TovarCategory.SelectedValue;
                _good.ManufacturerID = (int)CB_TovarManufact.SelectedValue;
                _good.UnitsID = (int)CB_TovarUnits.SelectedValue;
            await Http.AdminFunc.ChangeGood(_good);
            

        }

        private void TB_Num_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null) return;

            string proposedText = tb.Text + e.Text;

            if (string.IsNullOrEmpty(proposedText))
                return;

            char separator = '.';

            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c) && c != separator)
                {
                    e.Handled = true; 
                    return;
                }
            }

            int separatorCount = proposedText.Count(c => c == separator);
            if (separatorCount > 1)
            {
                e.Handled = true;
                return;
            }
        }
    }
}
