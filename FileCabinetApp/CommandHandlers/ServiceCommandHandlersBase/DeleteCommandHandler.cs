using System;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents delete command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class DeleteCommandHandler : ServiceCommandHandlerBase
    {
        private const string CommandName = "DELETE";

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public DeleteCommandHandler(IFileCabinetService service)
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

            int subIndex = commandRequest.Parameters.Trim().IndexOf("where ", StringComparison.InvariantCultureIgnoreCase);

            if (subIndex == -1)
            {
                Console.WriteLine("Missed where.");
                return;
            }

            try
            {
                foreach (var record in this.Service.Where(commandRequest.Parameters.Substring(subIndex + "where".Length).Trim()))
                {
                    this.Service.Remove(record.Id);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Parameter does not exist: {e.Message}");
                return;
            }

            this.Service.MemEntity.Clear();
        }
    }
}
