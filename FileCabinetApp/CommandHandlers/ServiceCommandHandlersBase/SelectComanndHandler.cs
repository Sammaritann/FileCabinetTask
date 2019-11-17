using System;
using FileCabinetApp.CommandHandlers.Printers;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase
{
    /// <summary>
    /// Represents select command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ServiceCommandHandlersBase.ServiceCommandHandlerBase" />
    public class SelectComanndHandler : ServiceCommandHandlerBase
    {
        private IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectComanndHandler" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="printer">The printer.</param>
        /// <exception cref="ArgumentNullException">Throws when printer is null.</exception>
        public SelectComanndHandler(IFileCabinetService service, IRecordPrinter printer)
            : base(service)
        {
            if (printer is null)
            {
                throw new ArgumentNullException(nameof(printer));
            }

            this.printer = printer;
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

            if (commandRequest.Command.ToUpperInvariant() != "SELECT")
            {
                this.NextHandler.Handle(commandRequest);
                return;
            }

            int subIndex = commandRequest.Parameters.IndexOf(" where ", StringComparison.InvariantCultureIgnoreCase);

            if (string.IsNullOrWhiteSpace(commandRequest.Parameters))
            {
                this.printer.Print(this.Service.GetRecords());
                return;
            }

            if (commandRequest.Parameters.StartsWith("where ", StringComparison.InvariantCultureIgnoreCase))
            {
                this.printer.Print(this.Service.Where(commandRequest.Parameters.Substring("where ".Length)));
            }
            else if (subIndex == -1)
            {
                var param = commandRequest.Parameters.Replace(",", " ", StringComparison.InvariantCultureIgnoreCase).Split(' ', StringSplitOptions.RemoveEmptyEntries);

                this.printer.Print(this.Service.GetRecords(), param);
            }
            else
            {
                var param = commandRequest.Parameters.Substring(0, subIndex).Replace(",", " ", StringComparison.InvariantCultureIgnoreCase).Split(' ', StringSplitOptions.RemoveEmptyEntries);

                this.printer.Print(this.Service.Where(commandRequest.Parameters.Substring(subIndex + 7)), param);
            }
        }
    }
}
