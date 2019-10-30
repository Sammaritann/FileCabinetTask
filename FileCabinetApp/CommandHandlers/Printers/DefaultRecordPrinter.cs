using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

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
        public void Print(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }
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
}
