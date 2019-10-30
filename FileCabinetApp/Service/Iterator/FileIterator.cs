using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FileCabinetApp.Service.Iterator
{
    public class FileIterator : IRecordIterator
    {
        private List<long> positions;
        private int index = -1;
        private FileStream stream;

        public FileIterator(List<long> positions, FileStream stream)
        {
            this.positions = positions;
            this.stream = stream;
        }

        public FileCabinetRecord GetNext()
        {
            index++;
            byte[] buffer = new byte[277];
            this.stream.Position = this.positions[index];
            this.stream.Read(buffer, 0, 277);
            FileCabinetRecord record = this.RecordFromBytes(buffer);
            return record;
        }

        public bool HasMore()
        {
            return this.index < this.positions.Count - 1;
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
