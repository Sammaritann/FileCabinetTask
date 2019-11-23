using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Writer
{
    /// <summary>
    /// Represents file cabinet record csv writer.
    /// </summary>
    public class FileCabinetRecordCsvWriter : IDisposable
    {
        private const char Comma = ',';
        private StreamWriter writer;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter" /> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">Throws when writer is null.</exception>
        public FileCabinetRecordCsvWriter(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.writer = writer;
        }

        /// <summary>
        /// Writes the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throws when record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} must not be null");
            }

            StringBuilder csv = new StringBuilder();
            csv.Append(record.Id.ToString(CultureInfo.InvariantCulture) + Comma);
            csv.Append(record.FirstName + Comma);
            csv.Append(record.LastName + Comma);
            csv.Append(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + Comma);
            csv.Append(record.Department.ToString(CultureInfo.InvariantCulture) + Comma);
            csv.Append(record.Salary.ToString(CultureInfo.InvariantCulture) + Comma);
            csv.Append(record.Class);

            this.writer.WriteLine(csv);
            this.writer.Flush();
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
                this.writer.Dispose();
            }

            this.disposed = true;
        }
    }
}
