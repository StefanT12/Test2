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
             *  MidnightFileBuildStrategy sets its time then it gets triggered when IsStratregyValid returns true;
             *  TimeProviderMock returns 1/1/2021 11:59:00 PM when strategy sets its time,
             *  then 1/2/2021 00:01:00 AM when validity is checked, therefore triggering the strategy 
            */
            IFileBuildStrategy fbStrat = new MidnightFileBuildStrategy(new TimeProviderMock(), new FileBuilderMock());

            fbStrat.Init();

            Assert.IsTrue(fbStrat.IsStrategyValid());
        }
    }
}