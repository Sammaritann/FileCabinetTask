using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
   public class ExportComanndHandler : ServiceCommandHandlerBase
    {

        public ExportComanndHandler(IFileCabinetService service):base(service)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "EXPORT")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            var param = commandRequest.Parameters.Split(' ');
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

            var snapshot = service.MakeSnapshot();

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
