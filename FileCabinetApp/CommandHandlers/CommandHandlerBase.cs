using System;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents base command handler.
    /// </summary>
    /// <seealso cref="FileCabinetApp.CommandHandlers.ICommandHandler" />
    public abstract class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// The next handler.
        /// </summary>
        private ICommandHandler nextHandler;

        /// <summary>
        /// Gets or sets the next handler.
        /// </summary>
        /// <value>
        /// The next handler.
        /// </value>
        protected ICommandHandler NextHandler { get => this.nextHandler; set => this.nextHandler = value; }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public abstract void Handle(AppCommandRequest commandRequest);

        /// <summary>
        /// Sets the next.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        /// <exception cref="ArgumentNullException">commandHandler.</exception>
        public void SetNext(ICommandHandler commandHandler)
        {
            this.NextHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }
    }
}