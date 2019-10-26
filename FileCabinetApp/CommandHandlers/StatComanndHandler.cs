using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class StatComanndHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public StatComanndHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "STAT")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            int recordsCount = service.GetStat();
            int recordsDelete = service.GetDeleteStat();
            Console.WriteLine($"{recordsCount} record(s).");
            Console.WriteLine($"{recordsDelete} record(s) delete.");
        }
    }
}
