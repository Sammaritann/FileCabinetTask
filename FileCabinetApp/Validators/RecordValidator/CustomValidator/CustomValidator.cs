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
        private const int MinNameLength = 4;
        private const int MaxNameLength = 30;
        private const char MinClass = 'A';
        private const char MaxClass = 'F';
        private static readonly DateTime DateMin = new DateTime(1900, 1, 1);

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
            new CustomFirstNameValidator().ValidateCabinetRecord(recordParams);
            new CustomLastNameValidator().ValidateCabinetRecord(recordParams);
            new CustomDateOfBirthValidator().ValidateCabinetRecord(recordParams);
            new CustomtSalaryValidator().ValidateCabinetRecord(recordParams);
            new CustomDepartmentValidator().ValidateCabinetRecord(recordParams);
            new CustomClassValidator().ValidateCabinetRecord(recordParams);
        }
    }
}
