using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    public class SalaryValidator : IRecordValidator
    {
        private readonly decimal minSalary;

        private readonly decimal maxSalary;

        public SalaryValidator(decimal minSalary, decimal maxSalary)
        {
            this.minSalary = minSalary;
            this.maxSalary = maxSalary;
        }

        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.Salary <= minSalary || recordParams.Salary>=maxSalary)
            {
                throw new ArgumentException($"{nameof(recordParams.Salary)} must be more than zero");
            }
        }
    }
}
