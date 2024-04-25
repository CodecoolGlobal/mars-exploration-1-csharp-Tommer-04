using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.Logger;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapElements.Service.Builder;
using Codecool.MarsExploration.MapElements.Service.Generator;
using Codecool.MarsExploration.MapElements.Service.Placer;
using Codecool.MarsExploration.MapLoader;
using Codecool.MarsExploration.MarsRover;
using Codecool.MarsExploration.Output.Service;
using System.Formats.Tar;
using System.Threading.Channels;

internal class Program
{
    public static void Main(string[] args)
    {
        ILogger fileLogger = new FileLogger();
        ILogger consoleLogger = new ConsoleLogger();
        IMapLoader mapLoader = new MapLoader();
        IDimensionCalculator dimensionCalculator = new DimensionCalculator();
        ICoordinateCalculator coordinateCalculator = new CoordinateCalculator();
        IMapElementBuilder mapElementBuilder = new MapElementBuilder(dimensionCalculator, coordinateCalculator);
        IMapElementsGenerator mapElementsGenerator = new MapElementsGenerator(mapElementBuilder);
        IMapConfigurationValidator mapConfigValidator = new MapConFigurationValidator();
        IMapElementPlacer mapElementPlacer = new MapElementPlacer();
        IMapGenerator mapGenerator = new MapGenerator(mapElementsGenerator, mapConfigValidator, mapElementPlacer, coordinateCalculator);
        IRoverConfigValidator roverConfigValidator = new RoverConfigValidator(coordinateCalculator, consoleLogger, mapLoader);
        RoverScan roverScan = new RoverScan();
        RoverMerge roverMerge = new RoverMerge();
        RoverDeployer roverDeployer = new RoverDeployer(roverConfigValidator, mapLoader, roverScan, roverMerge, coordinateCalculator);
        
        


        var mapConfig = GetConfiguration();
        CreateAndWriteMaps(3, mapGenerator, mapConfig);
        


        string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        string location = $"{WorkDir}\\map1.map";
        Coordinate landingSpot = new Coordinate(4, 4);
        int maxStep = 10000;
        IEnumerable<string> symbols = new[] { "%", "*" };

        SimulationContext simulationContext = new SimulationContext("Sim1", 10000, 1, symbols, location, landingSpot);

        //összeraktam ide, de mennem kell mert konzultáció
        Simulator simulator = new Simulator(mapLoader, roverDeployer, coordinateCalculator, roverScan, roverMerge);
        simulator.Run(simulationContext);

        Console.ReadKey();
    }

    private static void CreateAndWriteMaps(int count, IMapGenerator mapGenerator, MapConfiguration mapConfig)
    {
        string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        List<string[]> maps = new List<string[]>(); 

        for(int i = 0; i < count; i++)
        {
            Map map = mapGenerator.Generate(mapConfig);
            string[] writeableMap = new string[map.Representation.GetLength(0)];

            for (int j = 0; j < map.Representation.GetLength(0); j++)
            {
                string concatenated = "";
                for (int k = 0; k < map.Representation.GetLength(1); k++)
                {
                    concatenated += map.Representation[j, k];
                }
                writeableMap[j] = concatenated;
            }
            maps.Add(writeableMap);
        }
        
        for(int i = 0; i < count; i++)
        {
            File.WriteAllLines($"{WorkDir}\\map{i + 1}.map", maps[i]);
        }
        Console.WriteLine($"{count} map(s) generated to the Debug folder!");
    }

    private static MapConfiguration GetConfiguration()
    {
        const string mountainSymbol = "#";
        const string pitSymbol = "&";
        const string mineralSymbol = "%";
        const string waterSymbol = "*";

        var mountainsCfg = new MapElementConfiguration(mountainSymbol, "mountain", new[]
        {
            new ElementToSize(1, 15),
            new ElementToSize(3, 10)
        }, 1);
        var pitCfg = new MapElementConfiguration(pitSymbol, "pit", new[]
        {
            new ElementToSize(1,15),
            new ElementToSize(3, 10)
        }, 3);
        var mineralCfg = new MapElementConfiguration(mineralSymbol, "mineral", new[]
        {
            new ElementToSize(30, 1)
        }, 0, mountainSymbol);
        var waterCfg = new MapElementConfiguration(waterSymbol, "water", new[]
        {
            new ElementToSize(30, 1) 
        }, 0, pitSymbol);

        List<MapElementConfiguration> elementsCfg = new() { mountainsCfg, pitCfg, mineralCfg, waterCfg };
        return new MapConfiguration(40, 1, elementsCfg);
    }
}
