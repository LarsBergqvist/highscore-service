using System;
using System.Collections.Generic;
using Core.Models;
using Core.Helpers;
using Xunit;

namespace Core.Tests
{
    public class GameResultHelperTests
    {
        private readonly IList<GameResult> oldResults;
        public GameResultHelperTests()
        {
            oldResults = new List<GameResult>
            {
                new GameResult("userA", 10) { UtcDateTime = DateTime.Today.AddDays(-1) },
                new GameResult("userB", 20) { UtcDateTime = DateTime.Today.AddDays(-1) },
                new GameResult("userC", 30) { UtcDateTime = DateTime.Today.AddDays(-1) },
            };
        }

        [Fact]
        public void AddUserWithBestScore_LowIsBest()
        {
            var helper = new GameResultHelper();
            var newResult = new GameResult("userD", 5);
            var newResults = helper.GetSortedListWithNewResult(newResult, oldResults, true, 10);
            Assert.Equal(4, newResults.Count);
            Assert.Equal("userD", newResults[0].UserName);
        }

        [Fact]
        public void AddUserWithBestScore_HighIsBest()
        {
            var helper = new GameResultHelper();
            var newResult = new GameResult("userD", 40);
            var newResults = helper.GetSortedListWithNewResult(newResult, oldResults, false, 10);

            // User should be at the top of the list
            Assert.Equal(4, newResults.Count);
            Assert.Equal("userD", newResults[0].UserName);

            // Check that the rest of the list has been sorted correctly
            Assert.Equal("userC", newResults[1].UserName);
            Assert.Equal("userB", newResults[2].UserName);
            Assert.Equal("userA", newResults[3].UserName);
        }

        [Fact]
        public void SameScoreNewerDateTime_ShouldWin()
        {
            var helper = new GameResultHelper();
            var newResult = new GameResult("userD", 10);
            var newResults = helper.GetSortedListWithNewResult(newResult, oldResults, true, 10);
            Assert.Equal(4, newResults.Count);
            Assert.Equal("userD", newResults[0].UserName);
        }

        [Fact]
        public void AddUserWithLowestRank_MaxSizeReached()
        {
            var helper = new GameResultHelper();
            var newResult = new GameResult("userD", 100);
            // Use a max size of 3
            var newResults = helper.GetSortedListWithNewResult(newResult, oldResults, true, 3);
            Assert.Equal(3, newResults.Count);
            // userD did not make it into the highscore list
            Assert.Equal("userA", newResults[0].UserName);
            Assert.Equal("userB", newResults[1].UserName);
            Assert.Equal("userC", newResults[2].UserName);
        }

        [Fact]
        public void AddUserWithBestScore_MaxSizeReached()
        {
            var helper = new GameResultHelper();
            var newResult = new GameResult("userD", 5);
            // Use a max size of 3
            var newResults = helper.GetSortedListWithNewResult(newResult, oldResults, true, 3);
            Assert.Equal(3, newResults.Count);
            Assert.Equal("userD", newResults[0].UserName);
            // userC should no longer be in list
            // userB has lowest rank in list
            Assert.Equal("userB", newResults[2].UserName);
        }

    }
}
