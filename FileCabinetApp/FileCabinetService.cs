using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents file cabinet service.
    /// </summary>
    public class FileCabinetService
    {
        private const int MinNameLength = 2;
        private const int MaxNameLength = 60;
        private const char MinClass = 'A';
        private const char MaxClass = 'Z';
        private static readonly DateTime DateMin = new DateTime(1950, 1, 1);

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="department">The department.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="clas">The clas.</param>
        /// <returns>The identifier.</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short department, decimal salary, char clas)
        {
            ValidateCabinetRecord(firstName, lastName, dateOfBirth, department, salary, clas);

            FileCabinetRecord record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Department = department,
                Salary = salary,
                Class = clas,
            };

            this.list.Add(record);

            AddToDictionary<string, FileCabinetRecord>(this.firstNameDictionary, firstName.ToUpperInvariant(), record);
            AddToDictionary<string, FileCabinetRecord>(this.lastNameDictionary, lastName.ToUpperInvariant(), record);
            AddToDictionary<DateTime, FileCabinetRecord>(this.dateOfBirthDictionary, dateOfBirth, record);
            return record.Id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="department">The department.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="clas">The clas.</param>
        /// <exception cref="ArgumentException">wrong {nameof(id)}.</exception>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short department, decimal salary, char clas)
        {
            FileCabinetRecord record = this.list.Find((x) => x.Id == id);

            if (record is null)
            {
                throw new ArgumentException($"wrong {nameof(id)}");
            }

            ValidateCabinetRecord(firstName, lastName, dateOfBirth, department, salary, clas);

            if (record.FirstName.ToUpperInvariant() != firstName.ToUpperInvariant())
            {
                this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(record);
                AddToDictionary<string, FileCabinetRecord>(this.firstNameDictionary, firstName.ToUpperInvariant(), record);
            }

            if (record.LastName.ToUpperInvariant() != lastName.ToUpperInvariant())
            {
                this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(record);
                AddToDictionary<string, FileCabinetRecord>(this.lastNameDictionary, lastName.ToUpperInvariant(), record);
            }

            if (record.DateOfBirth != dateOfBirth)
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                AddToDictionary<DateTime, FileCabinetRecord>(this.dateOfBirthDictionary, dateOfBirth, record);
            }

            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.Department = department;
            record.Salary = salary;
            record.Class = clas;
        }

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <returns>The records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Gets the stat.
        /// </summary>
        /// <returns>Number of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Finds all records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Found records.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant())
                 ? this.firstNameDictionary[firstName.ToUpperInvariant()].ToArray()
                 : Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds all records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Found records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant())
                 ? this.lastNameDictionary[lastName.ToUpperInvariant()].ToArray()
                 : Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Finds all records by Date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>Found records.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            return this.dateOfBirthDictionary.ContainsKey(dateOfBirth)
                 ? this.dateOfBirthDictionary[dateOfBirth].ToArray()
                 : Array.Empty<FileCabinetRecord>();
        }

        private static void ValidateCabinetRecord(string firstName, string lastName, DateTime dateOfBirth, short department, decimal salary, char clas)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException($"{nameof(firstName)} must not be null or contain only spaces");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException($"{nameof(lastName)} must not be null or contain only spaces");
            }

            if ((firstName.Length < MinNameLength) || (firstName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(firstName)} length should be between 2 and 60");
            }

            if ((lastName.Length < MinNameLength) || (lastName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(lastName)} length should be between 2 and 60");
            }

            if (dateOfBirth < DateMin || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} shoud be between  01-Jan-1950 and now");
            }

            if (department <= 0)
            {
                throw new ArgumentException($"{nameof(department)} should be more than zero");
            }

            if (salary <= 0)
            {
                throw new ArgumentException($"{nameof(salary)} must be more than zero");
            }

            if (clas < MinClass || clas > MaxClass)
            {
                throw new ArgumentException($"{nameof(clas)} should be between A and Z");
            }
        }

        private static void AddToDictionary<TKey, TValue>(IDictionary<TKey, List<TValue>> dictioanry, TKey key, TValue value)
        {
            if (!dictioanry.ContainsKey(key))
            {
                dictioanry.Add(key, new List<TValue>());
            }

            dictioanry[key].Add(value);
        }
    }
}