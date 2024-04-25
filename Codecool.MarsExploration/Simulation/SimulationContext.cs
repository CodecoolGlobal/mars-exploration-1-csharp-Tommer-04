using Codecool.MarsExploration.Calculators.Model;
using System;

public record SimulationContext(string id, int timeOut, int viewDistance, IEnumerable<string> resources, string mapLocation, Coordinate landingSpot)
{

}
