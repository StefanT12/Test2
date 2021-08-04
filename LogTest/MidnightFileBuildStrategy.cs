using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    public class MidnightFileBuildStrategy : IFileBuildStrategy
    {
        private DateTime _storedDateTime;
        ITimeProvider _timeProvider;
        IFileBuilder _fileBuilder;
        public void Run()
        {
            if(IsStrategyValid())
            {
                _storedDateTime = _timeProvider.GetTimeNow();
                _fileBuilder.CreateNewFile(_storedDateTime);
            }
        }

        public bool IsStrategyValid()
        {
            return _storedDateTime.Day - _timeProvider.GetTimeNow().Day != 0;
        }

        public void Init()
        {
           
            _storedDateTime = _timeProvider.GetTimeNow();
            _fileBuilder.CreateNewFile(_storedDateTime);
        }

        public MidnightFileBuildStrategy(ITimeProvider timeProvider, IFileBuilder fileBuilder)
        {
            _timeProvider = timeProvider;
            _fileBuilder = fileBuilder;
        }
    }
}
