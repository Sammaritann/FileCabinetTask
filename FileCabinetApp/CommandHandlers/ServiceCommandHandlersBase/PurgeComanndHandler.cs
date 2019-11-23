using System;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents purge command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class PurgeComanndHandler : ServiceCommandHandlerBase
    {
        private const string CommandName = "PURGE";

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeComanndHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public PurgeComanndHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        /// <exception cref="ArgumentNullException">Throws when commandRequest is null.</exception>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != CommandName)
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            try
            {
                this.Service.Purge();
            }
            catch (NotSupportedException)
            {
                Console.WriteLine("Purge not supported on this service.");
            }
        }
    }
}