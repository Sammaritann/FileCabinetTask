using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
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

            if (string.IsNullOrWhiteSpace(recordParams.FirstName))
            {
                throw new ArgumentNullException($"{nameof(recordParams.FirstName)} must not be null or contain only spaces");
            }

            if (string.IsNullOrWhiteSpace(recordParams.LastName))
            {
                throw new ArgumentNullException($"{nameof(recordParams.LastName)} must not be null or contain only spaces");
            }

            if ((recordParams.FirstName.Length < MinNameLength) || (recordParams.FirstName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(recordParams.FirstName)} length should be between 2 and 60");
            }

            if ((recordParams.LastName.Length < MinNameLength) || (recordParams.LastName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(recordParams.LastName)} length should be between 2 and 60");
            }

            if (recordParams.DateOfBirth < DateMin || recordParams.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(recordParams.DateOfBirth)} shoud be between  01-Jan-1950 and now");
            }

            if (recordParams.Department <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Department)} should be more than zero");
            }

            if (recordParams.Salary <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Salary)} must be more than zero");
            }

            if (recordParams.Class < MinClass || recordParams.Class > MaxClass)
            {
                throw new ArgumentException($"{nameof(recordParams.Class)} should be between A and Z");
            }
        }
    }
}
