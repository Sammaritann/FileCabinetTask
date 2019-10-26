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
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord item in records)
            {
                Console.WriteLine(
                    "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                    item.Id,
                    item.FirstName,
                    item.LastName,
                    item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                    item.Department,
                    item.Salary,
                    item.Class);
            }
        }
    }
}
