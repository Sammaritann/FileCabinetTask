using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
  public  class ExitComanndHandler : CommandHandlerBase
    {
        private Action<bool> isRunning;

        public ExitComanndHandler(Action<bool> isRunning)
        {
            this.isRunning = isRunning;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "EXIT")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            Console.WriteLine("Exiting an application...");
            isRunning(false);
        }
    }
}
