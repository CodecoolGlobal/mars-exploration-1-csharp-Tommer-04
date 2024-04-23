using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MarsRover
{
    public class Rover
    {
        public string Id { get; }
        public Coordinate CurrentPosition { get; set; }
        public int ViewDistance { get; }

        public List<(string symbol, Coordinate coordinate)> foundResources = new List<(string symbol, Coordinate coordinate)>();
        public string[,] DiscoveredTiles { get; }

        public Rover(string id, Coordinate currentPosition, int viewDistance)
        {
            Id = id;
            CurrentPosition = currentPosition;
            ViewDistance = viewDistance;
        }
    }
}
