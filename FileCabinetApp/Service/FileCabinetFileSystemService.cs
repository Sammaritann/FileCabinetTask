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
        private FileStream fileStream;
        private readonly IRecordValidator validator;
        private int id = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFileSystemService"/> class.
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
                throw new ArgumentNullException($"{nameof(recordParams)} must not be null");
            }

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
            return this.id;
        }

        /// <inheritdoc/>
        public void EditRecord(int id, RecordParams recordParams)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public int GetStat()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
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
                this.fileStream.Close();
            }
        }
    }
}
