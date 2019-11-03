using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        public UpdateCommandHandler(IFileCabinetService service) : base(service) { }
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != "UPDATE")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }


            int subIndex = commandRequest.Parameters.IndexOf(" where ");
            int startIndex = commandRequest.Parameters.IndexOf("set ");

            if (subIndex == -1)
            {
                Console.WriteLine("Missed where.");
                return;
            }

            if (startIndex == -1)
            {
                Console.WriteLine("Missed set.");
                return;
            }

            var param = commandRequest.Parameters.Substring(startIndex + 3, subIndex)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            string firstName = default;
            string lastName = default;
            DateTime dateOfBirth = default;
            short department = default;
            decimal salary = default;
            char clas = default;

            foreach (var record in param)
            {
                var values = record.Replace("=", " ",StringComparison.InvariantCultureIgnoreCase)
                    .Replace("\'",string.Empty,StringComparison.InvariantCultureIgnoreCase)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                switch (values[0].ToUpperInvariant())
                {
                    case "FIRSTNAME":
                        firstName = values[1];
                        break;
                    case "LASTNAME":
                        lastName = values[1];
                        break;
                    case "DATEOFBIRTH":
                        DateTime.TryParseExact(values[1], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);
                        break;
                    case "SALARY":
                        if (!decimal.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out salary))
                        {
                            salary = default;
                        }

                        break;
                    case "DEPARTMENT":
                        if (!short.TryParse(values[1], out department))
                        {
                            department = default;
                        }

                        break;
                    case "CLASS":
                        if (!char.TryParse(values[1], out clas))
                        {
                            clas = default;
                        }

                        break;
                    default:
                        break;
                }
            }



            RecordParams recordParams = new RecordParams();

            foreach (var record in this.Service.Where(commandRequest.Parameters.Substring(subIndex + 7)))
            {
                recordParams.FirstName = firstName == default ? record.FirstName : firstName;
                recordParams.LastName = lastName == default ? record.LastName : lastName;
                recordParams.DateOfBirth = dateOfBirth == default ? record.DateOfBirth : dateOfBirth;
                recordParams.Salary = salary == default ? record.Salary : salary;
                recordParams.Department = department == default ? record.Department : department;
                recordParams.Class = clas == default ? record.Class : clas;

                this.Service.EditRecord(record.Id,recordParams);
            }
        }
    }
}
