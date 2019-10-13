using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.InpitValidator
{
    /// <summary>
    /// Represents the input validator.
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// Validate the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Validation result.</returns>
        (bool, string) FirstNameValidate(string firstName);

        /// <summary>
        /// Valdiate the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Validation result.</returns>
        (bool, string) LastNameValidate(string lastName);

        /// <summary>
        /// Validate the date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date.</param>
        /// <returns>Validation result.</returns>
        (bool, string) DateOfBirthValidate(DateTime dateOfBirth);

        /// <summary>
        /// Validate the department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns>Validation result.</returns>
        (bool, string) DepartmentValidate(short department);

        /// <summary>
        /// Validate the salsary.
        /// </summary>
        /// <param name="salary">The salary.</param>
        /// <returns>Validation result.</returns>
        (bool, string) SalaryValidate(decimal salary);

        /// <summary>
        /// Validate the class.
        /// </summary>
        /// <param name="clas">The clas.</param>
        /// <returns>Validation result.</returns>
        (bool, string) ClassValidate(char clas);
    }
}
