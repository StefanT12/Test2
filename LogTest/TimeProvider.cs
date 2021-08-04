using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetTimeNow()
        {
            return DateTime.Now;
        }
    }
}
