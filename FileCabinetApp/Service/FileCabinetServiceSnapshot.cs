using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using FileCabinetApp.Reader;
using FileCabinetApp.Writer;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents file cabinet snapshot.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot" /> class.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <exception cref="ArgumentNullException">Throws when list is null.</exception>
        private FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            this.records = list.ToArray();
        }

        /// <summary>
        /// Gets the records.
        /// </summary>
        /// <value>
        /// The records.
        /// </value>
        public ReadOnlyCollection<FileCabinetRecord> Records { get; private set; }

        /// <summary>
        /// Makes the snapshot.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>Snapshot.</returns>
        public static FileCabinetServiceSnapshot MakeSnapshot(List<FileCabinetRecord> list)
        {
            if (list is null)
            {
                return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>());
            }

            return new FileCabinetServiceSnapshot(list);
        }

        /// <summary>
        /// Saves to CSW.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">Throws when stream is null.</exception>
        public void SaveToCsw(StreamWriter stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException($"{nameof(stream)} must nit be null");
            }

            FileCabinetRecordCsvWriter writer = new FileCabinetRecordCsvWriter(stream);
            stream.WriteLine("Id,First Name,Last Name,Date of Birth,Department,Salary,Class");
            foreach (var item in this.records)
            {
                writer.Write(item);
            }
        }

        /// <summary>
        /// Saves to XML.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">Throws when stream is null.</exception>
        public void SaveToXml(StreamWriter stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException($"{nameof(stream)} must nit be null");
            }

            FileCabinetRecordXmlWriter writer = new FileCabinetRecordXmlWriter(stream);
            var doc = new XmlDocument();
            doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
            doc.AppendChild(doc.CreateElement("ArrayOfFileCabinetRecord"));
            doc.Save(stream);
            foreach (var item in this.records)
            {
                writer.Write(item);
            }
        }

        /// <summary>
        /// Loads from CSV.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void LoadFromCsv(StreamReader stream)
        {
            FileCabinetRecordCsvReader csvReader = new FileCabinetRecordCsvReader(stream);
            this.Records = new ReadOnlyCollection<FileCabinetRecord>(csvReader.ReadAll());
        }

        /// <summary>
        /// Loads from XML.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void LoadFromXml(StreamReader stream)
        {
            FileCabinetRecordXmlReader xmlReader = new FileCabinetRecordXmlReader(stream);
            this.Records = new ReadOnlyCollection<FileCabinetRecord>(xmlReader.ReadAll());
        }
    }
}
