using Poc.DownloadAndSaveInDatabase.Transversal.Files;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Xunit;

namespace Poc.DownloadAndSaveInDatabase.Tests
{
    public class CsvDataReaderTests
    {
        [Fact]
        public void Test1()
        {
            var columns = new List<string>
            {
                "PointOfSale",
                "Product",
                "Date",
                "Stock"
            };

            CsvPocDataReader reader = new CsvPocDataReader(@"[PATH_TO_FILE]", ';', columns.ToArray());

            System.Data.SqlClient.SqlBulkCopy bcp = new SqlBulkCopy([CONNECTION_STRING], SqlBulkCopyOptions.UseInternalTransaction);
                bcp.BatchSize = 10000;
                bcp.DestinationTableName = "CsvData";
                bcp.NotifyAfter = 500;
                bcp.SqlRowsCopied += (sender, e) =>
                {
                    Console.WriteLine("Written: " + e.RowsCopied.ToString());
                };
                bcp.WriteToServer(reader);
        }
    }
}
