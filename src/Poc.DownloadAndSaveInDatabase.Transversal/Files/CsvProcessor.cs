namespace Poc.DownloadAndSaveInDatabase.Transversal.Files
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Constants;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class CsvProcessor : ICsvProcessor
    {
        IEnumerable<CsvFile> ICsvProcessor.CutCsvInFiles(CsvCutOptions csvCutOptions)
        {
            if (csvCutOptions == null)
            {
                throw new ArgumentNullException("Parameter csvOptions is not filled");
            }

            if(string.IsNullOrWhiteSpace(csvCutOptions.Separator))
            {
                throw new ArgumentNullException("Argument separator is not filled");
            }

            if (csvCutOptions.ChunkParts ==default(int))
            {
                throw new ArgumentNullException("Argument ChunkParts is not filled");
            }

            if (string.IsNullOrWhiteSpace(csvCutOptions.DestinationPath))
            {
                throw new ArgumentNullException("Argument separator is not filled");
            }

            if (string.IsNullOrWhiteSpace(csvCutOptions.FilePattern))
            {
                throw new ArgumentNullException("Argument filepattern is not filled");
            }

            if (string.IsNullOrWhiteSpace(csvCutOptions.SourceFile))
            {
                throw new ArgumentNullException("Argument source is not filled");
            }



            var csvOriginalFile = File.ReadLines(csvCutOptions.SourceFile);
            var totalLinesfromCsv = csvOriginalFile.Count();

            var csvChunks = this.GetChunksCsvFiles(totalLinesfromCsv, csvCutOptions.ChunkParts, csvCutOptions.HasHeader);

            var totalChunks = csvChunks.Count();

            var newCsvFiles = new List<CsvFile>();

            var totalProcessors = Environment.ProcessorCount;

            foreach (var csvChunk in csvChunks)
            {
                Console.WriteLine("Processing  {0} file of {1}", (csvChunk.Index + 1), totalChunks);
                var lines = csvOriginalFile.Skip(csvChunk.StartLine).Take(csvChunk.EndLine);

                string fileName = string.Concat(csvCutOptions.FilePattern, csvChunk.Index, FileConstants.FileCsvExtension);
                string destinationPath = Path.Combine(csvCutOptions.DestinationPath, fileName);

                using (StreamWriter sw = new StreamWriter(destinationPath))
                {
                    foreach (var line in lines)
                    {

                        sw.WriteLine(line);
                    }
                }

                newCsvFiles.Add(new CsvFile { Path = destinationPath, Separator = csvCutOptions.Separator });
            }

            return newCsvFiles;
        }

        private IEnumerable<CsvChunkFile> GetChunksCsvFiles(int totalLines, int numberParts, bool hasHeader)
        {
         
            if (totalLines == default(int))
            {
                throw new ArgumentNullException("total argument is null");
            }

            if (numberParts == default(int))
            {
                throw new ArgumentNullException("numberparts argument is null");
            }

            int rest = totalLines % numberParts;

            double csvValue = totalLines / (double)numberParts;

            var splitLines = new List<KeyValuePair<int, int>>();

            for (int i = 0; i < numberParts; i++)
            {
                int value;

                rest--;

                if (rest > 0)
                {

                    value = (int)Math.Ceiling(csvValue); 
                }
                else
                {
                    value = (int)Math.Floor(csvValue); 
                }
                splitLines.Add(new KeyValuePair<int, int>(i, value));
            }

            var chunkParts = new List<CsvChunkFile>();

            var firstTime = true;
            var totalLinesProcessed = 0;
                

            foreach(var csvChunkPart in splitLines.OrderBy(chunkPart => chunkPart.Key))
            {
                if (firstTime && hasHeader)
                {
                    firstTime = false;
                    chunkParts.Add(new CsvChunkFile { Index = csvChunkPart.Key, StartLine = 1, EndLine = csvChunkPart.Value });
                }
                else
                {
                    chunkParts.Add(new CsvChunkFile { Index = csvChunkPart.Key, StartLine = totalLinesProcessed , EndLine = csvChunkPart.Value });
                }

                totalLinesProcessed += csvChunkPart.Value;
            }

            return chunkParts;
        }
    }
}
