using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
   public class StatComanndHandler : ServiceCommandHandlerBase
    {
        public StatComanndHandler(IFileCabinetService service) : base(service)
        {
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
