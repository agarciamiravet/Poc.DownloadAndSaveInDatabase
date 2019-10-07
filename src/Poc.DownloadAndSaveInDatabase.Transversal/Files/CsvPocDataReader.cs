namespace Poc.DownloadAndSaveInDatabase.Transversal.Files
{
    using System;
    using System.IO;
    using System.Data;

    public class CsvPocDataReader : IDataReader
    {
        private StreamReader csvFileStreamReader;

        private char delimiter { get; set; }

        private bool disposed = false;

        private bool isClosed = false;

   
        public string[] Header { get; }



        public string[] Line { get; private set; }

        public CsvPocDataReader(string fileName, char delimiter, string[] headers)
        {

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("Parameter filename is null or empty");
            }

            if (string.IsNullOrWhiteSpace(delimiter.ToString()))
            {
                throw new ArgumentNullException("Parameter separator is null or empty");
            }


            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("Filename doesn´t exists");
            }

            this.csvFileStreamReader = File.OpenText(fileName);

            this.Header = headers;
            this.delimiter = delimiter;

        }

        public bool Read()
        {
            if (csvFileStreamReader.EndOfStream)
            {
                return false;
            }


            string currentLine = csvFileStreamReader.ReadLine();

            Line = currentLine.Split(delimiter);

            for (int i = 0; i < Line.Length; i++)
            {
                Line[i] = Line[i].Trim('"');
            }

            return true;
        }

        public Object GetValue(int i)
        {
            return Line[i];
        }

        public String GetName(int i)
        {
            return Header[i];
        }

        public int FieldCount
        {
            // Return the count of the number of columns, which in
            // this case is the size of the column metadata
            // array.
            get { return Header.Length; }
        }

        public IDataReader GetData(int i)
        {
            return (IDataReader)this;
        }

        public int Depth
        {
            /*
             * Always return a value of zero if nesting is not supported.
             */
            get { return 0; }
        }

        public bool IsClosed
        {
            /*
             * Keep track of the reader state - some methods should be
             * disallowed if the reader is closed.
             */
            get { return isClosed; }
        }

        public int RecordsAffected
        {
            /*
             * RecordsAffected is only applicable to batch statements
             * that include inserts/updates/deletes. The sample always
             * returns -1.
             */
            get { return -1; }
        }

        public bool NextResult()
        {
            // The sample only returns a single resultset. However,
            // DbDataAdapter expects NextResult to return a value.
            return false;
        }



        public DataTable GetSchemaTable()
        {
            //$
            throw new NotSupportedException();
        }

        /****
         * METHODS / PROPERTIES FROM IDataRecord.
         ****/




        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            return DateTime.Parse(Line[i]);
        }

        public decimal GetDecimal(int i)
        {
            return Decimal.Parse(Line[i]);
        }

        public double GetDouble(int i)
        {
            return Double.Parse(Line[i]);
        }

        public Type GetFieldType(int i)
        {
            return typeof(String);
        }

        public float GetFloat(int i)
        {
            return float.Parse(Line[i]);
        }

        public Guid GetGuid(int i)
        {
            return Guid.Parse(Line[i]);
        }

        public short GetInt16(int i)
        {
            return Int16.Parse(Line[i]);
        }

        public int GetInt32(int i)
        {
            return Int32.Parse(Line[i]);
        }

        public long GetInt64(int i)
        {
            return Int64.Parse(Line[i]);
        }

        public int GetOrdinal(string name)
        {
            int result = -1;
            for (int i = 0; i < Header.Length; i++)
                if (Header[i] == name)
                {
                    result = i;
                    break;
                }
            return result;
        }

        public void Dispose()
        {
            // Based on https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                csvFileStreamReader.Dispose();

            disposed = true;

        }

        public bool GetBoolean(int i)
        {
            return Boolean.Parse(Line[i]);
        }

        public byte GetByte(int i)
        {
            return Byte.Parse(Line[i]);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return Char.Parse(Line[i]);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }


        public string GetString(int i)
        {
            return Line[i];
        }

    

        public int GetValues(object[] values)
        {
            values = Line;
            return 1;
        }

        public bool IsDBNull(int i)
        {
            return string.IsNullOrWhiteSpace(Line[i]);
        }

        public object this[string name]
        {
            get { return Line[GetOrdinal(name)]; }
        }

        public object this[int i]
        {
            get { return GetValue(i); }
        }

        public void Close()
        {
            csvFileStreamReader.Dispose();
        }

    }
}