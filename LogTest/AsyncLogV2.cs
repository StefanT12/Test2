using LogTest.Interfaces;
using System;
using System.Collections.Concurrent;
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
        private ConcurrentQueue<LogLineV2> _logs;
       
        private Task _currTask;

        private readonly CancellationTokenSource _logToken;

        private readonly IFileManager _fileManager;
        private readonly IFileBuildStrategy _fbStrat;
        private readonly ITimeProvider _timeProvider;

        private bool _quitWithFlush;

        public AsyncLogV2(ITimeProvider timeProvider, IFileManager fileManager, IFileBuildStrategy fbStrat)
        {
            _logs = new ConcurrentQueue<LogLineV2>();
            _logToken = new CancellationTokenSource();

            _fileManager = fileManager;
            _fbStrat = fbStrat;
            _timeProvider = timeProvider;

            _fbStrat.Init();

            _currTask = Task.CompletedTask;
        }

        private async Task RunLogger(CancellationToken token)
        {
            LogLineV2 log = null;
            
            while(!token.IsCancellationRequested && _logs.TryDequeue(out log))
            {
                try
                {
                    _fbStrat.Run();
                    await _fileManager.WriteInFile(log);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                } 
            }
        }

        public async Task Stop(bool withFlush = false)
        {
            if (!withFlush)//cancel everything
            {
                _logToken.Cancel();
               // _logToken.Dispose();
            }
            else
            {
                _quitWithFlush = true;//leave operations to finish on their own accord
            }

            await _currTask;

            _fileManager.Dispose();
        }

        public void Write(string text)
        {
            if (_quitWithFlush || _logToken.IsCancellationRequested) return;

            _logs.Enqueue(new LogLineV2 { Text = text, Timestamp = _timeProvider.GetTimeNow() });

            if (!_currTask.IsCompleted) return;

            _currTask = Task.Run(async () => await RunLogger(_logToken.Token), _logToken.Token);
        }
    }
}
