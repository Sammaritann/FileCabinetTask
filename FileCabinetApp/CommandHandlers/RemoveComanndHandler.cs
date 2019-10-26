using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class RemoveComanndHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "REMOVE")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            if (!int.TryParse(commandRequest.Parameters, out int id))
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
    }
}
