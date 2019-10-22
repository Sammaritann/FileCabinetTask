using CommandLine;

using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<ConsoleOption>(args)
           .WithParsed<ConsoleOption>(opts => RunOption(opts));
        }

        private static void RunOption(ConsoleOption opts)
        {
            if (opts.Writer.ToUpperInvariant() == "CSV")
            {
                using (StreamWriter writer = new StreamWriter(opts.FileName))
                {
                    var csvWriter = new FileCabinetApp.Writer.FileCabinetRecordCsvWriter(writer);
                    foreach (var item in Generator.Generate(opts.StartID, opts.RecordsAmount))
                    {
                        csvWriter.Write(item);
                    }
                }
            }

            if (opts.Writer.ToUpperInvariant() == "XML")
            {
                using (StreamWriter writer = new StreamWriter(opts.FileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetApp.FileCabinetRecord[]));
                    serializer.Serialize(writer, Generator.Generate(opts.StartID, opts.RecordsAmount).ToArray());
                }
            }
            Console.WriteLine("{0} records were written to {1}", opts.RecordsAmount, opts.FileName);
        }
    }
}