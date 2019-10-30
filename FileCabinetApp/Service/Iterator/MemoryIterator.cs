using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Service.Iterator
{
    public class MemoryIterator : IRecordIterator
    {
        private int index = -1;
        private List<FileCabinetRecord> list;

        public MemoryIterator(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        public FileCabinetRecord GetNext()
        {
            index++;
            return list[index];
        }

        public bool HasMore()
        {
            return this.index < this.list.Count - 1;
        }
    }
}
