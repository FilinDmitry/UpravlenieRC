using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.DTO
{
    public class TovarDTO
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; }
        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }

        public TovarDTO(Tovar tovar) 
        {
            Id = tovar.Id;
            BatchNumber = tovar.BatchNumber;
            ExpirationDate = tovar.ExpirationDate;
            Notes = tovar.Notes;
            Name = tovar.IdgoodNavigation.Name;
        }
    }
}
