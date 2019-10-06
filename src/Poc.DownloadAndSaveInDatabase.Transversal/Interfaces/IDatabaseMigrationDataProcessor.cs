namespace Poc.DownloadAndSaveInDatabase.Transversal.Interfaces
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using System.Collections.Generic;

    public interface IDatabaseMigrationDataProcessor
    {
        void MigrateCsvFilesToDatabaseTable(IEnumerable<CsvFile> csvFiles, string separator);
    }
}
