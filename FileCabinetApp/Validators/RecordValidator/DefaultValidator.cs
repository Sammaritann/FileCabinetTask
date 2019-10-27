using System;

namespace FileCabinetApp.Validators.RecordValidator
{
    /// <summary>
    /// Represent default validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.IRecordValidator" />
    public class DefaultValidator : CompositeValidator
    {
        public DefaultValidator()
            : base(new IRecordValidator[] {
        new FirstNameValidator(2,60),
        new LastNameValidator(2,60),
        new DateOfBirthValidator(new DateTime(1950, 1, 1),DateTime.Now),
        new SalaryValidator(0, decimal.MaxValue),
        new DepartmentValidator(0, short.MaxValue),
        new ClassValidator('A','Z'),
    }){ }
    }
}