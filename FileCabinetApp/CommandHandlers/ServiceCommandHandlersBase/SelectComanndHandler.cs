﻿using FileCabinetApp.CommandHandlers.Printers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    public class SelectComanndHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        public SelectComanndHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException(nameof(commandRequest));
            }

            if (commandRequest.Command.ToUpperInvariant() != "SELECT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            int subIndex = commandRequest.Parameters.IndexOf(" where ");
            var param = commandRequest.Parameters.Substring(0, subIndex).Replace(",", " ",StringComparison.InvariantCultureIgnoreCase).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            this.printer.Print(this.Service.Where(commandRequest.Parameters.Substring(subIndex + 7)), param);

        }
    }
}
