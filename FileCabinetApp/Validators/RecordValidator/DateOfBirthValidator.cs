using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;

        private readonly DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (recordParams.DateOfBirth < this.from || recordParams.DateOfBirth > this.to)
            {
                throw new ArgumentException($"{nameof(recordParams.DateOfBirth)} shoud be between  01-Jan-1950 and now");
            }
        }
    }
}
