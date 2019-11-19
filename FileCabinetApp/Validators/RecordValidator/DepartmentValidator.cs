using System;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represents department validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class DepartmentValidator : IRecordValidator
    {
        private readonly short from;

        private readonly short to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentValidator"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public DepartmentValidator(short from, short to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Validates the cabinet record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentNullException">Throws when recordParams is null.</exception>
        /// <exception cref="ArgumentException">Throws when validate is false.</exception>
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException(nameof(recordParams));
            }

            if (recordParams.Department <= this.from || recordParams.Department >= this.to)
            {
                throw new ArgumentException($"{nameof(recordParams.Department)} should be between {this.from} and {this.to}");
            }
        }
    }
}