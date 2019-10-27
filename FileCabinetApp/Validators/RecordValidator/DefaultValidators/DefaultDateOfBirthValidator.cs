using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
   public class DefaultDateOfBirthValidator:IRecordValidator
    {
        private static readonly DateTime DateMin = new DateTime(1950, 1, 1);
        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.DateOfBirth < DateMin || recordParams.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(recordParams.DateOfBirth)} shoud be between  01-Jan-1950 and now");
            }
        }
    }
}
