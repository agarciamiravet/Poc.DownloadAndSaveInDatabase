namespace Poc.DownloadAndSaveInDatabase
{
    using Microsoft.Extensions.DependencyInjection;
    using Poc.DownloadAndSaveInDatabase.Configs;
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;


    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

      
            ConsoleAppStartup.ConfigureApp();

            //var blobstorageProcessor = ConsoleAppStartup.ServiceProvider.GetRequiredService<IBlobStorageDownloadProcessor>();

            //await blobstorageProcessor.DownloadPublicFile(fileUrl: new Uri(ConsoleAppStartup.FileImporterSettings.BlobStorageFileUrl),
            //                                               localPath: ConsoleAppStartup.FileImporterSettings.DestinationPath,
            //                                                localFile: ConsoleAppStartup.FileImporterSettings.SourceFile);

            var csvProcessor = ConsoleAppStartup.ServiceProvider.GetRequiredService<ICsvProcessor>();

            var csvOptions = new CsvCutOptions
            {
                SourceFile = Path.Combine(ConsoleAppStartup.FileImporterSettings.DestinationPath, ConsoleAppStartup.FileImporterSettings.SourceFile),
                DestinationPath = ConsoleAppStartup.FileImporterSettings.DestinationPath,
                FilePattern = "csvSplit-",
                Separator = ConsoleAppStartup.FileImporterSettings.Separator,
                ChunkParts = 30,
                HasHeader = true
            };

            var csvFiles = csvProcessor.CutCsvInFiles(csvOptions);

            var databaseMigrationDataProcessor = ConsoleAppStartup.ServiceProvider.GetRequiredService<IDatabaseMigrationDataProcessor>();

            databaseMigrationDataProcessor.MigrateCsvFilesToDatabaseTable(csvFiles, ConsoleAppStartup.FileImporterSettings.Separator);


            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            Console.WriteLine(ts);

            Console.ReadLine();
        }
    }
}
