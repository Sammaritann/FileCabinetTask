using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    public class DepartmentValidator : IRecordValidator
    {
        private readonly short from;

        private readonly short to;

        public DepartmentValidator(short from, short to)
        {
            this.from = from;
            this.to = to;
        }

        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.Department <= from || recordParams.Department>=to)
            {
                throw new ArgumentException($"{nameof(recordParams.Department)} should be more than zero");
            }
        }
    }
}
