using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using WorldOfAdventures.BusinessLogic;
using WorldOfAdventures.DAL;
using WorldOfAdventures.Models;

namespace WorldOfAdventures.UnitTests.AdventureServiceTests
{
    public class BaseAdventureServiceTests
    {
        protected Mock<IValidationService> _validationServiceMock;
        protected Mock<IUserAdventureRepository> _userAdventureRepositoryMock;
        protected Mock<IAdventureRepository> _adventureRepositoryMock;
        protected IAdventureService _adventureService;

        protected const string TestAdventureName = "TestAdventure";
        protected const string TestUserName = "TestUser";

        [SetUp]
        public void Setup()
        {
            _validationServiceMock = new Mock<IValidationService>();
            _userAdventureRepositoryMock = new Mock<IUserAdventureRepository>();
            _adventureRepositoryMock = new Mock<IAdventureRepository>();

            _adventureService = new AdventureService(_validationServiceMock.Object, _adventureRepositoryMock.Object, _userAdventureRepositoryMock.Object);
        }

        protected Adventure CreateTestAdventure(string adventureName = TestAdventureName)
        {
            return new Adventure(adventureName, new AdventureStep
            {
                Sentence = "Do I want a donut?",
                NextSteps = new List<AdventureStep>
                {
                    new AdventureStep
                    {
                        Answer = "Yes",
                        Sentence = "Did I deserve it?",
                        NextSteps = new List<AdventureStep>
                        {
                            new AdventureStep
                            {
                                Answer = "Sure",
                                Sentence = "Buddy, go get it then!"
                            },
                            new AdventureStep
                            {
                                Answer = "Not really",
                                Sentence = "Sorry to hear! You should get some exercises first then :)"
                            }
                        }
                    },
                    new AdventureStep
                    {
                        Answer = "No",
                        Sentence = "Go eat an apple then :)"
                    }
                }
            });
        }

        protected DAL.Models.Adventure CreateDbTestAdventure(string adventureName = TestAdventureName)
        {
            return new DAL.Models.Adventure
            {
                Name = adventureName,
                InitialStep = new DAL.Models.AdventureStep
                {
                    Sentence = "Do I want a donut?",
                    NextSteps = new List<DAL.Models.AdventureStep>
                    {
                        new DAL.Models.AdventureStep
                        {
                            Answer = "Yes",
                            Sentence = "Did I deserve it?",
                            NextSteps = new List<DAL.Models.AdventureStep>
                            {
                                new DAL.Models.AdventureStep
                                {
                                    Answer = "Sure",
                                    Sentence = "Buddy, go get it then!"
                                },
                                new DAL.Models.AdventureStep
                                {
                                    Answer = "Not really",
                                    Sentence = "Sorry to hear! You should get some exercises first then :)"
                                }
                            }
                        },
                        new DAL.Models.AdventureStep
                        {
                            Answer = "No",
                            Sentence = "Go eat an apple then :)"
                        }
                    }
                }
            };
        }
    }
}