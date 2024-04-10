using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapElements.Service.Builder;
using Codecool.MarsExploration.MapElements.Service.Generator;
using Codecool.MarsExploration.MapElements.Service.Placer;
using Codecool.MarsExploration.Output.Service;

internal class Program
{
    //You can change this to any directory you like
    private static readonly string WorkDir = AppDomain.CurrentDomain.BaseDirectory;

    public static void Main(string[] args)
    {
        Console.WriteLine("Mars Exploration Sprint 1");
        var mapConfig = GetConfiguration();

        string[,] twodimArray =
        {
            {"asd","asd2" },
            {"secondasd","secondasd2" }
        };
        Map map = new Map(twodimArray);
        IDimensionCalculator dimensionCalculator = null;
        ICoordinateCalculator coordinateCalculator = new CordinateCalculator();
        Coordinate coordinate = new Coordinate(2,3);
        IMapElementBuilder mapElementFactory = null;
        IMapElementsGenerator mapElementsGenerator = null;

        IMapConfigurationValidator mapConfigValidator = null;
        IMapElementPlacer mapElementPlacer = null;

        IMapGenerator mapGenerator = null;

        CreateAndWriteMaps(3, mapGenerator, mapConfig);

        Console.WriteLine("Mars maps successfully generated.");
        Console.ReadKey();

        IEnumerable<Coordinate> coordinates = coordinateCalculator.GetAdjacentCoordinates(coordinate, 5);

        foreach(Coordinate cord in coordinates)
        {
            Console.WriteLine(cord);
        }

        Console.WriteLine(map);

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
            new ElementToSize(2, 20),
            new ElementToSize(1, 30),
        }, 3);

        List<MapElementConfiguration> elementsCfg = new() { mountainsCfg };
        return new MapConfiguration(1000, 0.5, elementsCfg);
    }
}
