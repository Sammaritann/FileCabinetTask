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
        /// <param name="recordParams">The record parameters.</param>
        /// <returns>The identifier.</returns>
        public int CreateRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must nit be null");
            }

            ValidateCabinetRecord(recordParams);

            FileCabinetRecord record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = recordParams.FirstName,
                LastName = recordParams.LastName,
                DateOfBirth = recordParams.DateOfBirth,
                Department = recordParams.Department,
                Salary = recordParams.Salary,
                Class = recordParams.Class,
            };

            this.list.Add(record);

            AddToDictionary<string, FileCabinetRecord>(this.firstNameDictionary, recordParams.FirstName.ToUpperInvariant(), record);
            AddToDictionary<string, FileCabinetRecord>(this.lastNameDictionary, recordParams.LastName.ToUpperInvariant(), record);
            AddToDictionary<DateTime, FileCabinetRecord>(this.dateOfBirthDictionary, recordParams.DateOfBirth, record);
            return record.Id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentException">wrong {nameof(id)}.</exception>
        public void EditRecord(int id, RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must nit be null");
            }

            FileCabinetRecord record = this.list.Find((x) => x.Id == id);

            if (record is null)
            {
                throw new ArgumentException($"wrong {nameof(id)}");
            }

            ValidateCabinetRecord(recordParams);

            if (record.FirstName.ToUpperInvariant() != recordParams.FirstName.ToUpperInvariant())
            {
                this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(record);
                AddToDictionary<string, FileCabinetRecord>(this.firstNameDictionary, recordParams.FirstName.ToUpperInvariant(), record);
            }

            if (record.LastName.ToUpperInvariant() != recordParams.LastName.ToUpperInvariant())
            {
                this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(record);
                AddToDictionary<string, FileCabinetRecord>(this.lastNameDictionary, recordParams.LastName.ToUpperInvariant(), record);
            }

            if (record.DateOfBirth != recordParams.DateOfBirth)
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                AddToDictionary<DateTime, FileCabinetRecord>(this.dateOfBirthDictionary, recordParams.DateOfBirth, record);
            }

            record.FirstName = recordParams.FirstName;
            record.LastName = recordParams.LastName;
            record.DateOfBirth = recordParams.DateOfBirth;
            record.Department = recordParams.Department;
            record.Salary = recordParams.Salary;
            record.Class = recordParams.Class;
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

        private static void ValidateCabinetRecord(RecordParams recordParams)
        {
            if (string.IsNullOrWhiteSpace(recordParams.FirstName))
            {
                throw new ArgumentNullException($"{nameof(recordParams.FirstName)} must not be null or contain only spaces");
            }

            if (string.IsNullOrWhiteSpace(recordParams.LastName))
            {
                throw new ArgumentNullException($"{nameof(recordParams.LastName)} must not be null or contain only spaces");
            }

            if ((recordParams.FirstName.Length < MinNameLength) || (recordParams.FirstName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(recordParams.FirstName)} length should be between 2 and 60");
            }

            if ((recordParams.LastName.Length < MinNameLength) || (recordParams.LastName.Length > MaxNameLength))
            {
                throw new ArgumentException($"{nameof(recordParams.LastName)} length should be between 2 and 60");
            }

            if (recordParams.DateOfBirth < DateMin || recordParams.DateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(recordParams.DateOfBirth)} shoud be between  01-Jan-1950 and now");
            }

            if (recordParams.Department <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Department)} should be more than zero");
            }

            if (recordParams.Salary <= 0)
            {
                throw new ArgumentException($"{nameof(recordParams.Salary)} must be more than zero");
            }

            if (recordParams.Class < MinClass || recordParams.Class > MaxClass)
            {
                throw new ArgumentException($"{nameof(recordParams.Class)} should be between A and Z");
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