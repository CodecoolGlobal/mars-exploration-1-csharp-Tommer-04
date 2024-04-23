using Codecool.MarsExploration.Calculators.Model;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Calculators.Service
{
    public class CoordinateCalculator : ICoordinateCalculator
    {
        public Coordinate GetRandomCoordinate(int dimension)
        {
            Random random = new Random();
            return new Coordinate(random.Next(0,dimension), random.Next(0,dimension));
        }
        public IEnumerable<Coordinate> GetAdjacentCoordinates(Coordinate coordinate, int dimension)
        {
            List<Coordinate> adjacentCoords = new List<Coordinate>();
            if (coordinate.X > 0)
            {
                adjacentCoords.Add(new Coordinate(coordinate.X - 1, coordinate.Y));
            }
            if (coordinate.X < dimension - 1)
            {
                adjacentCoords.Add(new Coordinate(coordinate.X + 1, coordinate.Y));
            }
            if (coordinate.Y > 0)
            {
                adjacentCoords.Add(new Coordinate(coordinate.X, coordinate.Y - 1));
            }
            if (coordinate.Y < dimension - 1)
            {
                adjacentCoords.Add(new Coordinate(coordinate.X, coordinate.Y + 1));
            }
            return adjacentCoords;
        }
        public IEnumerable<Coordinate> GetAdjacentCoordinates(IEnumerable<Coordinate> coordinates, int dimension)
        {
            List<Coordinate> adjacentCoordinates = new List<Coordinate>();
            return adjacentCoordinates;
        }
    }
}
