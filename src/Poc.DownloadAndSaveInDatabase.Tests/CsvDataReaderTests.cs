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

            CsvPocDataReader reader = new CsvPocDataReader(@"D:\borrar\alextest-5.csv", ';', columns.ToArray());

            //using (var conn = new SqlConnection("Server=localhost;Database=CsvData;Integrated Security=true"))
            //{
            // conn.Open();

            System.Data.SqlClient.SqlBulkCopy bcp = new SqlBulkCopy("Server=localhost;Database=CsvMigrationDatabase; Integrated Security=true", SqlBulkCopyOptions.UseInternalTransaction);
                bcp.BatchSize = 10000;
                bcp.DestinationTableName = "CsvData";
                bcp.NotifyAfter = 500;
                bcp.SqlRowsCopied += (sender, e) =>
                {
                    Console.WriteLine("Written: " + e.RowsCopied.ToString());
                };
                bcp.WriteToServer(reader);

            var g = "ssss";
           // }

            //var columns = new List<string>
            //{
            //    "PointOfSale",
            //    "Product",
            //    "Date",
            //    "Stock"
            //};


            //CsvPocDataReader reader = new CsvPocDataReader(@"D:\borrar\alextest-5.csv",';',columns.ToArray());

            //while(reader.Read())
            //{
            //    var pointofSale = reader[0];
            //    var product = reader[1];
            //    var data = reader[2];
            //    var stock = reader[1];



            //    var j = "";

            //}

            //var dh = "";
            //Assert.AreEqual(reader.IsClosed, false);
            // reader.Close();
            //Assert.AreEqual(reader.IsClosed, true);
            //reader.Dispose();
        }
    }
}
