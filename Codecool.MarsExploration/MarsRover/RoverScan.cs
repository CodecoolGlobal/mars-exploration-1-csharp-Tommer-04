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
            int distanceDown;
            int distanceUp;
            int distanceLeft;
            int distanceRight;
            string[,] scannedArea = new string[viewDistance * 2 + 1,viewDistance * 2 + 1];

            //Calculate down
            if(currentPosition.Y <= mapLength - 1 - viewDistance)
            {
                distanceDown = viewDistance;
            } else
            {
                distanceDown = viewDistance - (currentPosition.Y - (mapLength - 1 - viewDistance));
            }

            //Calculate up
            if(currentPosition.Y >= viewDistance) 
            {
                distanceUp = viewDistance;
            } else
            {
                distanceUp = viewDistance + (currentPosition.Y - viewDistance);
            }

            //Calculate right
            if(currentPosition.X <= mapLength - 1 - viewDistance)
            {
                distanceRight = viewDistance;
            } else
            {
                distanceRight = viewDistance - (currentPosition.X - (mapLength - 1 - viewDistance));
            }

            //Calculate left
            if(currentPosition.X >= viewDistance)
            {
                distanceLeft = viewDistance;
            } else
            {
                distanceLeft =  viewDistance + (currentPosition.X - viewDistance);
            }
            return default;
        }
    }
}
