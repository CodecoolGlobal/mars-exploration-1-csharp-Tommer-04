using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapElements.Service.Builder
{
    public class MapElementBuilder : IMapElementBuilder
    {
        private readonly IDimensionCalculator _dimensionCalculator;
        private readonly ICoordinateCalculator _coordinateCalculator;
        public MapElementBuilder(IDimensionCalculator dimensionCalculator, ICoordinateCalculator coordinateCalculator)
        {
            _dimensionCalculator = dimensionCalculator;
            _coordinateCalculator = coordinateCalculator;
        }
        public MapElement Build(int size, string symbol, string name, int dimensionGrowth, string? preferredLocationSymbol = null)
        {
            int dimension = _dimensionCalculator.CalculateDimension(size, dimensionGrowth);
            string[,] grid = new string[dimension,dimension];

            for(int i = 0; i < dimension; i++)
            {
                for(int j = 0; j < dimension; j++)
                {
                    grid[i, j] = " ";
                }
            }

            int placedSymbols = 0;
            while(placedSymbols != size)
            {
                Coordinate randomCord = _coordinateCalculator.GetRandomCoordinate(dimension);
                if (grid[randomCord.X,randomCord.Y] == " ")
                {
                    grid[randomCord.X, randomCord.Y] = symbol;
                    placedSymbols++;
                }
            }

            return new MapElement(grid, name, dimension, preferredLocationSymbol);
        }
    }
}
