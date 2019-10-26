using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    public abstract class ServiceCommandHandlerBase:CommandHandlerBase
    {
        protected IFileCabinetService service;

        public ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }

    }
}
