using System;
using System.IO;
using FileCabinetApp.Service;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents import command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class ImportComanndHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportComanndHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public ImportComanndHandler(IFileCabinetService service)
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

            if (commandRequest.Command.ToUpperInvariant() != "IMPORT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            string[] param = commandRequest.Parameters.Split(' ');

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
            try
            {
                if (param[0].ToUpperInvariant() == "CSV")
                {
                    serviceSnapshot.LoadFromCsv(new StreamReader(fileStream, leaveOpen: true));
                    this.Service.Restore(serviceSnapshot);
                    Console.WriteLine("records were imported from {0}", param[1]);
                }

                if (param[0].ToUpperInvariant() == "XML")
                {
                    serviceSnapshot.LoadFromXml(new StreamReader(fileStream, leaveOpen: true));
                    this.Service.Restore(serviceSnapshot);
                    Console.WriteLine("records were imported from {0}", param[1]);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Not correct format.");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Not correct format.");
            }

            fileStream.Close();
        }
    }
}