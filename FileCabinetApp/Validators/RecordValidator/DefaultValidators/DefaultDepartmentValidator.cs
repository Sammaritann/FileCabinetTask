using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
   public class DefaultDepartmentValidator:IRecordValidator
    {
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.Department <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Department)} should be more than zero");
            }


        }
    }
}
