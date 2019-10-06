namespace Poc.DownloadAndSaveInDatabase.Mappings.CsvToEntities
{
    using Poc.DownloadAndSaveInDatabase.Entities;
    using TinyCsvParser.Mapping;

    public class CsvRecordMapping : CsvMapping<CsvRecord>
    {
        public CsvRecordMapping() : base()
        {
            MapProperty(0, x => x.PointOfSale);
            MapProperty(1, x => x.Product);
            MapProperty(2, x => x.Date);
            MapProperty(3, x => x.Stock);
        }
    }
}
