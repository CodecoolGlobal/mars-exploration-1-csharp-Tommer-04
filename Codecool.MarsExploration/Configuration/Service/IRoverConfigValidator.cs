using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Configuration.Service
{
    public interface IRoverConfigValidator
    {
        bool validate(RoverConfig roverConfig);
    }
}
