using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
   public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        public DeleteCommandHandler(IFileCabinetService service):base(service)
        {

        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != "DELETE")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            int subIndex = commandRequest.Parameters.IndexOf("where ");

            if (subIndex == -1)
            {
                Console.WriteLine("Missed where.");
                return;
            }

            foreach (var record in this.Service.Where(commandRequest.Parameters.Substring(subIndex + 7)))
            {
                this.Service.Remove(record.Id);
            }
        }
    }
}
