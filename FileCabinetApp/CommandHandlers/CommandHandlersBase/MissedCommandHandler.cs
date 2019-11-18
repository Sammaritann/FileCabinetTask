using System;
using System.Collections.Generic;

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
                int commandDimension = this.Metric(commandRequest.Command, baseCommand);

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

        private static int Min(int a, int b, int c)
        {
            return (a = a < b ? a : b) < c ? a : c;
        }

        private int Metric(string command, string baseCommand)
        {
            var n = command.Length + 1;
            var m = baseCommand.Length + 1;
            int[][] arrayD = new int[n][];
            for (int i = 0; i < n; i++)
            {
                arrayD[i] = new int[m];
            }

            for (var i = 0; i < n; i++)
            {
                arrayD[i][0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                arrayD[0][j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var cost = command[i - 1] == baseCommand[j - 1] ? 0 : 1;

                    arrayD[i][j] = Min(
                        arrayD[i - 1][j] + 1,
                        arrayD[i][j - 1] + 1,
                        arrayD[i - 1][j - 1] + cost);

                    if (i > 1 && j > 1
                        && command[i - 1] == baseCommand[j - 2]
                        && command[i - 2] == baseCommand[j - 1])
                    {
                        arrayD[i][j] = Math.Min(
                            arrayD[i][j],
                            arrayD[i - 2][j - 2] + cost);
                    }
                }
            }

            return arrayD[n - 1][m - 1];
        }
    }
}