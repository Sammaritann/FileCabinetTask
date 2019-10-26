using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class PurgeComanndHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "PURGE")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            Program.fileCabinetService.Purge();
        }
    }
}
