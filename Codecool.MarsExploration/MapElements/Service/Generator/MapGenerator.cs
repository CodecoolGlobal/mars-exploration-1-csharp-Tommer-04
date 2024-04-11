using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapElements.Service.Placer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapElements.Service.Generator
{
    public class MapGenerator : IMapGenerator
    {
        private readonly IMapElementsGenerator _generator;
        private readonly IMapConfigurationValidator _validator;
        private readonly IMapElementPlacer _placer;
        private readonly ICoordinateCalculator _calculator;

        public MapGenerator(IMapElementsGenerator mapElementsGenerator, IMapConfigurationValidator mapConfigurationValidator, IMapElementPlacer mapElementPlacer, ICoordinateCalculator coordinateCalculator) 
        {
            _generator = mapElementsGenerator;
            _validator = mapConfigurationValidator;
            _placer = mapElementPlacer;
            _calculator = coordinateCalculator;
        }

        public Map Generate(MapConfiguration mapConfiguration)
        {
            bool isValid = _validator.Validate(mapConfiguration);
            if(!isValid)
            {
                return new Map(new string[mapConfiguration.MapSize, mapConfiguration.MapSize] , isValid);
            } else
            {
                IEnumerable<MapElement> elements = _generator.CreateAll(mapConfiguration);

                string[,] map = new string[mapConfiguration.MapSize, mapConfiguration.MapSize];

                for (int i = 0; i < mapConfiguration.MapSize; i++)
                {
                    for (int j = 0; j < mapConfiguration.MapSize; j++)
                    {
                        map[j, i] = " ";
                    }
                }

                foreach(MapElement element in elements) 
                {
                    List<Coordinate> availableCoords = new List<Coordinate>();
                    List<Coordinate> preferedCoords = new List<Coordinate>();

                    for(int i = 0; i <= mapConfiguration.MapSize - element.Dimension; i++)
                    {
                        for(int j = 0; j <=  mapConfiguration.MapSize - element.Dimension; j++)
                        {
                            if(_placer.CanPlaceElement(element, map, new Coordinate(j, i)))
                            {
                                if(element.PreferredLocationSymbol == null)
                                {
                                    availableCoords.Add(new Coordinate(j, i));
                                } else
                                {
                                    IEnumerable<Coordinate> adjacentCoords = _calculator.GetAdjacentCoordinates(new Coordinate(j, i), mapConfiguration.MapSize);
                                    if(adjacentCoords.Any(coord => map[coord.Y, coord.X] == element.PreferredLocationSymbol))
                                    {
                                        preferedCoords.Add(new Coordinate(j, i));
                                    } else
                                    {
                                        availableCoords.Add(new Coordinate(j, i));
                                    }
                                }
                            }
                        }
                    }

                    Coordinate? selectedPlacement = null;
                    if (preferedCoords.Any())
                    {
                        selectedPlacement = preferedCoords.OrderBy(item => Guid.NewGuid()).FirstOrDefault();
                    } else if(availableCoords.Any())
                    {
                        selectedPlacement = availableCoords.OrderBy(item => Guid.NewGuid()).FirstOrDefault();
                    }

                    if(selectedPlacement != null)
                    {
                        _placer.PlaceElement(element, map, selectedPlacement);
                    }
                }
                return new Map(map, isValid);
            }
        }
    }
}
