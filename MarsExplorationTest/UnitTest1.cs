using Codecool.MarsExploration.Calculators.Model;
using Codecool.MarsExploration.Calculators.Service;
using Codecool.MarsExploration.Configuration.Model;
using Codecool.MarsExploration.Configuration.Service;
using Codecool.MarsExploration.MapElements.Model;
using System.ComponentModel.DataAnnotations;

namespace MarsExplorationTest

{
    [TestFixture]
    public class MarsExplorationTest
    {
        ICoordinateCalculator coordinateCalculator;
        private MapConFigurationValidator validator;

        [SetUp]
        public void Setup()
        {
            coordinateCalculator = new CordinateCalculator();
            validator = new MapConFigurationValidator();
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
    }
}