using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents exit command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public class ExitComanndHandler : CommandHandlerBase
    {
        private readonly Action<bool> isRunning;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitComanndHandler"/> class.
        /// </summary>
        /// <param name="isRunning">The is running.</param>
        public ExitComanndHandler(Action<bool> isRunning)
        {
            this.isRunning = isRunning;
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

            if (commandRequest.Command.ToUpperInvariant() != "EXIT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            Console.WriteLine("Exiting an application...");
            this.isRunning(false);
        }
    }
}