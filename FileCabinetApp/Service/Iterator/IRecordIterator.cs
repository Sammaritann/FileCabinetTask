using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Service.Iterator
{
    public interface IRecordIterator
    {
        FileCabinetRecord GetNext();

        bool HasMore();
    }
}
