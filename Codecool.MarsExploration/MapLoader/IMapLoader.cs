using Codecool.MarsExploration.MapElements.Model;

namespace Codecool.MarsExploration.MapExplorer.MapLoader;

public interface IMapLoader
{
    Map Load(string mapFile);
}
