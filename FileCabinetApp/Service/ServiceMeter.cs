﻿using System;
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
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            this.service = service;
            this.MemEntity = service.MemEntity;
        }

        /// <summary>
        /// Gets the memory entity.
        /// </summary>
        /// <value>
        /// The memory entity.
        /// </value>
        public MemEntity MemEntity { get; }

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
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Insert(FileCabinetRecord record)
        {
            var sw = Stopwatch.StartNew();
            this.service.Insert(record);
            sw.Stop();
            Console.WriteLine("Insert method execution duration is {0} ticks", sw.ElapsedTicks);
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

        /// <summary>
        /// Wheres the specified parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> Where(string param)
        {
            var sw = Stopwatch.StartNew();
            var result = this.service.Where(param);
            sw.Stop();
            Console.WriteLine("Where method execution duration is {0} ticks", sw.ElapsedTicks);
            return result;
        }
    }
}