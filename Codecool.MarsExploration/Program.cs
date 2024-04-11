﻿using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapElements.Service.Builder;
using Codecool.MarsExploration.MapElements.Service.Generator;
using Codecool.MarsExploration.MapElements.Service.Placer;
using Codecool.MarsExploration.Output.Service;
using System.Formats.Tar;
using System.Threading.Channels;

internal class Program
{
    //You can change this to any directory you like

    public static void Main(string[] args)
    {

        Console.WriteLine("Mars Exploration Sprint 1");
        var mapConfig = GetConfiguration();
        string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

        IDimensionCalculator dimensionCalculator = new DimensionCalculator();
        ICoordinateCalculator coordinateCalculator = new CordinateCalculator();
        IMapElementBuilder mapElementBuilder = new MapElementBuilder(dimensionCalculator, coordinateCalculator);
        IMapElementsGenerator mapElementsGenerator = new MapElementsGenerator(mapElementBuilder);

        IMapConfigurationValidator mapConfigValidator = new MapConFigurationValidator();
        IMapElementPlacer mapElementPlacer = new MapElementPlacer();

        IMapGenerator mapGenerator = new MapGenerator(mapElementsGenerator, mapConfigValidator, mapElementPlacer, coordinateCalculator);

        //CreateAndWriteMaps(3, mapGenerator, mapConfig);

        Console.WriteLine("Mars maps successfully generated.");
        Map map = mapGenerator.Generate(mapConfig);
            for (int i = 0; i < map.Representation.GetLength(0); i++)
            {
                for (int j = 0; j < map.Representation.GetLength(1); j++)
                {
                    Console.Write(map.Representation[i, j]);
                }
                Console.WriteLine();
            }

        string[] lineArray = Enumerable.Range(0, map.Representation.GetLength(0))
        .Select(row => string.Concat(Enumerable.Range(0, map.Representation.GetLength(1))
        .Select(col => map.Representation[row, col])))
        .ToArray();
        File.WriteAllLines($"{WorkDir}\\test.map", lines);
        Console.ReadKey();
    }

    private static void CreateAndWriteMaps(int count, IMapGenerator mapGenerator, MapConfiguration mapConfig)
    {
        

        
    }

    private static MapConfiguration GetConfiguration()
    {
        const string mountainSymbol = "#";
        const string pitSymbol = "&";
        const string mineralSymbol = "%";
        const string waterSymbol = "*";

        var mountainsCfg = new MapElementConfiguration(mountainSymbol, "mountain", new[]
        {
            new ElementToSize(1, 5),
        }, 1);
        var pitCfg = new MapElementConfiguration(pitSymbol, "pit", new[]
        {
            new ElementToSize(1, 15),
            new ElementToSize(3, 10),
        }, 3);
        var mineralCfg = new MapElementConfiguration(mineralSymbol, "mineral", new[]
        {
            new ElementToSize(40, 1)
        }, 0, mountainSymbol);
        var waterCfg = new MapElementConfiguration(waterSymbol, "water", new[]
        {
            new ElementToSize(30, 1)
        }, 0, pitSymbol);

        List<MapElementConfiguration> elementsCfg = new() { mountainsCfg, pitCfg, mineralCfg, waterCfg };
        return new MapConfiguration(30, 1, elementsCfg);
    }
}
