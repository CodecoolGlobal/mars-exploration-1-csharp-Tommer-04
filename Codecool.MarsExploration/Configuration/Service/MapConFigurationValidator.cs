using Codecool.MarsExploration.Configuration.Model;

namespace Codecool.MarsExploration.Configuration.Service
{
    public class MapConFigurationValidator : IMapConfigurationValidator
    {
        public bool Validate(MapConfiguration configuration)
        {
            if (configuration.MapSize <= 0)
            {
                return false;
            }

            if (configuration.ElementToSpaceRatio < 0 || configuration.ElementToSpaceRatio > 1)
            {
                return false;
            }

            if (configuration.MapElementConfigurations == null || !configuration.MapElementConfigurations.Any())
            {
                return false;
            }


            foreach (var elementConfig in configuration.MapElementConfigurations)
            {
                
                if (string.IsNullOrEmpty(elementConfig.Symbol))
                {
                    return false;
                }

                
                if (string.IsNullOrEmpty(elementConfig.Name))
                {
                    return false;
                }

                
                if (elementConfig.ElementsToDimensions == null || !elementConfig.ElementsToDimensions.Any())
                {
                    return false;
                }

                switch (elementConfig.Symbol)
                {
                    case "#":                                            // mountain
                    case "&":                                            // pit
                        if (elementConfig.DimensionGrowth <= 0)
                        {
                            return false;
                        }
                        break;

                    case "%":                                            // mineral
                    case "*":                                            // water
                        if (elementConfig.DimensionGrowth != 0)
                        {
                            return false;
                        }
                        break;

                    default:
                        return false;
                }
            }

            
            int totalElementsToGenerate = (int)(configuration.MapSize * configuration.MapSize * configuration.ElementToSpaceRatio);

            int totalElementCounts = configuration.MapElementConfigurations.Sum(ec => ec.ElementsToDimensions.Sum(etd => etd.ElementCount));

            if (totalElementCounts > totalElementsToGenerate)
            {
                return false;
            }

            // All validations passed
            return true;
        }
    }
}
