namespace Poc.DownloadAndSaveInDatabase.Configs
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using Poc.DownloadAndSaveInDatabase.Transversal.BlobStorage;
    using Microsoft.Extensions.Options;
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using Poc.DownloadAndSaveInDatabase.Transversal.Database;
    using System;

    using Poc.DownloadAndSaveInDatabase.Transversal.Http;

    public static class ConsoleAppStartup
    {
        public static ServiceProvider ServiceProvider { get; set; }
        public static IConfiguration Configuration { get; set; }
        public static BlobStorageSettings BlobStorageSettings { get; set; }
        public static DatabaseCsvSettings DatabaseCsvSettings { get; set; }
        public static FileImporterSettings FileImporterSettings { get; set; }

        public static  void ConfigureApp()
        {
            ConfigureConfiguration();
            ConfigureServices();
            CheckConfigurations();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            services.Configure<BlobStorageSettings>(config.GetSection(typeof(BlobStorageSettings).Name));
            services.Configure<DatabaseCsvSettings>(config.GetSection(typeof(DatabaseCsvSettings).Name));
            services.Configure<FileImporterSettings>(config.GetSection(typeof(FileImporterSettings).Name));

            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<BlobStorageSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<DatabaseCsvSettings>>().Value);
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<FileImporterSettings>>().Value);

            services.AddSingleton<IBlobStorageDownloadProcessor, BlobStorageDownloadProcessor>();

            services.AddSingleton<ICsvProcessor, CsvProcessor>();

            services.AddSingleton<IDatabaseMigrationDataProcessor, DatabaseMigrationDataProcessor>();

            services.AddHttpClient<GenericHttpClient>();
            
            ServiceProvider = services.BuildServiceProvider();
        }

        private static void ConfigureConfiguration()
        {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json");

            Configuration = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json", true, true)
                           .Build();
        }

        private static  void CheckConfigurations()
        {
            BlobStorageSettings = ServiceProvider.GetRequiredService<BlobStorageSettings>();

            if (!BlobStorageSettings.ValidateConfiguration().Success)
            {
                Console.WriteLine("Blob storage settings is not configured properly");
            }

            DatabaseCsvSettings = ServiceProvider.GetRequiredService<DatabaseCsvSettings>();

            if (!BlobStorageSettings.ValidateConfiguration().Success)
            {
                Console.WriteLine("Databasecsv settings is not configured properly");
            }

            FileImporterSettings = ServiceProvider.GetRequiredService<FileImporterSettings>();

            if (!FileImporterSettings.ValidateConfiguration().Success)
            {
                Console.WriteLine("FileImporter settings is not configured properly");
            }
        }
    }
}