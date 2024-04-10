using Codecool.MarsExploration.MapElements.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.MapElements.Service.Builder
{
    public class MapElementBuilder : IMapElementBuilder
    {
        public MapElement Build(int size, string symbol, string name, int dimensionGrowth, string? preferredLocationSymbol = null)
        {







            string[,] tda =
            {
                {"asd",symbol },
                {"kutyá","majom" }
            };
            return new MapElement(tda, "asd", 5);
        }
    }
}
