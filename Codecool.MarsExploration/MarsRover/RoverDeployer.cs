using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MarsRover
{
    public class RoverDeployer
    {
        private readonly IRoverConfigValidator _roverConfigValidator;
        private readonly IMapLoader _mapLoader;
        public RoverDeployer(IRoverConfigValidator roverConfigValidator, IMapLoader mapLoader) 
        {
            _roverConfigValidator = roverConfigValidator;
            _mapLoader = mapLoader;
        }

        public Rover Deploy(string id, int viewDistance, RoverConfig roverConfig)
        {
            if(_roverConfigValidator.validate(roverConfig))
            {
                throw new InvalidDataException();
            }

            Map map = _mapLoader.Load(roverConfig.location);
            int mapLength = map.Representation.GetLength(0);

            return default;
        }

    }
}
