using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.MapElements.Model;
using Codecool.MarsExploration.MapElements.Service.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapElements.Service.Generator
{
    public class MapElementsGenerator : IMapElementsGenerator
    {
        private readonly IMapElementBuilder _builder;

        public MapElementsGenerator(IMapElementBuilder builder)
        {
            _builder = builder;
        }

        public IEnumerable<MapElement> CreateAll(MapConfiguration mapConfig)
        {
            IEnumerable<MapElementConfiguration> configs = mapConfig.MapElementConfigurations;

            foreach (MapElementConfiguration config in configs)
            {
                foreach(ElementToSize element in config.ElementsToDimensions)
                {
                    for(int i = 0; i < element.ElementCount; i++)
                    {
                        yield return _builder.Build(element.Size, config.Symbol, config.Name, config.DimensionGrowth, config.PreferredLocationSymbol);
                    }
                }
            }
        }
    }
}
