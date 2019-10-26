using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
   public class PurgeComanndHandler : ServiceCommandHandlerBase
    {


        public PurgeComanndHandler(IFileCabinetService service):base(service)
        {
        }
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "PURGE")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            service.Purge();
        }
    }
}
