namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Represents command handler interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets the next handler.
        /// </summary>
        /// <param name="commandHandler">The command handler.</param>
        public void SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public void Handle(AppCommandRequest commandRequest);
    }
}