using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Helpers;
using Core.Models;
using Core.Repositories;
using Core.Settings;
using Infrastructure.MongoDB;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

namespace Infrastructure.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task TestGetHighScoreListById()
        {
            //
            // Arrange
            //
            var list1Id = ObjectId.GenerateNewId();
            var highscoreLists = new List<HighScoreListDBModel>
            {
                new(list1Id, "item1", false, "apa", 10)
            };
            var mockCollection = new Mock<IMongoCollection<HighScoreListDBModel>>();
            var context = GetMockedContext(mockCollection, highscoreLists);
            var repo = GetRepository(context);

            //
            // Act
            //
            var list = await repo.GetHighScoreList(list1Id.ToString());

            //
            // Assert
            //
            Assert.Equal(highscoreLists[0].MaxSize, list.MaxSize);
            Assert.Equal(highscoreLists[0].Id.ToString(), list.Id);
            mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<HighScoreListDBModel>>(),
                                                   It.IsAny<FindOptions<HighScoreListDBModel,
                                                   HighScoreListDBModel>>(),
                                                   new CancellationToken()),
                                                   Times.Once);
        }

        [Fact]
        public async Task TestGetAllHighScoreLists()
        {
            //
            // Arrange
            //
            var list1Id = ObjectId.GenerateNewId();
            var list2Id = ObjectId.GenerateNewId();
            var highscoreLists = new List<HighScoreListDBModel>
            {
                new(list1Id, "item1", false, "apa", 10),
                new(list2Id, "item2", false, "apa", 15)
            };
            var mockCollection = new Mock<IMongoCollection<HighScoreListDBModel>>();
            var context = GetMockedContext(mockCollection, highscoreLists);
            var repo = GetRepository(context);

            //
            // Act
            //
            var lists = await repo.GetAllHighScoreLists();

            //
            // Assert
            //
            Assert.Equal(2, lists.ToList().Count);
            mockCollection.Verify(c => c.FindAsync(It.IsAny<FilterDefinition<HighScoreListDBModel>>(),
                                                   It.IsAny<FindOptions<HighScoreListDBModel,
                                                       HighScoreListDBModel>>(),
                                                   new CancellationToken()),
                                  Times.Once);
        }

        [Fact]
        public async Task TestCreateToHighScoreList()
        {
            //
            // Arrange
            //
            var list1Id = ObjectId.GenerateNewId();
            var highscoreLists = new List<HighScoreListDBModel>
            {
                new(list1Id, "item1", false, "apa", 10),
            };
            var mockCollection = new Mock<IMongoCollection<HighScoreListDBModel>>();
            var context = GetMockedContext(mockCollection, highscoreLists);
            var repo = GetRepository(context);

            //
            // Act
            //
            var highScoreListInput = new HighScoreListWriteModel
            {
                LowIsBest = true,
                MaxSize = 15,
                Name = "MyList",
                Unit = "score"
            };
            var createdList = await repo.CreateHighScoreList(highScoreListInput);

            //
            // Assert
            //
            Assert.NotEmpty(createdList.Id);
            Assert.Equal(highScoreListInput.LowIsBest, createdList.LowIsBest);
            Assert.Equal(highScoreListInput.MaxSize, createdList.MaxSize);
            Assert.Equal(highScoreListInput.Name, createdList.Name);
            Assert.Equal(highScoreListInput.Unit, createdList.Unit);
            Assert.Empty(createdList.Results);
            mockCollection.Verify(c => c.InsertOneAsync(It.IsAny<HighScoreListDBModel>(), It.IsAny<InsertOneOptions>(), new CancellationToken()), Times.Once);
        }

        [Fact]
        public async Task TestAddGameResultToHighScoreList()
        {
            //
            // Arrange
            //
            var list1Id = ObjectId.GenerateNewId();
            var highscoreLists = new List<HighScoreListDBModel>
            {
                new(list1Id, "item1", false, "apa", 10),
            };
            var mockCollection = new Mock<IMongoCollection<HighScoreListDBModel>>();
            var context = GetMockedContext(mockCollection, highscoreLists);
            var repo = GetRepository(context);

            //
            // Act
            //
            var gameResult = new GameResult("Pelle", 100);
            await repo.AddGameResultToHighScoreList(list1Id.ToString(), gameResult);

            //
            // Assert
            //
            mockCollection.Verify(c => c.ReplaceOneAsync(It.IsAny<FilterDefinition<HighScoreListDBModel>>(),
                                                         It.IsAny<HighScoreListDBModel>(),
                                                         It.IsAny<ReplaceOptions>(), new CancellationToken()
                                  ),
                                  Times.Once);
        }

        private IMongoDBContext GetMockedContext(Mock<IMongoCollection<HighScoreListDBModel>> mockCollection,
                                                 IEnumerable<HighScoreListDBModel> highscoreLists)
        {
            var cursor = new MockAsyncCursor<HighScoreListDBModel>(highscoreLists);
            var mockContext = new Mock<IMongoDBContext>();
            mockContext.Setup(c => c.GetCollection<HighScoreListDBModel>("highscores")).Returns(mockCollection.Object);

            mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<HighScoreListDBModel>>(),
                                                    It.IsAny<FindOptions<HighScoreListDBModel, HighScoreListDBModel>>(),
                                                    It.IsAny<CancellationToken>())).ReturnsAsync(cursor);

            mockCollection.Setup(op => op.ReplaceOneAsync(It.IsAny<FilterDefinition<HighScoreListDBModel>>(),
                                                          It.IsAny<HighScoreListDBModel>(),
                                                          It.IsAny<ReplaceOptions>(),
                                                          new CancellationToken()));

            mockCollection.Setup(op => op.InsertOneAsync(It.IsAny<HighScoreListDBModel>(),
                                                         It.IsAny<InsertOneOptions>(),
                                                         new CancellationToken()));

            mockCollection.SetReturnsDefault(cursor);

            return mockContext.Object;
        }

        private IHighScoreRepository GetRepository(IMongoDBContext context)
        {
            var settings = new RepositorySettings
            {
                ConnectionString = "connstring",
                HighScoresDBName = "games",
                HighScoresCollectionName = "highscores"
            };
            var logger = new NullLogger<HighScoreRepository>();
            var options = Options.Create(settings);

            return new HighScoreRepository(logger, options, context, new GameResultHelper());
        }
    }
}
