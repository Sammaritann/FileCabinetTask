using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represent custom validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class CustomValidator :CompositeValidator
    {
        public CustomValidator() 
            : base( new IRecordValidator[] {
        new FirstNameValidator(4,30),
        new LastNameValidator(4,30),
        new DateOfBirthValidator(new DateTime(1900, 1, 1),DateTime.Now),
        new SalaryValidator(0, decimal.MaxValue),
        new DepartmentValidator(0, short.MaxValue),
        new ClassValidator('A','F'),
    }) { }
    }
}
