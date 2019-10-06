using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.DownloadAndSaveInDatabase.Transversal.Files
{
    public class CsvCutFilesResult
    {
        public IEnumerable<string> CsvFiles { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }

        public bool HasErrors { get; set; }

    }
}
