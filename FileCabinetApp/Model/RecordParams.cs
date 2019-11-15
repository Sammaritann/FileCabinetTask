using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents file cabinet record params.
    /// </summary>
    public class RecordParams
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordParams"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="department">The department.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="clas">The clas.</param>
        public RecordParams(string firstName, string lastName, DateTime dateOfBirth, short department, decimal salary, char clas)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Department = department;
            this.Salary = salary;
            this.Class = clas;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordParams"/> class.
        /// </summary>
        public RecordParams()
        {
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the department.
        /// </summary>
        /// <value>
        /// The department.
        /// </value>
        public short Department { get; set; }

        /// <summary>
        /// Gets or sets the salary.
        /// </summary>
        /// <value>
        /// The salary.
        /// </value>
        public decimal Salary { get; set; }

        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        public char Class { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"FirstName = \'{this.FirstName}\', LastName = \'{this.LastName}\' DateofBirth = \'{this.DateOfBirth.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}\' Department = \'{this.Department}\' Salary = \'{this.Salary}\' Class = \'{this.Class}\'";
        }
    }
}
