using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.DTO
{
    public record ChangeNeededRequest(int amount, double? price);
    public class GoodsNeededInfo
    {
    
        int Id { get; set; }
        public string Name { get; set; }

        public string Manufacturer { get; set; }
        public string Units { get; set; }
        public double Amount { get; set; }
        
        public bool IsInStopList { get; set; }
        public double price { get; set; }

        public GoodsNeededInfo(GoodsNeeded good)
        {
            Id = good.GoodId;
            Name = good.Good.Name;
            Manufacturer = good.Good.IdmanufacturerNavigation.Name;
            Units = good.Good.IdunitsNavigation.Name;
            IsInStopList = good.Good.IsInStopList;
            price = good.Price;
            Amount = good.Amount;
        }
    }
}
