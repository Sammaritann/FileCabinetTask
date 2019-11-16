using System;
using System.Collections.Generic;
using System.Linq;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ValidateHandler
{
    /// <summary>
    /// Represents validate entity.
    /// </summary>
    public class ValidateEntity
    {
        private MemEntity memEntity;
        private List<(Predicate<FileCabinetRecord> predicate, string explanation)> predicates = new List<(Predicate<FileCabinetRecord>, string)>();
        private ValidateEntity nextEntities;
        private bool isOr = false;

        /// <summary>
        /// Creates the specified parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="memEntity">The memory entity.</param>
        /// <returns>
        /// ValidateEntity.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws when param is null.</exception>
        public ValidateEntity Create(string param, MemEntity memEntity)
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            this.memEntity = memEntity;

            while (param.Length > 0)
            {
                if (this.isOr)
                {
                    this.nextEntities = new ValidateEntity().Create(param, memEntity);
                    return this;
                }

                int andIndex = param.IndexOf(" and ", StringComparison.InvariantCultureIgnoreCase);
                int orIndex = param.IndexOf(" or ", StringComparison.InvariantCultureIgnoreCase);
                int subIndex = andIndex == -1 ? orIndex : Math.Min(andIndex, orIndex);

                if (subIndex == -1)
                {
                    this.predicates.Add(ValidateGenerator.Create(param));
                    return this;
                }

                this.predicates.Add(ValidateGenerator.Create(param.Substring(0, subIndex)));
                this.isOr = orIndex == subIndex ? true : false;
                param = this.isOr ? param.Substring(subIndex + 4) : param.Substring(subIndex + 5);
            }

            return this;
        }

        /// <summary>
        /// Filterings the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns>FileCabinetRecord.</returns>
        /// <exception cref="ArgumentNullException">Throws when records is null.</exception>
        public IEnumerable<FileCabinetRecord> Filtering(IEnumerable<FileCabinetRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            HashSet<FileCabinetRecord> set = new HashSet<FileCabinetRecord>();
            foreach (var record in this.Invoke(records))
            {
                if (set.Add(record))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ValidateEntity entity ? this.Equals(entity) : false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.predicates.Count;
        }

        private IEnumerable<FileCabinetRecord> Invoke(IEnumerable<FileCabinetRecord> records)
        {
            List<FileCabinetRecord> invokeResult;
            if ((invokeResult = this.memEntity.TryGetMemRecords(this)) is null)
            {
                foreach (FileCabinetRecord record in records)
                {
                    if (this.Sieve(record))
                    {
                        this.memEntity.AddRecord(this, record);
                        yield return record;
                    }
                }
            }
            else
            {
                foreach (var record in invokeResult)
                {
                    yield return record;
                }
            }

            if (this.nextEntities != null)
            {
                foreach (var record in this.nextEntities.Invoke(records))
                {
                        yield return record;
                }
            }
        }

        private bool Sieve(FileCabinetRecord record)
        {
            foreach (var predicate in this.predicates)
            {
               if (predicate.predicate != null)
                {
                    if (!predicate.predicate(record))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool Equals(ValidateEntity obj)
        {
            if (this.predicates.Count != obj.predicates.Count)
            {
                return false;
            }

            for (int i = 0; i < this.predicates.Count; i++)
            {
                if (this.predicates[i].explanation != obj.predicates[i].explanation)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
