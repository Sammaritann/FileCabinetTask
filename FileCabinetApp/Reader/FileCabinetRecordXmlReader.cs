using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Reader
{
    /// <summary>
    /// Represents record xml reader.
    /// </summary>
    public class FileCabinetRecordXmlReader : IDisposable
    {
        private readonly StreamReader reader;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader" /> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">Throws when stream is null.</exception>
        public FileCabinetRecordXmlReader(StreamReader stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

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
                this.reader.Dispose();
            }

            this.disposed = true;
        }
    }
}
