using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    public abstract class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        protected CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = new List<IRecordValidator>(validators);
        }

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
