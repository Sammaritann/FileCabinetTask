using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace FileCabinetApp.Writer
{
    /// <summary>
    /// Represents file cabinet record xml writer.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter" /> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <exception cref="ArgumentNullException">Throws when writer is null.</exception>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.writer = writer;
        }

        private FileCabinetRecordXmlWriter()
        {
        }

        /// <summary>
        /// Writes the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throws when record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} must not be null");
            }

            var doc = new XmlDocument();
            this.writer.BaseStream.Position = 0;
            doc.Load(this.writer.BaseStream);
            var root = doc.DocumentElement;
            var fileRecordNode = doc.CreateElement("FileCabinetRecord");

            AddChild("Id", record.Id.ToString(CultureInfo.InvariantCulture), fileRecordNode, doc);
            AddChild("FirstName", record.FirstName, fileRecordNode, doc);
            AddChild("LastName", record.LastName, fileRecordNode, doc);
            AddChild("DateOfBirth", record.DateOfBirth.ToString("o", CultureInfo.InvariantCulture), fileRecordNode, doc);
            AddChild("Department", record.Department.ToString(CultureInfo.InvariantCulture), fileRecordNode, doc);
            AddChild("Salary", record.Salary.ToString(CultureInfo.InvariantCulture), fileRecordNode, doc);
            AddChild("Class", ((int)record.Class).ToString(CultureInfo.InvariantCulture), fileRecordNode, doc);

            root.AppendChild(fileRecordNode);
            this.writer.BaseStream.Position = 0;
            doc.Save(this.writer.BaseStream);
        }

        private static void AddChild(string childName, string childText, XmlElement parent, XmlDocument doc)
        {
            var child = doc.CreateElement(childName);
            child.InnerText = childText;
            parent.AppendChild(child);
        }
    }
}
