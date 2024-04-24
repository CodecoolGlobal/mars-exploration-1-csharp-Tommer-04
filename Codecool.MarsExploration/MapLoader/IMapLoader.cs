using Codecool.MarsExploration.MapElements.Model;

namespace Codecool.MarsExploration.MapLoader;

public interface IMapLoader
{
    Map Load(string mapFile);
}
