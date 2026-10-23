using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UpravlenieRC.Client.modules;
namespace UpravlenieRC.Client.ViewModels
{
    

    public class GoodsNeededInfo : INotifyPropertyChanged
    {
        private int _id;
        private string? _name;
        private string? _manufacturer;
        private string? _units;
        private double _amount;
        private bool _isInStopList;
        private double _price;
        private string? _category;
        private double _amountToCell;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }

        public string? Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }
        public string? Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string? Manufacturer
        {
            get => _manufacturer;
            set { _manufacturer = value; OnPropertyChanged(); }
        }

        public string? Units
        {
            get => _units;
            set { _units = value; OnPropertyChanged(); }
        }

        public double Amount
        {
            get => _amount;
            set
            {
                if (_amount != value || value >= 0)
                {
                    _amount = value;
                    OnPropertyChanged();
                }
            }
        }
        public double AmountToSell
        {
            get => _amountToCell;
            set
            {
                if (_amountToCell != value || value >= 0)
                {
                    if (value >= Amount)
                    {
                        _amountToCell = Amount;
                    }
                    _amountToCell = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsInStopList
        {
            get => _isInStopList;
            set { _isInStopList = value; OnPropertyChanged(); }
        }

        public double Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(); }
        }
        public bool isChanged = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public static async Task<ObservableCollection<GoodsNeededInfo>?> GetGoodsNeeded()
        {
            return await Http.GetGoodsNeeded();
        }
        public static async Task<List<string>> GetCategories()
        {
            return await Http.GetCategories();
        }
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
