using System;
using Xunit;
using Moq;
using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.Models;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class MorningConferenceSessionTests
    {

        [Fact]
        public void Print_PrintsAllTalks()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var morningConferenceSession = new MorningConferenceSession(0, 2, 3, outputWriter.Object);
            morningConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number One 60mins"));
            morningConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number Two 60mins"));

            // Act
            morningConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("12:00AM Talk Number One 60mins"), Times.Once());
            outputWriter.Verify(o => o.WriteLine("01:00AM Talk Number Two 60mins"), Times.Once());
        }

        [Fact]
        public void Print_PrintsTrackNumber()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var morningConferenceSession = new MorningConferenceSession(0, 3, 0, outputWriter.Object);

            // Act
            morningConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("Track 1"), Times.Once());
        }

        [Fact]
        public void Print_PrintsLunchTime()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var morningConferenceSession = new MorningConferenceSession(0, 3, 0, outputWriter.Object);

            // Act
            morningConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("03:00AM Lunch"), Times.Once());
        }
    }
}
