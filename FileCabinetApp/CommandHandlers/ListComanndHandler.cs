﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class ListComanndHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public ListComanndHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "LIST")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            foreach (FileCabinetRecord item in service.GetRecords())
            {
                Console.WriteLine(
                    "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                    item.Id,
                    item.FirstName,
                    item.LastName,
                    item.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture),
                    item.Department,
                    item.Salary,
                    item.Class);
            }
        }
    }
}
