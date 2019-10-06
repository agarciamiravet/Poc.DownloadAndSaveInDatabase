namespace Poc.DownloadAndSaveInDatabase.Transversal.Files
{
    public class CsvCutOptions
    {
        public int ChunkParts { get; set; }

        public string DestinationPath { get; set; }

        public string FilePattern { get; set; }

        public bool HasHeader { get; set; }

        public string Separator { get; set; }

        public string SourceFile { get; set; }

    }
}
