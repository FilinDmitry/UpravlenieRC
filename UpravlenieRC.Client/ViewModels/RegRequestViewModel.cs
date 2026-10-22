using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpravlenieRC.Client.ViewModels
{
    internal class RegRequestViewModel
    {
        public int Id { get; set; }
        public string PartnerName { get; set; }
        public string Login {  get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt {  get; set; }
    }
}
