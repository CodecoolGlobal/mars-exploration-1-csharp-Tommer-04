using Codecool.MarsExploration.Configuration.Model;

namespace Codecool.MarsExploration.Configuration.Service
{
    public class MapConFigurationValidator : IMapConfigurationValidator
    {
        private readonly string _mountainSymbol;
        private readonly string _pitSymbol;
        private readonly string _mineralSymbol;
        private readonly string _waterSymbol;

        public MapConFigurationValidator(string mountainSymbol, string pitSymbol, string mineralSymbol, string waterSymbol) 
        { 
            _mountainSymbol = mountainSymbol;
            _pitSymbol = pitSymbol;
            _mineralSymbol = mineralSymbol;
            _waterSymbol = waterSymbol;
        }

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

                if(elementConfig.Symbol == _mountainSymbol || elementConfig.Symbol == _pitSymbol)
                {
                    if (elementConfig.DimensionGrowth <= 0)
                    {
                        return false;
                    }
                    break;
                } else if (elementConfig.Symbol == _mineralSymbol || elementConfig.Symbol == _waterSymbol)
                {
                    if (elementConfig.DimensionGrowth != 0)
                    {
                        return false;
                    }
                    break;
                } else { return false; }             
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
