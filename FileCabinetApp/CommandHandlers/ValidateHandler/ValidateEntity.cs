using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHandlers.ValidateHandler
{
    public class ValidateEntity
    {
        private List<Predicate<FileCabinetRecord>> predicates = new List<Predicate<FileCabinetRecord>>();
        private ValidateEntity nextEntities;
        private bool isOr = false;

        public ValidateEntity Create(string param)
        {
            if (param is null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            while (param.Length > 0)
            {
                if (this.isOr)
                {
                    nextEntities = new ValidateEntity().Create(param);
                    return this;
                }

                int andIndex = param.IndexOf(" and ");
                int orIndex = param.IndexOf(" or ");
                int subIndex = andIndex == -1 ? orIndex : Math.Min(andIndex, orIndex);

                if (subIndex == -1)
                {
                    predicates.Add(ValidateGenerator.Create(param));
                    return this;
                }
                this.predicates.Add(ValidateGenerator.Create(param.Substring(0, subIndex)));
                this.isOr = orIndex == subIndex ? true : false;
                param = this.isOr ? param.Substring(subIndex + 4) : param.Substring(subIndex + 5);
            }

            return this;
        }

        public IEnumerable<FileCabinetRecord> Filtering(IEnumerable<FileCabinetRecord> records)
        {
            HashSet<FileCabinetRecord> set = new HashSet<FileCabinetRecord>();
            foreach (var record in this.Invoke(records))
            {
                if (set.Add(record))
                {
                    yield return record;
                }
            }

        }

        private IEnumerable<FileCabinetRecord> Invoke(IEnumerable<FileCabinetRecord> records)
        {
            foreach (FileCabinetRecord record in records)
            {
                if (this.Sieve(record))
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
            foreach (var predicate in predicates)
            {
               if (predicate != null)
                {
                    if (!predicate(record))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
