using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.CommandHandlers;
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

        public static bool isRunning = true;

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

        public static IInputValidator inputValidator;

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

            var commandHandler = CreateCommandHandler();

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

                string parameters = inputs.Length > 1 ? inputs[1] : string.Empty;
                commandHandler.Handle(new AppCommandRequest(command, parameters));
            }
            while (isRunning);
        }

        private static ICommandHandler CreateCommandHandler()
        {
            var createHandler = new CreateComanndHandler(Program.fileCabinetService);
            var editHandler = new EditComanndHandler(Program.fileCabinetService);
            var exitHandler = new ExitComanndHandler();
            var exportHandler = new ExportComanndHandler(Program.fileCabinetService);
            var findHandler = new FindComanndHandler(Program.fileCabinetService);
            var helpHandler = new HelpComanndHandler();
            var listHandler = new ListComanndHandler(Program.fileCabinetService);
            var importHandler = new ImportComanndHandler(Program.fileCabinetService);
            var missedHandler = new MIssedComanndHandler();
            var purgeHandler = new PurgeComanndHandler(Program.fileCabinetService);
            var removeHandler = new RemoveComanndHandler(Program.fileCabinetService);
            var statHandler = new StatComanndHandler(Program.fileCabinetService);

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(editHandler);
            editHandler.SetNext(exitHandler);
            exitHandler.SetNext(exportHandler);
            exportHandler.SetNext(findHandler);
            findHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(removeHandler);
            removeHandler.SetNext(statHandler);
            statHandler.SetNext(listHandler);
            listHandler.SetNext(importHandler);
            importHandler.SetNext(missedHandler);

            return helpHandler;

        }
    }
}