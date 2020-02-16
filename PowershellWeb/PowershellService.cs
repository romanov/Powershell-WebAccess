using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowershellWeb
{
    public class PowershellService : IDisposable
    {

        public void Main()
        {
            PowershellSingleton.Instance.CreateConsoleProcess();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
