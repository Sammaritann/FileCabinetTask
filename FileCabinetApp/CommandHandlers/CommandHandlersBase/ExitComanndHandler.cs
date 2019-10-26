using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
  public  class ExitComanndHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "EXIT")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            Console.WriteLine("Exiting an application...");
            Program.isRunning = false;
        }
    }
}
