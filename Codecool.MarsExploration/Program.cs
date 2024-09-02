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
        FileLogger fileLogger = new FileLogger();
        ILogger consoleLogger = new ConsoleLogger();
        IMapLoader mapLoader = new MapLoader();
        IDimensionCalculator dimensionCalculator = new DimensionCalculator();
        ICoordinateCalculator coordinateCalculator = new CoordinateCalculator();
        IMapElementBuilder mapElementBuilder = new MapElementBuilder(dimensionCalculator, coordinateCalculator);
        IMapElementsGenerator mapElementsGenerator = new MapElementsGenerator(mapElementBuilder);
        IMapElementPlacer mapElementPlacer = new MapElementPlacer();
        RoverScan roverScan = new RoverScan();
        RoverMerge roverMerge = new RoverMerge();



        //
        //
        //
        //
        //
        //
        //
        // YOU CAN EDIT THE CONFIG BELOW FOR THE SIMULATION
        //
        //
        //
        //
        //
        //
        //



        //You can see the which symbol represents what on the map 
        const string mountainSymbol = "#";
        const string pitSymbol = "&";
        const string mineralSymbol = "%";
        const string waterSymbol = "*";

        //You can set the requirments of the 1st task the rover has
        const int requiredMineralCount = 3;
        const int requiredWaterCount = 3;

        //You can add extra spread for mountains and pits depending on how tight or separated you want them to be.
        const int mountainDimensionGrowth = 1;
        const int pitDimensionGrowth = 1;

        //You can set how much water and mineral you want to generate
        const int mineralCount = 30;
        const int waterCount = 30;

        //You can change where the map gets generated and the name of the generated map
        string WorkDir = AppDomain.CurrentDomain.BaseDirectory;
        string location = $"{WorkDir}\\map1.map";

        //You can change the size of the map
        const int mapSize = 40;

        //You can set a starting cordinate for the rover (Make sure its on the map)
        Coordinate landingSpot = new Coordinate(4, 4);

        //You can set how many steps the rover does before timing out and returning (If it completes the 2 task it stops before the timeout)
        int maxStep = 10000;

        //You can set the ID of the rover 
        const string roverId = "Rover-1";

        //You can set the viewdistance of the rover (How many units it can see in each direction)
        const int roverViewDistance = 1;


        static MapConfiguration GetConfiguration()
        {
            var mountainsCfg = new MapElementConfiguration(mountainSymbol, "mountain", new[]
            {
            new ElementToSize(1, 15), //Each of these new objects add a new mountain
            new ElementToSize(3, 10)  //This one adds a 3 unit big mountain spread across a 10 unit large area
        }, mountainDimensionGrowth);

            var pitCfg = new MapElementConfiguration(pitSymbol, "pit", new[]
            {
            new ElementToSize(1,15), //Each of these new objects add a new pit
            new ElementToSize(3, 10) //This one adds a 3 unit big pit spread across a 10 unit large area
        }, pitDimensionGrowth);

            var mineralCfg = new MapElementConfiguration(mineralSymbol, "mineral", new[]
            {
            new ElementToSize(mineralCount, 1), // No need to change this
        }, 0, mountainSymbol);

            var waterCfg = new MapElementConfiguration(waterSymbol, "water", new[]
            {
            new ElementToSize(waterCount, 1)  // No need to change this
        }, 0, pitSymbol);

            List<MapElementConfiguration> elementsCfg = new() { mountainsCfg, pitCfg, mineralCfg, waterCfg };
            return new MapConfiguration(mapSize, 1, elementsCfg);
        }
        



        //
        //
        //
        //
        //
        //
        //
        // YOU CAN EDIT THE CONFIG ABOVE FOR THE SIMULATION
        //
        //
        //
        //
        //
        //
        //
        //




        IMapConfigurationValidator mapConfigValidator = new MapConFigurationValidator(mountainSymbol, pitSymbol, mineralSymbol, waterSymbol);
        IMapGenerator mapGenerator = new MapGenerator(mapElementsGenerator, mapConfigValidator, mapElementPlacer, coordinateCalculator);
        IRoverConfigValidator roverConfigValidator = new RoverConfigValidator(coordinateCalculator, consoleLogger, mapLoader, mountainSymbol, pitSymbol, mineralSymbol, waterSymbol);
        RoverDeployer roverDeployer = new RoverDeployer(roverConfigValidator, mapLoader, roverScan, roverMerge, coordinateCalculator);
        var mapConfig = GetConfiguration();
        CreateAndWriteMaps(1, mapGenerator, mapConfig);
   
        IEnumerable<string> symbols = new[] { mineralSymbol, waterSymbol };

        SimulationContext simulationContext = new SimulationContext(roverId, maxStep, roverViewDistance, symbols, location, landingSpot);
        Simulator simulator = new Simulator(mapLoader, roverDeployer, coordinateCalculator, roverScan, roverMerge, fileLogger, mineralSymbol, waterSymbol, requiredMineralCount, requiredWaterCount);
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

    
}
