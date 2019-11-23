using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace FileCabinetApp.CommandHandlers.Exceptions
{
    /// <summary>
    /// Represents import record exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class ImportRecordException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRecordException"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="message">The message.</param>
        public ImportRecordException(int id, string message)
            : base(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", id, message))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRecordException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ImportRecordException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRecordException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
        public ImportRecordException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRecordException"/> class.
        /// </summary>
        public ImportRecordException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRecordException"/> class.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The Runtime.Serialization.StreamingContext that contains contextual information about the source or destination.</param>
        protected ImportRecordException(SerializationInfo info, StreamingContext context)
               : base(info, context)
        {
        }
    }
}
