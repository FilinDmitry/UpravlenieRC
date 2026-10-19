using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UpravlenieRC.Client.modules;
namespace UpravlenieRC.Client.ViewModels
{
    public class GoodsNeededInfo
    {

        int Id { get; set; }
        public string Name { get; set; }

        public string Manufacturer { get; set; }
        public string Units { get; set; }
        public double Amount { get; set; }

        public bool IsInStopList { get; set; }
        public double price { get; set; }
        
        public async static Task<List<GoodsNeededInfo>?> GetGoodsNeeded()
        {
            return await Http.GetGoodsNeeded();
        }
    }
}
