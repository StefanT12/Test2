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
        public void Run(IFileManager fileManager)
        {
            if ( (DateTime.Now is var dtNow) && TriggerCondition(dtNow) == true )
            {
                fileManager.CreateNewFile(dtNow);
                _storedDateTime = dtNow;
            }
        }
     
        public bool TriggerCondition(object param)
        {
            try
            {
                DateTime dt = (DateTime)param;
                return _storedDateTime.Day - dt.Day != 0;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
           
        }

        public MidnightFileBuildStrategy(DateTime _currDateTime)
        {
            _storedDateTime = _currDateTime;
        }
    }
}
