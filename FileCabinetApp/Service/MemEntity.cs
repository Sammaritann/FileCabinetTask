using System;
using System.Collections.Generic;
using System.Text;
using FileCabinetApp.CommandHandlers.ValidateHandler;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents memoization entity.
    /// </summary>
    public class MemEntity
    {
        private Dictionary<ValidateEntity, List<FileCabinetRecord>> memDictioanry = new Dictionary<ValidateEntity, List<FileCabinetRecord>>();

        /// <summary>
        /// Tries the get memory records.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>FileCabinetRecord.</returns>
        public List<FileCabinetRecord> TryGetMemRecords(ValidateEntity entity)
        {
            if (this.memDictioanry.ContainsKey(entity))
            {
                return this.memDictioanry[entity];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adds the record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="record">The record.</param>
        public void AddRecord(ValidateEntity entity, FileCabinetRecord record)
        {
            if (!this.memDictioanry.ContainsKey(entity))
            {
                this.memDictioanry.Add(entity, new List<FileCabinetRecord>());
            }

            this.memDictioanry[entity].Add(record);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.memDictioanry.Clear();
        }
    }
}
