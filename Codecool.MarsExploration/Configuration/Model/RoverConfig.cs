using Codecool.MarsExploration.Calculators.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Configuration.Model
{
    public record RoverConfig(string location, Coordinate landingSpot, IEnumerable<string> symbols, int maxSteps)
    {
        public override string ToString()
        {
            string symbol = "";
            foreach (var item in symbols)
            {
                symbol += item + " ";
            }
            return $"\n {location} \n {landingSpot} \n {symbol} \n {maxSteps}";
        }
    }
}
