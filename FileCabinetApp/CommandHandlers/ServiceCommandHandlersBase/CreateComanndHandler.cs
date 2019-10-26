using System;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents create command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class CreateComanndHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateComanndHandler"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public CreateComanndHandler(IFileCabinetService service)
            : base(service)
        {
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

            ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);

            RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);
            int id = this.Service.CreateRecord(recordParams);
            Console.WriteLine("Record #{0} is created.", id);
        }
    }
}