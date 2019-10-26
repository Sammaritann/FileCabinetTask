using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
   public class FindComanndHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public FindComanndHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "FIND")
            {
                nextHandler.Handle(commandRequest);
                return;
            }

            string[] param = commandRequest.Parameters.Split(' ');
            if (param.Length != 2)
            {
                Console.WriteLine("Invalid number of parameters");
                return;
            }

            if (param[0].ToUpperInvariant() == "FIRSTNAME")
            {
                foreach (FileCabinetRecord item in service.FindByFirstName(param[1].Trim('\"')))
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

            if (param[0].ToUpperInvariant() == "LASTNAME")
            {
                foreach (FileCabinetRecord item in service.FindByLastName(param[1].Trim('\"')))
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

            if (param[0].ToUpperInvariant() == "DATEOFBIRTH")
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParseExact(param[1].Trim('\"'), "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                {
                    Console.WriteLine("Invalid Date");
                    return;
                }

                foreach (FileCabinetRecord item in service.FindByDateOfBirth(dateOfBirth))
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
}
