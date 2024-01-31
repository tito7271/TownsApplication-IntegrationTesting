namespace TownApplication.IntegrationTests
{
    public class TownControllerIntegrationTests
    {
        private readonly TownController _controller;

        public TownControllerIntegrationTests()
        {
            _controller = new TownController();
            _controller.ResetDatabase();
        }

        [Fact]
        public void AddTown_ValidInput_ShouldAddTown()
        {
            // Arrange
            var validTownName = "Sofia";
            var ValidPopulation = 3000000;

            // Act
            _controller.AddTown(validTownName, ValidPopulation);

            // Assert
            var result = _controller.GetTownByName(validTownName);

            Assert.NotNull(result);
            Assert.Equal(ValidPopulation, result.Population);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AB")]
        public void AddTown_InvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var validPopulation = 100;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _controller.AddTown(invalidName, validPopulation));
            Assert.Equal("Invalid town name.", exception.Message);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddTown_InvalidPopulation_ShouldThrowArgumentException(int invalidPopulation)
        {
            // Arrange
            var validTownName = "Sofia";

            // Act
            var action = () => _controller.AddTown(validTownName, invalidPopulation);

            // Assert
            var exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("Population must be a positive number.", exception.Message);
        }

        [Fact]
        public void AddTown_DuplicateTownName_DoesNotAddDuplicateTown()
        {
            // Arrange
            var validTownName = "Sofia";
            var validPopulation = 3000000;
            
            _controller.AddTown(validTownName, validPopulation);

            // Act
            _controller.AddTown(validTownName, validPopulation + 1000);

            // Assert
            var result = _controller.ListTowns();
            Assert.Single(result);
            var addedTown = result[0];
            Assert.Equal(validPopulation, addedTown.Population);
            Assert.Equal(validTownName, addedTown.Name);
        }

        [Fact]
        public void UpdateTown_ShouldUpdatePopulation()
        {
            // Arrange
            var validTownName = "Sofia";
            var initialPopulation = 3000000;

            _controller.AddTown(validTownName, initialPopulation);

            var updatedPopulation = initialPopulation + 1000;

            // Act
            var townForUpdate = _controller.GetTownByName(validTownName);
            _controller.UpdateTown(townForUpdate.Id, updatedPopulation);

            // Assert
            var updatedTown = _controller.GetTownByName(validTownName);
            Assert.NotNull(updatedTown);
            Assert.Equal(updatedPopulation, updatedTown.Population);

        }

        [Fact]
        public void DeleteTown_ShouldDeleteTown()
        {
            // Arrange
            var validTownName = "Sofia";
            var ValidPopulation = 3000000;

            _controller.AddTown(validTownName, ValidPopulation);

            // Act
            var townForDelete = _controller.GetTownByName(validTownName);
            _controller.DeleteTown(townForDelete.Id);

            // Assert
            var listOfTowns = _controller.ListTowns();
            var deletedTown = _controller.GetTownByName(validTownName);
            Assert.Empty(listOfTowns);
            Assert.Null(deletedTown);
        }

        [Fact]
        public void ListTowns_ShouldReturnTowns()
        {
            // Arrange
            var firstTownName = "Sofia";
            var firstTownPopulation = 3000000;

            var secondTownName = "Plovdiv";
            var secondTownPopulation = 600000;

            var thirdTownName = "Varna";
            var thirdTownPopulation = 500000;

            _controller.AddTown(firstTownName, firstTownPopulation);
            _controller.AddTown(secondTownName, secondTownPopulation);
            _controller.AddTown(thirdTownName, thirdTownPopulation);

            // Act

            var listOfTowns = _controller.ListTowns();

            // Assert
            Assert.Equal(3, listOfTowns.Count);
            Assert.Contains(firstTownName, listOfTowns.Select(t => t.Name));
            Assert.Contains(secondTownName, listOfTowns.Select(t => t.Name));
            Assert.Contains(thirdTownName, listOfTowns.Select(t => t.Name));
            Assert.Contains(firstTownPopulation, listOfTowns.Select(t => t.Population));
            Assert.Contains(secondTownPopulation, listOfTowns.Select(t => t.Population));
            Assert.Contains(thirdTownPopulation, listOfTowns.Select(t => t.Population));
        }
    }
}
