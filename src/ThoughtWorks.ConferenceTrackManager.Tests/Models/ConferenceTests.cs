using System.Collections.Generic;
using Xunit;
using Moq;
using ThoughtWorks.ConferenceTrackManager.App;
using ThoughtWorks.ConferenceTrackManager.Models;

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
            var conference = new Conference(talks, sessionBuilder.Object);

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
            var conference = new Conference(talks, sessionBuilder.Object);

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
            var sessions = new List<IConferenceSession>();
            sessionBuilder.Setup(sb => sb.CreateSessionsOrEmptyList()).Returns(sessions);
            var conference = new Conference(talks, sessionBuilder.Object);

            // Act
            conference.Print();

            // Assert
            sessionBuilder.Verify(sb => sb.PopulateSessionsWithTalks(sessions, talks), Times.Once());
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
            var conference = new Conference(talks, sessionBuilder.Object);

            // Act
            conference.Print();

            // Assert
            firstSession.Verify(s => s.Print(), Times.Once());
            lastSession.Verify(s => s.Print(), Times.Once());
        }
    }
}
