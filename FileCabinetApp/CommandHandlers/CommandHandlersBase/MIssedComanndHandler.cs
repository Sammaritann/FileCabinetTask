using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents missed command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public class MissedComanndHandler : CommandHandlerBase
    {
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

            Console.WriteLine($"There is no '{commandRequest.Command}' command.");
            Console.WriteLine();
            return;
        }
    }
}