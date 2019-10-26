using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
   public class RemoveComanndHandler : ServiceCommandHandlerBase
    {

        public RemoveComanndHandler(IFileCabinetService service):base(service)
        {

        }
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
                service.Remove(id);
                Console.WriteLine("Record #{0} is removed.", id);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Record #{0} doesn't exists", id);
            }
        }
    }
}
