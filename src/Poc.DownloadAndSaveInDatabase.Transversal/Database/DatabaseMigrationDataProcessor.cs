namespace Poc.DownloadAndSaveInDatabase.Transversal.Database
{
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs;
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class DatabaseMigrationDataProcessor : IDatabaseMigrationDataProcessor
    {
        private readonly DatabaseCsvSettings databaseCsvSettings;

        public DatabaseMigrationDataProcessor(DatabaseCsvSettings databaseCsvSettings)
        {
            this.databaseCsvSettings = databaseCsvSettings;
        }

        public DatabaseMigrationDataProcessorResult MigrateCsvFilesToDatabaseTable(IEnumerable<CsvFile> csvFiles, string separator)
        {
             DatabaseMigrationDataProcessorResult databaseMigrationDataProcessorResult = new DatabaseMigrationDataProcessorResult();

            if (csvFiles == null || !csvFiles.Any())
            {
                throw new ArgumentNullException("Csv files parameter is null or empty");
            }

            if (string.IsNullOrWhiteSpace(separator))
            {
                throw new ArgumentNullException("separator parameter is null or empty");
            }

            this.CleanTableBeforeInsert(databaseCsvSettings.DataTable);


            var totalCsvFiles = csvFiles.Count();
            var currentProcessingFile = default(int);


            foreach (var csvFile in csvFiles)
            {
                currentProcessingFile++;

                Console.WriteLine("Procesando en base de datos {0} de {1}", currentProcessingFile, totalCsvFiles);

                var columns = new List<string>
                {
                    "PointOfSale",
                    "Product",
                    "Date",
                    "Stock"
                };

                CsvPocDataReader csvPocDataReader = new CsvPocDataReader(csvFile.Path, Convert.ToChar(csvFile.Separator), columns.ToArray());

                databaseMigrationDataProcessorResult = this.ProcessDatabaseInBatchesDatatable(csvPocDataReader);
            }

            return databaseMigrationDataProcessorResult;
        }

        private void CleanTableBeforeInsert(string tableName)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(databaseCsvSettings.ConnectionString))
                {
                    sqlConnection.Open();

                    var queryTruncateQuery = string.Format(databaseCsvSettings.CleanDataTableCommand, tableName);

                    SqlCommand sqlTruncateTableCommand = new SqlCommand(queryTruncateQuery, sqlConnection);
                    sqlTruncateTableCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                throw;
            }
        }
        private DatabaseMigrationDataProcessorResult ProcessDatabaseInBatchesDatatable(CsvPocDataReader csvDataReader)
        {
            var databaseMigrationDataProcessorResult = new DatabaseMigrationDataProcessorResult();

            try
            {
                using (var sqlConnection = new SqlConnection(databaseCsvSettings.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            using (var sqlBulk = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.TableLock, transaction))
                            {
                                sqlBulk.DestinationTableName = databaseCsvSettings.DataTable;
                                sqlBulk.WriteToServer(csvDataReader);
                                sqlBulk.BatchSize = databaseCsvSettings.BulkBatchSize;
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            databaseMigrationDataProcessorResult.CreateMessage(ex.ToString());
                            Console.WriteLine(ex.Message);
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                databaseMigrationDataProcessorResult.CreateMessage(ex.ToString());
                throw;
            }

            return databaseMigrationDataProcessorResult;

        }
    }
}
