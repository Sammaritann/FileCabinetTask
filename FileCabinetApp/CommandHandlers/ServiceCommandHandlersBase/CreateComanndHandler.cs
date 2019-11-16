using System;
using System.Globalization;
using FileCabinetApp.Service;
using FileCabinetApp.Validators.InpitValidator;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents create command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class CreateComanndHandler : ServiceCommandHandlerBase
    {
        private IInputValidator inputValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateComanndHandler" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="validator">The validator.</param>
        public CreateComanndHandler(IFileCabinetService service, IInputValidator validator)
            : base(service)
        {
            this.inputValidator = validator;
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != "CREATE")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            string firstName = null;
            string lastName = null;
            DateTime dateOfBirth;
            short department;
            decimal salary;
            char clas;

            this.ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);

            RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);
            int id = this.Service.CreateRecord(recordParams);
            Console.WriteLine("Record #{0} is created.", id);
            this.Service.MemEntity.Clear();
        }

        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <typeparam name="T">Read tupe.</typeparam>
        /// <param name="converter">The converter.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>Read item.</returns>
        private static T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// Strings the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert string.</returns>
        private static (bool, string, string) StringConverter(string str)
        {
            return (!string.IsNullOrEmpty(str), "string", str);
        }

        /// <summary>
        /// Dates the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert date.</returns>
        private static (bool, string, DateTime) DateConverter(string str)
        {
            return (DateTime.TryParseExact(str, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth), "date", dateOfBirth);
        }

        /// <summary>
        /// Shorts the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert short.</returns>
        private static (bool, string, short) ShortConverter(string str)
        {
            return (short.TryParse(str, out short department), "short", department);
        }

        /// <summary>
        /// Decimals the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert deciminal.</returns>
        private static (bool, string, decimal) DecimalConverter(string str)
        {
            return (decimal.TryParse(str, out decimal salary), "decimal", salary);
        }

        /// <summary>
        /// Characters the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert char.</returns>
        private static (bool, string, char) CharConverter(string str)
        {
            return (char.TryParse(str, out char clas), "char", clas);
        }

        /// <summary>
        /// Reads the record.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="department">The department.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="clas">The clas.</param>
        private void ReadRecord(out string firstName, out string lastName, out DateTime dateOfBirth, out short department, out decimal salary, out char clas)
        {
            firstName = null;
            lastName = null;

            Console.Write("First name: ");
            firstName = ReadInput<string>(StringConverter, this.inputValidator.FirstNameValidate);

            Console.Write("Last name: ");
            lastName = ReadInput(StringConverter, this.inputValidator.LastNameValidate);

            Console.Write("Date of birth: ");
            dateOfBirth = ReadInput(DateConverter, this.inputValidator.DateOfBirthValidate);

            Console.Write("Department: ");
            department = ReadInput(ShortConverter, this.inputValidator.DepartmentValidate);

            Console.Write("Salary: ");
            salary = ReadInput(DecimalConverter, this.inputValidator.SalaryValidate);

            Console.Write("Class: ");
            clas = ReadInput(CharConverter, this.inputValidator.ClassValidate);
        }
    }
}