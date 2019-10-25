using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.InpitValidator;

namespace FileCabinetApp.CommandHandlers
{
    public class CommandHandler : CommandHandlerBase
    {
        private Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistics on records", "The 'stat' command prints statistics on records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "prints list of records", "The 'list' command prints list of records." },
            new string[] { "edit", "edits a record", "The 'edit' command edits a record." },
            new string[] { "export", "eports records", "The 'export' command exports  records." },
            new string[] { "find", "finds  records", "The 'find' command finds records." },
            new string[] { "import", "imports  records", "The 'import' command imports records." },
            new string[] { "remove", "removes a record", "The 'remove' command removes a record." },
            new string[] { "purge", "perges records", "The 'purge' command purges  records." },
        };

        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            int index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(commandRequest.Command, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                this.commands[index].Item2(commandRequest.Parameters);
            }
            else
            {
                PrintMissedCommandInfo(commandRequest.Command);
            }

        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                int index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (string[] helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Stat(string parameters)
        {
            int recordsCount = Program.fileCabinetService.GetStat();
            int recordsDelete = Program.fileCabinetService.GetDeleteStat();
            Console.WriteLine($"{recordsCount} record(s).");
            Console.WriteLine($"{recordsDelete} record(s) delete.");
        }

        private static void Create(string parameters)
        {
            string firstName = null;
            string lastName = null;
            DateTime dateOfBirth;
            short department;
            decimal salary;
            char clas;

            ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);

            RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);
            int id = Program.fileCabinetService.CreateRecord(recordParams);
            Console.WriteLine("Record #{0} is created.", id);
        }

        private static void List(string parameters)
        {
            foreach (FileCabinetRecord item in Program.fileCabinetService.GetRecords())
            {
                Console.WriteLine(
                    "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                    item.Id,
                    item.FirstName,
                    item.LastName,
                    item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                    item.Department,
                    item.Salary,
                    item.Class);
            }
        }

        private static void Edit(string parameters)
        {
            string firstName;
            string lastName;
            DateTime dateOfBirth;
            short department;
            decimal salary;
            char clas;

            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("param not a number");
                return;
            }

            try
            {
                if (Program.fileCabinetService.ContainsId(id))
                {
                    ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);
                    RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);

                    Program.fileCabinetService.EditRecord(id, recordParams);
                }
                else
                {
                    Console.WriteLine("#id record is not found");
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("#id record is not found");
                return;
            }
        }

        private static void Find(string parameters)
        {
            string[] param = parameters.Split(' ');
            if (param.Length != 2)
            {
                Console.WriteLine("Invalid number of parameters");
                return;
            }

            if (param[0].ToUpperInvariant() == "FIRSTNAME")
            {
                foreach (FileCabinetRecord item in Program.fileCabinetService.FindByFirstName(param[1].Trim('\"')))
                {
                    Console.WriteLine(
                        "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                        item.Id,
                        item.FirstName,
                        item.LastName,
                        item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                        item.Department,
                        item.Salary,
                        item.Class);
                }
            }

            if (param[0].ToUpperInvariant() == "LASTNAME")
            {
                foreach (FileCabinetRecord item in Program.fileCabinetService.FindByLastName(param[1].Trim('\"')))
                {
                    Console.WriteLine(
                        "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                        item.Id,
                        item.FirstName,
                        item.LastName,
                        item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                        item.Department,
                        item.Salary,
                        item.Class);
                }
            }

            if (param[0].ToUpperInvariant() == "DATEOFBIRTH")
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParseExact(param[1].Trim('\"'), "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                {
                    Console.WriteLine("Invalid Date");
                    return;
                }

                foreach (FileCabinetRecord item in Program.fileCabinetService.FindByDateOfBirth(dateOfBirth))
                {
                    Console.WriteLine(
                        "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                        item.Id,
                        item.FirstName,
                        item.LastName,
                        item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                        item.Department,
                        item.Salary,
                        item.Class);
                }
            }
        }

        private static void Export(string parameters)
        {
            var param = parameters.Split(' ');
            if (param.Length != 2)
            {
                Console.WriteLine("Invalid number of parameters");
                return;
            }

            if (string.IsNullOrWhiteSpace(param[0]) || string.IsNullOrWhiteSpace(param[1]))
            {
                Console.WriteLine("Invalid parameters");
                return;
            }

            FileStream fileStream;
            try
            {
                string result;
                fileStream = new FileStream(param[1], FileMode.Open);
                do
                {
                    Console.Write("File is exist - rewrite {0}? [Y/n]", param[1]);
                    result = Console.ReadLine();
                    if (result.ToUpperInvariant() == "Y")
                    {
                        fileStream.SetLength(0);
                        break;
                    }

                    if (result.ToUpperInvariant() == "N")
                    {
                        fileStream.Close();
                        return;
                    }
                }
                while (true);
            }
            catch (FileNotFoundException)
            {
                fileStream = new FileStream(param[1], FileMode.Create);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Export failed: can't open file {0}", param[1]);
                return;
            }

            var snapshot = Program.fileCabinetService.MakeSnapshot();

            if (param[0].ToUpperInvariant() == "CSV")
            {
                snapshot.SaveToCsw(new StreamWriter(fileStream));
            }

            if (param[0].ToUpperInvariant() == "XML")
            {
                snapshot.SaveToXml(new StreamWriter(fileStream));
            }

            Console.WriteLine("All records are exported to file {0}", param[1]);
            fileStream.Close();
        }

        private static void Import(string parameters)
        {
            string[] param = parameters.Split(' ');

            if (param.Length != 2)
            {
                Console.WriteLine("Invalid number of parameters");
                return;
            }

            if (string.IsNullOrWhiteSpace(param[0]) || string.IsNullOrWhiteSpace(param[1]))
            {
                Console.WriteLine("Invalid parameters");
                return;
            }

            FileStream fileStream = null;

            try
            {
                fileStream = new FileStream(param[1], FileMode.Open);
            }
            catch (IOException)
            {
                fileStream?.Close();
                Console.WriteLine("Import failed: can't open file {0}", param[1]);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                return;
            }

            FileCabinetServiceSnapshot serviceSnapshot = new FileCabinetServiceSnapshot();
            if (param[0].ToUpperInvariant() == "CSV")
            {
                serviceSnapshot.LoadFromCsv(new StreamReader(fileStream, leaveOpen: true));
                Program.fileCabinetService.Restore(serviceSnapshot);
                Console.WriteLine("records were imported from {0}", param[1]);
            }

            if (param[0].ToUpperInvariant() == "XML")
            {
                serviceSnapshot.LoadFromXml(new StreamReader(fileStream, leaveOpen: true));
                Program.fileCabinetService.Restore(serviceSnapshot);
                Console.WriteLine("records were imported from {0}", param[1]);
            }

            fileStream.Close();
        }

        private static void Remove(string parameters)
        {
            if (!int.TryParse(parameters, out int id))
            {
                Console.WriteLine("param not a number");
                return;
            }

            try
            {
                Program.fileCabinetService.Remove(id);
                Console.WriteLine("Record #{0} is removed.", id);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Record #{0} doesn't exists", id);
            }
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }

        private static void Purge(string parameters)
        {
            Program.fileCabinetService.Purge();
        }

        private static T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        private static void ReadRecord(out string firstName, out string lastName, out DateTime dateOfBirth, out short department, out decimal salary, out char clas)
        {
            firstName = null;
            lastName = null;

            Console.Write("First name: ");
            firstName = ReadInput<string>(StringConverter, Program.inputValidator.FirstNameValidate);

            Console.Write("Last name: ");
            lastName = ReadInput(StringConverter, Program.inputValidator.LastNameValidate);

            Console.Write("Date of birth: ");
            dateOfBirth = ReadInput(DateConverter, Program.inputValidator.DateOfBirthValidate);

            Console.Write("Department: ");
            department = ReadInput(ShortConverter, Program.inputValidator.DepartmentValidate);

            Console.Write("Salary: ");
            salary = ReadInput(DecimalConverter, Program.inputValidator.SalaryValidate);

            Console.Write("Class: ");
            clas = ReadInput(CharConverter, Program.inputValidator.ClassValidate);
        }

        private static (bool, string, string) StringConverter(string str)
        {
            return (!string.IsNullOrEmpty(str), "string", str);
        }

        private static (bool, string, DateTime) DateConverter(string str)
        {
            return (DateTime.TryParseExact(str, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth), "date", dateOfBirth);
        }

        private static (bool, string, short) ShortConverter(string str)
        {
            return (short.TryParse(str, out short department), "short", department);
        }

        private static (bool, string, decimal) DecimalConverter(string str)
        {
            return (decimal.TryParse(str, out decimal salary), "decimal", salary);
        }

        private static (bool, string, char) CharConverter(string str)
        {
            return (char.TryParse(str, out char clas), "char", clas);
        }
    }
}
