namespace Poc.DownloadAndSaveInDatabase.Transversal.Interfaces
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Database;
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using System.Collections.Generic;

    public interface IDatabaseMigrationDataProcessor
    {
        DatabaseMigrationDataProcessorResult MigrateCsvFilesToDatabaseTable(IEnumerable<CsvFile> csvFiles, string separator);
    }
}
