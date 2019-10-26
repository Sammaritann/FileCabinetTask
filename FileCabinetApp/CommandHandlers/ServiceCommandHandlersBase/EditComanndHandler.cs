using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
  public class EditComanndHandler : ServiceCommandHandlerBase
    {

        public EditComanndHandler(IFileCabinetService service):base(service)
        {
            
        }
        public override void Handle(AppCommandRequest commandRequest)
        {
            if(commandRequest.Command.ToUpperInvariant()!="EDIT")
            {
                nextHandler.Handle(commandRequest);
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
                if (service.ContainsId(id))
                {
                    ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);
                    RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);

                    service.EditRecord(id, recordParams);
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
