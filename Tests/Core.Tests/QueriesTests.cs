using Xunit;
using Core.CQRS.Queries;
using System.Linq;

namespace Core.Tests
{
    public class QueriesTests: TestBase
    {
        [Fact]
        public async void TestGetAllHighScoreLists()
        {
            var highScoreLists = await Mediator.Send(new GetAllHighScoreLists.Request());
            Assert.NotNull(highScoreLists);
            Assert.Single(highScoreLists);
            var list = highScoreLists.FirstOrDefault();
            Assert.Equal(3, list.Results.Count);
        }

        [Fact]
        public async void TestGetAllHighScoreListById()
        {
            var list = await Mediator.Send(new GetHighScoreListById.Request("1"));
            Assert.Equal(3, list.Results.Count);
        }
    }
}
