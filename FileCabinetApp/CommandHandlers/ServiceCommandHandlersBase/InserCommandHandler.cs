using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    public class InserCommandHandler : ServiceCommandHandlerBase
    {

        public InserCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != "INSERT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            string firstName = null;
            string lastName = null;
            DateTime dateOfBirth = default;
            short department = default;
            decimal salary = default;
            char clas = default;
            int id = default;
            Regex regex = new Regex(@"\(.\w*\,.\w*\,.\w*\,.\w*\,.\w*\,.\w*\,.\w*\) values \(\'?.*\'?\,\'?.*\'?\)");
            if (!regex.IsMatch(commandRequest.Parameters))
            {
                Console.WriteLine("Incorrect expression");
                return;
            }

            string param = commandRequest.Parameters.Replace("(", string.Empty, StringComparison.OrdinalIgnoreCase).Replace(")",string.Empty, StringComparison.OrdinalIgnoreCase);
            int subIndex = param.IndexOf(" values ", StringComparison.InvariantCultureIgnoreCase);
            var fields = param.Substring(0, subIndex)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);
            var values = param.Substring(subIndex + 8)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim('\'')).ToArray();
            if (fields.Length != values.Length || fields.Length != 7)
            {
                Console.WriteLine("The number of parameters does not match");
                return;
            }

            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i].ToUpperInvariant())
                {
                    case "ID":
                        if (!int.TryParse(values[i], out id))
                        {
                            id = -1;
                        }

                        break;
                    case "FIRSTNAME":
                        firstName = values[i];
                        break;
                    case "LASTNAME":
                        lastName = values[i];
                        break;
                    case "DATEOFBIRTH":
                        DateTime.TryParseExact(values[i], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth);
                        break;
                    case "SALARY":
                        if (!decimal.TryParse(values[i],NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture, out salary))
                        {
                            salary = -1;
                        }

                        break;
                    case "DEPARTMENT":
                        if (!short.TryParse(values[i], out department))
                        {
                            department = -1;
                        }

                        break;
                    case "CLASS":
                        if (!char.TryParse(values[i], out clas))
                        {
                            clas = ' ';
                        }

                        break;
                    default:
                        break;
                }
            }

            FileCabinetRecord record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Department = department,
                Salary = salary,
                Class = clas,
            };

            try
            {
                this.Service.Insert(record);
                Console.WriteLine("Record #{0} is created.", id);
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Please, correct your input.");
            }
        }
    }
}
