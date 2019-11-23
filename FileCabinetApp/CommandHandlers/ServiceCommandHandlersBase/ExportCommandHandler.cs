﻿using System;
using System.IO;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents export command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class ExportCommandHandler : ServiceCommandHandlerBase
    {
        private const char Separator = ' ';
        private const string CommandName = "EXPORT";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ExportCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        /// <exception cref="ArgumentNullException">Throws when commandRequest is null.</exception>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != CommandName)
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            var param = commandRequest.Parameters.Split(Separator);
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
                try
                {
                 fileStream = new FileStream(param[1], FileMode.Create);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Wrong path.");
                    return;
                }
                finally
                {
                    fileStream?.Dispose();
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Export failed: can't open file {0}", param[1]);
                return;
            }

            FileCabinetServiceSnapshot snapshot;

            try
            {
                snapshot = this.Service.MakeSnapshot();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("File storage does not support export command.");
                fileStream.Close();
                return;
            }

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
    }
}