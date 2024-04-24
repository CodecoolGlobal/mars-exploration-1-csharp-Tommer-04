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
        
        public Coordinate RocketPosittion { get; }
        public int ViewDistance { get; }
        public List<string> TargetResources { get; } 

        public List<(string symbol, Coordinate coordinate)> foundResources = new List<(string symbol, Coordinate coordinate)>();
        public string[,] DiscoveredTiles { get; set; }

        public int Steps {  get; set; }

        public int TimeOutLimit { get; }

        public Rover(string id, Coordinate currentPosition, int viewDistance, List<string> targetResources, string[,] discoveredTiles, Coordinate rocketPosittion, int timeOutLimit)
        {
            Id = id;
            CurrentPosition = currentPosition;
            ViewDistance = viewDistance;
            TargetResources = targetResources;
            DiscoveredTiles = discoveredTiles;
            RocketPosittion = rocketPosittion;
            Steps = 0;
            TimeOutLimit = timeOutLimit;
        }
    }
}
