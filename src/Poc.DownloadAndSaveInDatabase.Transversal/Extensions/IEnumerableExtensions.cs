namespace Poc.DownloadAndSaveInDatabase.Transversal.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;

    public static class IEnumerableExtensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection, string _sTableName)
        {
            var table = new DataTable(_sTableName);
            var type = typeof(T);
            var properties = type.GetProperties();

            //Create the columns in the DataTable
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(int?))
                {
                    table.Columns.Add(property.Name, typeof(int));
                }
                else if (property.PropertyType == typeof(decimal?))
                {
                    table.Columns.Add(property.Name, typeof(decimal));
                }
                else if (property.PropertyType == typeof(double?))
                {
                    table.Columns.Add(property.Name, typeof(double));
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    table.Columns.Add(property.Name, typeof(DateTime));
                }
                else if (property.PropertyType == typeof(Guid?))
                {
                    table.Columns.Add(property.Name, typeof(Guid));
                }
                else if (property.PropertyType == typeof(bool?))
                {
                    table.Columns.Add(property.Name, typeof(bool));
                }
                else
                {
                    table.Columns.Add(property.Name, property.PropertyType);
                }
            }

            //Populate the table
            foreach (var item in collection)
            {
                var row = table.NewRow();
                row.BeginEdit();
                foreach (var property in properties)
                {
                    row[property.Name] = property.GetValue(item, null) ?? DBNull.Value;
                }

                row.EndEdit();
                table.Rows.Add(row);
            }

            return table;
        }

         public static DataTable CopyAnonymusToDataTable<T>(this IEnumerable<T> info)
         {
             var type = typeof(T);
             //if (type.Equals(typeof(DataRow))) 
             //    return (info as IEnumerable<DataRow>).CopyToDataTable();
             DataTable dt = new DataTable();
             DataRow r;
             type.GetProperties().ToList().ForEach(a=>  dt.Columns.Add(a.Name));
             foreach (var c in info)
             {
                 r = dt.NewRow();
                 c.GetType().GetProperties().ToList().ForEach(a => r[a.Name] = a.GetValue(c, null));
                 dt.Rows.Add(r);
             }
             return dt;
         }
         
     }

    }
