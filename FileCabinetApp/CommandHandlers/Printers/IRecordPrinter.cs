using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.Printers
{
    /// <summary>
    /// Represents record printer interface.
    /// </summary>
    public interface IRecordPrinter
    {
        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The record.</param>
        public void Print(IEnumerable<FileCabinetRecord> records);

        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <param name="param">The parameter.</param>
        public void Print(IEnumerable<FileCabinetRecord> records, params string[] param);
    }
}
