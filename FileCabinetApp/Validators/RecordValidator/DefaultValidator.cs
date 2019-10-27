using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Represent default validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class DefaultValidator : IRecordValidator
    {
        private const int MinNameLength = 2;
        private const int MaxNameLength = 60;
        private const char MinClass = 'A';
        private const char MaxClass = 'Z';
        private static readonly DateTime DateMin = new DateTime(1950, 1, 1);

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

            this.ValidateFirstName(recordParams);
            this.ValidateLastName(recordParams);
            this.ValidateDateOfBirth(recordParams);
            this.ValidateDepartment(recordParams);
            this.ValidateSalary(recordParams);
            this.ValidateClass(recordParams);
        }

        private void ValidateFirstName(RecordParams recordParams)
        {
            if (string.IsNullOrWhiteSpace(recordParams.FirstName))
            {
                throw new ArgumentNullException($"{nameof(recordParams.FirstName)} must not be null or contain only spaces");
            }
            if ((recordParams.FirstName.Length < MinNameLength) || (recordParams.FirstName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(recordParams.FirstName)} length should be between 2 and 60");
            }

        }
        private void ValidateLastName(RecordParams recordParams)
        {
            if (string.IsNullOrWhiteSpace(recordParams.LastName))
            {
                throw new ArgumentNullException($"{nameof(recordParams.LastName)} must not be null or contain only spaces");
            }

            if ((recordParams.LastName.Length < MinNameLength) || (recordParams.LastName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(recordParams.LastName)} length should be between 2 and 60");
            }

        }

        private void ValidateDateOfBirth(RecordParams recordParams)
        {
            if (recordParams.DateOfBirth < DateMin || recordParams.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(recordParams.DateOfBirth)} shoud be between  01-Jan-1950 and now");
            }

        }

        private void ValidateDepartment(RecordParams recordParams)
        {
            if (recordParams.Department <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Department)} should be more than zero");
            }

        }

        private void ValidateSalary(RecordParams recordParams)
        {
            if (recordParams.Salary <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Salary)} must be more than zero");
            }

        }

        private void ValidateClass(RecordParams recordParams)
        {
            if (recordParams.Class < MinClass || recordParams.Class > MaxClass)
            {
                throw new ArgumentException($"{nameof(recordParams.Class)} should be between A and Z");
            }

        }
    }
}
