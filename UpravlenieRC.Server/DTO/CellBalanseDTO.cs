using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.DTO
{
    public class CellBalanseDTO
    {
        public TovarDTO tovarDTO { get; set; }
        public StorageCellDTO storageCellDTO { get; set; }
        public double amount {  get; set; }
        
        public CellBalanseDTO(CellBalance cellBalance) 
        {
            tovarDTO = new TovarDTO(cellBalance.IdtovarNavigation);
            storageCellDTO = new StorageCellDTO(cellBalance.IdcellNavigation);
            amount = cellBalance.Amount;
        }
    }
}
