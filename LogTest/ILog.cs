using System.Linq;
using System.Threading.Tasks;

namespace LogTest
{
    public interface ILog
    {
        void Stop(bool withFlush = false);

        /// <summary>
        /// Write a message to the Log.
        /// </summary>
        /// <param name="text">The text to written to the log</param>
        void Write(string text);

    }
}
