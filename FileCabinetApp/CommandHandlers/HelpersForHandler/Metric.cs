using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.HelpersForHandler
{
    /// <summary>
    /// Represents a class for calculating the metric.
    /// </summary>
    public static class Metric
    {
        /// <summary>
        /// Calculates the metric.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="baseCommand">The base command.</param>
        /// <returns>
        /// Metric.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Throws when
        /// command
        /// or
        /// baseCommand
        /// is null.
        /// </exception>
        public static int CalculateMetric(string command, string baseCommand)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (baseCommand is null)
            {
                throw new ArgumentNullException(nameof(baseCommand));
            }

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

        private static int Min(int a, int b, int c)
        {
            return (a = a < b ? a : b) < c ? a : c;
        }
    }
}
