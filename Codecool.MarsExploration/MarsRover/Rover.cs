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
        
        public Coordinate RocketPosition { get; }
        public int ViewDistance { get; }
        public List<string> TargetResources { get; } 

        public List<(string symbol, Coordinate coordinate)> foundResources = new List<(string symbol, Coordinate coordinate)>();
        public string[,] DiscoveredMap { get; set; }

        public List<Coordinate> VisitedTiles { get; set; }

        public int Steps {  get; set; }

        public int TimeOutLimit { get; }

        public bool IsTask1Successful {  get; set; }
        public bool IsTask2Successful { get; set; }

        public Coordinate HabitableArea { get; set; }

        public Rover(string id, Coordinate currentPosition, int viewDistance, List<string> targetResources, string[,] discoveredMap, Coordinate rocketPosition, int timeOutLimit)
        {
            Id = id;
            CurrentPosition = currentPosition;
            ViewDistance = viewDistance;
            TargetResources = targetResources;
            DiscoveredMap = discoveredMap;
            RocketPosition = rocketPosition;
            Steps = 0;
            TimeOutLimit = timeOutLimit;
            VisitedTiles = [currentPosition];
            IsTask1Successful = false;
            IsTask2Successful = false;
            HabitableArea = new Coordinate(0,0);
        }

    }
}
