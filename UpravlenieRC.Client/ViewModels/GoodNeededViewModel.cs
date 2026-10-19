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
        private string _category;
        public int Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(); }
        }
        public string Category
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
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged();
                    // Если нужно сразу помечать как изменённое — раскомментируй:
                    // if (ChangeType == ChangeType.None) ChangeType = ChangeType.Updated;
                }
            }
        }

        public bool IsInStopList
        {
            get => _isInStopList;
            set { _isInStopList = value; OnPropertyChanged(); }
        }

        // Переименовали price → Price (WPF чувствителен к регистру)
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
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
