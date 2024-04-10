using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.MapElements.Model;

namespace Codecool.MarsExploration.MapElements.Service.Placer;

public interface IMapElementPlacer
{
    bool CanPlaceElement(MapElement element, string?[,] map, Coordinate coordinate)
    {
        bool isInsideTheMap = coordinate.X + element.Dimension - 1 <= map.Length && coordinate.Y + element.Dimension - 1 <= map.Length;
        bool canBePlaced = true;

        if (isInsideTheMap)
        {
            for (int i = coordinate.Y; i < element.Dimension; i++)
            {
                for (int j = coordinate.X; j < element.Dimension; j++)
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
    void PlaceElement(MapElement element, string?[,] map, Coordinate coordinate)
    {
        int representationIndexY = 0;
        int representationIndexX = 0;

        for (int i = coordinate.Y; i < element.Dimension; i++)
        {
            for (int j = coordinate.X; j < element.Dimension; j++)
            {
                map[i, j] = element.Representation[representationIndexY, representationIndexX];
                representationIndexX++;
            }
            representationIndexX = 0;
            representationIndexY++;
        }
    }
};
