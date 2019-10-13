using FileCabinetApp.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents file cabinet file system service.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    public class FileCabinetFileSystemService : IFileCabinetService
    {

        private FileStream fileStream;
        private readonly IRecordValidator validator;

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
            throw new NotImplementedException();
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
    }
}
