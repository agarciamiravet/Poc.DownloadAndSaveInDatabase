namespace Poc.DownloadAndSaveInDatabase.Transversal.Database
{
    using CsvHelper;
    using Poc.DownloadAndSaveInDatabase.Entities;
    using Poc.DownloadAndSaveInDatabase.Mappings.CsvToEntities;
    using Poc.DownloadAndSaveInDatabase.Transversal.Configs;
    using Poc.DownloadAndSaveInDatabase.Transversal.Extensions;
    using Poc.DownloadAndSaveInDatabase.Transversal.Files;
    using Poc.DownloadAndSaveInDatabase.Transversal.Http;
    using Poc.DownloadAndSaveInDatabase.Transversal.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Text;
    using TinyCsvParser;

    public class DatabaseMigrationDataProcessor : IDatabaseMigrationDataProcessor
    {
        private readonly DatabaseCsvSettings databaseCsvSettings;

        public DatabaseMigrationDataProcessor(DatabaseCsvSettings databaseCsvSettings)
        {
            this.databaseCsvSettings = databaseCsvSettings;
        }

        public void MigrateCsvFilesToDatabaseTable(IEnumerable<CsvFile> csvFiles, string separator)
        {
            if (csvFiles == null || !csvFiles.Any())
            {
                throw new ArgumentNullException("Csv files parameter is null or empty");
            }

            if (string.IsNullOrWhiteSpace(separator))
            {
                throw new ArgumentNullException("separator parameter is null or empty");
            }

            this.CleanTableBeforeInsert(databaseCsvSettings.DataTable);

            CsvParserOptions csvParserOptions = new CsvParserOptions(false, char.Parse(separator));

            var csvParser = new CsvParser<CsvRecord>(csvParserOptions, new CsvRecordMapping());

            var totalCsvFiles = csvFiles.Count();
            var currentProcessingFile = default(int);


            foreach (var csvFile in csvFiles)
            {
                currentProcessingFile++;

                Console.WriteLine("Procesando en base de datos {0} de {1}", currentProcessingFile, totalCsvFiles);

                var dt = new DataTable();
                dt.Columns.Add("PointOfSale", typeof(string));
                dt.Columns.Add("Date", typeof(DateTime));
                dt.Columns.Add("Stock", typeof(Int64));
                dt.Columns.Add("Product", typeof(string));

                using (TextReader reader = File.OpenText(csvFile.Path))
                {
                    CsvReader csv = new CsvReader(reader);
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.HasHeaderRecord = false;
                    csv.Configuration.MissingFieldFound = null;
                    while (csv.Read())
                    {
                        var row = dt.NewRow();
                        row.BeginEdit();
                        
                         row["PointOfSale"] = Convert.ToString(csv[0]);
                         row["Product"] = Convert.ToString(csv[1]);
                         row["Date"] = Convert.ToDateTime(csv[2]);
                         row["Stock"] = Convert.ToInt64(csv[3]);

                        row.EndEdit();
                        dt.Rows.Add(row);
                    }
                }

                //using (var sr = new StreamReader(csvFile.Path))
                //{
                //    var reader = new CsvReader<>(sr, new CsvHelper.Configuration.Configuration { HasHeaderRecord = false });

                //    while (reader.Read())
                //    {
                //        var g = reader.

                //        // use record...
                //    }
                //}

                // CsvConfiguration configuration = new CsvConfiguration { HasHeaderRecord = true };

                //using (var sr = new StreamReader(csvFile.Path))
                //{
                //    using (var csv = new CsvReader(sr, new CsvHelper.Configuration.Configuration { HasHeaderRecord = false  }))
                //    {
                //        // Do any configuration to `CsvReader` before creating CsvDataReader.
                //        using (var dr = new CsvDataReader(csv))
                //        {

                //            dt.Columns.Add("PointOfSale", typeof(string));
                //            dt.Columns.Add("Date", typeof(DateTime));
                //            dt.Columns.Add("Stock", typeof(Int64));
                //            dt.Columns.Add("Product", typeof(Int64));

                //            dt.Load(dr);
                //        }
                //    }
                //}


                // var recordsfromCsvFile = csvParser.ReadFromFile(csvFile.Path, Encoding.UTF8).Select(x => x.Result).AsEnumerable();

                this.ProcessDatabaseInBatchesDatatable(dt);
            }
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
        private void ProcessDatabaseInBatchesDatatable(DataTable datatable)
        {


            try
            {
                using (var sqlConnection = new SqlConnection(databaseCsvSettings.ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            using (var sqlBulk = new SqlBulkCopy(sqlConnection,SqlBulkCopyOptions.TableLock, transaction))
                            {
                                sqlBulk.DestinationTableName = databaseCsvSettings.DataTable;
                                sqlBulk.WriteToServer(datatable);
                                sqlBulk.BatchSize = databaseCsvSettings.BulkBatchSize;
                                transaction.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                throw;
            }


        }

       
    }
}
