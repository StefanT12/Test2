using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogTest.Interfaces
{
    public interface IFileManager
    {
        void CreateNewFile(DateTime dt);
        Task WriteInFile(ILogLine logLine);
    }
}
