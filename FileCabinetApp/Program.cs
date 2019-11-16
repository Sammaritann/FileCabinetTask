using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.CommandHandlers.Printers;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase;
using FileCabinetApp.Service;
using FileCabinetApp.Validators;
using FileCabinetApp.Validators.InpitValidator;
using FileCabinetApp.Validators.RecordValidator;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    /// <summary>
    /// Class program.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Frigin Pavel";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static bool isRunning = true;

        private static IConfigurationRoot config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("validation-rules.json").Build();
        private static Dictionary<string, IRecordValidator> recordValidators = new Dictionary<string, IRecordValidator>
        {
            { "DEFAULT", new ValidatorBuilder().CreateDefault() },
            { "CUSTOM", new ValidatorBuilder().CreateCustom() },
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

            CommandLine.Parser.Default.ParseArguments<ConsoleOption>(args)
           .WithParsed<ConsoleOption>(opts => RunOption(opts));

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
            var createHandler = new CreateComanndHandler(Program.fileCabinetService, inputValidator);
            var exitHandler = new ExitComanndHandler(x => isRunning = x);
            var exportHandler = new ExportComanndHandler(Program.fileCabinetService);
            var helpHandler = new HelpComanndHandler();
            var importHandler = new ImportComanndHandler(Program.fileCabinetService);
            var missedHandler = new MissedComanndHandler();
            var purgeHandler = new PurgeComanndHandler(Program.fileCabinetService);
            var statHandler = new StatComanndHandler(Program.fileCabinetService);
            var selectHandler = new SelectComanndHandler(Program.fileCabinetService, new CustomRecordPrinter());
            var insertHandler = new InserCommandHandler(Program.fileCabinetService);
            var deleteHandler = new DeleteCommandHandler(Program.fileCabinetService);
            var updateHandler = new UpdateCommandHandler(Program.fileCabinetService);

            helpHandler.SetNext(createHandler);
            createHandler.SetNext(exitHandler);
            exitHandler.SetNext(exportHandler);
            exportHandler.SetNext(purgeHandler);
            purgeHandler.SetNext(statHandler);
            statHandler.SetNext(importHandler);
            importHandler.SetNext(selectHandler);
            selectHandler.SetNext(deleteHandler);
            deleteHandler.SetNext(updateHandler);
            updateHandler.SetNext(insertHandler);
            insertHandler.SetNext(missedHandler);

            return helpHandler;
        }

        private static IRecordValidator CreateDefault(this ValidatorBuilder validator)
        {
            var defaultConfig = config.GetSection("default");

            return validator
                .ValidateFirstName(defaultConfig.GetSection("firstName:min").Get<int>(), defaultConfig.GetSection("firstName:max").Get<int>())
                .ValidateLastName(defaultConfig.GetSection("lastName:min").Get<int>(), defaultConfig.GetSection("lastName:max").Get<int>())
                .ValidateDateOfBirthValidator(defaultConfig.GetSection("dateOfBirth:from").Get<DateTime>(), defaultConfig.GetSection("dateOfBirth:to").Get<DateTime>())
                .ValidateSalary(defaultConfig.GetSection("salary:min").Get<decimal>(), defaultConfig.GetSection("salary:max").Get<decimal>())
                .ValidateDepartment(defaultConfig.GetSection("department:from").Get<short>(), defaultConfig.GetSection("department:to").Get<short>())
                .ValidateClass(defaultConfig.GetSection("class:min").Get<char>(), defaultConfig.GetSection("class:max").Get<char>())
                .Create();
        }

        private static IRecordValidator CreateCustom(this ValidatorBuilder validator)
        {
            var customConfig = config.GetSection("custom");

            return validator
                .ValidateFirstName(customConfig.GetSection("firstName:min").Get<int>(), customConfig.GetSection("firstName:max").Get<int>())
                .ValidateLastName(customConfig.GetSection("lastName:min").Get<int>(), customConfig.GetSection("lastName:max").Get<int>())
                .ValidateDateOfBirthValidator(customConfig.GetSection("dateOfBirth:from").Get<DateTime>(), customConfig.GetSection("dateOfBirth:to").Get<DateTime>())
                .ValidateSalary(customConfig.GetSection("salary:min").Get<decimal>(), customConfig.GetSection("salary:max").Get<decimal>())
                .ValidateDepartment(customConfig.GetSection("department:from").Get<short>(), customConfig.GetSection("department:to").Get<short>())
                .ValidateClass(customConfig.GetSection("class:min").Get<char>(), customConfig.GetSection("class:max").Get<char>())
                .Create();
        }

        private static void RunOption(ConsoleOption opts)
        {
            if (opts.FileSystem.ToUpperInvariant() == "MEMORY")
            {
                fileCabinetService = new FileCabinetMemoryService(recordValidators[opts.Validator.ToUpperInvariant()]);
                inputValidator = inputValidators[opts.Validator.ToUpperInvariant()];
                Console.WriteLine("Using {0} validation rules.", opts.Validator.ToUpperInvariant());
            }

            if (opts.FileSystem.ToUpperInvariant() == "FILE")
            {
                fileCabinetService = new FileCabinetFileSystemService(recordValidators[opts.Validator.ToUpperInvariant()]);
                inputValidator = inputValidators[opts.Validator.ToUpperInvariant()];
                Console.WriteLine("Using {0} validation rules.", opts.Validator.ToUpperInvariant());
            }

            if (opts.Watch)
            {
                fileCabinetService = new ServiceMeter(fileCabinetService);
            }

            if (opts.Logger)
            {
                fileCabinetService = new ServiceLogger(fileCabinetService);
            }
        }
    }
}