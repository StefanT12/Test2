using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTest.Interfaces
{
    public interface IFileBuildStrategy
    {
        void Run(IFileManager fileManager);
        bool TriggerCondition(object param);
    }
}
