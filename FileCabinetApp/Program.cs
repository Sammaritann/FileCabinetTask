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
            { "DEFAULT", new Validators.InpitValidator.CustomValidator(config.GetSection("default")) },
            { "CUSTOM", new Validators.InpitValidator.CustomValidator(config.GetSection("default")) },
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
            var createHandler = new CreateCommandHandler(Program.fileCabinetService, inputValidator);
            var exitHandler = new ExitCommandHandler(x => isRunning = x);
            var exportHandler = new ExportCommandHandler(Program.fileCabinetService);
            var helpHandler = new HelpCommandHandler();
            var importHandler = new ImportCommandHandler(Program.fileCabinetService);
            var missedHandler = new MissedCommandHandler();
            var purgeHandler = new PurgeComanndHandler(Program.fileCabinetService);
            var statHandler = new StatComanndHandler(Program.fileCabinetService);
            var selectHandler = new SelectComanndHandler(Program.fileCabinetService, new CustomRecordPrinter());
            var insertHandler = new InsertCommandHandler(Program.fileCabinetService);
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

            return CreateValidator(validator, defaultConfig);
        }

        private static IRecordValidator CreateCustom(this ValidatorBuilder validator)
        {
            var customConfig = config.GetSection("custom");

            return CreateValidator(validator, customConfig);
        }

        private static IRecordValidator CreateValidator(this ValidatorBuilder validator, IConfigurationSection configuration)
        {
            return validator
                .ValidateFirstName(configuration.GetSection("firstName:min").Get<int>(), configuration.GetSection("firstName:max").Get<int>())
                .ValidateLastName(configuration.GetSection("lastName:min").Get<int>(), configuration.GetSection("lastName:max").Get<int>())
                .ValidateDateOfBirthValidator(configuration.GetSection("dateOfBirth:from").Get<DateTime>(), configuration.GetSection("dateOfBirth:to").Get<DateTime>())
                .ValidateSalary(configuration.GetSection("salary:min").Get<decimal>(), configuration.GetSection("salary:max").Get<decimal>())
                .ValidateDepartment(configuration.GetSection("department:from").Get<short>(), configuration.GetSection("department:to").Get<short>())
                .ValidateClass(configuration.GetSection("class:min").Get<char>(), configuration.GetSection("class:max").Get<char>())
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