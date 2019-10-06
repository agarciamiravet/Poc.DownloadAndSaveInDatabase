using System;
using System.Collections.Generic;
using System.Text;

namespace Poc.DownloadAndSaveInDatabase.Transversal.Files
{
    public class CsvChunkFile
    {
        public int Index { get; set; }

        public int StartLine { get; set; }

        public int EndLine { get; set; }
    }
}
