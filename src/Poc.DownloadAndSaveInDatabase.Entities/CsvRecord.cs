using System;

namespace Poc.DownloadAndSaveInDatabase.Entities
{
    public class CsvRecord
    {
        public long PointOfSale { get; set; }

        public DateTime Date { get; set; }

        public int Stock { get; set; }

        public string Product { get; set; }

    }
}

