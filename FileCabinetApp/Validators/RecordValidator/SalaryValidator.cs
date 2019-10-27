using System;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represents salary validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal minSalary;

        private readonly decimal maxSalary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalaryValidator"/> class.
        /// </summary>
        /// <param name="minSalary">The minimum salary.</param>
        /// <param name="maxSalary">The maximum salary.</param>
        public SalaryValidator(decimal minSalary, decimal maxSalary)
        {
            this.minSalary = minSalary;
            this.maxSalary = maxSalary;
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

            if (recordParams.Salary <= this.minSalary || recordParams.Salary >= this.maxSalary)
            {
                throw new ArgumentException($"{nameof(recordParams.Salary)} must be more than zero");
            }
        }
    }
}