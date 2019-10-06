using System;
namespace Poc.DownloadAndSaveInDatabase.Transversal.Interfaces
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using System.Collections.Generic;

    public interface ICsvProcessor
    {
        IEnumerable<CsvFile> CutCsvInFiles(CsvCutOptions csvCutOptions);

    }
}
