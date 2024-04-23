using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void LogInfo(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] [INFO]  - {message}");
        }
        public void LogError(string message)
        {
            Console.WriteLine($"[{DateTime.Now}] [ERROR]  - {message}");
        }
    }
}
