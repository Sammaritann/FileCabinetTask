using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents insert command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private const string OpenBracket = "(";
        private const string CloseBracket = ")";
        private const char Comma = ',';
        private const char WhiteSpace = ' ';
        private const char Slash = '\'';
        private const string CommandName = "INSERT";
        private const int FieldsNumber = 7;
        private const int IDFlagIndex = 0;
        private const int FirstNameFlagIndex = 1;
        private const int LastNameFlagIndex = 2;
        private const int DateOfBirthFlagIndex = 3;
        private const int SalaryFlagIndex = 4;
        private const int DepartmentFlagIndex = 5;
        private const int ClassFlagIndex = 6;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        /// <exception cref="ArgumentNullException">Throws when commandRequesst is null.</exception>
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

            BitArray flags = new BitArray(7, false);
            FileCabinetRecord record = new FileCabinetRecord();
            Regex regex = new Regex(@"\(\s*\w*\s*\,\s*\w*\s*\,\s*\w*\s*\,\s*\w*\s*\,\s*\w*\s*\,\s*\w*\s*\,\s*\w*\s*\) values \(\'?.*\'?\,\'?.*\'?\)");
            if (!regex.IsMatch(commandRequest.Parameters))
            {
                Console.WriteLine("Incorrect expression");
                return;
            }

            string param = commandRequest.Parameters.Replace(OpenBracket, string.Empty, StringComparison.OrdinalIgnoreCase).Replace(CloseBracket, string.Empty, StringComparison.OrdinalIgnoreCase);
            int subIndex = param.IndexOf(" values ", StringComparison.InvariantCultureIgnoreCase);
            var fields = param.Substring(0, subIndex)
                .Split(Comma, StringSplitOptions.RemoveEmptyEntries);
            var values = param.Substring(subIndex + "values ".Length)
                .Split(Comma, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim(Slash, WhiteSpace)).ToArray();
            if (fields.Length != values.Length || fields.Length != FieldsNumber)
            {
                Console.WriteLine("The number of parameters does not match");
                return;
            }

            for (int i = 0; i < fields.Length; i++)
            {
                switch (fields[i].ToUpperInvariant().Trim())
                {
                    case "ID":
                        if (int.TryParse(values[i], out int id))
                        {
                            record.Id = id;
                            flags[IDFlagIndex] = true;
                        }

                        break;
                    case "FIRSTNAME":
                        record.FirstName = values[i];
                        flags[FirstNameFlagIndex] = true;
                        break;
                    case "LASTNAME":
                        record.LastName = values[i];
                        flags[LastNameFlagIndex] = true;
                        break;
                    case "DATEOFBIRTH":
                        if (DateTime.TryParseExact(values[i], "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth))
                        {
                            record.DateOfBirth = dateOfBirth;
                            flags[DateOfBirthFlagIndex] = true;
                        }

                        break;
                    case "SALARY":
                        if (decimal.TryParse(values[i], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal salary))
                        {
                            record.Salary = salary;
                            flags[SalaryFlagIndex] = true;
                        }

                        break;
                    case "DEPARTMENT":
                        if (short.TryParse(values[i], out short department))
                        {
                            record.Department = department;
                            flags[DepartmentFlagIndex] = true;
                        }

                        break;
                    case "CLASS":
                        if (char.TryParse(values[i], out char clas))
                        {
                            record.Class = clas;
                            flags[ClassFlagIndex] = true;
                        }

                        break;
                    default:
                        break;
                }
            }

            if (flags.Cast<bool>().All(x => x == true))
            {
                try
                {
                    this.Service.Insert(record);
                    Console.WriteLine("Record #{0} is created.", record.Id);
                    this.Service.MemEntity.Clear();
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Please, correct your input.");
            }
        }
    }
}
