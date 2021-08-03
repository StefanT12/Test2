using LogTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest
{
    public class LogLineV2 : ILogLine
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public string GetLine()
        {
            var sb = new StringBuilder();
            sb.Append(Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            sb.Append("\t");
            sb.Append(Text);
            sb.Append("\t");
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
