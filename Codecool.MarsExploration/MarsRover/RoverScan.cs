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
        public (string[,]scannedMap, Coordinate startingCoord) Scan(Coordinate currentPosition, Map map, int viewDistance)
        {
            int mapLength = map.Representation.GetLength(0);
            int distanceDown;
            int distanceUp;
            int distanceLeft;
            int distanceRight;

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

            string[,] scannedArea = new string[distanceUp + distanceDown + 1, distanceLeft + distanceRight + 1];
            
            Coordinate startingCoord = new Coordinate(currentPosition.X - distanceLeft, currentPosition.Y - distanceUp);

            int Xindex = 0;
            int Yindex = 0;

            for(int i = startingCoord.Y; i < startingCoord.Y + scannedArea.GetLength(0); i++)
            {
                for(int j = startingCoord.X; j < startingCoord.X + scannedArea.GetLength(1); j++)
                {
                    scannedArea[Yindex, Xindex] = map.Representation[i,j];
                    Xindex++;
                }
                Yindex++;
                Xindex = 0;
            }

            return (scannedArea, startingCoord);
        }
    }
}
