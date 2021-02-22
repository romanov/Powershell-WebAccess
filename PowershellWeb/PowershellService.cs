using System;

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
