using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class MIssedComanndHandler : CommandHandlerBase
    {
        public override void Handle(AppCommandRequest commandRequest)
        {
            Console.WriteLine($"There is no '{commandRequest.Command}' command.");
            Console.WriteLine();
            return;
        }
    }
}
