using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
   public class ClassValidator:IRecordValidator
    {
        private readonly char minClass;

        private readonly char maxClass;

        public ClassValidator(char minClass, char maxClass)
        {
            this.minClass = minClass;
            this.maxClass = maxClass;
        }

        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.Class < this.minClass || recordParams.Class > this.maxClass)
            {
                throw new ArgumentException($"{nameof(recordParams.Class)} should be between A and Z");
            }
        }
    }
}
