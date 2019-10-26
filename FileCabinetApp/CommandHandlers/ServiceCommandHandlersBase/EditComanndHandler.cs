using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents edit command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class EditComanndHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditComanndHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public EditComanndHandler(IFileCabinetService service)
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

            if (commandRequest.Command.ToUpperInvariant() != "EDIT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            string firstName;
            string lastName;
            DateTime dateOfBirth;
            short department;
            decimal salary;
            char clas;

            if (!int.TryParse(commandRequest.Parameters, out int id))
            {
                Console.WriteLine("param not a number");
                return;
            }

            try
            {
                if (this.Service.ContainsId(id))
                {
                    ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);
                    RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);

                    this.Service.EditRecord(id, recordParams);
                }
                else
                {
                    Console.WriteLine("#id record is not found");
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("#id record is not found");
                return;
            }
        }
    }
}