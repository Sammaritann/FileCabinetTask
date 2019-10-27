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
            new FirstNameValidator(2,60).ValidateCabinetRecord(recordParams);
            new LastNameValidator(2,60).ValidateCabinetRecord(recordParams);
            new DateOfBirthValidator(new DateTime(1950, 1, 1),DateTime.Now).ValidateCabinetRecord(recordParams);
            new SalaryValidator(0,decimal.MaxValue).ValidateCabinetRecord(recordParams);
            new DepartmentValidator(0,short.MaxValue).ValidateCabinetRecord(recordParams);
            new ClassValidator('A','Z').ValidateCabinetRecord(recordParams);
        }
    }
}
