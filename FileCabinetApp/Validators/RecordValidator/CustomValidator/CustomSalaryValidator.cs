using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
   public class CustomtSalaryValidator : IRecordValidator
    {
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.Salary <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Salary)} must be more than zero");
            }
        }
    }
}
