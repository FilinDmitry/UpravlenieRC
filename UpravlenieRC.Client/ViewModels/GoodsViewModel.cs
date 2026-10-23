using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpravlenieRC.Client.ViewModels
{
    public class GoodsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public string Manufacturer { get; set; }
        public string Units { get; set; }
        public double? Maxtemperature { get; set; }

        public double? Mintemperature { get; set; }

        public double VolumeInLitr { get; set; }

        public double WeightForOneUnit { get; set; }
        public bool IsInStopList { get; set; }
        public int ManufacturerID { get; set; }
        public int UnitsID { get; set; }
        public int CategoryID { get; set; }
        public double Price { get; set; }
    }
}
