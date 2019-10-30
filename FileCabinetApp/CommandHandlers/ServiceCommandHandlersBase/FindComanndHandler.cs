﻿using System;
using System.Globalization;
using FileCabinetApp.CommandHandlers.Printers;
using FileCabinetApp.Service.Iterator;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents find command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class FindComanndHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindComanndHandler" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="printer">The printer.</param>
        public FindComanndHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            this.printer = printer;
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

            if (commandRequest.Command.ToUpperInvariant() != "FIND")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            string[] param = commandRequest.Parameters.Split(' ');
            if (param.Length != 2)
            {
                Console.WriteLine("Invalid number of parameters");
                return;
            }

            if (param[0].ToUpperInvariant() == "FIRSTNAME")
            {
                var iterator = this.Service.FindByFirstName(param[1].Trim('\"'));
                foreach (var item in iterator)
                {
                    this.printer.Print(item);
                }
            }

            if (param[0].ToUpperInvariant() == "LASTNAME")
            {
                var iterator = this.Service.FindByLastName(param[1].Trim('\"'));
                foreach (var item in iterator)
                {
                    this.printer.Print(item);
                }
            }

            if (param[0].ToUpperInvariant() == "DATEOFBIRTH")
            {
                DateTime dateOfBirth;
                if (!DateTime.TryParseExact(param[1].Trim('\"'), "yyyy-MMM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                {
                    Console.WriteLine("Invalid Date");
                    return;
                }

                var iterator = this.Service.FindByDateOfBirth(dateOfBirth);
                foreach (var item in iterator)
                {
                    this.printer.Print(item);
                }
            }
        }
    }
}