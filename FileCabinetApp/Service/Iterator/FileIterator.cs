using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FileCabinetApp.Service.Iterator
{
    public class FileIterator : IEnumerator<FileCabinetRecord>,IEnumerable<FileCabinetRecord>
    {
        private List<long> positions;
        private int index = -1;
        private FileStream stream;
        private FileCabinetRecord record;
        public FileIterator(List<long> positions, FileStream stream)
        {
            this.positions = positions;
            this.stream = stream;
        }

        public FileCabinetRecord Current =>record;

        object IEnumerator.Current => record;

        public void Dispose()
        {
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            index++;         
            if(index<positions.Count)
            {
                byte[] buffer = new byte[277];
                this.stream.Position = this.positions[index];
                this.stream.Read(buffer, 0, 277);
                FileCabinetRecord record = this.RecordFromBytes(buffer);
                this.record= record;

                return true;
            }

            return false;
        }

        public void Reset()
        {
            index = -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        private FileCabinetRecord RecordFromBytes(byte[] buffer)
        {
            ToBytesDecimal toDecimal = default;
            int year;
            int month;
            int day;
            FileCabinetRecord record = new FileCabinetRecord();
            record.Department = BitConverter.ToInt16(buffer, 0);
            record.Id = BitConverter.ToInt32(buffer, 2);
            record.FirstName = Encoding.Default.GetString(buffer, 6, 120).Trim('\0');
            record.LastName = Encoding.Default.GetString(buffer, 126, 120).Trim('\0');
            year = BitConverter.ToInt32(buffer, 246);
            month = BitConverter.ToInt32(buffer, 250);
            day = BitConverter.ToInt32(buffer, 254);
            toDecimal.Bytes1 = BitConverter.ToInt64(buffer, 258);
            toDecimal.Bytes2 = BitConverter.ToInt64(buffer, 266);
            record.Salary = toDecimal.GetDecimal();
            record.Class = BitConverter.ToChar(buffer, 274);
            record.DateOfBirth = new DateTime(year, month, day);

            return buffer[276] == 0 ? record : throw new KeyNotFoundException();
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ToBytesDecimal
        {
            [FieldOffset(0)]
            public long Bytes1;
            [FieldOffset(8)]
            public long Bytes2;
            [FieldOffset(0)]
            private decimal number;

            public ToBytesDecimal(decimal number)
            {
                this.Bytes1 = 0;
                this.Bytes2 = 0;
                this.number = number;
            }

            public decimal GetDecimal()
            {
                return this.number;
            }
        }
    }
}
