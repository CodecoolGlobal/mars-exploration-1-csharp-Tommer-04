using Codecool.MarsExploration.Calculators.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MarsRover
{
    public class RoverMerge
    {
        public void Merge(string[,] map, string[,] scannedArea, Coordinate startingCoord)
        {
            int Yindex = 0;
            int Xindex = 0;
            for (int i = startingCoord.Y; i < startingCoord.Y + scannedArea.GetLength(0); i++)
            {
                for (int j = startingCoord.X; j < startingCoord.X + scannedArea.GetLength(1); j++)
                {
                    map[i,j] = scannedArea[Yindex, Xindex];
                    Xindex++;
                }
                Yindex++;
                Xindex = 0;
            }
        }
    }
}
