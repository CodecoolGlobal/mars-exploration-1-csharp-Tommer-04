using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using System.ComponentModel.DataAnnotations;
using Moq;
using Codecool.MarsExploration.MapElements.Service.Builder;
using Codecool.MarsExploration.MapElements.Service.Generator;
using Codecool.MarsExploration.MapElements.Service.Placer;

namespace MarsExplorationTest

{
    [TestFixture]
    public class MarsExplorationTest
    {
        private ICoordinateCalculator coordinateCalculator;
        private IDimensionCalculator dimensionCalculator;
        private MapConFigurationValidator validator;
        private IMapElementsGenerator elementsGenerator;
        private IMapElementBuilder mapElementBuilder;
        private IMapGenerator generator;
        private IMapElementPlacer elementPlacer;

        [SetUp]
        public void Setup()
        {
            
            coordinateCalculator = new CordinateCalculator();
            validator = new MapConFigurationValidator();
            dimensionCalculator = new DimensionCalculator();
            mapElementBuilder = new MapElementBuilder(dimensionCalculator, coordinateCalculator);
            elementsGenerator = new MapElementsGenerator(mapElementBuilder);
            elementPlacer = new MapElementPlacer();
            generator = new MapGenerator(elementsGenerator, validator, elementPlacer,coordinateCalculator);

        }
        [Test]
        public void AllDirections()
        {
            //Arrange
            int dimension = 5;
            Coordinate coords = new Coordinate(2,2);

            //Act
            IEnumerable<Coordinate> adjacentCoordinates = coordinateCalculator.GetAdjacentCoordinates(coords, dimension);
            int index = 0;

            //Assert
            Assert.That(adjacentCoordinates.Count, Is.EqualTo(4));
            foreach(Coordinate coord in adjacentCoordinates)
            { 
                if(index == 0)
                {
                    Assert.That(coord.X, Is.EqualTo(1));
                    Assert.That(coord.Y, Is.EqualTo(2));
                }
                if (index == 1)
                {
                    Assert.That(coord.X, Is.EqualTo(3));
                    Assert.That(coord.Y, Is.EqualTo(2));
                }
                if (index == 2)
                {
                    Assert.That(coord.X, Is.EqualTo(2));
                    Assert.That(coord.Y, Is.EqualTo(1));
                }
                if (index == 3)
                {
                    Assert.That(coord.X, Is.EqualTo(2));
                    Assert.That(coord.Y, Is.EqualTo(3));
                }
                index++;
            }

        }
        [Test]
        public void ThreeDirections()
        {
            //Arrange
            int dimension = 5;
            Coordinate coords = new Coordinate(0, 2);


            //Act
            IEnumerable<Coordinate> adjacentCoordinates = coordinateCalculator.GetAdjacentCoordinates(coords, dimension);
            int index = 0;


            //Assert
            Assert.That(adjacentCoordinates.Count, Is.EqualTo(3));
            foreach (Coordinate coord in adjacentCoordinates)
            {
                if (index == 0)
                {
                    Assert.That(coord.X, Is.EqualTo(1));
                    Assert.That(coord.Y, Is.EqualTo(2));
                }
                if (index == 1)
                {
                    Assert.That(coord.X, Is.EqualTo(0));
                    Assert.That(coord.Y, Is.EqualTo(1));
                }
                if (index == 2)
                {
                    Assert.That(coord.X, Is.EqualTo(0));
                    Assert.That(coord.Y, Is.EqualTo(3));
                }
                index++;
            }
        }
        [Test]
        public void TwoDirections()
        {
            //Arrange
            int dimension = 5;
            Coordinate coords = new Coordinate(0, 0);


            //Act
            IEnumerable<Coordinate> adjacentCoordinates = coordinateCalculator.GetAdjacentCoordinates(coords, dimension);
            int index = 0;


            //Assert
            Assert.That(adjacentCoordinates.Count, Is.EqualTo(2));
            foreach (Coordinate coord in adjacentCoordinates)
            {
                if (index == 0)
                {
                    Assert.That(coord.X, Is.EqualTo(1));
                    Assert.That(coord.Y, Is.EqualTo(0));
                }
                if (index == 1)
                {
                    Assert.That(coord.X, Is.EqualTo(0));
                    Assert.That(coord.Y, Is.EqualTo(1));
                }
                index++;
            }
        }
        [Test]
        public void ToStringRepresentationTest()
        {
            //Arrange
            string?[,] twoDimArray = new string?[2, 2];
            twoDimArray[0, 0] = "A";
            twoDimArray[0, 1] = "B";
            twoDimArray[1, 0] = "";
            twoDimArray[1, 1] = "C";

            Map map = new Map(twoDimArray);

            //Act
            string result = map.ToString();

            //Assert
            Assert.AreEqual("ABC",  result);
        }
        [Test]
        public void TrueValidation()
        {
            // Arrange
            var configuration = new MapConfiguration(
                MapSize: 1000,
                ElementToSpaceRatio: 0.5,
                MapElementConfigurations: new[]
                {
                    new MapElementConfiguration("#", "mountain", new[] { new ElementToSize(2, 20), new ElementToSize(1, 30) }, 3),
                    new MapElementConfiguration("&", "pit", new[] { new ElementToSize(3, 10) }, 10),
                    new MapElementConfiguration("%", "mineral", new[] { new ElementToSize(5, 5) }, 0),
                    new MapElementConfiguration("*", "water", new[] { new ElementToSize(4, 15) }, 0)
                });

            // Act
            bool isValid = validator.Validate(configuration);

            // Assert
            Assert.IsTrue(isValid);
        }
        [Test]
        public void FalseInvalidMapSizeValidation()
        {
            // Arrange
            var configuration = new MapConfiguration(
                MapSize: -100,
                ElementToSpaceRatio: 0.5,
                MapElementConfigurations: new[]
                {
                    new MapElementConfiguration("#", "mountain", new[] { new ElementToSize(2, 20), new ElementToSize(1, 30) }, 3)
                });

            // Act
            bool isValid = validator.Validate(configuration);

            // Assert
            Assert.IsFalse(isValid);
        }
        [Test]
        public void FalseInvalidElementToSpaceRatioValidation()
        {
            // Arrange
            var configuration = new MapConfiguration(
                MapSize: 1000,
                ElementToSpaceRatio: 1.5,
                MapElementConfigurations: new[]
                {
                    new MapElementConfiguration("#", "mountain", new[] { new ElementToSize(2, 20), new ElementToSize(1, 30) }, 3)
                });

            // Act
            bool isValid = validator.Validate(configuration);

            // Assert
            Assert.IsFalse(isValid);
        }
        [Test]
        public void DimensionCalculatorTrue()
        {
            //Arrange
            int size = 20;
            int growth = 2;
            int expectedDimension = (int)Math.Ceiling(Math.Sqrt(size)) + growth;

            //Act
            int result = dimensionCalculator.CalculateDimension(size, growth);

            //Assert
            Assert.AreEqual(expectedDimension, result);
        }
        [Test]
        public void DimensionCalculatorFalseWrongSize()
        {
            //Arrange
            int size = 0;
            int growth = 2;
            int expectedDimension = growth;

            //Act
            int result = dimensionCalculator.CalculateDimension(size, growth);

            //Assert
            Assert.AreEqual(expectedDimension, result);
        }
        [Test]
        public void DimensionCalculatorFalseWrongGrowth()
        {
            // Arrange
            int size = 100;
            int growth = -2;
            int expectedDimension = (int)Math.Ceiling(Math.Sqrt(size)) + growth;

            // Act
            int result = dimensionCalculator.CalculateDimension(size, growth);

            // Assert
            Assert.AreEqual(expectedDimension, result);
        }
        [Test]
        public void RandomValidCoordinate()
        {
            // Arrange
            int dimension = 10;

            // Act
            Coordinate randomCoordinate = coordinateCalculator.GetRandomCoordinate(dimension);

            // Assert
            Assert.That(randomCoordinate.X, Is.GreaterThanOrEqualTo(0).And.LessThan(dimension));
            Assert.That(randomCoordinate.Y, Is.GreaterThanOrEqualTo(0).And.LessThan(dimension));
        }
        [Test]
        public void MapElementBuilderTrue()
        {
            //Arrange
            int size = 10;
            string symbol = "#";
            string name = "Mountain";
            int dimensionGrowth = 2;
            string preferredLocationSymbol = ".";

            int expectedDimension = 6;
            
            // Act
            MapElement mapElement = mapElementBuilder.Build(size, symbol, name, dimensionGrowth, preferredLocationSymbol);
            int placedSymbolCount = 0;
            for (int i = 0; i < mapElement.Representation.GetLength(0); i++)
            {
                for(int j = 0; j < mapElement.Representation.GetLength(1); j++)
                {
                    if (mapElement.Representation[i,j] == symbol)
                    {
                        placedSymbolCount++;
                    }
                }
            }

            // Assert
            Assert.IsNotNull(mapElement);
            Assert.AreEqual(name, mapElement.Name);
            Assert.AreEqual(expectedDimension, mapElement.Dimension);
            Assert.AreEqual(preferredLocationSymbol, mapElement.PreferredLocationSymbol);
            Assert.AreEqual(placedSymbolCount, size);
        }
        [Test]
        public void MapElementPlacerTest()
        {
            //Arrange
            string[,] testMap = new string[5, 5]
            {
                {" "," "," "," "," "},
                {" "," "," "," "," "},
                {" "," "," "," "," "},
                {" "," "," "," "," "},
                {" "," "," "," "," "},
            };
            Coordinate testCord1 = new Coordinate(0, 0);
            Coordinate testCord2 = new Coordinate(3, 3);
            string[,] testRepresentation = new string[3, 3]
            {
                {" ", "#", " " },
                {"#", "#", "#" },
                {" ", "#", " " },
            };

            //Act
            MapElement testElement = new MapElement(testRepresentation, "mountain", 3);

            //Assert
            Assert.That(elementPlacer.CanPlaceElement(testElement, testMap, testCord1), Is.EqualTo(true));
            Assert.That(elementPlacer.CanPlaceElement(testElement, testMap, testCord2), Is.EqualTo(false));
            elementPlacer.PlaceElement(testElement, testMap, testCord1);
            Assert.That(testMap[1,1], Is.EqualTo("#"));

        }
        [Test]
        public void MapGeneratorTest()
        {
            //Arrange
            var mountainsCfg = new MapElementConfiguration("#", "mountain", new[]
            {
                new ElementToSize(2, 5),
            }, 1);

            List<MapElementConfiguration> elementsCfg = new() { mountainsCfg };
            MapConfiguration testConfig = new MapConfiguration(10, 1, elementsCfg);

            //Act
            Map resultingMap = generator.Generate(testConfig);

            //Assert
            Assert.AreEqual(testConfig.MapSize, resultingMap.Representation.GetLength(0));
        }
        [Test]
        public void ElementsGeneratorTest()
        {
            //Arrange
            var mountainsCfg = new MapElementConfiguration("#", "mountain", new[]
            {   
                new ElementToSize(2, 5),
            }, 1);

            List<MapElementConfiguration> elementsCfg = new() { mountainsCfg };
            MapConfiguration testConfig = new MapConfiguration(10, 1, elementsCfg);
            
            //Act
            IEnumerable<MapElement> resultingElements = elementsGenerator.CreateAll(testConfig);

            //Assert
            Assert.That(resultingElements.Count(), Is.EqualTo(2));
            
        }
    }
}