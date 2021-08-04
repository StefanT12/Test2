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
            _returnCount++;

            if (_returnCount == 1)
            {
                return Convert.ToDateTime("1/1/2021 11:59:00 PM", new CultureInfo("en-US"));
            }
            else
            {
                return Convert.ToDateTime("1/2/2021 00:01:00 AM", new CultureInfo("en-US"));
            }
           
        }

        private int _returnCount;
     
    }
}
