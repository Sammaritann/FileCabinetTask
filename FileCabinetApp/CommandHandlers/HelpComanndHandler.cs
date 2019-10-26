using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class HelpComanndHandler : CommandHandlerBase
    {
        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "prints statistics on records", "The 'stat' command prints statistics on records." },
            new string[] { "create", "creates a new record", "The 'create' command creates a new record." },
            new string[] { "list", "prints list of records", "The 'list' command prints list of records." },
            new string[] { "edit", "edits a record", "The 'edit' command edits a record." },
            new string[] { "export", "eports records", "The 'export' command exports  records." },
            new string[] { "find", "finds  records", "The 'find' command finds records." },
            new string[] { "import", "imports  records", "The 'import' command imports records." },
            new string[] { "remove", "removes a record", "The 'remove' command removes a record." },
            new string[] { "purge", "perges records", "The 'purge' command purges  records." },
        };
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "HELP")
            {
                nextHandler.Handle(commandRequest);
                return;
            }
            if (!string.IsNullOrEmpty(commandRequest.Parameters))
            {
                int index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[CommandHelpIndex], commandRequest.Parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{commandRequest.Parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (string[] helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[CommandHelpIndex], helpMessage[DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
