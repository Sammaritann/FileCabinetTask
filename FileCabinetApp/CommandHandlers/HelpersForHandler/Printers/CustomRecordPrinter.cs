using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp.CommandHandlers.Printers
{
    /// <summary>
    /// Represents custom record printer.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.Printers.IRecordPrinter" />
    public class CustomRecordPrinter : IRecordPrinter
    {
        private const string AllProperties = "*";
        private const char VerticalBorder = '|';
        private const char WhiteSpace = ' ';
        private const char HorizontalBorder = '-';
        private const string Cross = "+";
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

        private string[] titles;
        private int[] lengths;
        private List<string>[] rows;

        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The record.</param>
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            this.Print(records, "id", "firstname", "lastname");
        }

        /// <summary>
        /// Prints the specified records.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <param name="columName">Name of the colum.</param>
        /// <exception cref="ArgumentNullException">Throws when records.</exception>
        public void Print(IEnumerable<FileCabinetRecord> records, params string[] columName)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            if (columName is null)
            {
                throw new ArgumentNullException(nameof(columName));
            }

            this.titles = columName.Select(x => x.ToUpperInvariant())
                .Where(x => this.titlesName.Contains(x))
                .ToArray();
            if (this.titles.Length == 0)
            {
                if (columName.Contains(AllProperties))
                {
                    this.titles = this.titlesName;
                }
                else
                {
                    return;
                }
            }

            this.lengths = new int[this.titles.Length];
            this.rows = new List<string>[this.titles.Length];
            for (int i = 0; i < this.titles.Length; i++)
            {
                this.rows[i] = new List<string>();
                this.lengths[i] = this.titles[i].Length;
            }

            this.AddRows(records);
            this.PrintHat();

            for (int i = 0; i < this.rows[0].Count; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < this.rows.Length; j++)
                {
                    if (this.titles[j] == this.titlesName[0] || this.titles[j] == this.titlesName[1] || this.titles[j] == this.titlesName[6])
                    {
                        line += string.Concat(VerticalBorder, WhiteSpace, this.rows[j][i].PadRight(this.lengths[j]), WhiteSpace);
                    }
                    else
                    {
                        line += string.Concat(VerticalBorder, WhiteSpace, this.rows[j][i].PadLeft(this.lengths[j]), WhiteSpace);
                    }
                }

                Console.WriteLine(line + VerticalBorder);
            }

            foreach (var length in this.lengths)
            {
                Console.Write(string.Concat(Cross, HorizontalBorder, new string(HorizontalBorder, length), HorizontalBorder));
            }

            Console.WriteLine(Cross);
        }

        private static string GetTitle(string title)
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
                _ => string.Empty,
            };
        }

        private void PrintHat()
        {
            foreach (var length in this.lengths)
            {
                Console.Write(string.Concat(Cross, HorizontalBorder, new string(HorizontalBorder, length), HorizontalBorder));
            }

            Console.WriteLine(Cross);

            string line = string.Empty;
            for (int i = 0; i < this.titles.Length; i++)
            {
                line += string.Concat(VerticalBorder, WhiteSpace, GetTitle(this.titles[i]).PadRight(this.lengths[i]), WhiteSpace);
            }

            Console.WriteLine(line + VerticalBorder);

            foreach (var length in this.lengths)
            {
                Console.Write(string.Concat(Cross, HorizontalBorder, new string(HorizontalBorder, length), HorizontalBorder));
            }

            Console.WriteLine(Cross);
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
    }
}
