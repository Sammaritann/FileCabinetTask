using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Writer
{
    /// <summary>
    /// Represents file cabinet record csv writer.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private StreamWriter writer;

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

        private FileCabinetRecordCsvWriter()
        {
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
            csv.Append(record.Id.ToString(CultureInfo.InvariantCulture) + ',');
            csv.Append(record.FirstName + ',');
            csv.Append(record.LastName + ',');
            csv.Append(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + ',');
            csv.Append(record.Department.ToString(CultureInfo.InvariantCulture) + ',');
            csv.Append(record.Salary.ToString(CultureInfo.InvariantCulture) + ',');
            csv.Append(record.Class);

            this.writer.WriteLine(csv);
            this.writer.Flush();
        }
    }
}
