using System;
using FileCabinetApp.Validators;

namespace FileCabinetApp.FIleCabinetServices
{
    /// <summary>
    /// Represents default file cabinet service.
    /// </summary>
    /// <seealso cref="FileCabinetApp.FileCabinetService" />
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates the validator.
        /// </summary>
        /// <returns>
        /// Validator.
        /// </returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
