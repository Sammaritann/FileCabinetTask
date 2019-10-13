using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents file cabinet file system service.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    public class FileCabinetFileSystemService : IFileCabinetService
    {
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
