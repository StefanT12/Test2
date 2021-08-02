using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogTest
{
    public class AsyncLogV2 : ILog
    {
        private readonly int _limit;
        private int _counter;
        private List<LogLineV2> _logs;
        private StreamWriter _writer;
        private DateTime _dtPresent;

        private Task _logger;

        readonly CancellationTokenSource _writeToken;
        readonly CancellationTokenSource _logToken;
        
        public AsyncLogV2(int limit)
        {
            _limit = limit;
            _logs = new List<LogLineV2>();
            _dtPresent = DateTime.Now;
            _writeToken = new CancellationTokenSource();
            _logToken = new CancellationTokenSource();
            CreateNewFile(_dtPresent);

            _logger = Task.Run(() => RunLogger(_writeToken.Token), _logToken.Token);
        }

        private async Task RunLogger(CancellationToken writeToken)
        {
            while (true)
            {
                if((DateTime.Now.Day - _dtPresent.Day) != 0)
                {
                    _dtPresent = DateTime.Now;
                    CreateNewFile(_dtPresent);
                }

                if (_counter > _limit)
                {
                    _counter = 0;
                    await WriteInFile(writeToken);
                }
            }
        }

        private async Task WriteInFile(CancellationToken writeToken)
        {
            var logsarr = new LogLineV2[_logs.Count];
            _logs.CopyTo(logsarr);

            var sb = new StringBuilder();

            for (int i = 0; i < logsarr.Length; i++)
            {
                sb.Append(logsarr[i].Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                sb.Append("\t");
                sb.Append(logsarr[i].Text);
                sb.Append("\t");
                sb.Append(Environment.NewLine);
            }

            if (writeToken.IsCancellationRequested) 
            {  
                return; 
            }
            
            await Task.Run( () => _writer.WriteAsync(sb.ToString()), writeToken);

            _logs.RemoveRange(0, logsarr.Length);
        }

        private void CreateNewFile(DateTime dtNow)
        {
            if (!Directory.Exists(@"C:\LogTest"))
                Directory.CreateDirectory(@"C:\LogTest");

            this._writer = File.AppendText(@"C:\LogTest\Log" + dtNow.ToString("yyyyMMdd HHmmss fff") + ".log");

            this._writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

            this._writer.AutoFlush = true;
        }

        #region interface implementation
        public void Stop(bool withFlush = false)
        {
            _logToken.Cancel();

            var msg = $"Stopped, withFlush {withFlush}";

            if (withFlush)
            {
                _writeToken.Cancel();
                _logs = new List<LogLineV2>();
                Console.WriteLine(msg);
            }
            else
            {
                if(_logs.Count != 0)
                {
                    var task = Task.Run(() => WriteInFile(_writeToken.Token));
                    if (task.IsCompleted)
                    {
                        Console.WriteLine(msg);
                    }
                }
            }
        }

        public void Write(string text)
        {
            _logs.Add(new LogLineV2 { Text = text, Timestamp = DateTime.Now });

            _counter++;

        }
        #endregion
    }
}
