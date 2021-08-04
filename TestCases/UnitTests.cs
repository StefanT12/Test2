using LogTest;
using LogTest.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TestCases
{
    public class Tests
    {
        [Test]
        public async Task CheckIfLogWrites()
        {
            var folderPath = @"C:\LogTest\TestIsSomethingWritten";
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            var fileManager = new FileManager(folderPath);
            var timeProvider = new TimeProvider();
            var log = new AsyncLogV2(timeProvider, fileManager, new MidnightFileBuildStrategy(timeProvider, fileManager));

            log.Write("teeeest");
            await log.Stop(withFlush: true);

            var filesInDir = Directory.GetFiles(folderPath);
            var isSomethingWritten = false;
            foreach (var file in filesInDir)
            {
                var text = File.ReadAllText(file);

                if (!string.IsNullOrEmpty(text))
                {
                    isSomethingWritten = true;
                    break;
                }
            }

            Assert.IsTrue(isSomethingWritten);
        }

        [Test]
        public async Task CheckFlush()
        {
            var output = new List<ILogLine>();
            var folderPath = @"C:\LogTest";

            var fileManager = new FileManagerMock(output, folderPath);
            var timeProvider = new TimeProvider();
            var logger = new AsyncLogV2(timeProvider, fileManager, new MidnightFileBuildStrategy(timeProvider, fileManager));
            var count = 15;

            for(int i = 0; i < count; i++)
            {
                logger.Write($"Entry no: {i} w/flush.");
                Thread.Sleep(50);
            }

            await logger.Stop(withFlush: true);

            Assert.IsTrue(output.Count == count);
        }

        [Test]
        public async Task CheckNoFlush()
        {
            var output = new List<ILogLine>();
            var folderPath = @"C:\LogTest";
            
            var fileManager = new FileManagerMock(output, folderPath);
            var timeProvider = new TimeProvider();
            var logger = new AsyncLogV2(timeProvider, fileManager, new MidnightFileBuildStrategy(timeProvider, fileManager));
            
            var count = 50;

            for (var i = count; i > 0; i--)
            {
                logger.Write($"Entry no: {i} noflush.");
                Thread.Sleep(10);
            }

            await logger.Stop(withFlush: false);

            Assert.IsTrue(output.Count <= count);
        }

        [Test]
        public void CheckMidnightStrategyValidity()
        {
            /*  
             *  MidnightFileBuildStrategy sets its time in Init() then gets triggered when IsStratregyValid returns true;
             *  TimeProviderMock initializes with 1/1/2021 11:57:00 PM as DateTime
             *  TimeProviderMock increments with 2 minutes per each call
             *  First call, fbStrat.Init(), strategy initializez with 1/1/2021 11:59:00 PM as DateTime
             *  Second call, fbStrat.IsStrategyValid() will be + 2 minutes from the initial DateTime, making the strategy trigger condition valid.
             */
            IFileBuildStrategy fbStrat = new MidnightFileBuildStrategy(new TimeProviderMock(), new FileBuilderMock());

            fbStrat.Init();

            Assert.IsTrue(fbStrat.IsStrategyValid());
        }
    }
}