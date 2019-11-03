using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp.CommandHandlers.Printers
{
    public class CustomRecordPrinter : IRecordPrinter
    {
        private string[] titles;
        private int[] lengths;
        private List<string>[] rows;
        private readonly string[] titlesName =
        {
            "FIRSTNAME",
            "LASTNAME",
            "ID",
            "DATEOFBIRTH",
            "SALARY",
            "DEPARTMENT",
            "CLASS",
        };

        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            this.Print(records, "id", "firstname", "lastname");
        }

        public void Print(IEnumerable<FileCabinetRecord> records, params string[] columName)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            this.titles = columName.Select(x => x.ToUpperInvariant())
                .Where(x => this.titlesName.Contains(x))
                .ToArray();
            this.lengths = new int[this.titles.Length];
            this.rows = new List<string>[this.titles.Length];
            for (int i = 0; i < this.titles.Length; i++)
            {
                this.rows[i] = new List<string>();
                this.lengths[i] = this.titles[i].Length;
            }

            this.AddRows(records);
            this.PrintHat();

            Console.WriteLine();
            for (int i = 0; i < this.rows[0].Count; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < this.rows.Length; j++)
                {
                    if (this.titles[j] == "FIRSTNAME" || this.titles[j] == "LASTNAME" || this.titles[j] == "CLASS")
                    {
                        line += "| " + this.rows[j][i].PadRight(this.lengths[j]) + ' ';
                    }
                    else
                    {
                        line += "| " + this.rows[j][i].PadLeft(this.lengths[j]) + ' ';
                    }
                }

                Console.WriteLine(line + "|");
                Console.WriteLine();
            }

            foreach (var length in this.lengths)
            {
                Console.Write("+-" + new string('-', length) + '-');
            }

            Console.WriteLine("+");
        }

        private void PrintHat()
        {
            foreach (var length in this.lengths)
            {
                Console.Write("+-" + new string('-', length) + '-');
            }

            Console.WriteLine("+");

            string line = string.Empty;
            for (int i = 0; i < this.titles.Length; i++)
            {
                line += "| " + this.GetTitle(this.titles[i]).PadRight(this.lengths[i]) + ' ';
            }

            Console.WriteLine(line + "|");

            foreach (var length in this.lengths)
            {
                Console.Write("+-" + new string('-', length) + '-');
            }

            Console.WriteLine("+");
        }

        private void AddRows(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                for (int i = 0; i < this.titles.Length; i++)
                {
                    switch (this.titles[i])
                    {
                        case "FIRSTNAME":
                            this.AddRow(record.FirstName, i);
                            break;
                        case "LASTNAME":
                            this.AddRow(record.LastName, i);
                            break;
                        case "ID":
                            this.AddRow(record.Id.ToString(CultureInfo.InvariantCulture), i);
                            break;
                        case "DATEOFBIRTH":
                            this.AddRow(record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture), i);
                            break;
                        case "SALARY":
                            this.AddRow(record.Salary.ToString(CultureInfo.InvariantCulture), i);
                            break;
                        case "DEPARTMENT":
                            this.AddRow(record.Department.ToString(CultureInfo.InvariantCulture), i);
                            break;
                        case "CLASS":
                            this.AddRow(record.Class.ToString(CultureInfo.InvariantCulture), i);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddRow(string row, int i)
        {
            this.rows[i].Add(row);

            if (row.Length > this.lengths[i])
            {
                this.lengths[i] = row.Length;
            }
        }

        private string GetTitle(string title)
        {
            return title switch
            {
                "FIRSTNAME" => "FirstName",
                "LASTNAME" => "LastName",
                "ID" => "Id",
                "DATEOFBIRTH" => "DateOfBirth",
                "SALARY" => "Salary",
                "DEPARTMENT" => "Department",
                "CLASS" => "Class",
            };
        }

    }
}
