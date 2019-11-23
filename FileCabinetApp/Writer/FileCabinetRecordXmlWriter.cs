using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace FileCabinetApp.Writer
{
    /// <summary>
    /// Represents file cabinet record xml writer.
    /// </summary>
    public class FileCabinetRecordXmlWriter : IDisposable
    {
        private const int StreamStart = 0;
        private StreamWriter writer;
        private bool disposed;

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

        /// <summary>
        /// Writes the specified record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <exception cref="ArgumentNullException">Throws when record is null.</exception>
        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var doc = new XmlDocument();
            this.writer.BaseStream.Position = StreamStart;
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
            this.writer.BaseStream.Position = StreamStart;
            doc.Save(this.writer.BaseStream);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.writer.Dispose();
            }

            this.disposed = true;
        }

        private static void AddChild(string childName, string childText, XmlElement parent, XmlDocument doc)
        {
            var child = doc.CreateElement(childName);
            child.InnerText = childText;
            parent.AppendChild(child);
        }
    }
}
