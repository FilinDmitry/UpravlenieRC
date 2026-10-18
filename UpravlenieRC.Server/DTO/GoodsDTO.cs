using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.DTO
{
    public class GoodsDTO
    {
        int Id {  get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public string Manufacturer { get; set; }
        public string Units { get; set; }
        public double? Maxtemperature { get; set; }

        public double? Mintemperature { get; set; }

        public double VolumeInLitr { get; set; }

        public double WeightForOneUnit { get; set; }
        public bool IsInStopList { get; set; }

        public GoodsDTO(Good good)
        {
            Id = good.Id;
            Name = good.Name;
            Category = good.IdCategoryNavigation.Name;
            Manufacturer = good.IdmanufacturerNavigation.Name;
            Units = good.IdunitsNavigation.Name;
            Maxtemperature = good.Maxtemperature;
            Mintemperature = good.Mintemperature;
            VolumeInLitr = good.VolumeInLitr;
            WeightForOneUnit = good.WeightForOneUnit;
            IsInStopList = good.IsInStopList;
        }
    }
}
