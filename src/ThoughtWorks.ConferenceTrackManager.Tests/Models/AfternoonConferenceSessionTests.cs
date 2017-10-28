using System;
using ThoughtWorks.ConferenceTrackManager.Access;
using ThoughtWorks.ConferenceTrackManager.Models;
using Xunit;
using Moq;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class AfternoonConferenceSessionTests
    {
        [Fact]
        public void Print_PrintsAllTalks()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var afternoonConferenceSession = new AfternoonConferenceSession(0, 2, 3, 1, outputWriter.Object);
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number One 60mins"));
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number Two 60mins"));

            // Act
            afternoonConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("12:00AM Talk Number One 60mins"), Times.Once());
            outputWriter.Verify(o => o.WriteLine("01:00AM Talk Number Two 60mins"), Times.Once());
        }

        [Fact]
        public void Print_DoesNotPrintTrackNumber()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var afternoonConferenceSession = new AfternoonConferenceSession(0, 2, 3, 0, outputWriter.Object);

            // Act
            afternoonConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("Track 1"), Times.Never());
        }

        [Fact]
        public void Print_PrintsNetworkingSession_AtLatestTime()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var afternoonConferenceSession = new AfternoonConferenceSession(0, 2, 3, 1, outputWriter.Object);
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number One 60mins"));
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number Two 60mins"));
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number Three 60mins"));

            // Act
            afternoonConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("03:00AM Networking Event"), Times.Once());
        }

        // todo check all methods are facts or theories
        [Fact]
        public void Print_PrintsNetworkingSession_AtEarliestTime()
        {
            // Arrange
            var outputWriter = new Mock<IOutputWriter>();
            var afternoonConferenceSession = new AfternoonConferenceSession(0, 2, 3, 1, outputWriter.Object);
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number One 60mins"));
            afternoonConferenceSession.TryIncludeTalkInSession(new Talk("Talk Number Two 60mins"));

            // Act
            afternoonConferenceSession.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("02:00AM Networking Event"), Times.Once());
        }
    }
}
