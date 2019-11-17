using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Reader
{
    /// <summary>
    /// Represents record xml reader.
    /// </summary>
    public class FileCabinetRecordXmlReader
    {
        private readonly StreamReader reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FileCabinetRecordXmlReader(StreamReader stream)
        {
            this.reader = stream;
        }

        /// <summary>
        /// Reads all.
        /// </summary>
        /// <returns>All records.</returns>
        public IList<FileCabinetRecord> ReadAll()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            FileCabinetRecord[] records;
            using (XmlReader reader = XmlReader.Create(this.reader))
            {
                records = (FileCabinetRecord[])serializer.Deserialize(reader);
            }

            return records;
        }
    }
}
