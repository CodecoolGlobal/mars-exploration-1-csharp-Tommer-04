﻿using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Logger;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapLoader;
using Codecool.MarsExploration.MarsRover;
using System;
using System.Reflection.Metadata;

public class Simulator
{
	private readonly IMapLoader _mapLoader;
	private readonly RoverDeployer _roverDeployer;
	private readonly ICoordinateCalculator _coordinateCalculator;
	private readonly RoverScan _roverScan;
	private readonly RoverMerge _roverMerge;
	private readonly FileLogger _fileLogger;
	private readonly string workDir = Directory.GetCurrentDirectory();

	private readonly string _mineralSymbol;
	private readonly string _waterSymbol;
	private readonly int _expectedMineralCount;
	private readonly int _expectedWaterCount;


    public Simulator(IMapLoader mapLoader, RoverDeployer roverDeployer, ICoordinateCalculator coordinateCalculator, RoverScan roverScan, RoverMerge roverMerge, FileLogger fileLogger, string mineralSymbol, string waterSymbol, int expectedMineralCount, int expectedWaterCount)
	{
		_mapLoader = mapLoader;
		_roverDeployer = roverDeployer;
		_coordinateCalculator = coordinateCalculator;
		_roverScan = roverScan;
		_roverMerge = roverMerge;
		_fileLogger = fileLogger;
		_mineralSymbol = mineralSymbol;
		_waterSymbol = waterSymbol;
		_expectedMineralCount = expectedMineralCount;
		_expectedWaterCount = expectedWaterCount;
	}

	public void Run(SimulationContext simulationContext)
	{
		File.WriteAllText($"{workDir}\\logs.txt", "");

		Map map = _mapLoader.Load(simulationContext.mapLocation);
		RoverConfig roverConfig = new RoverConfig(simulationContext.mapLocation, simulationContext.landingSpot, simulationContext.resources, simulationContext.timeOut);
		Rover rover = _roverDeployer.Deploy(simulationContext.id, simulationContext.viewDistance, roverConfig);
        bool simulationOver = false;
		string resultOutcome = "";
		while (!simulationOver)
		{
            Move(rover, map);
            var scannedArea = _roverScan.Scan(rover.CurrentPosition, map.Representation, rover.ViewDistance);
            _roverMerge.Merge(rover.DiscoveredMap, scannedArea.scannedMap, scannedArea.startingCoord);
            Analize(rover);
            rover.Steps++;
            if (rover.IsTask1Successful && rover.IsTask2Successful)
			{
				if(rover.Steps + HowManyStepsToReturn(rover) < rover.TimeOutLimit)
				{
					resultOutcome = $"Everything was successful! Habitable area: X:{rover.HabitableArea.X} Y:{rover.HabitableArea.Y}";
					simulationOver = true;
                    File.AppendAllText($"{workDir}\\logs.txt", $"STEP {rover.Steps}; EVENT outcome; OUTCOME COLONIZABLE" + Environment.NewLine);
                } else
				{
                    resultOutcome = $"Maximum steps reached! (Both task was successful) Habitable area: X:{rover.HabitableArea.X} Y:{rover.HabitableArea.Y}";
					simulationOver = true;
                    File.AppendAllText($"{workDir}\\logs.txt", $"STEP {rover.Steps}; EVENT outcome; OUTCOME COLONIZABLE" + Environment.NewLine);
                }
			}

            if (rover.Steps + HowManyStepsToReturn(rover) > rover.TimeOutLimit)
            {
                resultOutcome = "Maximum steps reached! (No task was successful)";
                simulationOver = true;
				if(!rover.IsTask2Successful && !rover.IsTask1Successful)
                File.AppendAllText($"{workDir}\\logs.txt", $"STEP {rover.Steps}; EVENT outcome; OUTCOME UNHABITABLE" + Environment.NewLine);
            }

            if (rover.IsTask1Successful && rover.Steps + HowManyStepsToReturn(rover) > rover.TimeOutLimit)
            {
                resultOutcome = "Maximum steps reached! (1st task was successful)";
                simulationOver = true;
                File.AppendAllText($"{workDir}\\logs.txt", $"STEP {rover.Steps}; EVENT outcome; OUTCOME TASK ONE" + Environment.NewLine);
            }

            if (rover.IsTask2Successful && rover.Steps + HowManyStepsToReturn(rover) > rover.TimeOutLimit)
            {
                resultOutcome = $"Maximum steps reached! (2nd task was successful) Habitable area: X:{rover.HabitableArea.X} Y:{rover.HabitableArea.Y}";
                simulationOver = true;
                File.AppendAllText($"{workDir}\\logs.txt", $"STEP {rover.Steps}; EVENT outcome; OUTCOME TASK TWO" + Environment.NewLine);
            }

			if (!simulationOver)
			{
                File.AppendAllText($"{workDir}\\logs.txt", $"STEP {rover.Steps}; EVENT position; UNIT {rover.Id}; POSITION [{rover.CurrentPosition.Y},{rover.CurrentPosition.X}]" + Environment.NewLine);
            }
            Console.WriteLine($"STEP {rover.Steps}; EVENT position; UNIT {rover.Id}; POSITION [{rover.CurrentPosition.Y},{rover.CurrentPosition.X}]");			
        }

        Console.WriteLine(resultOutcome);
        Console.WriteLine(" - - - - - - - - - - - - - - - - - - - ");
        Console.WriteLine("Rover's map:");
        for (int i = 0; i < rover.DiscoveredMap.GetLength(0); i++)
		{
			for(int j = 0; j < rover.DiscoveredMap.GetLength(1); j++)
			{
				Console.Write(rover.DiscoveredMap[i, j]);
			}
            Console.WriteLine();
        }
        Console.WriteLine(" - - - - - - - - - - - - - - - - - - - ");
        Console.WriteLine("Original map:");
        for (int i = 0; i < map.Representation.GetLength(0); i++)
        {
            for (int j = 0; j < map.Representation.GetLength(1); j++)
            {
                Console.Write(map.Representation[i, j]);
            }
            Console.WriteLine();
        }
    }

