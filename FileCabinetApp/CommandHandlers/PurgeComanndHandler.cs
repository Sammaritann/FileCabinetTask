using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class PurgeComanndHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public PurgeComanndHandler(IFileCabinetService service)
        {
            this.service = service;
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
