using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
  public class EditComanndHandler : CommandHandlerBase
    {
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
                if (Program.fileCabinetService.ContainsId(id))
                {
                    ReadRecord(out firstName, out lastName, out dateOfBirth, out department, out salary, out clas);
                    RecordParams recordParams = new RecordParams(firstName, lastName, dateOfBirth, department, salary, clas);

                    Program.fileCabinetService.EditRecord(id, recordParams);
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
