using System;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Validators.InpitValidator
{
    /// <summary>
    /// Represents custom validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.InpitValidator.IInputValidator" />
    public class CustomValidator : IInputValidator
    {
        private readonly int minNameLength;
        private readonly int maxNameLength;
        private readonly decimal minSalary;
        private readonly decimal maxSalary;
        private readonly short minDepartment;
        private readonly short maxDepartment;
        private readonly char minClass;
        private readonly char maxClass;
        private readonly DateTime dateMin;
        private readonly DateTime dateMax;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomValidator"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">Throws when configuration is null.</exception>
        public CustomValidator(IConfigurationSection configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.minNameLength = configuration.GetSection("firstName:min").Get<int>();
            this.maxNameLength = configuration.GetSection("firstName:max").Get<int>();
            this.dateMin = configuration.GetSection("dateOfBirth:from").Get<DateTime>();
            this.dateMax = configuration.GetSection("dateOfBirth:to").Get<DateTime>();
            this.minSalary = configuration.GetSection("salary:min").Get<decimal>();
            this.maxSalary = configuration.GetSection("salary:max").Get<decimal>();
            this.minDepartment = configuration.GetSection("department:from").Get<short>();
            this.maxDepartment = configuration.GetSection("department:to").Get<short>();
            this.minClass = configuration.GetSection("class:min").Get<char>();
            this.maxClass = configuration.GetSection("class:max").Get<char>();
        }

        /// <summary>
        /// Validate the class.
        /// </summary>
        /// <param name="clas">The clas.</param>
        /// <returns>
        /// Validation result.
        /// </returns>
        public (bool, string) ClassValidate(char clas)
        {
            return (clas >= this.minClass && clas <= this.maxClass, "class");
        }

        /// <summary>
        /// Validate the date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date.</param>
        /// <returns>
        /// Validation result.
        /// </returns>
        public (bool, string) DateOfBirthValidate(DateTime dateOfBirth)
        {
            return (dateOfBirth >= this.dateMin && dateOfBirth <= this.dateMax, "date of birth");
        }

        /// <summary>
        /// Validate the department.
        /// </summary>
        /// <param name="department">The department.</param>
        /// <returns>
        /// Validation result.
        /// </returns>
        public (bool, string) DepartmentValidate(short department)
        {
            return (department > this.minDepartment && department < this.maxDepartment, "department");
        }

        /// <summary>
        /// Validate the first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// Validation result.
        /// </returns>
        public (bool, string) FirstNameValidate(string firstName)
        {
            return string.IsNullOrWhiteSpace(firstName)
                ? (false, "first name")
                : (firstName.Length > this.minNameLength && firstName.Length < this.maxNameLength, "first name");
        }

        /// <summary>
        /// Valdiate the last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// Validation result.
        /// </returns>
        public (bool, string) LastNameValidate(string lastName)
        {
            return string.IsNullOrWhiteSpace(lastName)
                ? (false, "last name")
                : (lastName.Length > this.minNameLength && lastName.Length < this.maxNameLength, "last name");
        }

        /// <summary>
        /// Validate the salsary.
        /// </summary>
        /// <param name="salary">The salary.</param>
        /// <returns>
        /// Validation result.
        /// </returns>
        public (bool, string) SalaryValidate(decimal salary)
        {
            return (salary > this.minSalary && salary < this.maxSalary, "salary");
        }
    }
}
