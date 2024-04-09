using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecool.MarsExploration.Calculators.Service
{
    public class DimensionCalculator : IDimensionCalculator
    {
        public int CalculateDimension(int size, int dimensionGrowth)
        {
            return (int)Math.Sqrt(size) + 1 + dimensionGrowth;
        }
    }
}
