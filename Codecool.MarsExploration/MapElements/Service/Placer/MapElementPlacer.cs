using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapElements.Service.Placer
{
    public class MapElementPlacer : IMapElementPlacer
    {
        public bool CanPlaceElement(MapElement element, string?[,] map, Coordinate coordinate)
        {
            bool isInsideTheMap = coordinate.X + element.Dimension - 1 < map.GetLength(0) && coordinate.Y + element.Dimension - 1 < map.GetLength(1);
            bool canBePlaced = true;

            if (isInsideTheMap)
            {
                for (int i = coordinate.Y; i < coordinate.Y + element.Dimension; i++)
                {
                    for (int j = coordinate.X; j < coordinate.X + element.Dimension; j++)
                    {
                        if (map[i, j] != " ")
                        {
                            canBePlaced = false;
                        }
                    }
                }
            }

            return isInsideTheMap && canBePlaced;
        }
        public void PlaceElement(MapElement element, string?[,] map, Coordinate coordinate)
        {
            int representationIndexY = 0;
            int representationIndexX = 0;

            for (int i = coordinate.Y; i < coordinate.Y + element.Dimension; i++)
            {
                for (int j = coordinate.X; j < coordinate.X + element.Dimension; j++)
                {
                    map[i, j] = element.Representation[representationIndexY, representationIndexX];
                    representationIndexX++;
                }
                representationIndexX = 0;
                representationIndexY++;
            }
        }
    }
}
