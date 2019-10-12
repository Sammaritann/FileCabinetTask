﻿using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short department, decimal salary, char clas)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException($"{nameof(firstName)} must not be null or contain only spaces");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException($"{nameof(lastName)} must not be null or contain only spaces");
            }

            if ((firstName.Length < 2) || (firstName.Length > 60))
            {
                throw new ArgumentException($"{nameof(firstName)} length should be between 2 and 60");
            }

            if ((lastName.Length < 2) || (lastName.Length > 60))
            {
                throw new ArgumentException($"{nameof(lastName)} length should be between 2 and 60");
            }

            if (dateOfBirth < new DateTime(1950, 1, 1) || dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException($"{nameof(dateOfBirth)} shoud be between  01-Jan-1950 and now");
            }

            if (department <= 0)
            {
                throw new ArgumentException($"{nameof(department)} should be more than zero");
            }

            if (salary <= 0)
            {
                throw new ArgumentException($"{nameof(salary)} must be more than zero");
            }

            if (clas < 'A' || clas > 'Z')
            {
                throw new ArgumentException($"{nameof(clas)} should be between A and Z");
            }

            FileCabinetRecord record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Department = department,
                Salary = salary,
                Class = clas,
            };

            this.list.Add(record);

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short department, decimal salary, char clas)
        {
            FileCabinetRecord record = this.list.Find((x) => x.Id == id);

            if (record is null)
            {
                throw new ArgumentException($"wrong {nameof(id)}");
            }

            record.FirstName = firstName;
            record.LastName = lastName;
            record.DateOfBirth = dateOfBirth;
            record.Department = department;
            record.Salary = salary;
            record.Class = clas;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.list.FindAll((x) => x.FirstName.ToUpperInvariant() == firstName.ToUpperInvariant()).ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.list.FindAll((x) => x.LastName.ToUpperInvariant() == lastName.ToUpperInvariant()).ToArray();
        }
    }
}