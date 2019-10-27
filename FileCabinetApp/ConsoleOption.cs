using CommandLine;

namespace FileCabinetApp
{
    /// <summary>
    /// Represents console option.
    /// </summary>
    public class ConsoleOption
    {
        /// <summary>
        /// Gets or sets the validator.
        /// </summary>
        /// <value>
        /// The validator.
        /// </value>
        [Option('v', "validation-rules", Required = false, Default = "default", HelpText = "Set validation rules")]
        public string Validator { get; set; }

        /// <summary>
        /// Gets or sets the file system.
        /// </summary>
        /// <value>
        /// The file system.
        /// </value>
        [Option('s', "storage", Required = false, Default = "memory", HelpText = "Set FileName")]
        public string FileSystem { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConsoleOption"/> is watch.
        /// </summary>
        /// <value>
        ///   <c>true</c> if watch; otherwise, <c>false</c>.
        /// </value>
        [Option("use-stopwatch", Required = false, Default = false)]
        public bool Watch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConsoleOption"/> is logger.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logger; otherwise, <c>false</c>.
        /// </value>
        [Option("use-logger", Required = false, Default = false)]
        public bool Logger { get; set; }
    }
}