using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MarsRover
{
    public class RoverScan
    {
        public string[,] Scan(Coordinate currentPosition, Map map, int viewDistance)
        {
            int mapLength = map.Representation.GetLength(0);
            string[,] scannedArea = new string[viewDistance * 2 + 1,viewDistance * 2 + 1];
            return default;
        }
    }
}
