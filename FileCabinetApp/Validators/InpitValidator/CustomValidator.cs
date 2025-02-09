﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators.InpitValidator
{
    /// <summary>
    /// Represents custom validator.
    /// </summary>
    /// <seealso cref="FileCabinetApp.Validators.InpitValidator.IInputValidator" />
    public class CustomValidator : IInputValidator
    {
        private const int MinNameLength = 4;
        private const int MaxNameLength = 30;
        private const char MinClass = 'A';
        private const char MaxClass = 'F';
        private static readonly DateTime DateMin = new DateTime(1900, 1, 1);

        /// <inheritdoc/>
        public (bool, string) ClassValidate(char clas)
        {
            return (clas >= MinClass && clas <= MaxClass, "class");
        }

        /// <inheritdoc/>
        public (bool, string) DateOfBirthValidate(DateTime dateOfBirth)
        {
            return (dateOfBirth >= DateMin && dateOfBirth <= DateTime.Now, "date of birth");
        }

        /// <inheritdoc/>
        public (bool, string) DepartmentValidate(short department)
        {
            return (department > 0, "department");
        }

        /// <inheritdoc/>
        public (bool, string) FirstNameValidate(string firstName)
        {
            return string.IsNullOrWhiteSpace(firstName)
                ? (false, "first name")
                : (firstName.Length > MinNameLength && firstName.Length < MaxNameLength, "first name");
        }

        /// <inheritdoc/>
        public (bool, string) LastNameValidate(string lastName)
        {
            return string.IsNullOrWhiteSpace(lastName)
                ? (false, "last name")
                : (lastName.Length > MinNameLength && lastName.Length < MaxNameLength, "last name");
        }

        /// <inheritdoc/>
        public (bool, string) SalaryValidate(decimal salary)
        {
            return (salary > 0, "salary");
        }
    }
}
