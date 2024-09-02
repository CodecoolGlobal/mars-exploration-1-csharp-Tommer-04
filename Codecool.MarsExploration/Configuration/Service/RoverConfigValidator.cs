using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Logger;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Configuration.Service
{
    public class RoverConfigValidator : IRoverConfigValidator
    {
        private readonly ICoordinateCalculator _coordinateCalculator;
        private readonly ILogger _logger;
        private readonly IMapLoader _mapLoader;

        private readonly string _mountainSymbol;
        private readonly string _pitSymbol;
        private readonly string _mineralSymbol;
        private readonly string _waterSymbol;

        public RoverConfigValidator(ICoordinateCalculator coordinateCalculator, ILogger logger, IMapLoader mapLoader, string mountainSymbol, string pitSymbol, string mineralSymbol, string waterSymbol) 
        { 
            _coordinateCalculator = coordinateCalculator;
            _logger = logger;
            _mapLoader = mapLoader;
            _mountainSymbol = mountainSymbol;
            _pitSymbol = pitSymbol;
            _mineralSymbol = mineralSymbol;
            _waterSymbol = waterSymbol;
        }

        public bool validate(RoverConfig roverConfig)
        {
            if (string.IsNullOrEmpty(roverConfig.location))
            {
                _logger.LogError("File not found");
                return false;
            };
            Map map = _mapLoader.Load(roverConfig.location);

            if (roverConfig == null)
            {
                _logger.LogError("RoverConfig is empty or null");
                return false;
            };

            if (map.Representation[roverConfig.landingSpot.Y, roverConfig.landingSpot.X] != " ")
            {
                _logger.LogError("Landing spot is occupied");
                _logger.LogError($"{map.Representation[roverConfig.landingSpot.Y, roverConfig.landingSpot.X]}");
                return false;
            };

            IEnumerable<Coordinate> adjacentCoords = _coordinateCalculator.GetAdjacentCoordinates(roverConfig.landingSpot, map.Representation.GetLength(0));
            bool hasValidAdjacentCoord = false;
            foreach (Coordinate coord in adjacentCoords)
            {
                if (map.Representation[coord.Y, coord.X] == " ") hasValidAdjacentCoord = true;
            }
            if (!hasValidAdjacentCoord)
            {
                _logger.LogError("Has no empty adjacent coordinate");
                return false;
            };

            string[] validSymbols = { _mineralSymbol, _waterSymbol, _mountainSymbol, _pitSymbol };
            if (roverConfig.symbols.Count() < 1)
            {
                _logger.LogError("Has no symbols");
                return false;

            } else
            {
                foreach(string symbol in roverConfig.symbols)
                {
                    if (!validSymbols.Contains(symbol))
                    {
                        _logger.LogError("Invalid symbol(s)");
                        return false;
                    };
                }
            }

            if (roverConfig.maxSteps <= 0)
            {
                _logger.LogError("Max steps are below or equal to 0");
                return false;
            };

            return true;
        }
    }
}
