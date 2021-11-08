using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using UserAdventure = WorldOfAdventures.DAL.Models.UserAdventure;

namespace WorldOfAdventures.UnitTests.AdventureServiceTests
{
    public class CreateOrUpdateTests: BaseAdventureServiceTests
    {
        [Test]
        public async Task When_AdventureWithNewNameIsProvided_Then_AdventureIsPassedForCreation()
        {
            // Arrange
            var newAdventure = CreateTestAdventure();

            // Act
            await _adventureService.CreateOrUpdateAsync(newAdventure.Name, newAdventure);

            // Assert
            _adventureRepositoryMock.Verify(r => r.CreateAsync(
                // TODO: to check the whole adventure model whether actual and expected data match instead of just a basic adventure name check
                It.Is<DAL.Models.Adventure>(a => a.Name == newAdventure.Name)), Times.Once);
        }

        [Test]
        public async Task When_AdventureWithExistingNameIsProvided_And_NoOneHasTriedThisAdventureYet_Then_AdventureIsPassedForUpdate()
        {
            // Arrange
            var newAdventure = CreateTestAdventure();

            _adventureRepositoryMock.Setup(r => r.FindAsync(newAdventure.Name))
                .ReturnsAsync(new DAL.Models.Adventure { Name = newAdventure.Name});
            
            _userAdventureRepositoryMock.Setup(r => r.FindAsync(newAdventure.Name))
                .ReturnsAsync(new List<UserAdventure>()); 

            // Act
            await _adventureService.CreateOrUpdateAsync(newAdventure.Name, newAdventure);

            // Assert
            _adventureRepositoryMock.Verify(r => r.UpdateAsync(
                // TODO: to check the whole adventure model whether actual and expected data match instead of just a basic adventure name check
                It.Is<DAL.Models.Adventure>(a => a.Name == newAdventure.Name)), Times.Once);
        }

        [Test]
        public void When_AdventureWithExistingNameIsProvided_And_SomeUsersAlreadyTriedThisAdventure_Then_ArgumentExceptionIsThrown()
        {
            // Arrange
            var newAdventure = CreateTestAdventure();

            _adventureRepositoryMock.Setup(r => r.FindAsync(newAdventure.Name))
                .ReturnsAsync(new DAL.Models.Adventure { Name = newAdventure.Name });

            _userAdventureRepositoryMock.Setup(r => r.FindAsync(newAdventure.Name))
                .ReturnsAsync(new List<UserAdventure>
                {
                    new UserAdventure
                    {
                        UserName = TestUserName,
                        AdventureName = TestAdventureName
                    }
                });

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => _adventureService.CreateOrUpdateAsync(newAdventure.Name, newAdventure));
        }

        [Test]
        public void When_NotMatchedAdventureNamesAreProvided_Then_ArgumentExceptionIsThrown()
        {
            // Arrange
            var newAdventure = CreateTestAdventure();

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(
                () => _adventureService.CreateOrUpdateAsync(newAdventure.Name + "v2", newAdventure));
        }
    }
}