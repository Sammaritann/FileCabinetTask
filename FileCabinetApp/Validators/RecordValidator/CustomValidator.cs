using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represent custom validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomValidator : IRecordValidator
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

            new FirstNameValidator(4,30).ValidateCabinetRecord(recordParams);
            new LastNameValidator(4,30).ValidateCabinetRecord(recordParams);
            new DateOfBirthValidator(new DateTime(1900, 1, 1),DateTime.Now).ValidateCabinetRecord(recordParams);
            new SalaryValidator(0,decimal.MaxValue).ValidateCabinetRecord(recordParams);
            new DepartmentValidator(0,short.MaxValue).ValidateCabinetRecord(recordParams);
            new ClassValidator('A','F').ValidateCabinetRecord(recordParams);
        }
    }
}
