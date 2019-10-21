using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.InpitValidator;

namespace FileCabinetApp
{
    /// <summary>
    /// Class program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Frigin Pavel";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
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
        };

        private static Dictionary<string, IRecordValidator> recordValidators = new Dictionary<string, IRecordValidator>
        {
            { "DEFAULT", new Validators.DefaultValidator() },
            { "CUSTOM", new Validators.DefaultValidator() },
        };

        private static Dictionary<string, IInputValidator> inputValidators = new Dictionary<string, IInputValidator>
        {
            { "DEFAULT", new Validators.InpitValidator.DefaultValidator() },
            { "CUSTOM", new Validators.InpitValidator.DefaultValidator() },
        };

        private static IFileCabinetService fileCabinetService;

        private static IInputValidator inputValidator;

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            if (args is null)
            {
                return;
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");

            if (args.Length == 0)
            {
                fileCabinetService = new FileCabinetMemoryService(recordValidators["DEFAULT"]);
                inputValidator = inputValidators["DEFAULT"];
                Console.WriteLine("Using DEFAULT validation rules.");
            }

            if (args.Length == 1)
            {
                var param = args[0].Split('=');

                if (param[0] == "--validation-rules")
                {
                        fileCabinetService = new FileCabinetMemoryService(recordValidators[param[1].ToUpperInvariant()]);
                        inputValidator = inputValidators[param[1].ToUpperInvariant()];
                        Console.WriteLine("Using {0} validation rules.", param[1].ToUpperInvariant());
                }

                if (param[0] == "--storage")
                {
                    if (param[1].ToUpperInvariant() == "MEMORY")
                    {
                        fileCabinetService = new FileCabinetMemoryService(recordValidators["DEFAULT"]);
                        inputValidator = inputValidators["DEFAULT"];
                        Console.WriteLine("Using DEFAULT validation rules.");
                    }

                    if (param[1].ToUpperInvariant() == "FILE")
                    {
                        fileCabinetService = new FileCabinetFileSystemService(recordValidators["DEFAULT"]);
                        inputValidator = inputValidators["DEFAULT"];
                        Console.WriteLine("Using DEFAULT validation rules.");
                    }
                }
            }

            if (args.Length == 2)
            {
                if (args[0] == "-v")
                {
                    fileCabinetService = new FileCabinetMemoryService(recordValidators[args[1].ToUpperInvariant()]);
                    inputValidator = inputValidators[args[1].ToUpperInvariant()];
                    Console.WriteLine("Using {0} validation rules.", args[1].ToUpperInvariant());
                }

                if (args[0] == "-s")
                {
                        if (args[1].ToUpperInvariant() == "MEMORY")
                        {
                            fileCabinetService = new FileCabinetMemoryService(recordValidators["DEFAULT"]);
                            inputValidator = inputValidators["DEFAULT"];
                            Console.WriteLine("Using DEFAULT validation rules.");
                        }

                        if (args[1].ToUpperInvariant() == "FILE")
                        {
                            fileCabinetService = new FileCabinetFileSystemService(recordValidators["DEFAULT"]);
                            inputValidator = inputValidators["DEFAULT"];
                            Console.WriteLine("Using DEFAULT validation rules.");
                        }
                }

                if (args[1] == "--storage")
                {
                    var param = args[0].Split('=');
                    var param1 = args[1].Split('=');

                    if (param1[1].ToUpperInvariant() == "MEMORY")
                    {
                        fileCabinetService = new FileCabinetMemoryService(recordValidators[param[1].ToUpperInvariant()]);
                        inputValidator = inputValidators[param[1].ToUpperInvariant()];
                        Console.WriteLine("Using {0} validation rules.", param[1].ToUpperInvariant());
                    }

                    if (param1[1].ToUpperInvariant() == "FILE")
                    {
                        fileCabinetService = new FileCabinetFileSystemService(recordValidators[param[1].ToUpperInvariant()]);
                        inputValidator = inputValidators[param[1].ToUpperInvariant()];
                        Console.WriteLine("Using {0} validation rules.", param[1].ToUpperInvariant());
                    }
                }
            }

            if (args.Length == 4)
            {
                if (args[2] == "-s")
                {
                    if (args[3].ToUpperInvariant() == "MEMORY")
                    {
                        fileCabinetService = new FileCabinetMemoryService(recordValidators[args[1].ToUpperInvariant()]);
                        inputValidator = inputValidators[args[1].ToUpperInvariant()];
                        Console.WriteLine("Using {0} validation rules.", args[1].ToUpperInvariant());
                    }

                    if (args[3].ToUpperInvariant() == "FILE")
                    {
                        fileCabinetService = new FileCabinetFileSystemService(recordValidators[args[1].ToUpperInvariant()]);
                        inputValidator = inputValidators[args[1].ToUpperInvariant()];
                        Console.WriteLine("Using {0} validation rules.", args[1].ToUpperInvariant());
                    }
                }
            }

            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                string[] inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                string command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                int index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    string parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
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
                int index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Stat(string parameters)
        {
            int recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
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
            int id = fileCabinetService.CreateRecord(recordParams);
            Console.WriteLine("Record #{0} is created.", id);
        }

        private static void List(string parameters)
        {
            foreach (FileCabinetRecord item in fileCabinetService.GetRecords())
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

            foreach (var item in Program.fileCabinetService.GetRecords())
            {
                if (item.Id == id)
                {
                    ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);
                    RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);

                    Program.fileCabinetService.EditRecord(id, recordParams);
                    return;
                }
            }

            Console.WriteLine("#id record is not found");
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
                foreach (FileCabinetRecord item in fileCabinetService.FindByFirstName(param[1].Trim('\"')))
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
                foreach (FileCabinetRecord item in fileCabinetService.FindByLastName(param[1].Trim('\"')))
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

                foreach (FileCabinetRecord item in fileCabinetService.FindByDateOfBirth(dateOfBirth))
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

            var snapshot = fileCabinetService.MakeSnapshot();

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

        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
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
            firstName = ReadInput<string>(StringConverter, inputValidator.FirstNameValidate);

            Console.Write("Last name: ");
            lastName = ReadInput(StringConverter, inputValidator.LastNameValidate);

            Console.Write("Date of birth: ");
            dateOfBirth = ReadInput(DateConverter, inputValidator.DateOfBirthValidate);

            Console.Write("Department: ");
            department = ReadInput(ShortConverter, inputValidator.DepartmentValidate);

            Console.Write("Salary: ");
            salary = ReadInput(DecimalConverter, inputValidator.SalaryValidate);

            Console.Write("Class: ");
            clas = ReadInput(CharConverter, inputValidator.ClassValidate);
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