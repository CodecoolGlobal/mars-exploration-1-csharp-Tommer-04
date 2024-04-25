using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapLoader;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MarsRover
{
    public class RoverDeployer
    {
        private readonly IRoverConfigValidator _roverConfigValidator;
        private readonly IMapLoader _mapLoader;
        private readonly RoverScan _roverScan;
        private readonly RoverMerge _roverMerge;
        private readonly ICoordinateCalculator _coordinateCalculator;
        public RoverDeployer(IRoverConfigValidator roverConfigValidator, IMapLoader mapLoader, RoverScan roverScan, RoverMerge roverMerge, ICoordinateCalculator coordinateCalculator) 
        {
            _roverConfigValidator = roverConfigValidator;
            _mapLoader = mapLoader;
            _roverScan = roverScan;
            _roverMerge = roverMerge;
            _coordinateCalculator = coordinateCalculator;
        }

        public Rover Deploy(string id, int viewDistance, RoverConfig roverConfig)
        {
            if(!_roverConfigValidator.validate(roverConfig))
            {
                 throw new InvalidDataException();
            }

            Map map = _mapLoader.Load(roverConfig.location);

            var adjCoords = _coordinateCalculator.GetAdjacentCoordinates(roverConfig.landingSpot, map.Representation.GetLength(0));
            Coordinate startingCoordinate = new Coordinate(0, 0);
            foreach (var coords in adjCoords)
            {
                if (map.Representation[coords.Y, coords.X] == " ")
                {
                    startingCoordinate = coords;
                }
            }

            string[,] roversMap = new string[map.Representation.GetLength(0), map.Representation.GetLength(1)];
            for (int i = 0; i < roversMap.GetLength(0); i++)
            {
                for (int j = 0; j < roversMap.GetLength(1); j++)
                {
                    roversMap[i, j] = "X";
                }
            }

            var startingScan = _roverScan.Scan(startingCoordinate, map.Representation, viewDistance);
            _roverMerge.Merge(roversMap, startingScan.scannedMap, startingScan.startingCoord);
            
            return new Rover(id, startingCoordinate, viewDistance, roverConfig.symbols.ToList(), roversMap, roverConfig.landingSpot, roverConfig.maxSteps);
        }

    }
}
