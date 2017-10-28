using System;
using ThoughtWorks.ConferenceTrackManager.Models;
using Xunit;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class ConferenceSessionTests
    {
        [Fact]
        public void TryIncludeTalkInSession_ReturnsTrue_WhenTalkFitsInSession()
        {
            // Arrange
            var session = new MorningConferenceSession(1, 2, 1);

            // Act
            var result = session.TryIncludeTalkInSession(new Talk("Some talk 10mins"));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TryIncludeTalkInSession_ReturnsFalse_WhenTalkDoesntFitInSession()
        {
            // Arrange
            var session = new MorningConferenceSession(1, 2, 1);

            // Act
            var result = session.TryIncludeTalkInSession(new Talk("Some talk 70mins"));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryIncludeTalkInSession_ReturnsFalse_WhenASecondTalkDoesntFitInSession()
        {
            // Arrange
            var session = new MorningConferenceSession(1, 2, 1);
            var result = session.TryIncludeTalkInSession(new Talk("Some talk 30mins"));


            // Act
            result = session.TryIncludeTalkInSession(new Talk("Some talk 40mins"));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void TryIncludeTalkInSession_ReturnsTrue_WhenASecondTalkOnlyJustFitsInSession()
        {
            // Arrange
            var session = new MorningConferenceSession(1, 2, 1);
            var result = session.TryIncludeTalkInSession(new Talk("Some talk 30mins"));

            // Act
            result = session.TryIncludeTalkInSession(new Talk("Some talk 30mins"));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void TryIncludeTalkInSession_ReturnsFalse_WhenASecondTalkOnlyJustDoesntFitInSession()
        {
            // Arrange
            var session = new MorningConferenceSession(1, 2, 1);
            var result = session.TryIncludeTalkInSession(new Talk("Some talk 30mins"));


            // Act
            result = session.TryIncludeTalkInSession(new Talk("Some talk 31mins"));

            // Assert
            Assert.False(result);
        }
    }
}
