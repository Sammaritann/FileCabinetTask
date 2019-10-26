using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        protected ICommandHandler nextHandler;

        public abstract void Handle(AppCommandRequest commandRequest);

        /// <summary>
        /// Sets the next.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <exception cref="ArgumentNullException">commandHandler.</exception>
        public void SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

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

        protected static (bool, string, string) StringConverter(string str)
        {
            return (!string.IsNullOrEmpty(str), "string", str);
        }

        protected static (bool, string, DateTime) DateConverter(string str)
        {
            return (DateTime.TryParseExact(str, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth), "date", dateOfBirth);
        }

        protected static (bool, string, short) ShortConverter(string str)
        {
            return (short.TryParse(str, out short department), "short", department);
        }

        protected static (bool, string, decimal) DecimalConverter(string str)
        {
            return (decimal.TryParse(str, out decimal salary), "decimal", salary);
        }

        protected static (bool, string, char) CharConverter(string str)
        {
            return (char.TryParse(str, out char clas), "char", clas);
        }
    }

}
