using System.Collections.Generic;
using FileCabinetApp.Service;

namespace FileCabinetApp
{
    /// <summary>
    /// Represent file cabinet service .
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Gets the memory entity.
        /// </summary>
        /// <value>
        /// The memory entity.
        /// </value>
        public MemEntity MemEntity { get; }

        /// <summary>
        /// Creates the record.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <returns>The identifier.</returns>
        public int CreateRecord(RecordParams recordParams);

        /// <summary>
        /// Edits the record.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recordParams">The record parameters.</param>
        /// <exception cref="KeyNotFoundException">Throws when id not found.</exception>
        public void EditRecord(int id, RecordParams recordParams);

        /// <summary>
        /// Determines whether the specified identifier contains identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified identifier contains identifier; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsId(int id);

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <returns>The records.</returns>
        IReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets the stat.
        /// </summary>
        /// <returns>Number of records.</returns>
        int GetStat();

        /// <summary>
        /// Wheres the specified parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>FileCabinetRecord.</returns>
        IEnumerable<FileCabinetRecord> Where(string param);

        /// <summary>
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        void Insert(FileCabinetRecord record);

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot();

        /// <summary>
        /// Restores the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot);

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="KeyNotFoundException">Throws when id not found.</exception>
        public void Remove(int id);

        /// <summary>
        /// Purges this instance.
        /// </summary>
        public void Purge();

        /// <summary>
        /// Gets the delete stat.
        /// </summary>
        /// <returns>Number of delete records.</returns>
        public int GetDeleteStat();
    }
}
