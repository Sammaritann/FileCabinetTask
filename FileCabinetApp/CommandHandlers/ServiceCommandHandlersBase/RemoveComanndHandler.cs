using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents remove command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class RemoveComanndHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveComanndHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public RemoveComanndHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != "REMOVE")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            if (!int.TryParse(commandRequest.Parameters, out int id))
            {
                Console.WriteLine("param not a number");
                return;
            }

            try
            {
                this.Service.Remove(id);
                Console.WriteLine("Record #{0} is removed.", id);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Record #{0} doesn't exists", id);
            }
        }
    }
}