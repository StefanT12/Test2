using LogTest;
using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestCases 
{
    public class FileManagerMock : IFileManager, IFileManagerLogRestricted
    {
        public void CreateNewFile(DateTime dt)
        {
            _realFileManager.CreateNewFile(dt);
        }

        public async Task WriteInFile(ILogLine logLine)
        {
            _output.Add(logLine);
            await _realFileManager.WriteInFile(logLine);
        }

        public void Dispose()
        {
            _realFileManager.Dispose();
        }

        private List<ILogLine> _output;
        private IFileManager _realFileManager;
        public FileManagerMock(List<ILogLine> output, string path)
        {
            _output = output;
            _realFileManager = new FileManager(path);
        }
    }
}
