using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Represent validator.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validates the cabinet record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
       public void ValidateCabinetRecord(RecordParams recordParams);
    }
}
