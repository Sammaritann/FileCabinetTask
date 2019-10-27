using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
   public class DefaultClassValidator:IRecordValidator
    {
        private const char MinClass = 'A';
        private const char MaxClass = 'Z';

        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.Class < MinClass || recordParams.Class > MaxClass)
            {
                throw new ArgumentException($"{nameof(recordParams.Class)} should be between A and Z");
            }
        }

    }
}
