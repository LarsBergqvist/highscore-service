using Core.CQRS.Commands;
using Core.CQRS.Queries;
using Core.Models;
using Xunit;

namespace Core.Tests
{
    public class CommandsTests : TestBase
    {
        [Fact]
        public async void TestGetAllHighScoreLists()
        {
            var input = new HighScoreListWriteModel
            {
                LowIsBest = true,
                MaxSize = 5,
                Name = "My game2",
                Unit = "Moves"
            };
            
            var list = await Mediator.Send(new CreateHighScoreList.Request(input));
            Assert.Equal(0, list.Results.Count);
            Assert.Equal("My game2", list.Name);
            Assert.Equal("Moves", list.Unit);
            Assert.True(list.LowIsBest);
        }

        [Fact]
        public async void TestAddGameResult()
        {
            var list = await Mediator.Send(new GetHighScoreListById.Request("1"));
            Assert.Equal(3, list.Results.Count);
            var gameResult = new GameResult("MyName", 100);
            var result = await Mediator.Send(new AddGameResult.Request("1", gameResult));
            list = await Mediator.Send(new GetHighScoreListById.Request("1"));
            Assert.Equal(4, list.Results.Count);
        }

        [Fact]
        public async void TestAddGameResult_NoMatchingListId()
        {
            var gameResult = new GameResult("MyName", 100);
            var result = await Mediator.Send(new AddGameResult.Request("2", gameResult));
            Assert.True(result.NoMatchingListId);
        }
    }
}
