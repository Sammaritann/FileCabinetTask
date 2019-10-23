using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        private FileStream fileStream;
        private int id = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService" /> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public FileCabinetFileSystemService(IRecordValidator validator)
        {
            this.validator = validator;
            this.fileStream = new FileStream("cabinet-records.db", FileMode.Create, FileAccess.ReadWrite);
        }

        /// <inheritdoc/>
        public int CreateRecord(RecordParams recordParams)
        {
            if (recordParams is null)
            {
                throw new ArgumentNullException($"{nameof(recordParams)} must nit be null");
            }

            this.validator.ValidateCabinetRecord(recordParams);

            this.dictionaryId.Add(this.id + 1, this.fileStream.Position);
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
            while (this.fileStream.Read(buffer, 0, 277) != 0)
            {
                if (BitConverter.ToInt32(buffer, 2) == id)
                {
                    this.fileStream.Position -= 277;

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
             }

            throw new KeyNotFoundException($"wrong {nameof(id)}");
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            int year;
            int month;
            int day;
            byte[] buffer = new byte[277];
            this.fileStream.Position = 0;
            while (this.fileStream.Read(buffer, 0, 277) != 0)
            {
                year = BitConverter.ToInt32(buffer, 246);
                month = BitConverter.ToInt32(buffer, 250);
                day = BitConverter.ToInt32(buffer, 254);

                if (dateOfBirth == new DateTime(year, month, day))
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
            }

            return result;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            byte[] buffer = new byte[277];
            this.fileStream.Position = 0;
            while (this.fileStream.Read(buffer, 0, 277) != 0)
            {
                if (firstName.ToUpperInvariant() == Encoding.Default.GetString(buffer, 6, 120).Trim('\0').ToUpperInvariant())
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
            }

            return result;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            byte[] buffer = new byte[277];
            this.fileStream.Position = 0;
            while (this.fileStream.Read(buffer, 0, 277) != 0)
            {
                if (lastName.ToUpperInvariant() == Encoding.Default.GetString(buffer, 126, 120).Trim('\0').ToUpperInvariant())
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
            }

            return result;
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

        /// <inheritdoc/>
        public int GetStat()
        {
            return this.dictionaryId.Count;
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
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
                    this.validator.ValidateCabinetRecord(RecordToParams(record));
                    if (this.dictionaryId.ContainsKey(record.Id))
                    {
                        this.EditRecord(record.Id, RecordToParams(record));
                    }
                    else
                    {
                        this.dictionaryId.Add(record.Id, this.fileStream.Position);
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
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine($"{record.Id}: {e.Message}");
                }
            }
        }


        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Remove(int id)
        {
            long position = this.dictionaryId[id];
            this.dictionaryId.Remove(id);
            this.fileStream.Position = position + 276;
            this.fileStream.Write(new byte[] { 1 });
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

        private FileCabinetRecord RecordFromBytes(byte[] buffer)
        {
            ToBytesDecimal toDecimal = default(ToBytesDecimal);
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
