using LogTest;
using LogTest.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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

            var log = new AsyncLogV2(new FileManager(folderPath), new MidnightFileBuildStrategy(DateTime.Now));

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
            var logs = new List<ILogLine>();
            var logger = new AsyncLogV2(new FileManagerTest(logs), new MidnightFileBuildStrategy(DateTime.Now));
            var count = 15;

            for(int i = 0; i < count; i++)
            {
                logger.Write($"Entry no: {i} w/flush.");
                Thread.Sleep(50);
            }

            await logger.Stop(withFlush: true);

            Assert.IsTrue(logs.Count == count);
        }

        [Test]
        public async Task CheckNoFlush()
        {
            var logs = new List<ILogLine>();
            var logger = new AsyncLogV2(new FileManagerTest(logs), new MidnightFileBuildStrategy(DateTime.Now));
            var count = 50;

            for (var i = count; i > 0; i--)
            {
                logger.Write($"Entry no: {i} noflush.");
                Thread.Sleep(10);
            }

            await logger.Stop(withFlush: false);

            Assert.IsTrue(logs.Count <= count);
        }

        [Test]
        public void CheckMidnightStrategy()
        {
            var dt1 = Convert.ToDateTime("1/1/2021 11:59:00 PM", new CultureInfo("en-US"));

            IFileBuildStrategy fbStrat = new MidnightFileBuildStrategy(dt1);

            var dt2 = Convert.ToDateTime("1/2/2021 00:01:00 AM", new CultureInfo("en-US"));

            Assert.IsTrue(fbStrat.TriggerCondition(dt2));
        }
    }
}