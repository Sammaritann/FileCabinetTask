using System;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represents class validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class ClassValidator : IRecordValidator
    {
        private readonly char minClass;

        private readonly char maxClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassValidator"/> class.
        /// </summary>
        /// <param name="minClass">The minimum class.</param>
        /// <param name="maxClass">The maximum class.</param>
        public ClassValidator(char minClass, char maxClass)
        {
            this.minClass = minClass;
            this.maxClass = maxClass;
        }

        /// <summary>
        /// Validates the cabinet record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentNullException">Throws when recordParams is null. </exception>
        /// <exception cref="ArgumentException">Throws when validate is false.</exception>
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException(nameof(recordParams));
            }

            if (recordParams.Class < this.minClass || recordParams.Class > this.maxClass)
            {
                throw new ArgumentException($"{nameof(recordParams.Class)} should be between {this.minClass} and {this.maxClass}");
            }
        }
    }
}