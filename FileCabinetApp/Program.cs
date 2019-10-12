using System;
using System.Globalization;

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
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistics on records", "The 'stat' command prints statistics on records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "prints list of records", "The 'list' command prints list of records." },
            new string[] { "edit", "edits a record", "The 'edit' command edits a record." },
            new string[] { "find", "finds a records", "The 'find' command finds a records." },
        };

        private static FileCabinetService fileCabinetService = new FileCabinetService();

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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

            ValidateRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);

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

            if (Array.FindIndex(Program.fileCabinetService.GetRecords(), (x) => x.Id == id) == -1)
            {
                Console.WriteLine("#id record is not found");
                return;
            }

            ValidateRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);
            RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);

            Program.fileCabinetService.EditRecord(id, recordParams);
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

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void ValidateRecord(out string firstName, out string lastName, out DateTime dateOfBirth, out short department, out decimal salary, out char clas)
        {
            firstName = null;
            lastName = null;

            while (string.IsNullOrWhiteSpace(firstName))
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();
            }

            while (string.IsNullOrWhiteSpace(lastName))
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();
            }

            Console.Write("Date of birth: ");
            while (!DateTime.TryParseExact(Console.ReadLine(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
            {
                Console.WriteLine("Invalid Date");
                Console.WriteLine("Date must be format mm/dd/yyyy");
                Console.Write("Date of birth: ");
            }

            Console.Write("Department: ");
            while (!short.TryParse(Console.ReadLine(), out department))
            {
                Console.WriteLine("Invalid Department");
                Console.Write("Department: ");
            }

            Console.Write("Salary: ");
            while (!decimal.TryParse(Console.ReadLine(), out salary))
            {
                Console.WriteLine("Invalid salary");
                Console.Write("Salary: ");
            }

            Console.Write("Class: ");
            while (!char.TryParse(Console.ReadLine(), out clas))
            {
                Console.WriteLine("Invalid class");
                Console.Write("Class: ");
            }
        }
    }
}