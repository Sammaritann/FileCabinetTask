using System;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represents last name validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class LastNameValidator : IRecordValidator
    {
        private readonly int minLength;

        private readonly int maxLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">The minimum length.</param>
        /// <param name="maxLength">The maximum length.</param>
        public LastNameValidator(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        /// <summary>
        /// Validates the cabinet record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentNullException">Throws when recordsParams is null.</exception>
        /// <exception cref="ArgumentException">Throws when validate is false.</exception>
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException(nameof(recordParams));
            }

            if (string.IsNullOrWhiteSpace(recordParams.LastName))
            {
                throw new ArgumentException($"{nameof(recordParams.LastName)} must not be null or contain only spaces");
            }

            if ((recordParams.LastName.Length < this.minLength) || (recordParams.LastName.Length > this.maxLength))
            {
                throw new ArgumentException($"{nameof(recordParams.LastName)} length should be between {this.minLength} and {this.maxLength}");
            }
        }
    }
}