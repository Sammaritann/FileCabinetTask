using System;
using System.Collections;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents update command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private const string CommandName = "UPDATE";
        private const string WhiteSpace = " ";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

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

            if (commandRequest.Command.ToUpperInvariant() != CommandName)
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            int subIndex = commandRequest.Parameters.IndexOf(" where ", StringComparison.InvariantCultureIgnoreCase);
            int startIndex = commandRequest.Parameters.IndexOf("set ", StringComparison.InvariantCultureIgnoreCase);

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

            var param = commandRequest.Parameters.Substring(startIndex + "set".Length, subIndex)
                .Split(',', StringSplitOptions.RemoveEmptyEntries);

            BitArray flags = new BitArray(6, false);

            RecordParams recordParams = new RecordParams();

            foreach (var record in param)
            {
                var values = record.Replace("=", WhiteSpace, StringComparison.InvariantCultureIgnoreCase)
                    .Replace("\'", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                switch (values[0].ToUpperInvariant())
                {
                    case "FIRSTNAME":
                        recordParams.FirstName = values[1];
                        flags[0] = true;
                        break;
                    case "LASTNAME":
                        recordParams.LastName = values[1];
                        flags[1] = true;
                        break;
                    case "DATEOFBIRTH":
                        if (DateTime.TryParseExact(values[1], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth))
                        {
                            recordParams.DateOfBirth = dateOfBirth;
                            flags[2] = true;
                        }

                        break;
                    case "SALARY":
                        if (decimal.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal salary))
                        {
                            recordParams.Salary = salary;
                            flags[3] = true;
                        }

                        break;
                    case "DEPARTMENT":
                        if (short.TryParse(values[1], out short department))
                        {
                            recordParams.Department = department;
                            flags[4] = true;
                        }

                        break;
                    case "CLASS":
                        if (char.TryParse(values[1], out char clas))
                        {
                            recordParams.Class = clas;
                            flags[5] = true;
                        }

                        break;
                    default:
                        break;
                }
            }

            try
            {
                foreach (var record in this.Service.Where(commandRequest.Parameters.Substring(subIndex + 7)))
                {
                    recordParams.FirstName = flags[0] ? recordParams.FirstName : record.FirstName;
                    recordParams.LastName = flags[1] ? recordParams.LastName : record.LastName;
                    recordParams.DateOfBirth = flags[2] ? recordParams.DateOfBirth : record.DateOfBirth;
                    recordParams.Salary = flags[3] ? recordParams.Salary : record.Salary;
                    recordParams.Department = flags[4] ? recordParams.Department : record.Department;
                    recordParams.Class = flags[5] ? recordParams.Class : record.Class;
                    try
                    {
                        this.Service.EditRecord(record.Id, recordParams);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Parameter does not exist: {e.Message}");
                return;
            }

            this.Service.MemEntity.Clear();
        }
    }
}
