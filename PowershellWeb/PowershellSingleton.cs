using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

        string _currentPath = string.Empty;

        public string CurrentPath
        {
            get => _currentPath;
            set => _currentPath = value;
        }

        public HashSet<string> RecentData { get; } = new HashSet<string>();

        public static PowershellSingleton Instance { get; } = new PowershellSingleton();

        static PowershellSingleton() { }
        private PowershellSingleton() { }

        Process process = null;

        public void CreateConsoleProcess(string command = "(Get-Location).path")
        {
            if (process == null)
            {
                process = new Process();
                process.StartInfo.FileName = @"C:\Program Files\PowerShell\7-preview\pwsh.exe";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.OutputDataReceived += (sender, data) => SaveResult(data.Data);
                process.ErrorDataReceived += (sender, data) => SaveResult(data.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        public void InvokeCommand(string command)
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
            else
            {
                return (0, string.Empty);
            }



        }


    }
}
