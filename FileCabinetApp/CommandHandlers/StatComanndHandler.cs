using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class StatComanndHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "STAT")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            int recordsCount = Program.fileCabinetService.GetStat();
            int recordsDelete = Program.fileCabinetService.GetDeleteStat();
            Console.WriteLine($"{recordsCount} record(s).");
            Console.WriteLine($"{recordsDelete} record(s) delete.");
        }
    }
}
