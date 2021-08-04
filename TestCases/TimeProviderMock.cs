using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCases
{
    public class TimeProviderMock : ITimeProvider
    {
        public DateTime GetTimeNow()
        {
            _time = _time.AddMinutes(2);

            return _time;
        }

        private DateTime _time;

        public TimeProviderMock()
        {
            _time = Convert.ToDateTime("1/1/2021 11:57:00 PM", new CultureInfo("en-US"));
        }
    }
}
