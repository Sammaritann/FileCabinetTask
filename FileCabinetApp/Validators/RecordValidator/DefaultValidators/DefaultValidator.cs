using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represent default validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validates the cabinet record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws when <paramref name="recordParams"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Throw when  the parameters are incorrect.
        /// </exception>
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must not be null");
            }
            new DefaultFirstNameValidator().ValidateCabinetRecord(recordParams);
            new DefaultLastNameValidator().ValidateCabinetRecord(recordParams);
            new DefaultDateOfBirthValidator().ValidateCabinetRecord(recordParams);
            new DefaultSalaryValidator().ValidateCabinetRecord(recordParams);
            new DefaultDepartmentValidator().ValidateCabinetRecord(recordParams);
            new DefaultClassValidator().ValidateCabinetRecord(recordParams);
        }
    }
}
