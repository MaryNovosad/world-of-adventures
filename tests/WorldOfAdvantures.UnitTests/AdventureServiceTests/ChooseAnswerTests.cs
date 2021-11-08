using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.UnitTests.AdventureServiceTests
{
    public class ChooseAnswerTests: BaseAdventureServiceTests
    {
        [Test]
        public async Task When_ValidUserChoiceIsProvided_Then_ChoiceIsAppendedToUserAdventureChain()
        {
            // Arrange
            var adventureTemplate = CreateDbTestAdventure();
            _adventureRepositoryMock.Setup(r => r.FindAsync(adventureTemplate.Name))
                .ReturnsAsync(adventureTemplate);

            var userChoice = new UserChoice
            {
                AdventureLevel = 2,
                Answer = "Sure"
            };

            _validationServiceMock.Setup(v => v.ValidateUserChoice(
                adventureTemplate.Name,
                It.IsAny<UserAdventure>(),
                It.Is<UserChoice>(c =>
                    c.AdventureLevel == userChoice.AdventureLevel && c.Answer == userChoice.Answer)));

            var initialAnswer = "Yes";

            var existingUserAdventure = new DAL.Models.UserAdventure
            {
                AdventureName = TestAdventureName,
                UserName = TestUserName,
                InitialChoice = new DAL.Models.UserChoice
                {
                    Answer = initialAnswer
                }
            };

            _userAdventureRepositoryMock.Setup(r => r.FindAsync(TestUserName, TestAdventureName))
                .ReturnsAsync(existingUserAdventure);

            // Act
            await _adventureService.ChooseAnswerAsync(TestUserName, TestAdventureName, userChoice);

            // Assert
            Assert.IsNotNull(existingUserAdventure.InitialChoice.NextChoice);
            Assert.IsTrue(existingUserAdventure.InitialChoice.Answer == initialAnswer
                          && existingUserAdventure.InitialChoice.NextChoice.Answer == userChoice.Answer);
        }

        [Test]
        public async Task When_ValidInitialUserChoiceIsProvided_Then_UserAdventureChainIsCreated()
        {
            // Arrange
            var adventureTemplate = CreateDbTestAdventure();
            _adventureRepositoryMock.Setup(r => r.FindAsync(adventureTemplate.Name))
                .ReturnsAsync(adventureTemplate);

            var userInitialChoice = new UserChoice
            {
                AdventureLevel = 1,
                Answer = "Yes"
            };

            _validationServiceMock.Setup(v => v.ValidateUserChoice(
                adventureTemplate.Name,
                It.IsAny<UserAdventure>(),
                It.Is<UserChoice>(c =>
                    c.AdventureLevel == userInitialChoice.AdventureLevel && c.Answer == userInitialChoice.Answer)));

            // Act
            await _adventureService.ChooseAnswerAsync(TestUserName, TestAdventureName, userInitialChoice);

            // Assert
            _userAdventureRepositoryMock.Verify(r => r.CreateAsync(It.Is<DAL.Models.UserAdventure>(a =>
                a.AdventureName == TestAdventureName &&
                a.UserName == TestUserName &&
                a.InitialChoice.Answer == userInitialChoice.Answer &&
                a.InitialChoice.NextChoice == null)));
        }
    }
}
