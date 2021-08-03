using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogTest
{
    public class FileManagerTest : IFileManager
    {
        public void CreateNewFile(DateTime dt)
        {
            
        }

        public async Task WriteInFile(ILogLine logLine)
        {
            _logs.Add(logLine);
            await Task.Run(() => Thread.Sleep(20));//simulate writing inside file
        }

        private List<ILogLine> _logs;

        public FileManagerTest(List<ILogLine> logs)
        {
            _logs = logs;
        }
    }
}
