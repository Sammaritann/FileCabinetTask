using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public class CreateComanndHandler : CommandHandlerBase
    {
        private IFileCabinetService service;

        public CreateComanndHandler(IFileCabinetService service)
        {
            this.service = service;
        }
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest.Command.ToUpperInvariant() != "CREATE")
            {
                nextHandler.Handle(commandRequest);
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
            int id = service.CreateRecord(recordParams);
            Console.WriteLine("Record #{0} is created.", id);
        }
    }
}
