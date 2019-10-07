namespace Poc.DownloadAndSaveInDatabase
{
    using Microsoft.Extensions.DependencyInjection;
    using Poc.DownloadAndSaveInDatabase.Configs;
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using System;
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

            try
            {
                ConsoleAppStartup.ConfigureApp();

                var blobstorageProcessor = ConsoleAppStartup.ServiceProvider.GetRequiredService<IBlobStorageDownloadProcessor>();

                await blobstorageProcessor.DownloadPublicFile(fileUrl: new Uri(ConsoleAppStartup.FileImporterSettings.BlobStorageFileUrl),
                                                               localPath: ConsoleAppStartup.FileImporterSettings.DestinationPath,
                                                                localFile: ConsoleAppStartup.FileImporterSettings.SourceFile);

                var csvProcessor = ConsoleAppStartup.ServiceProvider.GetRequiredService<ICsvProcessor>();

                var csvOptions = new CsvCutOptions
                {
                    SourceFile = Path.Combine(ConsoleAppStartup.FileImporterSettings.DestinationPath, ConsoleAppStartup.FileImporterSettings.SourceFile),
                    DestinationPath = ConsoleAppStartup.FileImporterSettings.DestinationPath,
                    FilePattern = "csvSplit-",
                    Separator = ConsoleAppStartup.FileImporterSettings.Separator,
                    ChunkParts = ConsoleAppStartup.BlobStorageSettings.ChunkBlockSize,
                    HasHeader = true
                };

                var csvFiles = csvProcessor.CutCsvInFiles(csvOptions);

                var databaseMigrationDataProcessor = ConsoleAppStartup.ServiceProvider.GetRequiredService<IDatabaseMigrationDataProcessor>();

                var databaseMigrationResult = databaseMigrationDataProcessor.MigrateCsvFilesToDatabaseTable(csvFiles, ConsoleAppStartup.FileImporterSettings.Separator);

                if(databaseMigrationResult.HasErrors)
                {
                    Console.WriteLine("We are found errors migrating data to Database");

                    foreach (var message in databaseMigrationResult.ErrorMessages)
                    {
                        Console.WriteLine(message);
                    }
                }

            }
            catch (AggregateException agregateException)
            {
                foreach (var ex in agregateException.Flatten().InnerExceptions)
                {

                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("App has exceptions: " + exception.ToString());
            }
           


            stopWatch.Stop();
         
            TimeSpan ts = stopWatch.Elapsed;

            Console.WriteLine(ts);

            Console.ReadLine();
        }
    }
}
