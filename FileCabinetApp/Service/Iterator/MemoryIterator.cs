using System.Collections;
using System.Collections.Generic;

namespace FileCabinetApp.Service.Iterator
{
    public class MemoryIterator : IEnumerator<FileCabinetRecord>,IEnumerable<FileCabinetRecord>
    {
        private int index = -1;
        private List<FileCabinetRecord> list;

        public MemoryIterator(List<FileCabinetRecord> list)
        {
            this.list = list;
        }

        public FileCabinetRecord Current => list[index];

        object IEnumerator.Current => Current;

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

            return index < list.Count;
        }

        public void Reset()
        {
            index = -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
