using CommandLine;

namespace FileCabinetGenerator
{
    internal class ConsoleOption
    {
        [Option('t', "output-type", Required = true, HelpText = "Set output-type")]
        public string Writer { get; set; }

        [Option('o', "output", Required = true, HelpText = "Set FileName")]
        public string FileName { get; set; }

        [Option('a', "records-amount", Required = true, HelpText = "Record amount")]
        public int RecordsAmount { get; set; }

        [Option('i', "start-id", Required = true, HelpText = "Record amount")]
        public int StartID { get; set; }
    }
}