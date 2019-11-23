using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.Reader
{
    /// <summary>
    /// Represents  record csv reader.
    /// </summary>
    public class FileCabinetRecordCsvReader : IDisposable
    {
        private readonly StreamReader reader;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileCabinetRecordCsvReader(StreamReader stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            this.reader = stream;
        }

        /// <summary>
        /// Reads all.
        /// </summary>
        /// <returns>All records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            this.reader.ReadLine();
            while (!this.reader.EndOfStream)
            {
                FileCabinetRecord record = new FileCabinetRecord();
                var param = this.reader.ReadLine().Split(',');

                record.Id = int.Parse(param[0], CultureInfo.InvariantCulture);
                record.FirstName = param[1];
                record.LastName = param[2];
                record.DateOfBirth = DateTime.ParseExact(param[3], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                record.Department = short.Parse(param[4], CultureInfo.InvariantCulture);
                record.Salary = decimal.Parse(param[5], CultureInfo.InvariantCulture);
                record.Class = char.Parse(param[6]);

                records.Add(record);
            }

            return records;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.reader.Dispose();
            }

            this.disposed = true;
        }
    }
}
