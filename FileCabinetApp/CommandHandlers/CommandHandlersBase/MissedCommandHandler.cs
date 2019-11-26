using System;
using System.Collections.Generic;
using FileCabinetApp.CommandHandlers.HelpersForHandler;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents missed command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public class MissedCommandHandler : CommandHandlerBase
    {
        private const int MistakesLimit = 3;

        private readonly string[] baseCommands = new string[]
        {
           "help",
           "exit",
           "stat",
           "create",
           "export",
           "import",
           "purge",
           "select",
           "insert",
           "delete",
           "update",
        };

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
            (string similarCommand, int dimension) simmularCommand = (string.Empty, int.MaxValue);
            List<string> similartCommands = new List<string>();
            foreach (var baseCommand in this.baseCommands)
            {
                int commandDimension = Metric.CalculateMetric(commandRequest.Command, baseCommand);

                if (commandDimension > MistakesLimit)
                {
                    continue;
                }

                if (commandDimension == simmularCommand.dimension)
                {
                    similartCommands.Add(baseCommand);
                }

                if (commandDimension < simmularCommand.dimension)
                {
                    simmularCommand.dimension = commandDimension;
                    simmularCommand.similarCommand = baseCommand;
                    similartCommands.Clear();
                    similartCommands.Add(baseCommand);
                }
            }

            string helpCommandString = similartCommands.Count > 1 ? "The most similar commands are" : "The most similar command is";

            Console.WriteLine(helpCommandString);

            foreach (var simularCommand in similartCommands)
            {
                Console.WriteLine(simularCommand);
            }

            Console.WriteLine();
            return;
        }
    }
}