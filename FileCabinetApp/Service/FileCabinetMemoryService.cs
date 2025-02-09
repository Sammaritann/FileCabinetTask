﻿using System;
using System.Collections.Generic;
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
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private readonly IRecordValidator validator;
        private readonly Dictionary<int, FileCabinetRecord> dictionaryId = new Dictionary<int, FileCabinetRecord>();
        private int id = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
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
        /// <exception cref="KeyNotFoundException">Throws when id not found.</exception>
        public void EditRecord(int id, RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must nit be null");
            }

            FileCabinetRecord record = this.dictionaryId[id];

            this.validator.ValidateCabinetRecord(recordParams);

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
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list;
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
        /// <exception cref="ArgumentNullException">Throws when snampshot is null.</exception>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            foreach (FileCabinetRecord record in snapshot.Records)
            {
                try
                {
                    if (!this.dictionaryId.ContainsKey(record.Id))
                    {
                    this.validator.ValidateCabinetRecord(RecordToParams(record));
                    this.list.Add(record);
                    this.dictionaryId.Add(record.Id, record);
                    AddToDictionary<string, FileCabinetRecord>(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), record);
                    AddToDictionary<string, FileCabinetRecord>(this.lastNameDictionary, record.LastName.ToUpperInvariant(), record);
                    AddToDictionary<DateTime, FileCabinetRecord>(this.dateOfBirthDictionary, record.DateOfBirth, record);

                    this.id = Math.Max(this.id, record.Id);
                    }
                    else
                    {
                    this.EditRecord(record.Id, RecordToParams(record));
                    }
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("{0}:{1}", record.Id, e.Message);
                }
            }
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
        /// <returns>FileCabinetREcords.</returns>
        public IEnumerable<FileCabinetRecord> Where(string param)
        {
            ValidateEntity entity = new ValidateEntity().Create(param, this.MemEntity);

            foreach (var record in entity.Filtering(this.list))
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

            if (record.Id == -1)
            {
                throw new ArgumentException($"Record id must be more than zero");
            }

            if (!this.dictionaryId.ContainsKey(record.Id))
            {
                this.validator.ValidateCabinetRecord(RecordToParams(record));
                this.list.Add(record);
                this.dictionaryId.Add(record.Id, record);
                AddToDictionary<string, FileCabinetRecord>(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), record);
                AddToDictionary<string, FileCabinetRecord>(this.lastNameDictionary, record.LastName.ToUpperInvariant(), record);
                AddToDictionary<DateTime, FileCabinetRecord>(this.dateOfBirthDictionary, record.DateOfBirth, record);

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
    }
}