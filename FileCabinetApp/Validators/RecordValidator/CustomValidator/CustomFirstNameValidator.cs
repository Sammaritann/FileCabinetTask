using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
   public class CustomFirstNameValidator:IRecordValidator
    {
        private const int MinNameLength = 4;
        private const int MaxNameLength = 30;
        public void ValidateCabinetRecord(RecordParams recordParams)
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
    }
}
