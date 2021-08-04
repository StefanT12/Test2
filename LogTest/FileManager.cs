using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    public class FileManager : IFileManager
    {
        //TODO check if any optimisation can be done here and with the AutoFlush
        private StreamWriter _writer;
        private readonly string _path;
        public void CreateNewFile(DateTime dt)
        {
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);


            this._writer = File.AppendText(_path+ @"\Log" + dt.ToString("yyyyMMdd HHmmss fff") + ".log");

            this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

            this._writer.AutoFlush = true;
        }

        public async Task WriteInFile(ILogLine logLine)
        {
            await _writer.WriteAsync(logLine.GetLine());
        }

        public void Dispose()
        {
            _writer.Close();
        }

        public FileManager(string path)
        {
            _path = path;
        }
    }
}
