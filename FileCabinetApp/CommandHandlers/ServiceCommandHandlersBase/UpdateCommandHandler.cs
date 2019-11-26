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
        private const char WhiteSpace = ' ';
        private const char Comma = ',';
        private const string SingleQuote = "\'";
        private const int FirstNameFlagIndex = 0;
        private const int LastNameFlagIndex = 1;
        private const int DateOfBirthFlagIndex = 2;
        private const int SalaryFlagIndex = 3;
        private const int DepartmentFlagIndex = 4;
        private const int ClassFlagIndex = 5;

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
                .Split(Comma, StringSplitOptions.RemoveEmptyEntries);

            BitArray flags = new BitArray(6, false);

            RecordParams recordParams = new RecordParams();

            foreach (var record in param)
            {
                var values = record.Replace('=', WhiteSpace)
                    .Replace(SingleQuote, string.Empty, StringComparison.InvariantCultureIgnoreCase)
                    .Split(WhiteSpace, StringSplitOptions.RemoveEmptyEntries);
                switch (values[0].ToUpperInvariant())
                {
                    case "FIRSTNAME":
                        recordParams.FirstName = values[1];
                        flags[FirstNameFlagIndex] = true;
                        break;
                    case "LASTNAME":
                        recordParams.LastName = values[1];
                        flags[LastNameFlagIndex] = true;
                        break;
                    case "DATEOFBIRTH":
                        if (DateTime.TryParseExact(values[1], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth))
                        {
                            recordParams.DateOfBirth = dateOfBirth;
                            flags[DateOfBirthFlagIndex] = true;
                        }

                        break;
                    case "SALARY":
                        if (decimal.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal salary))
                        {
                            recordParams.Salary = salary;
                            flags[SalaryFlagIndex] = true;
                        }

                        break;
                    case "DEPARTMENT":
                        if (short.TryParse(values[1], out short department))
                        {
                            recordParams.Department = department;
                            flags[DepartmentFlagIndex] = true;
                        }

                        break;
                    case "CLASS":
                        if (char.TryParse(values[1], out char clas))
                        {
                            recordParams.Class = clas;
                            flags[ClassFlagIndex] = true;
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
                    recordParams.FirstName = flags[FirstNameFlagIndex] ? recordParams.FirstName : record.FirstName;
                    recordParams.LastName = flags[LastNameFlagIndex] ? recordParams.LastName : record.LastName;
                    recordParams.DateOfBirth = flags[DateOfBirthFlagIndex] ? recordParams.DateOfBirth : record.DateOfBirth;
                    recordParams.Salary = flags[SalaryFlagIndex] ? recordParams.Salary : record.Salary;
                    recordParams.Department = flags[DepartmentFlagIndex] ? recordParams.Department : record.Department;
                    recordParams.Class = flags[ClassFlagIndex] ? recordParams.Class : record.Class;
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
