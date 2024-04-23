using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string _workDir = Directory.GetCurrentDirectory();
        public void LogInfo(string message)
        {
            File.AppendAllText($"{_workDir}\\Log.txt", $" {DateTime.Now} [INFO] - {message} " + Environment.NewLine );
        }
        public void LogError(string message)
        {
            File.AppendAllText($"{_workDir}\\Log.txt", $" {DateTime.Now} [ERROR] - {message} " + Environment.NewLine);
        }
    }
}
