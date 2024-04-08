namespace Codecool.MarsExploration.Configuration.Model;

public record MapElementConfiguration(
    string Symbol,
    string Name,
    IEnumerable<ElementToSize> ElementsToDimensions,
    int DimensionGrowth = 0,
    string? PreferredLocationSymbol = null);
