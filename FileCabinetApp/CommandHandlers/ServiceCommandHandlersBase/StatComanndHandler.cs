using System;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents stat command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class StatComanndHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatComanndHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public StatComanndHandler(IFileCabinetService service)
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

            if (commandRequest.Command.ToUpperInvariant() != "STAT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            int recordsCount = this.Service.GetStat();
            int recordsDelete = this.Service.GetDeleteStat();
            Console.WriteLine($"{recordsCount} record(s).");
            Console.WriteLine($"{recordsDelete} record(s) delete.");
        }
    }
}