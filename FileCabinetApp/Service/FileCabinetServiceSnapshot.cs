using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using FileCabinetApp.Writer;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Represents file cabinet snapshot.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        private FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.records = list.ToArray();
        }

        private FileCabinetServiceSnapshot()
        {
        }

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
            doc.AppendChild(doc.CreateElement("ArrayOfFileCabinetRecord xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\""));
            doc.Save(stream);
            foreach (var item in this.records)
            {
                writer.Write(item);
            }
        }
    }
}
