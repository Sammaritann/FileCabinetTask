using FileCabinetApp.Service.Iterator;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents meter service.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    public class ServiceMeter : IFileCabinetService
    {
        private IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceMeter"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Determines whether the specified identifier contains identifier and logs.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified identifier contains identifier; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsId(int id)
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.ContainsId(id);
            sw.Stop();
            Console.WriteLine("ContainsId method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Creates the record and logs.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <returns>
        /// The identifier.
        /// </returns>
        public int CreateRecord(RecordParams recordParams)
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.CreateRecord(recordParams);
            sw.Stop();
            Console.WriteLine("CreateRecord method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Edits the record and logs.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recordParams">The record parameters.</param>
        public void EditRecord(int id, RecordParams recordParams)
        {
            var sw = Stopwatch.StartNew();
            this.service.EditRecord(id, recordParams);
            sw.Stop();
            Console.WriteLine("EditRecord method execution duration is {0} ticks", sw.ElapsedTicks);
        }

        /// <summary>
        /// Finds all records by Date and logs.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.FindByDateOfBirth(dateOfBirth);
            sw.Stop();
            Console.WriteLine("FindByDateOfBirth method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Finds all records by first name and logs.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.FindByFirstName(firstName);
            sw.Stop();
            Console.WriteLine("FindByFirstName method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Finds all records by last name and logs.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.FindByLastName(lastName);
            sw.Stop();
            Console.WriteLine("FindByLastName method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Gets the delete stat and logs.
        /// </summary>
        /// <returns>
        /// Number of delete records.
        /// </returns>
        public int GetDeleteStat()
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.GetDeleteStat();
            sw.Stop();
            Console.WriteLine("GetDeleteStat method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Gets the records and logs.
        /// </summary>
        /// <returns>
        /// The records.
        /// </returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.GetRecords();
            sw.Stop();
            Console.WriteLine("GetRecords method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Gets the stat and logs.
        /// </summary>
        /// <returns>
        /// Number of records.
        /// </returns>
        public int GetStat()
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.GetStat();
            sw.Stop();
            Console.WriteLine("GetStat method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Makes the snapshot and logs.
        /// </summary>
        /// <returns>
        /// Snapshot.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.MakeSnapshot();
            sw.Stop();
            Console.WriteLine("MakeSnapshot method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }

        /// <summary>
        /// Purges this instance and logs.
        /// </summary>
        public void Purge()
        {
            var sw = Stopwatch.StartNew();
            this.service.Purge();
            sw.Stop();
            Console.WriteLine("Purge method execution duration is {0} ticks", sw.ElapsedTicks);
        }

        /// <summary>
        /// Removes the specified identifier and logs.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Remove(int id)
        {
            var sw = Stopwatch.StartNew();
            this.service.Remove(id);
            sw.Stop();
            Console.WriteLine("Remove method execution duration is {0} ticks", sw.ElapsedTicks);
        }

        /// <summary>
        /// Restores the specified snapshot and logs.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            var sw = Stopwatch.StartNew();
            this.service.Restore(snapshot);
            sw.Stop();
            Console.WriteLine("Restore method execution duration is {0} ticks", sw.ElapsedTicks);
        }
    }
}