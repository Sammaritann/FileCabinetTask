using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents logger service.
    /// </summary>
    /// <seealso cref="FileCabinetApp.IFileCabinetService" />
    public class ServiceLogger : IFileCabinetService
    {
        private readonly string loggerPath = Directory.GetCurrentDirectory() + "logger.txt";
        private IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <exception cref="ArgumentNullException">Throws when service is null.</exception>
        public ServiceLogger(IFileCabinetService service)
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
        /// Determines whether the specified identifier contains identifier and measures time.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <c>true</c> if the specified identifier contains identifier; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsId(int id)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with id = {2}\n", DateTime.Now, "ContainsId()", id);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.ContainsId(id);

                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - ContainsId() returned \'{1}\'\n", DateTime.Now, result.ToString(CultureInfo.InvariantCulture));
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Creates the record and measures time.
        /// </summary>
        /// <param name="recordParams">The record parameters.</param>
        /// <returns>
        /// The identifier.
        /// </returns>
        public int CreateRecord(RecordParams recordParams)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with {2}\n", DateTime.Now, "CreateRecord()", recordParams);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.CreateRecord(recordParams);
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - Create() returned \'{1}\'\n", DateTime.Now, result.ToString(CultureInfo.InvariantCulture));
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Edits the record and measures time.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recordParams">The record parameters.</param>
        public void EditRecord(int id, RecordParams recordParams)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with id = {2} {3}\n", DateTime.Now, "EditRecord()", id, recordParams);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                this.service.EditRecord(id, recordParams);
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the delete stat and measures time.
        /// </summary>
        /// <returns>
        /// Number of delete records.
        /// </returns>
        public int GetDeleteStat()
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1}\n", DateTime.Now, "GetDeleteStat()");
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.GetDeleteStat();
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - GetDeleteStat() returned \'{1}\'\n", DateTime.Now, result.ToString(CultureInfo.InvariantCulture));
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the records and measures time.
        /// </summary>
        /// <returns>
        /// The records.
        /// </returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1}\n", DateTime.Now, "GetRecords()");
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.GetRecords();
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - GetRecords() returned \'{1}\'\n", DateTime.Now, result.ToString());
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Finds all records by Date and measures time.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with dateOfBirth = {2}\n", DateTime.Now, "FindByDateOfBirth()", dateOfBirth);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.FindByDateOfBirth(dateOfBirth);
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - FindByDateOfBirth() returned \'{1}\'\n", DateTime.Now, result.ToString());
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Finds all records by first name and measures time.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with firstName = {2}\n", DateTime.Now, "FindByFirstName()", firstName);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.FindByFirstName(firstName);
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - FindByFirstName() returned \'{1}\'\n", DateTime.Now, result.ToString());
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Finds all records by last name and measures time.
        /// </summary>
        /// <param name="lastName">The last name.</param>
        /// <returns>
        /// Found records.
        /// </returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with lastName = {2}\n", DateTime.Now, "FindByLastName()", lastName);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.FindByLastName(lastName);
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - FindByLastName() returned \'{1}\'\n", DateTime.Now, result.ToString());
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Gets the stat and measures time.
        /// </summary>
        /// <returns>
        /// Number of records.
        /// </returns>
        public int GetStat()
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1}\n", DateTime.Now, "GetStat()");
            File.AppendAllText(this.loggerPath, context);
            try
            {
                var result = this.service.GetStat();
                string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - GetStat() returned \'{1}\'\n", DateTime.Now, result.ToString(CultureInfo.InvariantCulture));
                File.AppendAllText(this.loggerPath, resultContext);
                return result;
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Inserts the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        public void Insert(FileCabinetRecord record)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with record = {2}\n", DateTime.Now, "Insert()", record);
            File.AppendAllText(this.loggerPath, context);
            try
            {
                this.service.Insert(record);
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Makes the snapshot and measures time.
        /// </summary>
        /// <returns>
        /// Snapshot.
        /// </returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1}\n", DateTime.Now, "MakeSnapshot()");
            File.AppendAllText(this.loggerPath, context);
            var result = this.service.MakeSnapshot();
            string resultContext = string.Format(CultureInfo.InvariantCulture, "{0} - MakeSnapshot() returned \'{1}\'\n", DateTime.Now, result.ToString());
            File.AppendAllText(this.loggerPath, resultContext);
            return result;
        }

        /// <summary>
        /// Purges this instance and measures time.
        /// </summary>
        public void Purge()
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1}\n", DateTime.Now, "Purge()");
            File.AppendAllText(this.loggerPath, context);
            this.service.Purge();
        }

        /// <summary>
        /// Removes the specified identifier and measures time.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void Remove(int id)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with id = {2}\n", DateTime.Now, "Remove()", id);
            File.AppendAllText(this.loggerPath, context);
            this.service.Remove(id);
        }

        /// <summary>
        /// Restores the specified snapshot and measures time.
        /// </summary>
        /// <param name="snapshot">The snapshot.</param>
        /// <returns>
        /// Exceptions.
        /// </returns>
        public IReadOnlyCollection<Exception> Restore(FileCabinetServiceSnapshot snapshot)
        {
            string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with {2}\n", DateTime.Now, "Restore()", snapshot);
            File.AppendAllText(this.loggerPath, context);
            var exceptions = this.service.Restore(snapshot);
            foreach (var exception in exceptions)
            {
                File.AppendAllText(this.loggerPath, string.Format(CultureInfo.InvariantCulture, "\t {0} \n", exception.Message));
            }

            return exceptions;
        }

        /// <summary>
        /// Wheres the specified parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> Where(string param)
        {
            try
            {
                string context = string.Format(CultureInfo.InvariantCulture, "{0} - Calling {1} with param = {2} \n", DateTime.Now, "Where()", param);
                File.AppendAllText(this.loggerPath, context);
                return this.service.Where(param);
            }
            catch (Exception e)
            {
                File.AppendAllText(this.loggerPath, e.ToString());
                throw;
            }
        }
    }
}