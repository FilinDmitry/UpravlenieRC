using UpravlenieRC.Server.Models;

namespace UpravlenieRC.Server.DTO
{
    public class StorageCellDTO
    {
        public int Id { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public double? Temperature { get; set; }

        public double Maxvolume { get; set; }

        public double Maxweight { get; set; }
        public StorageCellDTO(StorageCell storageCell) 
        {
            Id = storageCell.Id;
            Row = storageCell.Row;
            Column = storageCell.Column;
            Temperature = storageCell.Temperature;
            Maxvolume = storageCell.Maxvolume;
            Maxweight = storageCell.Maxweight;
        }
    }
}
