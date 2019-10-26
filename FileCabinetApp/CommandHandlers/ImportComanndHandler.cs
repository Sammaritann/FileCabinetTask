using FileCabinetApp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
  public class ImportComanndHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public ImportComanndHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "IMPORT")
            {
                nextHandler.Handle(commandRequest);
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
            if (param[0].ToUpperInvariant() == "CSV")
            {
                serviceSnapshot.LoadFromCsv(new StreamReader(fileStream, leaveOpen: true));
                service.Restore(serviceSnapshot);
                Console.WriteLine("records were imported from {0}", param[1]);
            }

            if (param[0].ToUpperInvariant() == "XML")
            {
                serviceSnapshot.LoadFromXml(new StreamReader(fileStream, leaveOpen: true));
                service.Restore(serviceSnapshot);
                Console.WriteLine("records were imported from {0}", param[1]);
            }

            fileStream.Close();
        }
    }
}
