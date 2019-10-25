using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers
{
    public interface ICommandHandler
    {

        /// <summary>
        /// Sets the next handler.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        public void SetNext(ICommandHandler commandHandler);

        public void Handle(AppCommandRequest commandRequest);

    }
}
