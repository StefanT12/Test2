using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest.Interfaces
{
    public interface ILogLine
    {
        string Text { get; set; }
        DateTime Timestamp { get; set; }
        string GetLine();
    }
}
