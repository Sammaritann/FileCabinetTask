using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents help command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.CommandHandlerBase" />
    public class HelpCommandHandler : CommandHandlerBase
    {
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistics on records", "The 'stat' command prints statistics on records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "purge", "purges records", "The 'purge' command purges  records." },
            new string[]
            {
                "export", "exports records", "The 'export' command exports  records." +
                $"{Environment.NewLine}Example: export csv test.csv",
            },
            new string[]
            {
                "import", "imports  records", "The 'import' command imports records." +
                $"{Environment.NewLine}Example: import xml test.xml",
            },
            new string[]
            {
                "select", "selects records", "The 'select' command selects records." +
                $"{Environment.NewLine}Example: select field_1, ..., field_n where condition_1 [or,and] condition_n " +
                $"{Environment.NewLine}Number of fields optional" +
                $"{Environment.NewLine}Number of condition is not limited" +
                $"{Environment.NewLine}Symbol \'*\' represents all fields",
            },
            new string[]
            {
                "update", "updates a record", "the 'update' command updates a record." +
                $"{Environment.NewLine}Example: update set field_1 = [new field value] , ..., field_n where condition_1 [or,and] condition_m " +
                $"{Environment.NewLine}Number of fields optional" +
                $"{Environment.NewLine}Number of condition is not limited",
            },
            new string[]
            {
                "insert", "inserts a record", "the 'insert' command inserts a record.",
                $"{Environment.NewLine}Example: insert (field_1, ..., field_n) values (value_1, ..., value_n)" +
                $"{Environment.NewLine}All fields must be entered",
            },
            new string[]
            {
                "delete", "deletes records", "the 'delete' command deletes records." +
                $"{Environment.NewLine}Example: delete where condition_1 [or,and] condition_n " +
                $"{Environment.NewLine}Number of condition is not limited",
            },
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

            if (commandRequest.Command.ToUpperInvariant() != "HELP")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            if (!string.IsNullOrEmpty(commandRequest.Parameters))
            {
                int index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[CommandHelpIndex], commandRequest.Parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{commandRequest.Parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (string[] helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}