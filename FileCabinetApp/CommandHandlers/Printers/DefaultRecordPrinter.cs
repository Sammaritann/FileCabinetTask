using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers.Printers
{
    /// <summary>
    /// Represents default printer.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.Printers.IRecordPrinter" />
    public class DefaultRecordPrinter : IRecordPrinter
    {
        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }
            foreach (var record in records)
            {
                Console.WriteLine(
                       "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                       record.Id,
                       record.FirstName,
                       record.LastName,
                       record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                       record.Department,
                       record.Salary,
                       record.Class);
            }
        }

        public void Print(IEnumerable<FileCabinetRecord> records, params string[] param)
        {
            this.Print(records);
        }
    }
}