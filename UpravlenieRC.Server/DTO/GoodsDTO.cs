using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.DTO
{
    public class GoodsDTO
    {
        public int Id {  get; set; }
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

        public static GoodsDTO CreateGoodsDTO(Good good)
        {
            return new GoodsDTO
            {
                Id = good.Id,
                Name = good.Name,
                Category = good.IdCategoryNavigation.Name,
                Manufacturer = good.IdmanufacturerNavigation.Name,
                Units = good.IdunitsNavigation.Name,
                Maxtemperature = good.Maxtemperature,
                Mintemperature = good.Mintemperature,
                VolumeInLitr = good.VolumeInLitr,
                WeightForOneUnit = good.WeightForOneUnit,
                IsInStopList = good.IsInStopList,
                ManufacturerID = good.Idmanufacturer,
                CategoryID = good.IdCategory,
                UnitsID = good.Idunits,
                Price = good.GoodsNeeded.Price
            };
        }
        public static Good CreateGoodFromDTO(GoodsDTO goodsDTO)
        {
            return new Good
            {
                Name = goodsDTO.Name,
                Mintemperature = goodsDTO.Mintemperature,
                Maxtemperature = goodsDTO.Maxtemperature,
                WeightForOneUnit = goodsDTO.WeightForOneUnit,
                IsInStopList = goodsDTO.IsInStopList,
                VolumeInLitr = goodsDTO.VolumeInLitr,
                IdCategory = goodsDTO.CategoryID,
                Idmanufacturer = goodsDTO.ManufacturerID,
                Idunits = goodsDTO.UnitsID,
            };
        }
    }
}
