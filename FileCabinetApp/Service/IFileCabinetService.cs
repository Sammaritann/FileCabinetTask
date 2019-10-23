using System;
using System.Collections.Generic;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Represent file cabinet service .
    /// </summary>
    public interface IFileCabinetService
    {
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
        /// Finds all records by first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>Found records.</returns>
        IReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds all records by last name.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>Found records.</returns>
        IReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds all records by Date.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>Found records.</returns>
        IReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

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
    }
}
