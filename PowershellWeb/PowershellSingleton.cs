using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowershellWeb
{

    public sealed class PowershellSingleton
    {

        string _key = RandomHelper.RandomString(20);

        public string Key
        {
            get { return _key; }
        }

        public void TrySetKey(string key)
        {
            if (!string.IsNullOrEmpty(key))
                _key = key;

            Console.WriteLine("SERVER KEY: " + Key);
        }


        public string CurrentPath
        {
          get
          {
            InvokeCommand();
            WaitForResults(2);

            var path = RecentData.Last();
            RecentData.Clear();
            return path;
          }
        }

        internal void WaitForResults(int minimumResults = 1)
        {
          var waitTask = Task.Factory.StartNew(() =>
          {
            var recentCount = -1;
            while (RecentData.Count != recentCount || RecentData.Count < minimumResults)
            {
              recentCount = RecentData.Count;
              Task.Delay(30).Wait();
            }
          });

          waitTask.Wait();
        }

        public HashSet<string> RecentData { get; } = new HashSet<string>();

        public static PowershellSingleton Instance { get; } = new PowershellSingleton();

        static PowershellSingleton() { }
        private PowershellSingleton() { }

        Process process = null;

        public void CreateConsoleProcess()
        {
            if (process == null)
            {
              process = new Process
              {
                StartInfo =
                {
                  FileName = "pwsh.exe",
                  CreateNoWindow = true,
                  UseShellExecute = false,
                  RedirectStandardOutput = true,
                  RedirectStandardError = true,
                  RedirectStandardInput = true
                }
              };
              process.OutputDataReceived += (sender, data) => SaveResult(data.Data);
                process.ErrorDataReceived += (sender, data) => SaveResult(data.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        public void InvokeCommand(string command = "(Get-Location).path")
        {
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
        }

        void SaveResult(string inputData)
        {
            RecentData.Add(inputData);
            Console.WriteLine(inputData);
        }

        public (byte, string) GetLastResult()
        {
          if (RecentData.Count > 0)
            {
                var sb = new StringBuilder();

                foreach (var str in RecentData)
                    sb.AppendLine(str);

                RecentData.Clear();

                return (1, sb.ToString());

            }

          return (0, string.Empty);



        }


    }
}
