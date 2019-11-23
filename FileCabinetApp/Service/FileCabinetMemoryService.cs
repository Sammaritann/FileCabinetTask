using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.CommandHandlers.Exceptions;
using FileCabinetApp.CommandHandlers.ValidateHandler;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents abstract file cabinet service.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private const string WhiteSpace = " ";
        private const char SingleQuote = '\'';
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;
        private readonly Dictionary<int, FileCabinetRecord> dictionaryId = new Dictionary<int, FileCabinetRecord>();
        private int id = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService" /> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <exception cref="ArgumentNullException">Throws when validator is null.</exception>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            this.validator = validator;
        }

        /// <summary>
        /// Gets the memory entity.
        /// </summary>
        /// <value>
        /// The memory entity.
        /// </value>
        public MemEntity MemEntity { get; } = new MemEntity();

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

            this.validator.ValidateCabinetRecord(recordParams);

            FileCabinetRecord record = new FileCabinetRecord
            {
                Id = ++this.id,
                FirstName = recordParams.FirstName,
                LastName = recordParams.LastName,
                DateOfBirth = recordParams.DateOfBirth,
                Department = recordParams.Department,
                Salary = recordParams.Salary,
                Class = recordParams.Class,
            };

            this.list.Add(record);
            this.dictionaryId.Add(record.Id, record);
            AddToDictionary(this.firstNameDictionary, recordParams.FirstName.ToUpperInvariant(), record);
            AddToDictionary(this.lastNameDictionary, recordParams.LastName.ToUpperInvariant(), record);
            AddToDictionary(this.dateOfBirthDictionary, recordParams.DateOfBirth, record);
            return record.Id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="KeyNotFoundException">Throws when id not found.</exception>
        public void EditRecord(int id, RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException(nameof(recordParams));
            }

            FileCabinetRecord record = this.dictionaryId[id];

            this.validator.ValidateCabinetRecord(recordParams);

            if (record.FirstName.ToUpperInvariant() != recordParams.FirstName.ToUpperInvariant())
            {
                this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(record);
                AddToDictionary(this.firstNameDictionary, recordParams.FirstName.ToUpperInvariant(), record);
            }

            if (record.LastName.ToUpperInvariant() != recordParams.LastName.ToUpperInvariant())
            {
                this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(record);
                AddToDictionary(this.lastNameDictionary, recordParams.LastName.ToUpperInvariant(), record);
            }

            if (record.DateOfBirth != recordParams.DateOfBirth)
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                AddToDictionary(this.dateOfBirthDictionary, recordParams.DateOfBirth, record);
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
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list;
        }

        /// <summary>
        /// Finds all records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                foreach (FileCabinetRecord item in this.firstNameDictionary[firstName?.ToUpperInvariant()])
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Finds all records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                foreach (FileCabinetRecord item in this.lastNameDictionary[lastName?.ToUpperInvariant()])
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Finds all records by Date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>Found records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                foreach (FileCabinetRecord item in this.dateOfBirthDictionary[dateOfBirth])
                {
                    yield return item;
                }
            }
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
        /// Makes the snapshot.
        /// </summary>
        /// <returns>
        /// Snapshot.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return FileCabinetServiceSnapshot.MakeSnapshot(this.list);
        }

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <returns>
        /// Exceptions.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws when snampshot is null.</exception>
        public IReadOnlyCollection<Exception> Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            List<Exception> exceptions = new List<Exception>();

            foreach (FileCabinetRecord record in snapshot.Records)
            {
                try
                {
                    if (!this.dictionaryId.ContainsKey(record.Id))
                    {
                        this.validator.ValidateCabinetRecord(RecordToParams(record));
                        this.list.Add(record);
                        this.dictionaryId.Add(record.Id, record);
                        AddToDictionary(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), record);
                        AddToDictionary(this.lastNameDictionary, record.LastName.ToUpperInvariant(), record);
                        AddToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);

                        this.id = Math.Max(this.id, record.Id);
                    }
                    else
                    {
                        this.EditRecord(record.Id, RecordToParams(record));
                    }
                }
                catch (ArgumentException e)
                {
                    exceptions.Add(new ImportRecordException(record.Id, e.Message));
                }
            }

            return exceptions;
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="KeyNotFoundException">Throws when id not found.</exception>
        public void Remove(int id)
        {
            this.list.Remove(this.dictionaryId[id]);
            this.dateOfBirthDictionary.Remove(this.dictionaryId[id].DateOfBirth);
            this.firstNameDictionary.Remove(this.dictionaryId[id].FirstName);
            this.lastNameDictionary.Remove(this.dictionaryId[id].LastName);
            this.dictionaryId.Remove(id);
        }

        /// <summary>
        /// Determines whether the specified identifier contains identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified identifier contains identifier; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsId(int id)
        {
            return this.dictionaryId.ContainsKey(id);
        }

        /// <summary>
        /// Purges this instance.
        /// </summary>
        public void Purge()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the delete stat.
        /// </summary>
        /// <returns>0.</returns>
        public int GetDeleteStat()
        {
            return 0;
        }

        /// <summary>
        /// Wheres the specified parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>
        /// FileCabinetRecords.
        /// </returns>
        public IEnumerable<FileCabinetRecord> Where(string param)
        {
            ValidateWhereParam(param);
            ValidateEntity entity = new ValidateEntity().Create(param, this);
            foreach (FileCabinetRecord record in entity.Filtering(this.list))
            {
                yield return record;
            }
        }

        /// <summary>
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throws when record is null.</exception>
        /// <exception cref="ArgumentException">
        /// Record id must be more than zero
        /// or
        /// Such identifier already exists.
        /// </exception>
        public void Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Id < 0)
            {
                throw new ArgumentException($"Record id must be more than zero");
            }

            if (!this.dictionaryId.ContainsKey(record.Id))
            {
                this.validator.ValidateCabinetRecord(RecordToParams(record));
                this.list.Add(record);
                this.dictionaryId.Add(record.Id, record);
                AddToDictionary(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), record);
                AddToDictionary(this.lastNameDictionary, record.LastName.ToUpperInvariant(), record);
                AddToDictionary(this.dateOfBirthDictionary, record.DateOfBirth, record);

                this.id = Math.Max(this.id, record.Id);
            }
            else
            {
                throw new ArgumentException("Such identifier already exists.");
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

        private static RecordParams RecordToParams(FileCabinetRecord record)
        {
            return new RecordParams(record.FirstName, record.LastName, record.DateOfBirth, record.Department, record.Salary, record.Class);
        }

        private static void ValidateWhereParam(string param)
        {
            if (param is null)
            {
                throw new ArgumentNullException(param);
            }

            param = param.Replace("or", WhiteSpace, StringComparison.InvariantCultureIgnoreCase);
            param = param.Replace("and", WhiteSpace, StringComparison.InvariantCultureIgnoreCase);
            const char Separator = ' ';
            string[] validateParam = param.Replace("=", WhiteSpace, StringComparison.InvariantCultureIgnoreCase)
                .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim(SingleQuote))
                .ToArray();
            if (validateParam.Length % 2 != 0)
            {
                throw new ArgumentException("Not correct param string");
            }

            for (int i = 0; i < validateParam.Length / 2; i++)
            {
                Type type = typeof(FileCabinetRecord);
                if (!type.GetProperties().Select(x => x.Name.ToUpperInvariant()).Contains(validateParam[i * 2]))
                {
                    throw new ArgumentNullException(validateParam[i * 2]);
                }
            }
        }
    }
}