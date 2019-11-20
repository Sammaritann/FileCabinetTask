using System;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers.ValidateHandler
{
    /// <summary>
    /// Represent predicate generator.
    /// </summary>
    public static class ValidateGenerator
    {
        /// <summary>
        /// Creates the specified validate parameter.
        /// </summary>
        /// <param name="validateParam">The validate parameter.</param>
        /// <returns>Validation predicate.</returns>
        /// <exception cref="ArgumentNullException">Throws when validateParam is null.</exception>
        public static (Predicate<FileCabinetRecord> predicate, string explanation) Create(string validateParam)
        {
            if (validateParam is null)
            {
                throw new ArgumentNullException(nameof(validateParam));
            }

            var param = validateParam.Replace("=", " ", StringComparison.InvariantCultureIgnoreCase).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (param.Length != 2)
            {
                throw new ArgumentException(validateParam);
            }

            return (param[0].ToUpperInvariant().Trim('\'') switch
            {
                "FIRSTNAME" => x => x.FirstName.ToUpperInvariant() == param[1].ToUpperInvariant().Trim('\''),
                "LASTNAME" => x => x.LastName.ToUpperInvariant() == param[1].ToUpperInvariant().Trim('\''),
                "ID" => x => x.Id.ToString(CultureInfo.InvariantCulture) == param[1].ToUpperInvariant().Trim('\''),
                "DATEOFBIRTH" => x => x.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) == param[1].ToUpperInvariant().Trim('\''),
                "SALARY" => x => x.Salary.ToString(CultureInfo.InvariantCulture) == param[1].ToUpperInvariant().Trim('\''),
                "DEPARTMENT" => x => x.Department.ToString(CultureInfo.InvariantCulture) == param[1].ToUpperInvariant().Trim('\''),
                "CLASS" => x => x.Class.ToString(CultureInfo.InvariantCulture) == param[1].ToUpperInvariant().Trim('\''),
                _ => throw new ArgumentException(param[0]),
            }, $"{param[0]}  {param[1]}");
        }
    }
}
