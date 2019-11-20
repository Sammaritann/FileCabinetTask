using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using FileCabinetApp.CommandHandlers.Exceptions;
using FileCabinetApp.CommandHandlers.ValidateHandler;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents file cabinet file system service.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    public class FileCabinetFileSystemService : IFileCabinetService, IDisposable
    {
        private readonly IRecordValidator validator;
        private readonly Dictionary<int, long> dictionaryId = new Dictionary<int, long>();
        private readonly Dictionary<string, List<long>> firstNameDictionary = new Dictionary<string, List<long>>();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new Dictionary<string, List<long>>();
        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new Dictionary<DateTime, List<long>>();

        private FileStream fileStream;
        private int id = 0;
        private int deleteStat = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService" /> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <exception cref="ArgumentNullException">Throws when validator is null.</exception>
        public FileCabinetFileSystemService(IRecordValidator validator)
        {
            if (validator is null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            this.validator = validator;
            this.fileStream = new FileStream("cabinet-records.db", FileMode.Create, FileAccess.ReadWrite);
        }

        /// <summary>
        /// Gets the memory entity.
        /// </summary>
        /// <value>
        /// The memory entity.
        /// </value>
        public MemEntity MemEntity { get; } = new MemEntity();

        /// <inheritdoc/>
        public int CreateRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must nit be null");
            }

            this.validator.ValidateCabinetRecord(recordParams);
            this.fileStream.Position = this.fileStream.Length;
            this.dictionaryId.Add(this.id + 1, this.fileStream.Position);
            AddToDictionary<string, long>(this.firstNameDictionary, recordParams.FirstName.ToUpperInvariant(), this.fileStream.Position);
            AddToDictionary<string, long>(this.lastNameDictionary, recordParams.LastName.ToUpperInvariant(), this.fileStream.Position);
            AddToDictionary<DateTime, long>(this.dateOfBirthDictionary, recordParams.DateOfBirth, this.fileStream.Position);

            byte[] tempFirstName = Encoding.Default.GetBytes(recordParams.FirstName);
            byte[] tempLastName = Encoding.Default.GetBytes(recordParams.LastName);
            byte[] firstName = new byte[120];
            byte[] lastName = new byte[120];
            ToBytesDecimal toBytesDecimal = new ToBytesDecimal(recordParams.Salary);
            byte[] bytesSalary = BitConverter.GetBytes(toBytesDecimal.Bytes1).Concat(BitConverter.GetBytes(toBytesDecimal.Bytes2)).ToArray();
            Array.Copy(tempFirstName, 0, firstName, 0, tempFirstName.Length);
            Array.Copy(tempLastName, 0, lastName, 0, tempLastName.Length);
            this.fileStream.Write(BitConverter.GetBytes(recordParams.Department));
            this.fileStream.Write(BitConverter.GetBytes(++this.id));
            this.fileStream.Write(firstName);
            this.fileStream.Write(lastName);
            this.fileStream.Write(BitConverter.GetBytes(recordParams.DateOfBirth.Year));
            this.fileStream.Write(BitConverter.GetBytes(recordParams.DateOfBirth.Month));
            this.fileStream.Write(BitConverter.GetBytes(recordParams.DateOfBirth.Day));
            this.fileStream.Write(bytesSalary);
            this.fileStream.Write(BitConverter.GetBytes(recordParams.Class));
            this.fileStream.Write(new byte[] { 0 });
            return this.id;
        }

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="ArgumentNullException">Throws when recordParams is null.</exception>
        /// <exception cref="KeyNotFoundException">Throws when Id not found.</exception>
        public void EditRecord(int id, RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must nit be null");
            }

            this.validator.ValidateCabinetRecord(recordParams);
            byte[] buffer = new byte[277];
            this.fileStream.Position = 0;

            if (this.dictionaryId.ContainsKey(id))
            {
                long position = this.dictionaryId[id];
                this.fileStream.Position = position;
                this.fileStream.Read(buffer, 0, 277);
                FileCabinetRecord record = this.RecordFromBytes(buffer);

                if (record.FirstName.ToUpperInvariant() != recordParams.FirstName.ToUpperInvariant())
                {
                    this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(position);
                    AddToDictionary<string, long>(this.firstNameDictionary, recordParams.FirstName.ToUpperInvariant(), position);
                }

                if (record.LastName.ToUpperInvariant() != recordParams.LastName.ToUpperInvariant())
                {
                    this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(position);
                    AddToDictionary<string, long>(this.lastNameDictionary, recordParams.LastName.ToUpperInvariant(), position);
                }

                if (record.DateOfBirth != recordParams.DateOfBirth)
                {
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(position);
                    AddToDictionary<DateTime, long>(this.dateOfBirthDictionary, recordParams.DateOfBirth, position);
                }

                this.fileStream.Position = position;

                byte[] tempFirstName = Encoding.Default.GetBytes(recordParams.FirstName);
                byte[] tempLastName = Encoding.Default.GetBytes(recordParams.LastName);
                byte[] firstName = new byte[120];
                byte[] lastName = new byte[120];
                ToBytesDecimal toBytesDecimal = new ToBytesDecimal(recordParams.Salary);
                byte[] bytesSalary = BitConverter.GetBytes(toBytesDecimal.Bytes1).Concat(BitConverter.GetBytes(toBytesDecimal.Bytes2)).ToArray();
                Array.Copy(tempFirstName, 0, firstName, 0, tempFirstName.Length);
                Array.Copy(tempLastName, 0, lastName, 0, tempLastName.Length);

                this.fileStream.Write(BitConverter.GetBytes(recordParams.Department));
                this.fileStream.Write(BitConverter.GetBytes(id));
                this.fileStream.Write(firstName);
                this.fileStream.Write(lastName);
                this.fileStream.Write(BitConverter.GetBytes(recordParams.DateOfBirth.Year));
                this.fileStream.Write(BitConverter.GetBytes(recordParams.DateOfBirth.Month));
                this.fileStream.Write(BitConverter.GetBytes(recordParams.DateOfBirth.Day));
                this.fileStream.Write(bytesSalary);
                this.fileStream.Write(BitConverter.GetBytes(recordParams.Class));
                this.fileStream.Write(new byte[] { 0 });
                this.fileStream.Position = this.fileStream.Length;

                return;
            }

            throw new KeyNotFoundException($"wrong {nameof(id)}");
        }

        /// <summary>
        /// Wheres the specified parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> Where(string param)
        {
            ValidateWhereParam(param);
            ValidateEntity entity = new ValidateEntity().Create(param, this);
            foreach (var record in entity.Filtering(this.GetRecords()))
            {
                yield return record;
            }
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            byte[] buffer = new byte[277];
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.fileStream.Position = 0;
            while (this.fileStream.Read(buffer, 0, 277) != 0)
            {
                try
                {
                    FileCabinetRecord record = this.RecordFromBytes(buffer);
                    result.Add(record);
                }
                catch (KeyNotFoundException)
                {
                }
            }

            return result;
        }

        /// <summary>
        /// Finds all records by Date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                foreach (var item in this.dateOfBirthDictionary[dateOfBirth])
                {
                    byte[] buffer = new byte[277];
                    this.fileStream.Position = item;
                    this.fileStream.Read(buffer, 0, 277);
                    FileCabinetRecord record = this.RecordFromBytes(buffer);
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds all records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                foreach (var item in this.firstNameDictionary[firstName?.ToUpperInvariant()])
                {
                    byte[] buffer = new byte[277];
                    this.fileStream.Position = item;
                    this.fileStream.Read(buffer, 0, 277);
                    FileCabinetRecord record = this.RecordFromBytes(buffer);
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Finds all records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                foreach (var item in this.lastNameDictionary[lastName?.ToUpperInvariant()])
                {
                    byte[] buffer = new byte[277];
                    this.fileStream.Position = item;
                    this.fileStream.Read(buffer, 0, 277);
                    FileCabinetRecord record = this.RecordFromBytes(buffer);
                    yield return record;
                }
            }
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.dictionaryId.Count;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return FileCabinetServiceSnapshot.MakeSnapshot(this.GetRecords().ToList());
        }

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <returns>
        /// Exceptions.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws when snapshot is null.</exception>
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
                    this.validator.ValidateCabinetRecord(RecordToParams(record));
                    if (this.dictionaryId.ContainsKey(record.Id))
                    {
                        this.EditRecord(record.Id, RecordToParams(record));
                    }
                    else
                    {
                        this.fileStream.Position = this.fileStream.Length;
                        this.dictionaryId.Add(record.Id, this.fileStream.Position);
                        AddToDictionary<string, long>(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), this.fileStream.Position);
                        AddToDictionary<string, long>(this.lastNameDictionary, record.LastName.ToUpperInvariant(), this.fileStream.Position);
                        AddToDictionary<DateTime, long>(this.dateOfBirthDictionary, record.DateOfBirth, this.fileStream.Position);

                        byte[] tempFirstName = Encoding.Default.GetBytes(record.FirstName);
                        byte[] tempLastName = Encoding.Default.GetBytes(record.LastName);
                        byte[] firstName = new byte[120];
                        byte[] lastName = new byte[120];
                        ToBytesDecimal toBytesDecimal = new ToBytesDecimal(record.Salary);
                        byte[] bytesSalary = BitConverter.GetBytes(toBytesDecimal.Bytes1).Concat(BitConverter.GetBytes(toBytesDecimal.Bytes2)).ToArray();
                        Array.Copy(tempFirstName, 0, firstName, 0, tempFirstName.Length);
                        Array.Copy(tempLastName, 0, lastName, 0, tempLastName.Length);
                        this.fileStream.Write(BitConverter.GetBytes(record.Department));
                        this.fileStream.Write(BitConverter.GetBytes(record.Id));
                        this.fileStream.Write(firstName);
                        this.fileStream.Write(lastName);
                        this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Year));
                        this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Month));
                        this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Day));
                        this.fileStream.Write(bytesSalary);
                        this.fileStream.Write(BitConverter.GetBytes(record.Class));
                        this.fileStream.Write(new byte[] { 0 });
                    }

                    this.id = Math.Max(this.id, record.Id);
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
        public void Remove(int id)
        {
            long position = this.dictionaryId[id];
            byte[] buffer = new byte[277];

            this.fileStream.Position = position;
            this.fileStream.Read(buffer, 0, 277);
            FileCabinetRecord record = this.RecordFromBytes(buffer);

            this.dateOfBirthDictionary[record.DateOfBirth].Remove(position);
            this.firstNameDictionary[record.FirstName.ToUpperInvariant()].Remove(position);
            this.lastNameDictionary[record.LastName.ToUpperInvariant()].Remove(position);
            this.dictionaryId.Remove(id);
            this.fileStream.Position = position + 276;
            this.fileStream.Write(new byte[] { 1 });
            this.deleteStat++;
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
            byte[] buffer = new byte[277];
            this.fileStream.Position = 0;
            long startPosition = 0;
            long endPosition = 0;
            while (this.fileStream.Read(buffer, 0, 277) != 0)
            {
                if (buffer[276] == 0)
                {
                    int validId = BitConverter.ToInt32(buffer, 2);
                    string firstName = Encoding.Default.GetString(buffer, 6, 120).Trim('\0');
                    string lastName = Encoding.Default.GetString(buffer, 126, 120).Trim('\0');
                    int year = BitConverter.ToInt32(buffer, 246);
                    int month = BitConverter.ToInt32(buffer, 250);
                    int day = BitConverter.ToInt32(buffer, 254);
                    DateTime dateOfBirth = new DateTime(year, month, day);

                    this.firstNameDictionary[firstName.ToUpperInvariant()].Add(startPosition);
                    this.lastNameDictionary[lastName.ToUpperInvariant()].Add(startPosition);
                    this.dateOfBirthDictionary[dateOfBirth].Add(startPosition);

                    this.dateOfBirthDictionary[dateOfBirth].Remove(this.fileStream.Position - 277);
                    this.firstNameDictionary[firstName.ToUpperInvariant()].Remove(this.fileStream.Position - 277);
                    this.lastNameDictionary[lastName.ToUpperInvariant()].Remove(this.fileStream.Position - 277);

                    this.dictionaryId[validId] = startPosition;
                    endPosition = this.fileStream.Position;
                    this.fileStream.Position = startPosition;
                    this.fileStream.Write(buffer);
                    startPosition = this.fileStream.Position;
                    this.fileStream.Position = endPosition;
                }
            }

            this.deleteStat = 0;
            this.fileStream.SetLength(this.dictionaryId.Count * 277);
        }

        /// <summary>
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throws when record is null.</exception>
        /// <exception cref="ArgumentException">Record id must be more than zero.
        /// or
        /// Such identifier already exists.</exception>
        public void Insert(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (record.Id == -1)
            {
                throw new ArgumentException("Record id must be more than zero.");
            }

            this.validator.ValidateCabinetRecord(RecordToParams(record));
            if (this.dictionaryId.ContainsKey(record.Id))
            {
                throw new ArgumentException("Such identifier already exists.");
            }
            else
            {
                this.fileStream.Position = this.fileStream.Length;
                this.dictionaryId.Add(record.Id, this.fileStream.Position);
                AddToDictionary<string, long>(this.firstNameDictionary, record.FirstName.ToUpperInvariant(), this.fileStream.Position);
                AddToDictionary<string, long>(this.lastNameDictionary, record.LastName.ToUpperInvariant(), this.fileStream.Position);
                AddToDictionary<DateTime, long>(this.dateOfBirthDictionary, record.DateOfBirth, this.fileStream.Position);

                byte[] tempFirstName = Encoding.Default.GetBytes(record.FirstName);
                byte[] tempLastName = Encoding.Default.GetBytes(record.LastName);
                byte[] firstName = new byte[120];
                byte[] lastName = new byte[120];
                ToBytesDecimal toBytesDecimal = new ToBytesDecimal(record.Salary);
                byte[] bytesSalary = BitConverter.GetBytes(toBytesDecimal.Bytes1).Concat(BitConverter.GetBytes(toBytesDecimal.Bytes2)).ToArray();
                Array.Copy(tempFirstName, 0, firstName, 0, tempFirstName.Length);
                Array.Copy(tempLastName, 0, lastName, 0, tempLastName.Length);
                this.fileStream.Write(BitConverter.GetBytes(record.Department));
                this.fileStream.Write(BitConverter.GetBytes(record.Id));
                this.fileStream.Write(firstName);
                this.fileStream.Write(lastName);
                this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Year));
                this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Month));
                this.fileStream.Write(BitConverter.GetBytes(record.DateOfBirth.Day));
                this.fileStream.Write(bytesSalary);
                this.fileStream.Write(BitConverter.GetBytes(record.Class));
                this.fileStream.Write(new byte[] { 0 });
            }

            this.id = Math.Max(this.id, record.Id);
        }

        /// <summary>
        /// Gets the delete stat.
        /// </summary>
        /// <returns>Number of delete records.</returns>
        public int GetDeleteStat()
        {
            return this.deleteStat;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.fileStream != null)
                {
                    this.fileStream.Close();
                    this.fileStream = null;
                }
            }
        }

        private static RecordParams RecordToParams(FileCabinetRecord record)
        {
            return new RecordParams(record.FirstName, record.LastName, record.DateOfBirth, record.Department, record.Salary, record.Class);
        }

        private static void AddToDictionary<TKey, TValue>(IDictionary<TKey, List<TValue>> dictioanry, TKey key, TValue value)
        {
            if (!dictioanry.ContainsKey(key))
            {
                dictioanry.Add(key, new List<TValue>());
            }

            dictioanry[key].Add(value);
        }

        private static void ValidateWhereParam(string param)
        {
            if (param is null)
            {
                throw new ArgumentNullException(param);
            }

            param = param.Replace("or", " ", StringComparison.InvariantCultureIgnoreCase);
            param = param.Replace("and", " ", StringComparison.InvariantCultureIgnoreCase);
            var validateParam = param.Replace("=", " ", StringComparison.InvariantCultureIgnoreCase)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim('\''))
                .ToArray();
            if (validateParam.Length % 2 != 0)
            {
                throw new ArgumentException("Not correct param string");
            }

            for (int i = 0; i < validateParam.Length / 2; i++)
            {
                Type type = typeof(FileCabinetRecord);
                if (!type.GetProperties().Select(x => x.Name.ToUpperInvariant()).Contains(validateParam[i * 2].ToUpperInvariant()))
                {
                    throw new ArgumentException(validateParam[i * 2]);
                }
            }
        }

        private FileCabinetRecord RecordFromBytes(byte[] buffer)
        {
            ToBytesDecimal toDecimal = default;
            int year;
            int month;
            int day;
            FileCabinetRecord record = new FileCabinetRecord();
            record.Department = BitConverter.ToInt16(buffer, 0);
            record.Id = BitConverter.ToInt32(buffer, 2);
            record.FirstName = Encoding.Default.GetString(buffer, 6, 120).Trim('\0');
            record.LastName = Encoding.Default.GetString(buffer, 126, 120).Trim('\0');
            year = BitConverter.ToInt32(buffer, 246);
            month = BitConverter.ToInt32(buffer, 250);
            day = BitConverter.ToInt32(buffer, 254);
            toDecimal.Bytes1 = BitConverter.ToInt64(buffer, 258);
            toDecimal.Bytes2 = BitConverter.ToInt64(buffer, 266);
            record.Salary = toDecimal.GetDecimal();
            record.Class = BitConverter.ToChar(buffer, 274);
            record.DateOfBirth = new DateTime(year, month, day);

            return buffer[276] == 0 ? record : throw new KeyNotFoundException();
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ToBytesDecimal
        {
            [FieldOffset(0)]
            public long Bytes1;
            [FieldOffset(8)]
            public long Bytes2;
            [FieldOffset(0)]
            private decimal number;

            public ToBytesDecimal(decimal number)
            {
                this.Bytes1 = 0;
                this.Bytes2 = 0;
                this.number = number;
            }

            public decimal GetDecimal()
            {
                return this.number;
            }
        }
    }
}
