using System.Collections.Generic;
using Xunit;
using Moq;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Models;
using ThoughtWorks.ConferenceTrackManager.Access;

namespace ThoughtWorks.ConferenceTrackManager.Tests.Models
{
    public class ConferenceTests
    {
        [Fact]
        public void Print_BuildsSessions()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(new List<IConferenceSession>());
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true });
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            sessionBuilder.Verify(sb => sb.CreateSessionsOrEmptyList(), Times.Once());
        }

        [Fact]
        public void Print_SortsTalks()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(new List<IConferenceSession>());
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true });
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            sessionBuilder.Verify(sb => sb.SortTalks(talks), Times.Once());
        }

        [Fact]
        public void Print_PopulatesSessions()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(sessions);
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true });
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            sessionBuilder.Verify(sb => sb.PopulateSessionsWithTalks(sessions, It.IsAny<IList<ITalk>>()), Times.Once());
        }

        [Fact]
        public void Print_PrintsErrorMessage_WhenPopulationFails()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(sessions);
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = false, Message = "Error message from PopulateSessionsWithTalks" });
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("Error message from PopulateSessionsWithTalks"), Times.Once());
        }

        [Fact]
        public void Print_DoesNotPrintErrorMessage_WhenPopulationSucceeds()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(sessions);
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true, Message = "Error message from PopulateSessionsWithTalks" });
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            outputWriter.Verify(o => o.WriteLine("Error message from PopulateSessionsWithTalks"), Times.Never());
        }

        [Fact]
        public void Print_UsesSortedTalk_ToPopulatesSessions()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            var sessions = new List<IConferenceSession> { new Mock<IConferenceSession>().Object, new Mock<IConferenceSession>().Object };
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(sessions);
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true });
            var sortedTalks = new List<ITalk>();
            sessionBuilder.Setup(sb => sb.SortTalks(talks)).Returns(sortedTalks);
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            sessionBuilder.Verify(sb => sb.PopulateSessionsWithTalks(sessions, sortedTalks), Times.Once());
        }

        [Fact]
        public void Print_PrintsEachSession()
        {
            // Arrange
            List<ITalk> talks = new List<ITalk>();
            var sessionBuilder = new Mock<IConferenceSessionBuilder>();
            var sessions = new List<IConferenceSession>();
            var firstSession = new Mock<IConferenceSession>();
            sessions.Add(firstSession.Object);
            var lastSession = new Mock<IConferenceSession>();
            sessions.Add(lastSession.Object);
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(sessions);
            sessionBuilder.Setup(sb => sb.PopulateSessionsWithTalks(It.IsAny<IList<IConferenceSession>>(), It.IsAny<IList<ITalk>>())).Returns(new PopulateSessionsWithTalksResponse { SuccessfullyAddedAllTalksToSession = true });
            var outputWriter = new Mock<IOutputWriter>();
            var conference = new Conference(talks, sessionBuilder.Object, outputWriter.Object);

            // Act
            conference.Print();

            // Assert
            firstSession.Verify(s => s.Print(), Times.Once());
            lastSession.Verify(s => s.Print(), Times.Once());
        }
    }
}
