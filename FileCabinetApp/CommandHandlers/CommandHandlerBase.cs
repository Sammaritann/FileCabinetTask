using System;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents base command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ICommandHandler" />
    public abstract class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// The next handler.
        /// </summary>
        private ICommandHandler nextHandler;

        /// <summary>
        /// Gets or sets the next handler.
        /// </summary>
        /// <value>
        /// The next handler.
        /// </value>
        protected ICommandHandler NextHandler { get => this.nextHandler; set => this.nextHandler = value; }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public abstract void Handle(AppCommandRequest commandRequest);

        /// <summary>
        /// Sets the next.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <exception cref="ArgumentNullException">commandHandler.</exception>
        public void SetNext(ICommandHandler commandHandler)
        {
            this.NextHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        /// <summary>
        /// Reads the input.
        /// </summary>
        /// <typeparam name="T">Read tupe.</typeparam>
        /// <param name="converter">The converter.</param>
        /// <param name="validator">The validator.</param>
        /// <returns>Read item.</returns>
        protected static T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
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
        /// Reads the record.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="department">The department.</param>
        /// <param name="salary">The salary.</param>
        /// <param name="clas">The clas.</param>
        protected static void ReadRecord(out string firstName, out string lastName, out DateTime dateOfBirth, out short department, out decimal salary, out char clas)
        {
            firstName = null;
            lastName = null;

            Console.Write("First name: ");
            firstName = ReadInput<string>(StringConverter, Program.inputValidator.FirstNameValidate);

            Console.Write("Last name: ");
            lastName = ReadInput(StringConverter, Program.inputValidator.LastNameValidate);

            Console.Write("Date of birth: ");
            dateOfBirth = ReadInput(DateConverter, Program.inputValidator.DateOfBirthValidate);

            Console.Write("Department: ");
            department = ReadInput(ShortConverter, Program.inputValidator.DepartmentValidate);

            Console.Write("Salary: ");
            salary = ReadInput(DecimalConverter, Program.inputValidator.SalaryValidate);

            Console.Write("Class: ");
            clas = ReadInput(CharConverter, Program.inputValidator.ClassValidate);
        }

        /// <summary>
        /// Strings the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert string.</returns>
        protected static (bool, string, string) StringConverter(string str)
        {
            return (!string.IsNullOrEmpty(str), "string", str);
        }

        /// <summary>
        /// Dates the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert date.</returns>
        protected static (bool, string, DateTime) DateConverter(string str)
        {
            return (DateTime.TryParseExact(str, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth), "date", dateOfBirth);
        }

        /// <summary>
        /// Shorts the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert short.</returns>
        protected static (bool, string, short) ShortConverter(string str)
        {
            return (short.TryParse(str, out short department), "short", department);
        }

        /// <summary>
        /// Decimals the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert deciminal.</returns>
        protected static (bool, string, decimal) DecimalConverter(string str)
        {
            return (decimal.TryParse(str, out decimal salary), "decimal", salary);
        }

        /// <summary>
        /// Characters the converter.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>Convert char.</returns>
        protected static (bool, string, char) CharConverter(string str)
        {
            return (char.TryParse(str, out char clas), "char", clas);
        }
    }
}