	private int HowManyStepsToReturn(Rover rover)
	{
		return rover.VisitedTiles.Count();
	}

    public void Move(Rover rover, Map map)
    {
		Random random = new Random();
		string[,] mapRepresentation = map.Representation;
		string[,] roverMap = rover.DiscoveredMap;
        int mapLength = mapRepresentation.GetLength(0);
		
		IEnumerable<Coordinate> adjacentCoords = _coordinateCalculator.GetAdjacentCoordinates(rover.CurrentPosition, mapLength);
		List<Coordinate> preferedTiles = new List<Coordinate>();
        List<Coordinate> availableTiles = new List<Coordinate>();
		foreach (Coordinate coord in adjacentCoords)
		{
			if (roverMap[coord.Y, coord.X] == " " && !rover.VisitedTiles.Contains(coord))
			{
				preferedTiles.Add(coord);
			} else if (roverMap[coord.Y, coord.X] == " ")
			{
				availableTiles.Add(coord);
			}
		}

		Coordinate selectedTile;
		if(preferedTiles.Count > 0)
		{
			selectedTile = preferedTiles[random.Next(preferedTiles.Count)];
		} else
		{
			selectedTile = availableTiles[random.Next(availableTiles.Count)];
		}

		rover.VisitedTiles.Add(rover.CurrentPosition);
        rover.CurrentPosition = selectedTile;	
    }

	public void Analize(Rover rover)
	{
		string[,] roverMap = rover.DiscoveredMap;
		List<string> targetResources = rover.TargetResources;

		rover.foundResources.Clear();
		for (int i = 0; i < roverMap.GetLength(0); i++)
		{
			for(int j = 0; j < roverMap.GetLength(1); j++)
			{
				if (targetResources.Contains(roverMap[i, j]))
				{
					rover.foundResources.Add((roverMap[i, j], new Coordinate(j, i)));
				}
			}
		}

		int waterCount = 0;
		int mineralCount = 0;
		foreach(var touple in rover.foundResources)
		{
            if (touple.symbol == _waterSymbol)
			{
				waterCount++;
            }
            if (touple.symbol == _mineralSymbol)
            {
                mineralCount++;
            }
        }

		// TASK 1 CHECK
		if(waterCount >= _expectedWaterCount && mineralCount >= _expectedMineralCount)
		{
			rover.IsTask1Successful = true;
		}

		bool didFindHabitableSpot = false;
        for (int i = 1; i < roverMap.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < roverMap.GetLength(1) - 1; j++)
            {
                var scanResults = _roverScan.Scan(new Coordinate(j, i), roverMap, 1);
				string[,] scanMap = scanResults.scannedMap;
				bool IsThereWater = false;
				bool isThereMineral = false;
				bool isThereSpace = false;
				for(int k = 0; k < scanMap.GetLength(0); k++)
				{
                    for (int l = 0; l < scanMap.GetLength(1); l++)
					{
                        if (scanMap[k,l] == " ")
						{
							isThereSpace = true;
						}
						if(scanMap[k,l] == _waterSymbol)
						{
							IsThereWater = true;
						}
						if (scanMap[k,l] == _mineralSymbol)
						{
							isThereMineral = true;
						}

                        if (isThereMineral && isThereSpace && IsThereWater)
                        {
							rover.HabitableArea = new Coordinate(j, i);
                        }
                    }
				}
                // TASK 2 CHECK
                if (isThereMineral && isThereSpace && IsThereWater)
				{
					rover.IsTask2Successful = true;
				}
				
            }
        }


    }
}
