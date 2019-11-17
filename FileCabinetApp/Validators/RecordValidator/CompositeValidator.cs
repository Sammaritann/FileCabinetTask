using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represents composite validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator" /> class.
        /// </summary>
        /// <param name="validators">The validators.</param>
        /// <exception cref="ArgumentNullException">Throws when validators is null.</exception>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            if (validators is null)
            {
                throw new ArgumentNullException(nameof(validators));
            }

            this.validators = new List<IRecordValidator>(validators);
        }

        /// <summary>
        /// Validates the cabinet record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentNullException">Throws when recordsParams is null.</exception>
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must not be null");
            }

            foreach (var validator in this.validators)
            {
                validator.ValidateCabinetRecord(recordParams);
            }
        }
    }
}